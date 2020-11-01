using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using ServerLib;


namespace AsyncTcp
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server(IPAddress.Parse("127.0.0.1"), 2048);
            server.Start();
        }
    }
}
