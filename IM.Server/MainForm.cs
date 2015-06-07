using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IM.Server
{
    public partial class MainForm : Form
    {
        IM.Core.IMServer Server { get; set; }
        public MainForm()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Server = new IM.Core.IMServer();
            //注册接受消息事件
            Server.OnReciveMessage+=Server_OnReciveMessage;

            Server.OnConnectServer += Server_OnConnectServer;
            
        }

        void Server_OnConnectServer(Core.ConnectionSocket client)
        {
            listBox1.Items.Add(client.ID);
        }

        public void Server_OnReciveMessage(IM.Core.Message Message)
        {
            if (Message.MsgType == Core.MessageType.SetID)
            {
                this.richTextBox1.AppendText(Message.Sender + "上线\n");
            }
            else
            {
                this.richTextBox1.AppendText(Message.Sender+":"+Message.Content + "\n");
            }
            
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Server.Close();
        }
    }
}
