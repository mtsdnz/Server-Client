using System.Net.Sockets;
using System.Text;
using server.Class.Manager;
namespace server.Commands
{
    public class IOFunctions
    {
        public static void ReadFile(string directory, TcpClient c)
        {
            Packet sp = new Packet();
            sp.Type = (byte)PacketType.CONNECTED;
            byte[] sbuff = Encoding.UTF8.GetBytes("readfile|" + directory);
            sp.Length = sbuff.Length;
            sp.Data = sbuff;
            sp.Send(c);
        }
        public static void Listdir(string path, TcpClient c)
        {
            Packet sp = new Packet();
            sp.Type = (byte)PacketType.CONNECTED;
            byte[] sbuff = Encoding.ASCII.GetBytes("listdir|" + path);
            sp.Length = sbuff.Length;
            sp.Data = sbuff;
            sp.Send(c);
        }
    }
}
