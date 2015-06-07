using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace IM.Client
{
    public partial class ClientForm : Form
    {
        public IM.Core.ClientSocket ClientSocket { get; set; }
        public ClientForm()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
           
        }

        private void ClientSocket_OnReciveMessage(IM.Core.Message message)
        {
            this.richTextBox1.AppendText(message.Content+"\n");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClientSocket.SendTextMessage(textBox3.Text.Trim(), textBox1.Text.Trim());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("请输入用户名");
                return;
            }
            ClientSocket = new Core.ClientSocket(IPAddress.Parse("127.0.0.1"), 9090);
            ClientSocket.OnReciveMessage +=ClientSocket_OnReciveMessage;
            ClientSocket.OnConnectComplate += ClientSocket_OnConnectComplate;
        }

        private void ClientSocket_OnConnectComplate(Core.ClientSocket clientSocket)
        {
            string username = textBox2.Text.Trim();
            clientSocket.SetID(username);
        }
    }
}
