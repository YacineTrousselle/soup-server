using Ice;
using Soup;

namespace FirstServer.iceImpl.Soup;

public class FileDownloaderI: FileDownloaderDisp_
{
    public override void startDownload(SongData songData, Current current = null)
    {
        throw new NotImplementedException();
    }

    public override void sendPacket(byte[] data, int pos, Current current = null)
    {
        throw new NotImplementedException();
    }

    public override void endDownload(Current current = null)
    {
        throw new NotImplementedException();
    }
}