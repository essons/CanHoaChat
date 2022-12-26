using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace CanHoaChat
{
    public partial class UCMain : MetroFramework.Controls.MetroUserControl
    {
        public UCMain()
        {
            InitializeComponent();
            
        }

        CancellationTokenSource cancel = new CancellationTokenSource();
        private void UCMain_Load(object sender, EventArgs e)
        {
            this.MaximumSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            this.Size = this.MaximumSize;        
        }

        bool MoThung = false;
        bool CanTram1 = false;
        private void mCanTuDong_Click(object sender, EventArgs e)
        {
            MoThung = true;
            CanTram1 = false;

            pCheckNV.Visible = true;

           
        }

        private void mCanBanTuDong_Click(object sender, EventArgs e)
        {
            MoThung = false;
            CanTram1 = true;

            pCheckNV.Visible = true;
           
        }

        private void metroTile2_Click(object sender, EventArgs e)
        {
            FormUsed.FormNow = "";
            Form1.Instance.MetroContainer.Controls.Clear();
            PictureBox myform1textbox1 = (Form1.Instance.Controls["pbTram"] as PictureBox);
            myform1textbox1.Image = Image.FromFile("2.png");
            myform1textbox1.Visible = true;
            if (!Form1.Instance.MetroContainer.Controls.ContainsKey("UCCanBanTuDong2"))
            {
                UCCanBanTuDong2 uc = new UCCanBanTuDong2();
                uc.Dock = DockStyle.Fill;
                Form1.Instance.MetroContainer.Controls.Add(uc);
                Form1.Instance.MetroBack.Visible = true;
            }

        }

        private void metroTile3_Click(object sender, EventArgs e)
        {
            FormUsed.FormNow = "";
            Form1.Instance.MetroContainer.Controls.Clear();
            if (!Form1.Instance.MetroContainer.Controls.ContainsKey("UCLichSuCan"))
            {
                UCLichSuCan uc = new UCLichSuCan();
                uc.Dock = DockStyle.Fill;
                Form1.Instance.MetroContainer.Controls.Add(uc);
                Form1.Instance.MetroBack.Visible = true;
            }
        }

        private void pCheckNV_VisibleChanged(object sender, EventArgs e)
        {
            if (pCheckNV.Visible == true)
                txtQRNV.Focus();
        }

        private void txtQRNV_TextChanged(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            pCheckNV.Visible = false;
            lbError.Text = "";
        }


        private void button1_Click(object sender, EventArgs e)
        {

        }

        bool dongthung = false;
        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void txtQRNV_Click(object sender, EventArgs e)
        {

        }

        int dem = 0;
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            dem += 1;
            if(dem == 2)
            {
                timer1.Enabled = false;
                if (txtQRNV.Text.Length >= 4)
                {
                    DataTable dt;
                    if (txtQRNV.Text.Length == 4)
                        dt = SQL_Conn.CheckNV("0" + txtQRNV.Text);
                    else
                        dt = SQL_Conn.CheckNV(txtQRNV.Text);

                    if (dt.Rows.Count > 0)
                    {
                        FormUsed.FormNow = "";
                        Thread.Sleep(2000);
                        lbError.Text = "";
                        pCheckNV.Visible = false;
                        Form1.Instance.MetroContainer.Controls.Clear();
                        if (MoThung == true)
                        {
                            if (!Form1.Instance.MetroContainer.Controls.ContainsKey("UCCanTuDong"))
                            {
                                UCCanTuDong uc = new UCCanTuDong();
                                uc.Dock = DockStyle.Fill;
                                Form1.Instance.MetroContainer.Controls.Add(uc);
                                Form1.Instance.MetroBack.Visible = true;
                            }
                        }

                        if (CanTram1 == true)
                        {
                            PictureBox myform1textbox1 = (Form1.Instance.Controls["pbTram"] as PictureBox);
                            myform1textbox1.Image = Image.FromFile("1.png");
                            myform1textbox1.Visible = true;
                            if (!Form1.Instance.MetroContainer.Controls.ContainsKey("UCCanBanTuDong"))
                            {
                                UCCanBanTuDong uc = new UCCanBanTuDong();
                                uc.Dock = DockStyle.Fill;
                                Form1.Instance.MetroContainer.Controls.Add(uc);
                                Form1.Instance.MetroBack.Visible = true;
                            }
                        }

                        Error.User = txtQRNV.Text;
                        txtQRNV.Text = "";
                    }
                    else
                    {
                        txtQRNV.Text = "";
                        txtQRNV.Clear();
                        txtQRNV.Focus();
                        txtQRNV.SelectAll();
                        lbError.Text = "Số thẻ không có quyền truy cập";
                    }
                }
            }           
        }
    }
}
