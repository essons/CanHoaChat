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
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        static Form1 _instance;
        public static Form1 Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Form1();
                return _instance;
            }
        }

        public MetroFramework.Controls.MetroPanel MetroContainer
        {
            get { return mResult; }
            set { mResult = value; }
        }

        public MetroFramework.Controls.MetroLink MetroBack
        {
            get { return mlBack; }
            set { mlBack = value; }
        }

        public Form1()
        {
            InitializeComponent();
            //Đặt winform full màn hình
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mlBack.Visible = false;
            //Thêm UserControl vào Form
            UCMain uc = new UCMain();
            _instance = this;
            uc.Dock = DockStyle.Fill;
            mResult.Controls.Add(uc);
        }

        private void mlBack_Click(object sender, EventArgs e)
        {
            Form1.Instance.MetroContainer.Controls.Clear();
            if (!Form1.Instance.MetroContainer.Controls.ContainsKey("UCMain"))
            {
                //Back trả về giao diện chính
                UCMain uc = new UCMain();
                uc.Dock = DockStyle.Fill;
                Form1.Instance.MetroContainer.Controls.Add(uc);
                Form1.Instance.MetroBack.Visible = false;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
