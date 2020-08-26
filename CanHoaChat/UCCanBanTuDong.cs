using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Threading;
using ActUtlTypeLib;
using System.Data.SqlTypes;
using System.Drawing.Printing;
using QRCoder;
using System.Globalization;

namespace CanHoaChat
{
    public partial class UCCanBanTuDong : MetroFramework.Controls.MetroUserControl
    {
        //Tao 1 thể hiện của SerialPort;
        public static SerialPort comport = new SerialPort();
        public Thread a = new Thread(openThread); //Tạo 1 thread dùng để chạy ẩn
        public UCCanBanTuDong()
        {
            InitializeComponent();
            Au = null;
            Total = null;
            soau = 1;
            autrung = false;
        }

        public static void openThread()
        { }

        DataTable dtM = new DataTable();
        DataTable dtChoose = new DataTable();
        DataTable dtChooseCTY = new DataTable();
        DataTable MaterialTemp = new DataTable();
        string[,] Au;
        string[,] Total;
        string[,] Total_Temp;
        int Stt_Temp = 0;
        int Stt_Can_Temp = 0;
        int Stt_Au_Temp = 1;

        string QRCode = "";
        bool autrung = false;
        bool aule = false;
        bool inlai = false;
        int soau = 1;
        private void UCCanBanTuDong_Load(object sender, EventArgs e)
        {
            txtKG.UseCustomBackColor = true;
            //Lấy danh sách lệnh được điều động
            dtM = SQL_Conn.SelectCommand();
            cbLenh.DataSource = dtM;
            cbLenh.DisplayMember = "ManufactureOrderNo";
            cbLenh.ValueMember = "ManufactureOrderNo";

            //Mở cổng com kiểm tra cân điện
            COMOpen();
        }

        public void COMOpen()
        {
            if (comport.IsOpen == true)
                comport.Close();

            try
            {
                string strCOMname = "COM5";
                comport.PortName = strCOMname.Trim();
                comport.BaudRate = 9600;
                comport.Parity = System.IO.Ports.Parity.None;
                comport.DataBits = 8;
                comport.StopBits = System.IO.Ports.StopBits.One;
                comport.Open();
                btRun.Enabled = true;

            }
            catch (Exception ex)
            {
                btRun.Enabled = false;
                timer1.Enabled = false;
                msg m = new msg("Không kết nối được cân điện");
                m.Show();
            }
        }

        //Ham nhan du lieu tu cong COM
        private string COMgetData(string kg)
        {
            string strData = "";
            string strData2 = "";
            Double dResult;

            try
            {
                //Lấy dữ liệu từ cổng COM chuyển sang string
                strData = comport.ReadExisting().ToString();
                if(strData.Length >= 17)
                {
                    strData2 = strData.Substring(strData.Length - 17, 10);
                    //strData2 = strData.Substring(7, strData.Length - 7);
                    dResult = Convert.ToDouble(strData2);
                    if (dResult >= 0)
                    {
                        //Lấy số lượng hiện tại bằng số lượng cân trừ đi trọng lượng âu và tổng trọng lượng các chát hiện có của Au
                        double slhientai = dResult - Double.Parse(Au[soau - 1, 1]) - Double.Parse(Total[soau - 1, 1]);
                        slhientai = Math.Round(slhientai, 3);
                        if (slhientai > 0)
                            txtKG.WaterMark = slhientai.ToString();
                        else
                            txtKG.WaterMark = "0";
                    }
                    //Lấy xác suất lệch cho phép là 1%
                    var minKG = Double.Parse(kg) - Double.Parse(kg) / 100;
                    var maxKG = Double.Parse(kg) + Double.Parse(kg) / 100;
                    var ActualKG = dResult - Double.Parse(Au[soau - 1, 1]) - Double.Parse(Total[soau - 1, 1]);
                    ActualKG = Math.Round(ActualKG, 3);
                    if (ActualKG >= minKG && ActualKG <= maxKG)
                    {
                        Total[soau - 1, 1] = (Double.Parse(Total[soau - 1, 1]) + ActualKG).ToString(); //Gán chất vừa cân được vào khối lượng tổng của âu
                        return "OK";
                    }
                }
                else
                    return "Fail";
            }
            catch (Exception ex)
            {
                if (comport.IsOpen == false && showmsg == false)
                {
                    if (this.InvokeRequired)
                    {
                        SetTimerCheckAu d = new SetTimerCheckAu(ShowMsg);
                        this.Invoke
                            (d, new object[] { "" });
                    }
                    showmsg = true;
                }
                return "Fail";
            }
            return "Fail";
        }

