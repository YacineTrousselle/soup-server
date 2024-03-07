module Demo
{
    sequence<byte> Bytes;
    sequence<string> Strings;

    interface FileTransfer
    {
        void sendChunk(Bytes data, string uniqueId, int pos);
        void completeTransfer(string uniqueId, string title);
    };

    interface DataReceiver 
    {
        void receiveChunk(Bytes data, string uniqueId, int pos);
        void completeTransfer(string uniqueId);
    };

    interface FileSender 
    {
        void sendFile(DataReceiver* proxy, string title);
    };

    interface SearchSongs 
    {
         Strings search(string search);
    };
    
    struct SongData {
        string title;
        Strings artists;
        int filesize;
    };
    
    interface Test {
        void startDownload(SongData songData);
        void sendPacket(Bytes data, int pos);
        void endDownload();
    };
};
