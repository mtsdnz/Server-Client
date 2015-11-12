using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using server.Class;
using server.Class.Config;
namespace server.Class.Initializer
{
    public class ServerStart
    {
        public static TcpListener Listener;
        public static IPAddress IP;
        public static int Port;
        public ServerStart(IPAddress ip, int port) //class to start the server
        {
            try
            {
                if (ip.ToString() != string.Empty || port.ToString() != string.Empty)
                {
                    Listener = new TcpListener(ip, port);
                    Listener.Start();
                    IP = ip;
                    Port = port;
                    WriteManager.wl(">> Server Started Sucessfully!", ConsoleColor.Green);
                    WriteManager.wl(">> Server IP: ", ConsoleColor.White);
                    WriteManager.w("      " + IP, ConsoleColor.Yellow);
                    WriteManager.wl("\n>> Server Port: ", ConsoleColor.White);
                    WriteManager.w("       " + Port.ToString(), ConsoleColor.Yellow);
                    WriteManager.wl("\n>> Waiting from a client connection...", ConsoleColor.White);
                }
                else
                    WriteManager.wl("Some parameters are empty.", ConsoleColor.Red);
            }
            catch(Exception err)
            {
                WriteManager.wl("Could not start the server, error: \n" + err.Message, ConsoleColor.Red);
            }
        }
    }
}
