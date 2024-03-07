using FirstServer.iceImpl.Soup;
using FirstServer.service;
using Ice;
using Microsoft.Extensions.Configuration;
using Exception = System.Exception;

namespace FirstServer
{
    public class ApplicationI : Application
    {
        private MongoDbService _client;

        public ApplicationI(MongoDbService client)
        {
            _client = client;
        }

        public override int run(string[] args)
        {
            try
            {
                using (Communicator communicator = Application.communicator())
                {
                    var adapter =
                        communicator.createObjectAdapterWithEndpoints("SoupAdapter",
                            $"default -h {args[0]} -p {args[1]}");
                    InitSoup(adapter);
                    adapter.activate();
                    Console.Out.WriteLine("Server is running...");
                    communicator.waitForShutdown();
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                return 1;
            }

            return 0;
        }

        private void InitSoup(ObjectAdapter adapter)
        {
            adapter.add(new FileDownloaderI(), Util.stringToIdentity("Soup.FileDownloader"));
            adapter.add(new FileUploaderI(), Util.stringToIdentity("Soup.FileUploader"));
            adapter.add(new FileSenderI(), Util.stringToIdentity("Soup.FileSender"));
            adapter.add(new SongDataModuleI(), Util.stringToIdentity("Soup.SongDataModule"));
        }
    }

    public class Program
    {
        public static int Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("settings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            checkConfig(configuration);

            var client = new MongoDbService(configuration["databaseUrl"]);

            Application application = new ApplicationI(client);

            return application.main([
                configuration["url"],
                configuration["port"],
            ]);
        }

        private static void checkConfig(IConfigurationRoot configuration)
        {
            string[] necessaryKeys = ["url", "port", "databaseUrl"];
            foreach (var necessaryKey in necessaryKeys)
            {
                ArgumentNullException.ThrowIfNull(configuration[necessaryKey]);
            }
        }
    }
}