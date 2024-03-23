
module Streaming {

    const int ChunkSize = 65536;

    sequence<byte> Packet;

    interface StreamCallback {
        void receiveData(Packet packet);
    };

    interface StreamServer {
        void pushStream(StreamCallback* callback);
    };
    
    interface AudioService {
        void send(string songId);
    };

};
