using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Core
{
    /// <summary>
    /// 接受消息处理程序
    /// </summary>
    /// <param name="message"></param>
    public delegate void ReciveMessageDelegate(Message message);

    /// <summary>
    /// 客户端连接处理程序
    /// </summary>
    /// <param name="client"></param>
    public delegate void ConnectServerDelegate(ConnectionSocket client);


    /// <summary>
    /// 客户端断开连接处理程序
    /// </summary>
    /// <param name="client"></param>
    public delegate void DisconnectServerDelegate(ConnectionSocket client);



    public delegate void ClientConnectComplate(ClientSocket clientSocket);
}
