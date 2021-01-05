using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace CanHoaChat
{
    public partial class msg : Form
    {
        public msg(string s)
        {
            InitializeComponent();
            content = s;
        }

        string content = "";
        private void msg_Load(object sender, EventArgs e)
        {
            label1.Text = content;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void msg_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }
    }
}
