using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using server.Class;
using server.Class.Initializer;
using server.Class.Config;
using server.Commands;
using server.Class.Manager;
using server.Class.Functions;
namespace server
{
    class Program
    {

        static List<string> clients = new List<string>();
        static List<TcpClient> clientestcp = new List<TcpClient>();
        static bool serverStarted = false;
        private static void cmd()
        {
            while (true)
            {
                string[] serverInfo = Console.ReadLine().Split(' ');
                if (serverInfo[0] == "server_start" && serverStarted == false) //parametro
                {
                    if (serverInfo.Length >= 3 || serverInfo.Length <= 1)
                        WriteManager.wl("Some parameters are empty.");
                    else
                    {
                        Console.Clear();
                        ServerStart st = new ServerStart(IPAddress.Parse(serverInfo[1]), int.Parse(serverInfo[2]));
                        serverStarted = true;
                    }
                    
                }
                else if (serverInfo[0] == "--help")
                    WriteManager.wl("commands soon");
                else if (serverInfo[0] == "server_default" && serverStarted == false) //load the default configs
                {
                    Console.Clear();
                    ServerStart st = new ServerStart(ServerDefaultConfig.IP, ServerDefaultConfig.port);
                    serverStarted = true;
                }else if(serverInfo[0] == "clients_list" && serverStarted == true) //load the list of connected clients
                {
                   // MClients.GetConnectedClients(clientsName);
                }
                else
                    WriteManager.wl("Unknow command");
            }
        }
        
        
        //public static TcpListener listener;
        static void Main(string[] args)
        {
            Console.Title = "Server"; //window title
            Console.WindowWidth = 110; //window width
            Console.WindowHeight = 42; //windot heigth
            
            WriteManager.wl("\n>> Start the server with the command: server_start <ip> <port>");
            WriteManager.wl("\n>> If you want start the server on localhost, write the command: server_default, the server will be started on port 8881");
            WriteManager.wl("\n>> Write --help to view all commands or exit, to close the server! ", ConsoleColor.DarkGray);
            new Thread(cmd).Start();
            while (true)
            {
                if (!serverStarted)
                    continue;

                TcpClient client = ServerStart.Listener.AcceptTcpClient();
                clients.Add(client.Client.RemoteEndPoint.ToString());
                clientestcp.Add(client);
                WriteManager.wl("\n>> Client Connected (" + client.Client.RemoteEndPoint.ToString() + ")", ConsoleColor.White);

                new Thread(new ParameterizedThreadStart((o) =>
                {
                    TcpClient c = (TcpClient)o;

                    while (true)
                    {
                        try
                        {
                            //NetworkStream ns = c.GetStream();
                            byte[] rbuff = new byte[ServerDefaultConfig.MAX_PACKET_SIZE];
                            int recvlen = c.Client.Receive(rbuff);

                            if (recvlen == 0)
                            {
                                Console.WriteLine(">> Client disconnected");
                                clients.RemoveAll(cstr => cstr == c.Client.RemoteEndPoint.ToString());
                                break;
                            }

                            Packet p = Packet.Deserialize(rbuff);
                            PacketType type = (PacketType)p.Type;

                            switch (type)
                            {
                                /* REQ */
                                case PacketType.REQ_CONNECTED:
                                    {
                                        string connectedClients = String.Join("|", clients);
                                        Packet sp = new Packet();
                                        sp.Type = (byte)PacketType.CONNECTED;
                                        byte[] sbuff = Encoding.UTF8.GetBytes(connectedClients);
                                        sp.Length = sbuff.Length;
                                        sp.Data = sbuff;
                                        sp.Send(c);
                                    }
                                    break;

                                case PacketType.STRING:
                                    {
                                       // Console.WriteLine("loko");
                                        string msg = Encoding.UTF8.GetString(p.Data, 0, p.Length);
                                        WriteManager.wl("String from " + c.Client.RemoteEndPoint);
                                        WriteManager.wl(msg, ConsoleColor.Yellow);
                                    }
                                    break;

                                case PacketType.CLIENT_INFO:
                                    {
                                        string clientInfo = Encoding.UTF8.GetString(p.Data, 0, p.Length);
                                        WriteManager.wl("\n ----Client Infos: ---- ", ConsoleColor.White);
                                        WriteManager.wl("   PC Name:", ConsoleColor.White);
                                        WriteManager.w("     " + clientInfo.Split('|')[0], ConsoleColor.Yellow);
                                        WriteManager.wl("\n   File location: ", ConsoleColor.White);
                                        WriteManager.w("     " + clientInfo.Split('|')[1], ConsoleColor.Yellow);
                                        WriteManager.wl("\n ---- End ----", ConsoleColor.White);
                                        //clientsName.Add(clientInfo.Split('|')[0]);
                                    }
                                    break;
                                case PacketType.IMAGE:
                                    {

                                    }
                                    break;

                                case PacketType.LIST_DIR:
                                    {
                                        WriteManager.wl("Directories from " + c.Client.RemoteEndPoint);
                                        string dirsource = Encoding.ASCII.GetString(p.Data, 0, p.Length);
                                        if (dirsource == "FileNotExists")
                                            WriteManager.wl("Directory not found!", ConsoleColor.Red);
                                        else
                                        {
                                            string[] dirsplit = dirsource.Split('|');

                                            string maindir = dirsplit[0];
                                            WriteManager.wl("\t" + maindir, ConsoleColor.White);

                                            for (int i = 1; i < dirsplit.Length; i++)
                                                WriteManager.wl("\t" + dirsplit[i], ConsoleColor.Yellow);
                                        }
                                    }
                                    break;
                            }
                            string[] infos_to_list = Console.ReadLine().Split(' ');
                            IOFunctions.Listdir(infos_to_list[1], c);
                        }

                        catch(Exception er)
                        {
                            clients.RemoveAll(cstr => cstr == c.Client.RemoteEndPoint.ToString());
                            clientestcp.Remove(client);
                            Console.WriteLine(er.Message);
                            break;
                        }

                    }

                })).Start(client);
            }
        }
    }
}
