using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
namespace IM.Server
{
    public class XmppConnection : IM.Core.ConnectionSocket
    {
        public XmppConnection(IM.Core.IMServer server,Socket socket)
            :base(server,socket)
        {

        }
        public override void ProcessRecive(string reciveString)
        {
            var server = (XmppServer)this.Server;
            server.Console(reciveString);
            base.ProcessRecive(reciveString);
        }
    }
}
