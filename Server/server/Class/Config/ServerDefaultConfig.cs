using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
namespace server.Class.Config
{
    public class ServerDefaultConfig
    {
        //by darkhero
        //this infos will be used, if you dont specific any ip, and port on command server_start
        public static IPAddress IP = IPAddress.Parse("127.0.0.1"); //default ip
        public static int port = 8881; //default port
        public const int MAX_PACKET_SIZE = 2048; //max packet size
    }
}
