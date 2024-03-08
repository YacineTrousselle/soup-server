module Soup
{
    sequence<byte> Bytes;
    sequence<string> Strings;
    
    struct SongData {
        string title;
        Strings artists;
        int filesize;
    };

    sequence<SongData> Songs;

    interface FileUploader
    {
        string startUpload(SongData songData);
        void sendChunk(Bytes data, string uniqueId, int pos);
        void completeUpload(string uniqueId);
    };

    interface FileDownloader 
    {
        void startDownload(SongData songData);
        void sendPacket(Bytes data, int pos);
        void endDownload();
    };

    interface FileSender
    {
        void sendFile(FileDownloader* proxy, string songId);
    };

    interface SongDataModule 
    {
         Songs searchByTitle(string search);
         Songs searchByArtist(string search);
         void updateSong(SongData songData);
         void deleteSong(string songId);
    }; 
};
