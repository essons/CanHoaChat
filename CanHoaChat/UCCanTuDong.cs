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
    public partial class UCCanTuDong : MetroFramework.Controls.MetroUserControl
    {
        public UCCanTuDong()
        {
            InitializeComponent();
        }

        public void PLCOpen()
        {
            try
            {
                cJ2Compolet1.UseRoutePath = false;
                cJ2Compolet1.PeerAddress = "10.0.14.22";
                cJ2Compolet1.LocalPort = 2;
                cJ2Compolet1.Active = true;
            }
            catch (Exception ex)
            {
                msg m = new msg("Không kết nối được PLC");
                m.Show();
            }
        }

        private void UCCanTuDong_Load(object sender, EventArgs e)
        {
            PLCOpen();
            txtQRCode.Focus();
        }

        private void txtQRCode_TextChanged(object sender, EventArgs e)
        {
            if(txtQRCode.Text.Length >= 3)
            {
                string[] open = new string[2]; //Mo thung
                string[] light = new string[2];
                DataTable dt = SQL_Conn.GetBucketPosition(int.Parse(txtQRCode.Text));

                try
                {
                    open = dt.Rows[0][2].ToString().Split('.');
                    light = dt.Rows[0][3].ToString().Split('.');

                    var a = cJ2Compolet1.ReadMemoryBit(OMRON.Compolet.CIP.CJ2Compolet.MemoryTypes.CIO, int.Parse(open[0]), Int16.Parse(open[1]));
                    if (a == true)
                    {
                        cJ2Compolet1.ForceCancel(OMRON.Compolet.CIP.CJ2Compolet.ForceMemoryTypes.CIO, int.Parse(open[0]), Int16.Parse(open[1]));
                        cJ2Compolet1.ForceCancel(OMRON.Compolet.CIP.CJ2Compolet.ForceMemoryTypes.CIO, int.Parse(light[0]), Int16.Parse(light[1]));
                    }
                    else
                    {
                        cJ2Compolet1.ForceSet(OMRON.Compolet.CIP.CJ2Compolet.ForceMemoryTypes.CIO, int.Parse(open[0]), Int16.Parse(open[1]));
                        cJ2Compolet1.ForceSet(OMRON.Compolet.CIP.CJ2Compolet.ForceMemoryTypes.CIO, int.Parse(light[0]), Int16.Parse(light[1]));
                    }
                }
                catch { }

                txtQRCode.Text = "";
            }
        }
    }
}
