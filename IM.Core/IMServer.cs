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

        public Thread ListenThread { get; set; }
        public Socket Listener { get; set; }

        public List<ConnectionSocket> ClientList { get; set; }

        public bool Closed { get; set; }

        public IMServer()
        {
            try
            {
                Port = 8080;
                Closed = false;
                ClientList = new List<ConnectionSocket>();
                Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Listener.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), Port));
                Listener.Listen(10);

                ListenThread = new Thread(new ThreadStart(Listen));
                ListenThread.IsBackground = true;
                ListenThread.Start();
            }
            catch(Exception err)
            {

            }

        }

        protected void Listen()
        {
            while (!Closed)
            {
                Socket socket = Listener.Accept();
                this.AddConnection(socket);
            }

        }

        public virtual void AddConnection(Socket socket)
        {
            ConnectionSocket conn = new ConnectionSocket(this, socket);
            this.ClientList.Add(conn);
        }

        public void AddConnection(ConnectionSocket conn)
        {
            this.ClientList.Add(conn);
        }

        public void RemoveConnection(ConnectionSocket conn)
        {
            this.ClientList.Remove(conn);
        }

        public void Close()
        {
            this.Closed = true;

        }
    }
}
