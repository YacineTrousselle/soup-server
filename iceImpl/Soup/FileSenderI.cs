using Ice;
using Soup;

namespace FirstServer.iceImpl.Soup;

public class FileSenderI: FileSenderDisp_
{
    public override void sendFile(FileDownloaderPrx proxy, string title, Current current = null)
    {
        throw new NotImplementedException();
    }
}