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
            Server = new XmppServer(richTextBox1);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Server.Close();
        }
    }
}
