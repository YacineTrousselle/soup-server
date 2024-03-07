using MongoDB.Bson;

namespace FirstServer.model;

public class Song(ObjectId id, string title, List<string> artists)
{
    public ObjectId Id
    {
        get => id;
        set => id = value;
    }

    public string Title
    {
        get => title;
        set => title = value ?? throw new ArgumentNullException(nameof(value));
    }

    public List<string> Artists
    {
        get => artists;
        set => artists = value ?? throw new ArgumentNullException(nameof(value));
    }

    public void AddArtists(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        Artists.Add(value);
    }

    public void RemoveArtist(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (Artists.Contains(value))
        {
            Artists.Remove(value);
        }
    }
}