        bool showmsg = false;
        private string COMgetAu()
        {
            string strData = "";
            string strData2 = "";
            Double dResult;
            try
            {
                strData = comport.ReadExisting().ToString();
                if (strData != "")
                    strData2 = strData.Substring(strData.Length - 17,10);
                    //strData2 = strData.Substring(7, strData.Length - 7);
                else
                    strData2 = "0";
                dResult = Convert.ToDouble(strData2);
                dResult = Math.Round(dResult, 3);
                if (dResult > 0)
                    txtCanAu.WaterMark = dResult.ToString();
                if (dResult > 0)
                    return "OK";
            }
            catch (Exception ex)
            {
                return "Fail";
            }
            return "Fail";
        }

        Color[] Mau = { Color.DodgerBlue, Color.DodgerBlue, Color.LightSkyBlue, Color.LightSkyBlue, Color.White, Color.White };
        int iDem = 0;


        private void btRun_Click(object sender, EventArgs e)
        {
            if (a.IsAlive)
            {
                a.Abort();
            }
            COMOpen();
            btIn.Enabled = false;
            lbMaKeo.Visible = true;
            lbSoMe.Visible = true;
            panelAu.Visible = true; //Hiện panel Âu để cân âu


            //Lấy thông tin của lệnh vừa được chọn
            var expression = "ManufactureOrderNo = '" + cbLenh.Text.ToString() + "'";
            dtChoose = dtM.Select(expression).CopyToDataTable();
            dtChooseCTY = SQL_Conn.SelectCommandCTY(cbLenh.Text.ToString());
            MaterialTemp = SQL_Conn.SelectCommandTemp(cbLenh.Text.ToString());
            Au = new string[int.Parse(dtChoose.Rows[0][3].ToString()), 2]; //Khai báo số Âu theo lệnh điều động
            Total = new string[int.Parse(dtChoose.Rows[0][3].ToString()), 2]; //Khai báo tổng số lượng của từng âu

            //Gán dữ liệu để quét tiếp nếu bị lỗi cân khi đang quét
            for (int i = 0; i < MaterialTemp.Rows.Count; i++)
            {
                Au[i, 0] = MaterialTemp.Rows[i][1].ToString();
                Total[i, 0] = MaterialTemp.Rows[i][1].ToString();
                Au[i, 1] = MaterialTemp.Rows[i][4].ToString();
                Total[i, 1] = MaterialTemp.Rows[i][5].ToString();

                int ChemicalStt = int.Parse(MaterialTemp.Rows[i][3].ToString());
                int AuNumber = int.Parse(MaterialTemp.Rows[i][2].ToString());
                string QRAu = MaterialTemp.Rows[i][1].ToString();
                if (AuNumber >= Stt_Au_Temp && QRAu != "")
                {
                    Stt_Au_Temp = AuNumber + 1;
                }

                if (ChemicalStt >= Stt_Temp && ChemicalStt > 0)
                {
                    Stt_Temp = ChemicalStt;
                    if (AuNumber == int.Parse(dtChoose.Rows[0][3].ToString()))
                    {
                        Stt_Temp += 1;
                        Stt_Can_Temp = 0;
                    }
                    else
                        Stt_Can_Temp = AuNumber;
                }
            }

            //Gán thông tin keo mà tổng số âu
            lbMaKeo.Text = "Mã keo: " + dtChoose.Rows[0][1].ToString();
            lbSoMe.Text = "Tổng số hộp: " + dtChoose.Rows[0][3].ToString();

            //Mở timer 2 kiểm tra Âu
            timer2.Enabled = true;

            //Chạy thread để lấy dữ liệu từ cân
            a = new Thread(checkQRAu);
            a.IsBackground = true;
            a.Start();
        }

