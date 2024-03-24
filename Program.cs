using FirstServer.iceImpl.Soup;
using FirstServer.service;
using Ice;
using LibVLCSharp.Shared;
using Microsoft.Extensions.Configuration;
using Exception = System.Exception;
using FileSenderI = FirstServer.iceImpl.Soup.FileSenderI;

namespace FirstServer;

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
            Core.Initialize();
            using (Communicator communicator = Application.communicator())
            {
                var adapter =
                    communicator.createObjectAdapter("SoupAdapter");

                _client.setDatabase(communicator.getProperties().getProperty("SoupAdapter.AdapterId"));
                InitSoup(adapter, _client);
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

    private void InitSoup(ObjectAdapter adapter, MongoDbService mongoDbService)
    {
        adapter.add(new FileUploaderI(mongoDbService), Util.stringToIdentity("Soup.FileUploader"));
        adapter.add(new FileSenderI(mongoDbService), Util.stringToIdentity("Soup.FileSender"));
        adapter.add(new SongDataModuleI(mongoDbService), Util.stringToIdentity("Soup.SongDataModule"));
        adapter.add(new AudioPlayerI(mongoDbService), Util.stringToIdentity("Soup.AudioPlayer"));
    }
}

public class Program
{
    public static string SongPath;
    
    public static int Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("settings.json", optional: false, reloadOnChange: true);
        IConfiguration configuration = builder.Build().GetSection("Properties");
        CheckConfig(configuration);

        var client = new MongoDbService(configuration["databaseUrl"]);
        SongPath = configuration["songsPath"];

        Application application = new ApplicationI(client);

        return application.main(args);
    }

    private static void CheckConfig(IConfiguration configuration)
    {
        string[] necessaryKeys = ["databaseUrl", "songsPath"];
        foreach (var necessaryKey in necessaryKeys)
        {
            ArgumentNullException.ThrowIfNull(configuration[necessaryKey]);
        }
    }
}