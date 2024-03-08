using Demo;
using FirstServer.service;
using Ice;

namespace FirstServer.iceImpl;

public class SearchSongsI : SearchSongsDisp_
{
    public override string[] search(string search, Current current = null)
    {
        return Directory.GetFiles(FileTransferI.SongPath, $"*.{Extensions.AudioExt}")
            .Select(Path.GetFileNameWithoutExtension)
            .Where(s => s != null && Path.GetFileNameWithoutExtension(s).Contains(search))
            .ToArray()!;
    }
}