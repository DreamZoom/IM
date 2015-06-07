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
    public class ClientSocket
    {
        /// <summary>
        /// 客户端socket
        /// </summary>
        public Socket Socket { get; set; }

        /// <summary>
        /// 远程IP地址
        /// </summary>
        public IPAddress ServerAdderss { get; set; }

        /// <summary>
        /// 连接远程端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 消息接受线程
        /// </summary>
        public Thread ReciveThread { get; set; }

        /// <summary>
        /// 是否关闭
        /// </summary>
        public bool Closed { get; set; }

        public string ID { get; set; }

        /// <summary>
        /// 接受到消息
        /// </summary>
        public event ReciveMessageDelegate OnReciveMessage;

        public event ClientConnectComplate OnConnectComplate;


        public ClientSocket(IPAddress serverIP, int port)
        {
            this.ServerAdderss = serverIP;
            this.Port = port;
            this.Closed = false;
            ID = "";

            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //Socket.Connect(new IPEndPoint(serverIP, port));
            Socket.BeginConnect(new IPEndPoint(serverIP, port), new AsyncCallback(ConnectionCallBack), Socket);

            ReciveThread = new Thread(new ThreadStart(Recive));
            ReciveThread.IsBackground = true;

        }

        private void ConnectionCallBack(IAsyncResult ar)
        {
            try
            {
                Socket s = (Socket)ar.AsyncState;
                s.EndConnect(ar);
                ReciveThread.Start();

                if (OnConnectComplate != null)
                {
                    OnConnectComplate(this);
                }
            }
            catch (Exception ex)
            {
                
            }

        }

        public void Recive()
        {
            while (!Closed)
            {
                byte[] msgByte = new byte[1024 * 1024 * 4];
                int length = 0;
                try
                {
                    length = Socket.Receive(msgByte, msgByte.Length, SocketFlags.None);
                    if (length > 0)
                    {
                        string strMsgRec = Encoding.UTF8.GetString(msgByte, 1, length - 1);
                        Message message = MessageHelper.Object(strMsgRec);
                        if (null != OnReciveMessage)
                        {
                            OnReciveMessage(message);
                        }
                    }
                }
                catch
                {

                }
            }
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
            string str= MessageHelper.Json(message);
            SendMessage(str);
        }

        public void SendTextMessage(string user,string text)
        {
            Message message = new Message();
            message.Content = text;
            message.Sender = this.ID;
            message.Reciver = user;
            message.MsgType =MessageType.P2P;
            message.ContentType = "";
            SendMessage(message);
        }

        public virtual void SetID(string Id)
        {
            this.ID = Id;
            Message msg = new Message()
            {
                Sender = Id,
                ContentType = "",
                Content = "设置ID",
                MsgType = MessageType.SetID,
                Reciver = "server"
            };
            SendMessage(msg);
        }
    }
}
