using Ice;
using Soup;

namespace FirstServer.iceImpl.Soup;

public class SongDataModuleI: SongDataModuleDisp_
{
    public override SongData[] searchByTitle(string search, Current current = null)
    {
        throw new NotImplementedException();
    }

    public override SongData[] searchByArtist(string search, Current current = null)
    {
        throw new NotImplementedException();
    }

    public override void updateSong(SongData songData, Current current = null)
    {
        throw new NotImplementedException();
    }

    public override void deleteSong(int songId, Current current = null)
    {
        throw new NotImplementedException();
    }
}