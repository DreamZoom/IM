using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace IM.Core
{
    public class ConnectionSocket
    {
        /// <summary>
        /// 对服务引用
        /// </summary>
        public IMServer Server { get; set; }

        public Socket Socket { get; set; }


        protected Thread ReciveThread { get; set; }

        public bool IsClosed { get; set; }

        public ConnectionSocket(IMServer server,Socket socket)
        {
            Server = server;
            Socket = socket;
            IsClosed = false;

            ReciveThread = new Thread(new ThreadStart(Recive));
            ReciveThread.IsBackground = true;
            ReciveThread.Start();
        }

        public void Recive()
        {
            while (!IsClosed)
            {
                try
                {
                    byte[] byteMsgRec = new byte[1024 * 1024 * 4];
                    int length = Socket.Receive(byteMsgRec, byteMsgRec.Length, SocketFlags.None);
                    if (length > 0)
                    {
                        string strMsgRec = Encoding.UTF8.GetString(byteMsgRec, 1, length - 1);
                        ProcessRecive(strMsgRec);
                    }
                }
                catch 
                {
                    Server.RemoveConnection(this);
                    this.Close();
                }
            }
        }

        public void Close()
        {
            this.IsClosed = true;
        }

        public virtual void ProcessRecive(string reciveString)
        {

        }


        public virtual void SendMessage(byte[] message)
        {
            Socket.Send(message);
        }
    }
}
