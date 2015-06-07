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
    public class IMServer
    {
        /// <summary>
        /// 绑定监测端口号
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 监听连接请求线程
        /// </summary>
        public Thread ListenThread { get; set; }

        /// <summary>
        /// 监听socket
        /// </summary>
        public Socket Listener { get; set; }

        /// <summary>
        /// socket 连接列表
        /// </summary>
        public List<ConnectionSocket> Clients { get; set; }

        /// <summary>
        /// 是否关闭（用于结束线程）
        /// </summary>
        public bool Closed { get; set; }

        /// <summary>
        /// 接受消息事件
        /// </summary>
        public event ReciveMessageDelegate OnReciveMessage;

        /// <summary>
        /// 连接到服务器
        /// </summary>
        public event ConnectServerDelegate OnConnectServer;

        public IMServer()
        {
            try
            {


                Port = 9090;
                Closed = false;
                Clients = new List<ConnectionSocket>();
                Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Listener.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), Port));
                Listener.Listen(10);

                ListenThread = new Thread(new ThreadStart(Listen));
                ListenThread.IsBackground = true;
                ListenThread.Start();
            }
            catch
            {

            }
        }

        protected void Listen()
        {
            while (!Closed)
            {
                Socket socket = Listener.Accept();

                var conn = this.AddConnection(socket);

            }

        }


        public void ConnectServer(ConnectionSocket clientSocket)
        {
            if (OnConnectServer != null)
            {
                OnConnectServer(clientSocket);
            }
        }

        public void ReciveMessage(Message message)
        {
            if (null != OnReciveMessage)
            {
                OnReciveMessage(message);
            }
        }

        public virtual ConnectionSocket AddConnection(Socket socket)
        {
            ConnectionSocket conn = new ConnectionSocket(this, socket);
            this.Clients.Add(conn);
            return conn;
        }

        public void AddConnection(ConnectionSocket conn)
        {
            this.Clients.Add(conn);
        }

        public void RemoveConnection(ConnectionSocket conn)
        {
            this.Clients.Remove(conn);
        }


        public ConnectionSocket GetConnection(string ID)
        {
           var socket =  Clients.FirstOrDefault(m=>m.ID==ID);
           return socket;
        }

        public void SendMessage(Message message)
        {
            var toUser = GetConnection(message.Reciver);
            toUser.SendMessage(message);
        }

        public void Close()
        {
            this.Closed = true;

        }
    }
}
