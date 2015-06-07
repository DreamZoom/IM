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
    /// <summary>
    /// 客户端连接处理类
    /// </summary>
    public class ConnectionSocket
    {
        /// <summary>
        /// 对服务引用
        /// </summary>
        public IMServer Server { get; set; }

        /// <summary>
        /// 用于与客户端连接的Socket
        /// </summary>
        public Socket Socket { get; set; }

        /// <summary>
        /// 接受消息线程
        /// </summary>
        protected Thread ReciveThread { get; set; }

        /// <summary>
        /// 用于结束线程
        /// </summary>
        public bool Closed { get; set; }

        /// <summary>
        /// 接受到消息
        /// </summary>
        public event ReciveMessageDelegate OnReciveMessage;

        /// <summary>
        /// 客户端ID
        /// </summary>
        public string ID { get; set; }

        public ConnectionSocket(IMServer server, Socket socket)
        {
            Server = server;
            Socket = socket;
            Closed = false;

            ReciveThread = new Thread(new ThreadStart(Recive));
            ReciveThread.IsBackground = true;
            ReciveThread.Start();
        }

        public void Recive()
        {
            while (!Closed)
            {
                try
                {
                    byte[] byteMsgRec = new byte[1024 * 1024 * 4];
                    int length = Socket.Receive(byteMsgRec, byteMsgRec.Length, SocketFlags.None);
                    if (length > 0)
                    {
                        string strMsgRec = Encoding.UTF8.GetString(byteMsgRec, 1, length - 1);
                        Message message = MessageHelper.Object(strMsgRec);
                        ProcessMessage(message);
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
            this.Closed = true;
            ReciveThread.Abort();
        }

        protected void ProcessMessage(Message message)
        {
            if (message.MsgType == MessageType.SetID)
            {
                ProcSetID(message);
            }
            else if (message.MsgType == MessageType.P2P)
            {
                this.Server.SendMessage(message);
            }
            if (null != OnReciveMessage)
            {
                OnReciveMessage(message);
            }
            Server.ReciveMessage(message);
        }


        public void ProcSetID(Message message)
        {
            this.ID = message.Sender;
            Server.ConnectServer(this);
        }

        public virtual void SendMessage(byte[] message)
        {
            if (Closed) return;
            Socket.Send(message);
        }

        public virtual void SendMessage(string message)
        {
            byte[] msgSendByte = Encoding.UTF8.GetBytes(message);
            byte[] finalByte = new byte[msgSendByte.Length + 1];
            finalByte[0] = 0;
            Buffer.BlockCopy(msgSendByte, 0, finalByte, 1, msgSendByte.Length);
            SendMessage(finalByte);
        }
        public virtual void SendMessage(Message message)
        {
            string str = MessageHelper.Json(message);
            SendMessage(str);
        }

        public void SendTextMessage(string text)
        {
            Message message = new Message();
            message.Content = text;
            message.Sender = "Server";
            message.Reciver = "Server";
            message.MsgType = MessageType.Server;
            message.ContentType = "";
            SendMessage(message);
        }
    }
}