        string result = "";
        public void getMaterial()
        {
            int j = 5;
            soau = Stt_Can_Temp + 1;
            if (Stt_Temp > 0)
            {
                j = 5 + ((Stt_Temp - 1) * 2);
            }

            for (int i = 0; i < dtChooseCTY.Rows.Count; i++)
            {
                //Lấy dữ liệu từ cột 5 đến cột 32 tương đương với 14 chất và khối lượng
                for (; j <= 32; j++)
                {
                    //Kiểm tra xem cột hóa chất có bị rỗng không
                    if (dtChooseCTY.Rows[i][j].ToString() != "" && dtChooseCTY.Rows[i][j].ToString() != "0")
                    {
                        //Nháy màu thay đổi tên nguyên liệu
                        if (this.InvokeRequired)
                        {
                            SetTimerCheckAu d = new SetTimerCheckAu(SetNL);
                            this.Invoke
                                (d, new object[] { "" });
                        }

                        //Set lại gridview hóa chất của nguyên liệu
                        if (this.InvokeRequired)
                        {
                            SetTextCallback d = new SetTextCallback(setDgv);
                            this.Invoke
                                (d, new object[] { (soau - 1).ToString() });
                        }

                        txtKGR.WaterMark = dtChooseCTY.Rows[soau - 1][j + 1].ToString(); //set số lượng cần
                        //Cân đủ số âu thì chuyển sang chất khác
                        while (result != "OK" && soau <= int.Parse(dtChoose.Rows[i][3].ToString()))
                        {
                            //Set label tên nguyên liệu
                            if (this.InvokeRequired)
                            {
                                SetTextCallback d = new SetTextCallback(SetLabelNL);
                                this.Invoke
                                    (d, new object[] { dtChooseCTY.Rows[i][j].ToString()});
                            }

                            //Set label số thứ tự hộp
                            if (this.InvokeRequired)
                            {
                                SetTextCallback d = new SetTextCallback(SetLabelStt);
                                this.Invoke
                                    (d, new object[] { soau.ToString() });
                            }

                            result = COMgetData(dtChooseCTY.Rows[soau - 1][j + 1].ToString()); //Lấy liệu từ cổng COM
                            Thread.Sleep(50);
                            if (result == "OK")
                            {                   
                                //Update bảng lưu tạm
                                if (this.InvokeRequired)
                                {
                                    SetTimerCheckAu d = new SetTimerCheckAu(UpdateTemp);
                                    this.Invoke
                                        (d, new object[] { "2" });
                                }
                                if (this.InvokeRequired)
                                {
                                    SetTimerCheckAu d = new SetTimerCheckAu(UpdateTemp);
                                    this.Invoke
                                        (d, new object[] { "3" });
                                }
                                //Thay đổi khổi lượng gridview của từng hộp
                                if (this.InvokeRequired)
                                {
                                    SetTextCallback d = new SetTextCallback(setDgv);
                                    this.Invoke
                                        (d, new object[] { (soau - 1).ToString() });
                                }

                                soau++;
                                
                                //Cân đủ khối lượng chuyển sang chất kế tiếp   
                                if (soau > int.Parse(dtChoose.Rows[i][3].ToString()))
                                {
                                    Thread.Sleep(2000);
                                    result = "OK";
                                }
                                else
                                {                                   
                                    Thread.Sleep(5000);
                                    //Set màu cho label số hộp
                                    if (this.InvokeRequired)
                                    {
                                        SetTimerCheckAu d = new SetTimerCheckAu(SetStt);
                                        this.Invoke
                                            (d, new object[] { "" });
                                    }
                                    txtKGR.WaterMark = dtChooseCTY.Rows[soau - 1][j + 1].ToString(); //set số lượng cần
                                    result = "";
                                }
                            }
                        }
                        soau = 1; //Reset âu để cân hóa chất tiếp theo
                        if (this.InvokeRequired)
                        {
                            SetTextCallback d = new SetTextCallback(SetLabelStt);
                            this.Invoke
                                (d, new object[] { soau.ToString() });
                        }
                    }
                    j++;
                    result = "";
                }
            }
            bool b = SQL_Conn.updateCommand(dtChooseCTY.Rows[0][0].ToString()); //Cập nhật hoàn thành lệnh
            stoptime = true; //Dừng cân
        }

