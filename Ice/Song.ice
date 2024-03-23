module Soup
{
    const int ChunkSize = 65536;
    sequence<byte> Bytes;
    sequence<string> Strings;
    
    struct SongData {
        string id = null;
        string title;
        Strings artists;
        int filesize = 0;
    };

    sequence<SongData> Songs;

    interface FileUploader
    {
        string startUpload(SongData songData);
        void sendChunk(Bytes data, string uniqueId, int pos);
        void completeUpload(string uniqueId);
    };

    interface FileSender
    {
        SongData getSongData(string songId);
        Bytes getChunk(string songId, int pos);
    };

    interface SongDataModule 
    {
         Songs searchByTitle(string search);
         Songs searchByArtist(string search);
         void updateSong(SongData songData);
         void deleteSong(string songId);
    };

    interface AudioPlayer {
        string initializeAudioPlayer(string songId);
        void play(string rtspUrl);
        void pause(string rtspUrl);
        void close(string rtspUrl);
    };
};
