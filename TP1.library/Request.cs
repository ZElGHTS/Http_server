using System.Net.Sockets;
using System.Text;

namespace TP1.library
{
    class Request
    {
        private byte[] _buffer;
        private readonly Socket _socket;

        public byte[] Data
        {
            get { return _buffer; }
            set { _buffer = value; }
        }

        public Request(Socket socket)
        {
            _socket = socket;
        }

        public void ReceiveRequest(Header headers)
        {
            using var stream = new NetworkStream(_socket);
            Data = new byte[_socket.Available]; 
            stream.Read(Data, 0, Data.Length);
            var requestMessage = Encoding.UTF8.GetString(Data);
            
            headers.SplitHeaders(requestMessage);
        }
    }
}