        //Set dữ liệu lên form khi dùng thread
        delegate void SetTextCallback(string text);
        delegate void SetTimerCheckAu(string text);
        private void SetTimer(string text)
        {
            this.timer3.Enabled = true;
        }
        private void SetLabelStt(string text)
        {
            this.lbStt.Text = "Hộp thứ: " + text;
        }
        private void SetLabelNL(string text)
        {
            this.lbNguyenLieu.Text = "Hóa chất " + text;
        }
        private void SetText(string text)
        {
            this.txtQRAu.Text = text;
        }
        private void ShowMsg(string text)
        {
            msgPrintError m = new msgPrintError("Không kết nối được cân điện. Bạn có muốn tiếp tục cân không ?");
            m.Show();
        }
        private void UpdateTemp(string text)
        {
            if (text == "1")
                SQL_Conn.updateAuTemp(cbLenh.Text.ToString(), txtCanAu.WaterMark, txtQRAu.Text, stt.ToString());
            if (text == "2")
                SQL_Conn.updateAuTotalTemp(cbLenh.Text, Total[soau - 1, 1], soau.ToString());
            if (text == "3")
                SQL_Conn.updateSttTemp(cbLenh.Text, soau.ToString());
        }
        private void SetNL(string text)
        {
            timer4.Enabled = true;
        }
        private void SetStt(string text)
        {
            timer5.Enabled = true;
        }
        private void setDgv(string text)
        {
            int i = int.Parse(text);
            //Thêm dữ liệu vào datagridview show thông tin hóa chất
            dgCty.Rows.Clear();
            dgCty2.Rows.Clear();
            dgCty3.Rows.Clear();
            dgCty4.Rows.Clear();
            //Kiểm tra hóa chất
            string m1 = dtChooseCTY.Rows[0]["Material01"].ToString();
            string m2 = dtChooseCTY.Rows[0]["Material02"].ToString();
            string m3 = dtChooseCTY.Rows[0]["Material03"].ToString();
            string m4 = dtChooseCTY.Rows[0]["Material04"].ToString();
            string m5 = dtChooseCTY.Rows[0]["Material05"].ToString();
            string m6 = dtChooseCTY.Rows[0]["Material06"].ToString();
            string m7 = dtChooseCTY.Rows[0]["Material07"].ToString();
            string m8 = dtChooseCTY.Rows[0]["Material08"].ToString();
            string m9 = dtChooseCTY.Rows[0]["Material09"].ToString();
            string m10 = dtChooseCTY.Rows[0]["Material10"].ToString();
            string m11 = dtChooseCTY.Rows[0]["Material11"].ToString();
            string m12 = dtChooseCTY.Rows[0]["Material12"].ToString();
            string m13 = dtChooseCTY.Rows[0]["Material13"].ToString();
            string m14 = dtChooseCTY.Rows[0]["Material14"].ToString();

            string w1 = dtChooseCTY.Rows[i]["weight01"].ToString();
            string w2 = dtChooseCTY.Rows[i]["weight02"].ToString();
            string w3 = dtChooseCTY.Rows[i]["weight03"].ToString();
            string w4 = dtChooseCTY.Rows[i]["weight04"].ToString();
            string w5 = dtChooseCTY.Rows[i]["weight05"].ToString();
            string w6 = dtChooseCTY.Rows[i]["weight06"].ToString();
            string w7 = dtChooseCTY.Rows[i]["weight07"].ToString();
            string w8 = dtChooseCTY.Rows[i]["weight08"].ToString();
            string w9 = dtChooseCTY.Rows[i]["weight09"].ToString();
            string w10 = dtChooseCTY.Rows[i]["weight10"].ToString();
            string w11 = dtChooseCTY.Rows[i]["weight11"].ToString();
            string w12 = dtChooseCTY.Rows[i]["weight12"].ToString();
            string w13 = dtChooseCTY.Rows[i]["weight13"].ToString();
            string w14 = dtChooseCTY.Rows[i]["weight14"].ToString();

            dgCty.Rows.Add(m1, w1, m2, w2, m3, w3, m4, w4);
            dgCty2.Rows.Add(m5, w5, m6, w6, m7, w7, m8, w8);
            dgCty3.Rows.Add(m9, w9, m10, w10, m11, w11, m12, w12);
            dgCty4.Rows.Add(m13, w13, m14, w14);

            for (int k = 0; k < dgCty.Columns.Count; k++)
            {
                string val = dgCty.Rows[0].Cells[k].Value.ToString().Replace(" ", "");  //check cell value
                if (string.IsNullOrEmpty(val) || val == "0.000")
                {
                    dgCty.Columns[k].Visible = false;     //Hides Column
                }
            }

            for (int k = 0; k < dgCty2.Columns.Count; k++)
            {
                string val = dgCty2.Rows[0].Cells[k].Value.ToString().Replace(" ", "");  //check cell value
                if (string.IsNullOrEmpty(val) || val == "0.000")
                {
                    dgCty2.Columns[k].Visible = false;     //Hides Column
                }
            }

            for (int k = 0; k < dgCty3.Columns.Count; k++)
            {
                string val = dgCty3.Rows[0].Cells[k].Value.ToString().Replace(" ", "");  //check cell value
                if (string.IsNullOrEmpty(val) || val == "0.000")
                {
                    dgCty3.Columns[k].Visible = false;     //Hides  Column
                }
            }

            for (int k = 0; k < dgCty4.Columns.Count; k++)
            {
                string val = dgCty4.Rows[0].Cells[k].Value.ToString().Replace(" ", "");  //check  cell value
                if (string.IsNullOrEmpty(val) || val == "0.000")
                {
                    dgCty4.Columns[k].Visible = false;     //Hides Column
                }
            }
        }
        //---------------------------------------------------

