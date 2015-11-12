using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using server.Commands;

namespace server.Class.Manager
{
        public struct Packet
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
}
