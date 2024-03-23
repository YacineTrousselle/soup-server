using Demo;
using FirstServer.service;
using Ice;
using Exception = System.Exception;

namespace FirstServer.iceImpl;

public class FileSenderI: FileSenderDisp_
{
    private const int ChunkSize = 1024;

    public override void sendFile(DataReceiverPrx proxy, string title, Current current = null)
    {
        DataReceiverPrx dataReceiverProxy = (DataReceiverPrx) proxy.ice_batchOneway();
        var path = Path.Join(FileTransferI.SongPath, $"{title}.{Extensions.AudioExt}");
        if (!File.Exists(path))
        {
            throw new Exception("File not found");
        }
    
        string uniqueId = Guid.NewGuid().ToString();
        var data = File.ReadAllBytes(path);
        for (int offset = 0, i = 0; offset < data.Length; offset += ChunkSize, i++)
        {
            int length = Math.Min(ChunkSize, data.Length - offset);
            byte[] chunk = data.Slice(offset, length);
            dataReceiverProxy.receiveChunk(chunk, uniqueId, i);
        }
    
        dataReceiverProxy.completeTransfer(uniqueId);
        dataReceiverProxy.ice_flushBatchRequests();
    }
}