        string QRAu = "";
        int stt = 1;
        public void checkQRAu()
        {
            string result = "";

            //int sttTime = 1;
            //Kiểm tra đã quét chưa
            stt = Stt_Au_Temp;
            soau = Stt_Au_Temp;

            //Quét QR âu và cân bì âu
            while (stt <= int.Parse(dtChoose.Rows[0][3].ToString()))
            {
                //Đúng chuẩn mã QR Âu
                if (txtQRAu.Text.Length >= 4)
                {
                    //Kiểm tra âu lẻ khác 25kg có kí tự cuối là A
                    if (dtChooseCTY.Rows[stt - 1][3].ToString() == "25.000" && txtQRAu.Text.Substring(1, 1) == "U" ||
                        (dtChooseCTY.Rows[stt - 1][3].ToString() != "25.000" && txtQRAu.Text.Substring(1, 1) == "A"))
                    {
                        //Kiểm tra quét trùng Âu
                        for (int i = 0; i < int.Parse(dtChoose.Rows[0][3].ToString()); i++)
                        {
                            //Nếu trùng
                            if (Au[i, 0] == txtQRAu.Text)
                            {
                                if (this.txtQRAu.InvokeRequired)
                                {
                                    //Set textbox QR âu bằng "" nếu dùng thread;
                                    SetTextCallback d = new SetTextCallback(SetText);
                                    this.Invoke
                                        (d, new object[] { "" });
                                    autrung = true;
                                }
                                else
                                {
                                    //Set textbox QR âu bằng "" nếu dùng bình thường;
                                    this.txtQRAu.Text = "";
                                    autrung = true;
                                }
                                //Set biến QR Au bằng rỗng 
                                QRAu = "";
                            }
                            else
                                QRAu = txtQRAu.Text; //Không trùng thì gán biến = QR đang quét
                        }

                        //QR Au không rỗng thì cân số bì của âu
                        if (QRAu != "")
                        {
                            if (this.InvokeRequired)
                            {
                                SetTimerCheckAu d = new SetTimerCheckAu(SetTimer);
                                this.Invoke
                                    (d, new object[] { "" });
                            }

                            while (timer3.Enabled)
                            {
                                result = COMgetAu();
                                Thread.Sleep(50); //Dừng 0.2s
                            }

                            //Đủ 7s
                            if (timer3.Enabled == false)
                            {
                                Au[stt - 1, 0] = txtQRAu.Text; //Gán QR vào mảng Au
                                Au[stt - 1, 1] = txtCanAu.WaterMark; //Gán khối lượng bì của âu
                                Total[stt - 1, 0] = txtQRAu.Text; //Gán QR vào mảng Total
                                Total[stt - 1, 1] = "0"; //Set khối lượng bằng 0

                                if (this.InvokeRequired)
                                {
                                    SetTimerCheckAu d = new SetTimerCheckAu(UpdateTemp);
                                    this.Invoke
                                        (d, new object[] { "1" });
                                }

                                stt++;
                                if (this.txtQRAu.InvokeRequired)
                                {
                                    //Set TextBox Qr Au về "" dùng trong thread
                                    SetTextCallback d = new SetTextCallback(SetText);
                                    this.Invoke
                                        (d, new object[] { "" });
                                }
                                else
                                {
                                    //Set TextBox Qr Au về "" dùng bình thường
                                    this.txtQRAu.Text = "";
                                }
                                txtCanAu.WaterMark = "0"; //Set Khối lượng Âu về 0 dùng WaterMart trong textbox
                                soau++; //Tăng số âu tiếp theo
                            }
                        }
                    }
                    else
                    {
                        if (this.txtQRAu.InvokeRequired)
                        {
                            //Set textbox QR âu bằng "" nếu dùng thread;
                            SetTextCallback d = new SetTextCallback(SetText);
                            this.Invoke
                                (d, new object[] { "" });
                            aule = true;
                        }
                    }
                }
            }
            checkAu = true; //Kiểm tra âu thành công
        }

