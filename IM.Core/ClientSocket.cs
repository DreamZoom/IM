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
        public Socket Socket { get; set; }

        public IPAddress ServerIPAdderss { get; set; }

        public int Port { get; set; }

        public Thread ReciveThread { get; set; }

        public bool Closed { get; set; }

        public ClientSocket(IPAddress serverIP, int port)
        {
            this.ServerIPAdderss = serverIP;
            this.Port = port;
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket.Connect(new IPEndPoint(serverIP, port));
            ReciveThread = new Thread(new ThreadStart(Recive));
            ReciveThread.IsBackground = true;
        }

        public void Recive()
        {
            while (!Closed)
            {
                byte[] msgByte = new byte[1024 * 1024 * 2];
                int length = 0;
                try
                {
                    length = Socket.Receive(msgByte, msgByte.Length, SocketFlags.None);
                    if (length > 0)
                    {
                        ProcessRecive(msgByte);

                    }
                }
                catch
                {

                }
            }
        }


        public virtual void ProcessRecive(byte[] stream)
        {
            if (stream[0] == 0)//接受文字
            {

            }
            else if (stream[0] == 1)//接受文件
            {

            }
            else//抖动窗体
            {

            }
        }


        public virtual void SendMessage(byte[] message)
        {
            Socket.Send(message);
        }
    }
}
