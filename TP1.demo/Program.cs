using TP1.library;

namespace TP1.Console
{
    class Program
    {

        static void Main(string[] args)
        {
            Server server = new Server(8080);
            server.Start();
        }
    }
}