        bool stoptime = false;
        int soauhientai;
        int tongau;
        string maau;
        string khoiluong;
        string thoigianin;
        string ngayin;
        string makeo;
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (stoptime || Error.PrintError == "OK") //Cân thành công hoặc cân tiếp khi cân bị lỗi
                {
                    thoigianin = DateTime.Now.ToString("HH:mm:ss");
                    ngayin = DateTime.Now.ToString("dd-MM-yyyy");
                    showmsg = false;

                    if (Error.PrintError == "OK")
                    {
                        Total_Temp = new string[dtChooseCTY.Rows.Count, 2];
                        for (int i = 0; i < dtChooseCTY.Rows.Count; i++)
                        {
                            double val = double.Parse(dtChooseCTY.Rows[i]["weight01"].ToString()) + double.Parse(dtChooseCTY.Rows[i]["weight08"].ToString()) +
                                         double.Parse(dtChooseCTY.Rows[i]["weight02"].ToString()) + double.Parse(dtChooseCTY.Rows[i]["weight08"].ToString()) +
                                         double.Parse(dtChooseCTY.Rows[i]["weight03"].ToString()) + double.Parse(dtChooseCTY.Rows[i]["weight10"].ToString()) +
                                         double.Parse(dtChooseCTY.Rows[i]["weight04"].ToString()) + double.Parse(dtChooseCTY.Rows[i]["weight11"].ToString()) +
                                         double.Parse(dtChooseCTY.Rows[i]["weight05"].ToString()) + double.Parse(dtChooseCTY.Rows[i]["weight12"].ToString()) +
                                         double.Parse(dtChooseCTY.Rows[i]["weight06"].ToString()) + double.Parse(dtChooseCTY.Rows[i]["weight13"].ToString()) +
                                         double.Parse(dtChooseCTY.Rows[i]["weight07"].ToString()) + double.Parse(dtChooseCTY.Rows[i]["weight14"].ToString());
                            Total_Temp[i, 0] = dtChooseCTY.Rows[i][1].ToString();
                            Total_Temp[i, 1] = val.ToString();
                        }
                        for (int i = 0; i < dtChooseCTY.Rows.Count; i++)
                        {
                            double minKG = double.Parse(Total_Temp[i, 1]) - (double.Parse(Total_Temp[i, 1]) / 100);
                            double maxKG = double.Parse(Total_Temp[i, 1]) + (double.Parse(Total_Temp[i, 1]) / 100);
                            if (double.Parse(Total[i, 1]) <= maxKG && 
                                double.Parse(Total[i, 1]) >= minKG)
                            {

                            }
                            else
                            {
                                Total[i, 1] = Total_Temp[i, 1];
                            }
                        }
                    }

                    PrintDocument printDocument = new PrintDocument(); //add the document to the dialog box..
                    string directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    printDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("custom", 100, 200);
                    printDocument.DefaultPageSettings.PaperSize.RawKind = 119;
                    printDocument.PrinterSettings.DefaultPageSettings.PaperSize.RawKind = 119;
                    printDocument.DefaultPageSettings.Landscape = false;
                    printDocument.PrinterSettings.PrinterName = "Microsoft Print to PDF";
                    printDocument.PrinterSettings.PrintToFile = true;
                    makeo = lbMaKeo.Text;
                    for (int i = 0; i < int.Parse(dtChoose.Rows[0][3].ToString()); i++)
                    {
                        printDocument.PrinterSettings.PrintFileName = Path.Combine(directory, i + ".pdf");
                        soauhientai = i + 1;
                        tongau = int.Parse(dtChoose.Rows[0][3].ToString());
                        maau = Au[i, 0];
                        khoiluong = Total[i, 1];
                        printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(CreateReceipt); //add an event handler that will do the printing
                        //on a till you will not want to ask the user where to print but this is fine for the test envoironment.
                        printDocument.Print();

                    }

                    //Show msgbox
                    msg m = new msg("Quá trình cân đã hoàn thành");
                    m.TopMost = true;
                    m.Show();
                    a.Abort();
                    btIn.Enabled = true;

                    //Cập nhật lại bảng lệnh điều động
                    dtM = SQL_Conn.SelectCommand();
                    cbLenh.DataSource = dtM;
                    cbLenh.DisplayMember = "ManufactureOrderNo";
                    cbLenh.ValueMember = "ManufactureOrderNo";

                    //Tắt timer
                    timer1.Enabled = false;
                }
                else
                {
                    
                    //Chỉnh màu cho ô khối lượng
                    if(result != "OK") // Nếu chưa đúng trọng lượng
                    {
                        txtKG.BackColor = Mau[iDem];
                        iDem++;
                        if (iDem == 6) iDem = 0;
                    }
                    else
                    {
                        txtKG.BackColor = Color.Red;
                    }
                }
            }
            catch
            {
                
            }
        }

        bool checkAu = false;
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (checkAu)
            {
                lbNguyenLieu.Visible = true;
                lbStt.Visible = true;
                txtKG.Visible = true;
                txtKGR.Visible = true;

                soau = 1; //Set số âu về 1 để cân hóa chất
                a = new Thread(getMaterial);
                a.IsBackground = true;
                a.Start();
                //Mở timer 1 cân hóa chất
                timer1.Enabled = true;
                panelCan.Visible = true;

                //Tắt timer 2 kiểm tra âu 
                timer2.Enabled = false;
                stoptime = false; //Dừng cân dùng để khi cân hoàn thành thì true
                panelAu.Visible = false;

            }
            else
            {
                if (autrung) //Kiểm tra âu trùng
                {
                    msg m = new msg("Hộp này đã được quét");
                    m.TopMost = true;
                    m.Show();
                    autrung = false;
                }
                if (aule)
                {
                    msg m = new msg("Mã loại hộp không đúng");
                    m.TopMost = true;
                    m.Show();
                    aule = false;
                }

                txtQRAu.Focus();
                lbMaAu.Text = "Mã hộp thứ " + soau;
            }

        }

        public void CreateReceipt(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //this prints the reciept
            Graphics graphic = e.Graphics;
            Font font = new Font("Segoe UI", 12); //must use a mono spaced font as the spaces need to line up

            float fontHeight = font.GetHeight();

            int startX = 10;
            int startY = 10;
            int offset = 40;

            graphic.DrawString(" ESSONS GLOBAL", new Font("Segoe UI", 18, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY);
            offset = offset + 20;
            string top = makeo;
            graphic.DrawString(top, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight; //make the spacing consistent
            graphic.DrawString("----------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 20; //make some room so that the total stands out.
            graphic.DrawString("Số hộp: " + soauhientai + "/" + tongau, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30;
            graphic.DrawString("Mã hộp: " + maau, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            if(inlai == false)
                graphic.DrawString("Mã lệnh: " + cbLenh.Text, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            else
                graphic.DrawString("Mã lệnh: " + cbLenhIn.Text, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Tổng khối lượng: " + khoiluong + "g", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Giờ cân: " + thoigianin, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset); ;
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Ngày cân: " + ngayin , new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("----------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30;
            QRCodeGenerator _qrCode = new QRCodeGenerator();
            string qrcode = "";
            if (inlai == false)
                qrcode = dtChoose.Rows[0][2].ToString() + "," + DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("HHmmss");
            else
                qrcode = QRCode;
            QRCodeData _qrCodeData = _qrCode.CreateQrCode(qrcode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(_qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            graphic.DrawImage(qrCodeImage, startX + 60, startY + offset, 100, 100);
            bool b = SQL_Conn.updateQRCode(cbLenh.Text.ToString(), soauhientai, qrcode, ngayin, thoigianin, khoiluong); //Cập nhật QR code
        }

        int demau = 1;
        private void timer3_Tick(object sender, EventArgs e)
        {
            demau++;
            if (demau == 7)
            {
                timer3.Enabled = false;
                demau = 1;
            }
        }

        int baohieu = 0;
        private void timer4_Tick(object sender, EventArgs e)
        {
            baohieu++;
            if (baohieu == 2)
            {
                lbNguyenLieu.BackColor = Color.White;
                baohieu = 0;
                timer4.Enabled = false;
            }
            else
            {
                lbNguyenLieu.BackColor = Color.Red;
            }
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            baohieu++;
            if (baohieu == 2)
            {
                lbStt.BackColor = Color.White;
                baohieu = 0;
                timer5.Enabled = false;
            }
            else
            {
                lbStt.BackColor = Color.Red;
            }
        }

        private void btHuy_Click(object sender, EventArgs e)
        {
            pPrint.Visible = false;
        }

        private void btIn_Click(object sender, EventArgs e)
        {
            pPrint.Visible = true;
            MaterialTemp = SQL_Conn.SelectCommandPrint();
            cbLenhIn.DataSource = MaterialTemp;
            cbLenhIn.DisplayMember = "ManufactureOrderNo";
            cbLenhIn.ValueMember = "ManufactureOrderNo";

        }

        private void btIn2_Click(object sender, EventArgs e)
        {
            inlai = true;
            var expression = "ManufactureOrderNo = '" + cbLenhIn.Text.ToString() + "'";
            dtChoose = MaterialTemp.Select(expression).CopyToDataTable();
            dtChooseCTY = SQL_Conn.SelectCommandCTY(cbLenhIn.Text.ToString());
            MaterialTemp = SQL_Conn.SelectCommandTemp(cbLenhIn.Text.ToString());
            Au = new string[MaterialTemp.Rows.Count, 2]; //Khai báo số Âu theo lệnh điều động
            Total = new string[MaterialTemp.Rows.Count, 2]; //Khai báo tổng số lượng của từng âu

            //Gán dữ liệu để quét tiếp nếu bị lỗi cân khi đang quét
            for (int i = 0; i < MaterialTemp.Rows.Count; i++)
            {
                Au[i, 0] = MaterialTemp.Rows[i][1].ToString();
                Total[i, 0] = MaterialTemp.Rows[i][1].ToString();
                Au[i, 1] = MaterialTemp.Rows[i][4].ToString();
                Total[i, 1] = MaterialTemp.Rows[i][5].ToString();
            }

            makeo = dtChoose.Rows[0]["ChemicalOrderCode"].ToString();

            PrintDocument printDocument = new PrintDocument(); //add the document to the dialog box..
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            printDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("custom", 100, 200);
            printDocument.DefaultPageSettings.PaperSize.RawKind = 119;
            printDocument.PrinterSettings.DefaultPageSettings.PaperSize.RawKind = 119;
            printDocument.DefaultPageSettings.Landscape = false;
            printDocument.PrinterSettings.PrinterName = "Microsoft Print to PDF";
            printDocument.PrinterSettings.PrintToFile = true;
            for (int i = 0; i < MaterialTemp.Rows.Count; i++)
            {
                printDocument.PrinterSettings.PrintFileName = Path.Combine(directory, i + ".pdf");
                soauhientai = i + 1;
                tongau = MaterialTemp.Rows.Count;
                maau = Au[i, 0];
                khoiluong = Total[i, 1];
                QRCode = dtChooseCTY.Rows[i]["QRCode"].ToString();
                thoigianin = dtChooseCTY.Rows[i]["PrintTime"].ToString();
                ngayin = dtChooseCTY.Rows[i]["PrintDate"].ToString();
                printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(CreateReceipt); //add an event handler that will do the printing
                                                                                                             //on a till you will not want to ask the user where to print but this is fine for the test envoironment.
                printDocument.Print();

            }
            inlai = false;
            msg a = new msg("In lại thành công");
            a.Show();
        }

    }
}
