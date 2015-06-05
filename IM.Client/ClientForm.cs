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
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            ClientSocket = new Core.ClientSocket(IPAddress.Parse("127.0.0.1"), 8080);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string msgSend = textBox1 .Text.Trim();
            byte[] orgByte = Encoding.UTF8.GetBytes(msgSend);
            byte[] finalByte = new byte[orgByte.Length + 1];
            finalByte[0] = 0;
            Buffer.BlockCopy(orgByte, 0, finalByte, 1, orgByte.Length);
            ClientSocket.SendMessage(finalByte);
        }
    }
}
