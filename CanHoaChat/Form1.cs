using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
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
            this.MaximumSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            this.MinimumSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            WindowState = FormWindowState.Normal;
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                mlBack.Visible = false;
                //Thêm UserControl vào Form
                UCMain uc = new UCMain();
                _instance = this;
                uc.Dock = DockStyle.Fill;
                mResult.MaximumSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                mResult.Size = mResult.MaximumSize;
                mResult.Controls.Add(uc);
                this.ShowIcon = true;
                this.ShowInTaskbar = true;
                FormUsed.FormNow = "Main";
            }
            catch { }
        }

        private void mlBack_Click(object sender, EventArgs e)
        {
            try
            {
                FormUsed.FormNow = "Main";
                mResult.Controls["UCCanBanTuDong"].Enabled = false;
                UserControl uc = (mResult.Controls["UCCanBanTuDong"] as UserControl);
                uc.Parent.Controls.Remove(this);
                Form1_Load(sender, e);
                 mResult.Controls["UCCanBanTuDong"].Controls.Clear();
            }
            catch {
               
            }

            try
            {
                FormUsed.FormNow = "Main";
                mResult.Controls["UCCanBanTuDong2"].Enabled = false;
                UserControl uc = (mResult.Controls["UCCanBanTuDong2"] as UserControl);
                uc.Parent.Controls.Remove(this);
                Form1_Load(sender, e);
                mResult.Controls["UCCanBanTuDong2"].Controls.Clear();
            }
            catch
            {
              
            }

            mResult.Controls.Clear();
            Form1.Instance.MetroContainer.Controls.Clear();
            PictureBox myform1textbox1 = (Form1.Instance.Controls["pbTram"] as PictureBox);
            myform1textbox1.Visible = false;

            if (!Form1.Instance.MetroContainer.Controls.ContainsKey("UCMain"))
            {
                //Back trả về giao diện chính
                UCMain u2c = new UCMain();
                u2c.Dock = DockStyle.Fill;
                Form1.Instance.MetroContainer.Controls.Add(u2c);
                Form1.Instance.MetroBack.Visible = false;
            }
        }
        
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
