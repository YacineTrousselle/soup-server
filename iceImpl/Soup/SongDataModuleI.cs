using FirstServer.model;
using FirstServer.service;
using Ice;
using MongoDB.Bson;
using MongoDB.Driver;
using Soup;

namespace FirstServer.iceImpl.Soup;

public class SongDataModuleI: SongDataModuleDisp_
{
    private MongoDbService _mongoDbService;
    
    public SongDataModuleI(MongoDbService mongoDbService)
    {
        _mongoDbService = mongoDbService;
    }
    
    public override SongData[] searchByTitle(string search, Current current = null)
    {
        var filter = Builders<Song>.Filter.Regex("Title", new BsonRegularExpression(search, "i"));
        var songs = _mongoDbService.GetSongCollection().Find(filter).ToList();
        var songDataList = new List<SongData>();
        
        foreach (var song in songs)
        {
            var songData = new SongData
            {
                id = song.Id.ToString(),
                title = song.Title,
                artists = song.Artists.ToArray(),
            };
            songDataList.Add(songData);
        }

        Console.WriteLine("Search by title " + songDataList.ToArray());
        
        return songDataList.ToArray();
    }

    public override SongData[] searchByArtist(string search, Current current = null)
    {
        var filter = Builders<Song>.Filter.AnyEq("Artists", search);
        var songs = _mongoDbService.GetSongCollection().Find(filter).ToList();
        var songDataList = new List<SongData>();

        foreach (var song in songs)
        {
            var songData = new SongData
            {
                id = song.Id.ToString(),
                title = song.Title,
                artists = song.Artists.ToArray(),
            };
            songDataList.Add(songData);
        }

        return songDataList.ToArray();
    }

    public override void updateSong(SongData songData, Current current = null)
    {
        var filter = Builders<Song>.Filter.Eq("_id", ObjectId.Parse(songData.id));
        var update = Builders<Song>.Update.Set("Title", songData.title).Set("Artists", songData.artists.ToList());
        _mongoDbService.GetSongCollection().UpdateOne(filter, update);
    }

    public override void deleteSong(string songId, Current current = null)
    {
        var filter = Builders<Song>.Filter.Eq("_id", ObjectId.Parse(songId));
        _mongoDbService.GetSongCollection().DeleteOne(filter);
        File.Delete(Path.Join(Program.SongPath, $"{songId}.{Extensions.AudioExt}"));
    }
}