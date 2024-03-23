using FirstServer.model;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Clusters;
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
        _client.ListDatabases();
        if (_client.Cluster.Description.State != ClusterState.Connected)
        {
            throw new Exception("I think the database isn't up. Try after turn it on");
        }
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

    public Song? FindSongById(string songId)
    {
        var filter = Builders<Song>.Filter.Eq("_id", ObjectId.Parse(songId));

        return GetSongCollection().Find(filter).FirstOrDefault();
    }
}