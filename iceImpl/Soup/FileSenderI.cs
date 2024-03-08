using FirstServer.model;
using FirstServer.service;
using Ice;
using MongoDB.Driver;
using Soup;

namespace FirstServer.iceImpl.Soup;

public class FileSenderI : FileSenderDisp_
{
    private const int ChunkSize = 1024;
    private MongoDbService _mongoDbService;

    public FileSenderI(MongoDbService mongoDbService)
    {
        _mongoDbService = mongoDbService;
    }

    public override void sendFile(FileDownloaderPrx proxy, string songId, Current current = null)
    {
        Song song = _mongoDbService.GetSongCollection()
            .Find(Builders<Song>.Filter.Eq("_id", songId))
            .First();
        ArgumentNullException.ThrowIfNull(song);
        byte[] data = File.ReadAllBytes(Path.Join(Program.SongPath, $"{songId}.mp3"));

        SongData songData = new SongData
        {
            title = song.Title,
            artists = song.Artists.ToArray(),
            filesize = data.Length
        };

        FileDownloaderPrx fileDownloaderPrx = (FileDownloaderPrx)proxy.ice_batchOneway();
        fileDownloaderPrx.startDownload(songData);
        for (int offset = 0, i = 0; offset < data.Length; offset += ChunkSize, i++)
        {
            int length = Math.Min(ChunkSize, data.Length - offset);
            byte[] chunk = data.Slice(offset, length);
            fileDownloaderPrx.sendPacket(chunk, i);
        }
        fileDownloaderPrx.endDownload();
        fileDownloaderPrx.ice_flushBatchRequests();
    }
}