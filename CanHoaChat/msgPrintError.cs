using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CanHoaChat
{
    public partial class msgPrintError : Form
    {
        public msgPrintError(string text)
        {
            InitializeComponent();
            label1.Text = text;
        }

        private void btCan_Click(object sender, EventArgs e)
        {
            Error.PrintError = "OK";
            this.Close();
        }

        private void msgPrintError_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
