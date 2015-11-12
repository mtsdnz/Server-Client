using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Commands
{
        public enum PacketType : byte
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
}
