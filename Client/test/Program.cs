using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace test
{
    class Program
    {
        const int MAX_PACKET_SIZE = 2048;

        enum PacketType : byte
        {
            REQ_CONNECTED,
            CONNECTED,

            REQ_STRING,
            STRING,

            REQ_IMAGE,
            IMAGE,

            REQ_LIST_DIR,
            LIST_DIR,

            REQ_READ_FILE,
            READ_FILE,

            REQ_DELETE_FILE,
            DELETE_FILE,

            REQ_ADD_FILE,
            ADD_FILE,

            REQ_CLIENT_INFO,
            CLIENT_INFO
        }

        struct Packet
        {
            public byte Type { get; set; }
            public int Length { get; set; }
            public byte[] Data { get; set; }

            public void Send(TcpClient client)
            {

                NetworkStream ns = client.GetStream();
                byte[] sendbuff = new byte[1 + 4 + Data.Length];
                sendbuff[0] = Type;
                byte[] szBuff = BitConverter.GetBytes(Length);
                for (int i = 0; i < 4; i++)
                    sendbuff[i + 1] = szBuff[i];

                for (int i = 0; i < Data.Length; i++)
                    sendbuff[i + 5] = Data[i];

                ns.Write(sendbuff, 0, sendbuff.Length);

                ns.Flush();
            }
            public static Packet Deserialize(byte[] source)
            {
                Packet packet = new Packet();
                packet.Type = source[0];
                byte[] szBuff = new byte[4];
                for (int i = 0; i < 4; i++)
                    szBuff[i] = source[i + 1];
                int len = BitConverter.ToInt32(szBuff, 0);
                packet.Length = len;
                byte[] dataBuff = new byte[len];

                for (int i = 4; i < (len + 4); i++)
                    dataBuff[i - 4] = source[i + 1];

                packet.Data = dataBuff;
                PacketType type = (PacketType)packet.Type;
                return packet;
            }
        }
        public static TcpClient client = new TcpClient();
        public static void Main()
        {
            try
            {
                
                client.Connect(IPAddress.Parse("127.0.0.1"), 8881);

                new Thread(new ParameterizedThreadStart(on_receive)).Start(client);

                NetworkStream ns = client.GetStream();
                Packet p = new Packet();
                p.Type = (byte)PacketType.CLIENT_INFO;
                p.Data = System.Text.Encoding.ASCII.GetBytes(Environment.UserName + "|" + Environment.CurrentDirectory);
                p.Length = p.Data.Length;
                p.Send(client);
                Console.ReadLine();
            }
            catch
            {

            }
        }



        static void on_receive(object o)
        {
            TcpClient c = (TcpClient)o;
            while(true)
            {
                NetworkStream ns = c.GetStream();
                byte[] buff = new byte[MAX_PACKET_SIZE];
                ns.Read(buff, 0, MAX_PACKET_SIZE);
                Packet rp = Packet.Deserialize(buff);

                PacketType type = (PacketType)rp.Type;

                if(type == PacketType.CONNECTED)
                {
                    string[] connectedclients = Encoding.UTF8.GetString(rp.Data, 0, rp.Length).Split('|');
                    Console.WriteLine("Command receive > " + connectedclients[0]);
                    if(connectedclients[0] == "listdir")
                    {
                        NetworkStream nss = client.GetStream();
                        Packet p = new Packet();
                        string paths = string.Join("|", System.IO.Directory.GetDirectories(connectedclients[1]));
                        p.Type = (byte)PacketType.LIST_DIR;
                        if (System.IO.Directory.Exists(connectedclients[1]))
                            p.Data = System.Text.Encoding.ASCII.GetBytes(paths);
                        else
                            p.Data = System.Text.Encoding.ASCII.GetBytes("FileNotExists");
                        p.Length = p.Data.Length;
                        p.Send(client);
                    }
                }
                else
                {
                    Console.WriteLine("Server off");
                    break;
                }
            }
        }
    }
}
