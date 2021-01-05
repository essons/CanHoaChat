using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Controls;

namespace CanHoaChat
{
    public partial class UCLichSuCan : UserControl
    {
        public UCLichSuCan()
        {
            InitializeComponent();
        }

        int timeout = 0;
        private void UCLichSuCan_Load(object sender, EventArgs e)
        {
            timeout = 0;
            string MO, SoThe, ChemicalOrder, BatchNo, Weight, intime, indat, username, status = "";
            DataTable dt = SQL_Conn.SelectHistory();
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                MO = dt.Rows[i][0].ToString();
                SoThe = dt.Rows[i][1].ToString();
                ChemicalOrder = dt.Rows[i][2].ToString();
                BatchNo = dt.Rows[i][3].ToString();
                Weight = dt.Rows[i][4].ToString();
                intime = dt.Rows[i][5].ToString();
                indat = dt.Rows[i][6].ToString();
                username = dt.Rows[i][8].ToString();
                if (dt.Rows[i][8].ToString() == "1")
                    status = "Hoàn thành";
                else
                    status = "Đang chạy";
                dataGridView1.Rows.Add(MO, SoThe, ChemicalOrder, BatchNo, Weight, intime, indat, username, status);
                
            }
            dataGridView1.Refresh();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timeout = timeout + 5;
            if (timeout >= 700)
            {
                MetroLink link = (Form1.Instance.Controls["mlBack"] as MetroLink);
                link.PerformClick();
            }

            if (FormUsed.FormNow == "Main")
            {
                timer1.Enabled = false;
                timeout = 0;
            }
        }

        private void UCLichSuCan_MouseHover(object sender, EventArgs e)
        {
            timeout = 0;
        }
    }
}
