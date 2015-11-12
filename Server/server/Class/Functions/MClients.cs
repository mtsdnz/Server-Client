using server.Class.Manager;
using server.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace server.Class.Functions
{
    public class MClients
    {
        public static void SendToClient(string ClientName, string content, TcpClient c)
        {
            if (ClientName != string.Empty || content != string.Empty)
            {
                Packet sp = new Packet();
                sp.Type = (byte)PacketType.CONNECTED;
                byte[] sbuff = Encoding.ASCII.GetBytes(ClientName + "|" + content);
                sp.Length = sbuff.Length;
                sp.Data = sbuff;
                sp.Send(c);
            }
        }

        public static void GetConnectedClients(List<string> clients)
        {
            foreach(var clientname in clients)
            {
                WriteManager.wl(clientname, ConsoleColor.DarkGray);
            }
        }
    }
}
