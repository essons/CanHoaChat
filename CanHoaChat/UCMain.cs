using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CanHoaChat
{
    public partial class UCMain : MetroFramework.Controls.MetroUserControl
    {
        public UCMain()
        {
            InitializeComponent();
        }

        private void UCMain_Load(object sender, EventArgs e)
        {
          
        }

        private void mCanTuDong_Click(object sender, EventArgs e)
        {
            Form1.Instance.MetroContainer.Controls.Clear();
            if (!Form1.Instance.MetroContainer.Controls.ContainsKey("UCCanTuDong"))
            {
                UCCanTuDong uc = new UCCanTuDong();
                uc.Dock = DockStyle.Fill;
                Form1.Instance.MetroContainer.Controls.Add(uc);
                Form1.Instance.MetroBack.Visible = true;
            }
        }

        private void mCanBanTuDong_Click(object sender, EventArgs e)
        {
            Form1.Instance.MetroContainer.Controls.Clear();
            if (!Form1.Instance.MetroContainer.Controls.ContainsKey("UCCanBanTuDong"))
            {
                UCCanBanTuDong uc = new UCCanBanTuDong();
                uc.Dock = DockStyle.Fill;
                Form1.Instance.MetroContainer.Controls.Add(uc);
                Form1.Instance.MetroBack.Visible = true;
            }
        }
    }
}
