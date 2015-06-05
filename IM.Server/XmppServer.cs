using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IM.Server
{
    public class XmppServer : IM.Core.IMServer
    {
        RichTextBox richBox { get; set; }
        public XmppServer(RichTextBox console)
        {
            richBox = console;
        }

        public override void AddConnection(System.Net.Sockets.Socket socket)
        {
            XmppConnection xmppConn = new XmppConnection(this,socket);
            base.AddConnection(xmppConn);
        }

        public void Console(string msg)
        {
            richBox.AppendText(msg+'\n');
        }
    }
}
