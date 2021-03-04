using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace TP1.library
{
    class Response
    {

        private readonly StringBuilder _response;
        private readonly Socket _socket;

        public Response(Socket socket)
        {
            _response = new StringBuilder();
            _socket = socket;
        }

        public void StartResponse(Header headers)
        {
            if(HandleError(headers.Path)) return;
            CreateResponse(false, 200, "OK", headers.Path);
        }

        private bool HandleError(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                CreateResponse(true, 400, "Bad Request", path);
                return true;
            }
            if (!File.Exists(path))
            {
                CreateResponse(true, 404, "Not found", path);
                return true;
            }
            return false;
        }

        private void CreateResponse(bool invalid, int statusCode, string status, string path)
        {
            if (invalid)
            {
                _response.AppendLine($"HTTP/1.1 {statusCode} {status}");
                _response.AppendLine();
                _socket.Send(Encoding.UTF8.GetBytes(_response.ToString()));
                return;
            }
            
            _response.AppendLine($"HTTP/1.1 {statusCode} {status}");
            _response.AppendLine(GetContentType(path));
            _response.AppendLine("Accept: */*");

            _response.AppendLine();
            
            _socket.Send(GetByteContent(path));
        }

        private static string GetContentType(string path)
        {
            if (path.Contains(".ico"))
            {
                return "Content-Type: image/x-icon";
            }
            if (path.Contains(".css"))
            {
                return "Content-Type: text/css; charset=utf-8";
            }
            if (path.Contains(".js"))
            {
                return "Content-Type: application/javascript";
            }

            return "Content-Type: text/html; charset=utf-8";
        }

        private byte[] GetByteContent(string path)
        {
            var responseArray = new byte[Encoding.UTF8.GetBytes(_response.ToString()).Length + File.ReadAllBytes(path).Length];
            Encoding.UTF8.GetBytes(_response.ToString()).CopyTo(responseArray, 0);
            File.ReadAllBytes(path).CopyTo(responseArray, Encoding.UTF8.GetBytes(_response.ToString()).Length);
            return responseArray;
        }
    }
}
