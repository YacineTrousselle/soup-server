using FirstServer.model;
using MongoDB.Bson;
using MongoDB.Driver;
using Soup;

namespace FirstServer.service;

public class MongoDbService
{
    private MongoClient _client;
    private const string Database = "soup";
    private const string SongCollection = "songs";

    public MongoDbService(string databaseUrl)
    {
        _client = new MongoClient(databaseUrl);
    }

    public IMongoCollection<Song> GetSongCollection()
    {
        return _client.GetDatabase(Database).GetCollection<Song>(SongCollection);
    }

    public ObjectId InsertNewSong(SongData songData)
    {
        Song song = new Song(new ObjectId(), songData.title, new List<string>(songData.artists));
        GetSongCollection().InsertOne(song);

        return song.Id;
    }
}