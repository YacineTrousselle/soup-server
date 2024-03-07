using FirstServer.model;
using MongoDB.Driver;

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
}