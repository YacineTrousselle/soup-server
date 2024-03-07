using Demo;
using FirstServer.service;
using Ice;
using NAudio.Wave;

namespace FirstServer.iceImpl;

public class FileTransferI : FileTransferDisp_
{
    public const string SongPath = "C:\\Users\\yacin\\Documents\\Project\\dotNet\\FirstServer\\songs";

    private Dictionary<string, Dictionary<int, byte[]>> files = new Dictionary<string, Dictionary<int, byte[]>>();

    public override void sendChunk(byte[] data, string uniqueId, int pos, Current current = null)
    {
        Console.WriteLine("Receive packet pos {0}, from {1}", pos, uniqueId);
        if (!files.ContainsKey(uniqueId))
        {
            files.Add(uniqueId, new Dictionary<int, byte[]>());
        }

        files[uniqueId][pos] = data;
    }

    public override void completeTransfer(string uniqueId, string title, Current current = null)
    {
        Console.WriteLine("Transfer completed for {0}, from {1}", title, uniqueId);

        byte[] data = ConvertDictionaryToArray(files[uniqueId]);
        using (Mp3FileReader mp3FileReader = new Mp3FileReader(new MemoryStream(data)))
        {
            WaveFileWriter.CreateWaveFile(Path.Join(SongPath, $"{title}.{Extensions.AudioExt}"), mp3FileReader);
        }

        files.Remove(uniqueId);
    }

    private byte[] ConvertDictionaryToArray(Dictionary<int, byte[]> chunks)
    {
        int size = (chunks[0].Length * (chunks.Count - 1)) + chunks[chunks.Count - 1].Length;
        byte[] result = new byte[size];
        int currentPosition = 0;

        foreach (var chunk in chunks.OrderBy(chunk => chunk.Key))
        {
            Buffer.BlockCopy(chunk.Value, 0, result, currentPosition, chunk.Value.Length);
            currentPosition += chunk.Value.Length;
        }

        return result;
    }
}