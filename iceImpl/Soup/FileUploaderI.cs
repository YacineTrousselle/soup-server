using Ice;
using Soup;

namespace FirstServer.iceImpl.Soup;

public class FileUploaderI: FileUploaderDisp_
{
    public override void startUpload(SongData songData, Current current = null)
    {
        throw new NotImplementedException();
    }

    public override void sendChunk(byte[] data, string uniqueId, int pos, Current current = null)
    {
        throw new NotImplementedException();
    }

    public override void completeUpload(string uniqueId, string title, Current current = null)
    {
        throw new NotImplementedException();
    }
}