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
using System.Net;
using System.Net.Sockets;
using SimpleTCP;
using MetroFramework.Controls;

namespace CanHoaChat
{
    public partial class UCCanBanTuDong : MetroFramework.Controls.MetroUserControl
    {

        SimpleTcpServer server;
        string mo = "";
        string ComName = "";
        //Tao 1 thể hiện của SerialPort;
        public static SerialPort comport = new SerialPort();
        public Thread a = new Thread(openThread); //Tạo 1 thread dùng để chạy ẩn
        public UCCanBanTuDong()
        {
            InitializeComponent();
        }

        public static void openThread()
        { }

        DataTable dtBuckets = new DataTable();
        DataTable dtM = new DataTable();
        DataTable CheckPrint = new DataTable();
        DataTable dtChoose = new DataTable();
        DataTable dtChooseCTY = new DataTable();
        DataTable MaterialTemp = new DataTable();
        DataTable QRPrint = new DataTable();
        DataTable dtPrintQT1 = new DataTable();
        DataTable dtPrintQT2 = new DataTable();
        DataTable dtDataPrintQT1 = new DataTable();
        DataTable dtTyLe = new DataTable();
        DataTable dtSortMateiral = new DataTable();
        string[,] Au;
        string[,] Total;
        string[,] Total_Temp;
        int Stt_Temp = 0;
        int Stt_Can_Temp = 0;
        int Stt_Au_Temp = 1;

        bool quytrinh1 = false;
        string QRCode = "";
        bool autrung = false;
        bool aule = false;
        bool inlai = false;
        int soau = 1;
        int timeout = 0;
        bool choqua = false;
        string printerName = "";
        string username = "";
        string tenhop = "";
        private void UCCanBanTuDong_Load(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader("ComCan1.txt");
            ComName = sr.ReadLine().Trim();
            sr.Close();
            timeout = 0;
            sr = new StreamReader("printername.txt");
            printerName = sr.ReadLine().Trim();
            sr.Close();

            username = Error.User;

            this.MaximumSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            this.Size = this.MaximumSize;

            panelCan.Size = new Size(Screen.PrimaryScreen.Bounds.Width, 700);

            txtKG.UseCustomBackColor = true;
            //Lấy danh sách lệnh được điều động

            //Mở cổng com kiểm tra cân điện
            COMClose();
            PLCClose();

            txtQRCode.Text = "";
            txtQRCode.Focus();
            txtQRCode.SelectAll();
        }

        double ActualKG = 0;
        private void Server_DataReceived(object sender, SimpleTCP.Message e)
        {
            //txtTest.Invoke((MethodInvoker)delegate ()
            //{
            //    //txtTest.Text += e.MessageString;
            //    if (mo.Length >= 13)
            //        e.ReplyLine(mo);
            //    else
            //        e.ReplyLine("");
            //});
        }

        public void PLCOpen()
        {
            try
            {
                cJ2Compolet1.UseRoutePath = false;
                cJ2Compolet1.PeerAddress = "10.0.14.22";
                cJ2Compolet1.LocalPort = 2;
                cJ2Compolet1.Active = true;
                cJ2Compolet1.RunMode = OMRON.Compolet.CIP.CJ2Compolet.RunModeTypes.Monitor;
            }
            catch (Exception ex)
            {

                timer1.Enabled = false;
                msg m = new msg("Không kết nối được PLC");
                m.Show();
            }
        }
        public void COMOpen()
        {
            try
            {
                string strCOMname = ComName;
                comport.PortName = strCOMname.Trim();
                comport.BaudRate = 9600;
                comport.Parity = System.IO.Ports.Parity.None;
                comport.DataBits = 8;
                comport.StopBits = System.IO.Ports.StopBits.One;
                comport.Open();

            }
            catch (Exception ex)
            {
                timer1.Enabled = false;
                msg m = new msg("Không kết nối được cân điện");
                m.Show();
            }
        }

        public void PLCClose()
        {
            try
            {
                cJ2Compolet1.Active = false;
            }
            catch (Exception ex)
            {

                timer1.Enabled = false;
                msg m = new msg("Không kết nối được PLC");
                m.Show();
            }
        }
        public void COMClose()
        {
            if (comport.IsOpen == true)
            {
                comport.Close();
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
                timeout = 0;
                //Lấy dữ liệu từ cổng COM chuyển sang string
                strData = comport.ReadExisting().ToString();
                if (strData.Length >= 17)
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
                    ActualKG = dResult - Double.Parse(Au[soau - 1, 1]) - Double.Parse(Total[soau - 1, 1]);
                    ActualKG = Math.Round(ActualKG, 3);
                    if (ActualKG >= minKG && ActualKG <= maxKG)
                    {
                        if (this.InvokeRequired)
                        {
                            SetTextCallback d = new SetTextCallback(SetBtXacNhanEnable);
                            this.Invoke
                                (d, new object[] { soau.ToString() });
                        }
                        /*Total[soau - 1, 1] = (Double.Parse(Total[soau - 1, 1]) + ActualKG).ToString();*/ //Gán chất vừa cân được vào khối lượng tổng của âu
                        return "OK";
                    }
                    else
                    {
                        if (this.InvokeRequired)
                        {
                            SetTextCallback d = new SetTextCallback(SetBtXacNhanDisable);
                            this.Invoke
                                (d, new object[] { soau.ToString() });
                        }
                        choqua = false;
                    }
                }
                else
                    return "Fail";
            }
            catch (Exception ex)
            {
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
                timeout = 0;
                strData = comport.ReadExisting().ToString();
                if (strData != "")
                    strData2 = strData.Substring(strData.Length - 17, 10);
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

        string[] open = new string[2]; //Mo thung
        string[] light = new string[2]; //Mo den
        string result = "";
        public void getMaterial()
        {
            int j = 5;
            int l = 0; //Thu tu plc
            soau = Stt_Can_Temp + 1;
            if (Stt_Temp > 0)
            {
                j = 5 + ((Stt_Temp - 1) * 2);
            }

            for (int i = 0; i < dtBuckets.Rows.Count; i++)
            {
                try
                {
                    open = dtBuckets.Rows[i][2].ToString().Split('.');
                    light = dtBuckets.Rows[i][3].ToString().Split('.');
                    cJ2Compolet1.ForceCancel(OMRON.Compolet.CIP.CJ2Compolet.ForceMemoryTypes.CIO, int.Parse(open[0]), Int16.Parse(open[1]));
                    cJ2Compolet1.ForceCancel(OMRON.Compolet.CIP.CJ2Compolet.ForceMemoryTypes.CIO, int.Parse(light[0]), Int16.Parse(light[1]));
                }
                catch { }
            }

            for (int i = 0; i < dtChooseCTY.Rows.Count; i++)
            {
                for (int k = 0; k < dtBuckets.Rows.Count; k++)
                {
                    try
                    {
                        light = dtBuckets.Rows[k][3].ToString().Split('.');
                        cJ2Compolet1.ForceSet(OMRON.Compolet.CIP.CJ2Compolet.ForceMemoryTypes.CIO, int.Parse(light[0]), Int16.Parse(light[1]));
                    }
                    catch { }
                }

                //Lấy dữ liệu từ cột 5 đến cột 32 tương đương với 14 chất và khối lượng
                for (; j <= 32; j++)
                {
                    //Kiểm tra xem cột hóa chất có bị rỗng không
                    if (dtChooseCTY.Rows[i][j].ToString() != "" && dtChooseCTY.Rows[i][j].ToString() != "0")
                    {
                        //Set den, mo thung plc    
                        try
                        {
                            var row = dtBuckets.Select("MaterialName = '" + dtChooseCTY.Rows[i][j].ToString().Trim() + "'");
                            foreach (var item in row)
                            {
                                open = item[2].ToString().Split('.');
                                light = item[3].ToString().Split('.');
                            }

                            //open = dtBuckets.Rows[l][2].ToString().Split('.');
                            //light = dtBuckets.Rows[l][3].ToString().Split('.');
                        }
                        catch { }


                        try
                        {
                            cJ2Compolet1.ForceSet(OMRON.Compolet.CIP.CJ2Compolet.ForceMemoryTypes.CIO, int.Parse(open[0]), Int16.Parse(open[1]));
                        }
                        catch { }

                        //Disable dong thung button
                        if (this.InvokeRequired)
                        {
                            SetTextCallback d = new SetTextCallback(SetBtDongThungDisable);
                            this.Invoke
                                (d, new object[] { "" });
                        }

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
                        while (choqua == false && soau <= int.Parse(dtChoose.Rows[i]["BatchNo"].ToString()))
                        {
                            //Set label tên nguyên liệu                                                                  
                            if (this.InvokeRequired)
                            {
                                SetTextCallback d = new SetTextCallback(SetLabelNL);
                                this.Invoke
                                    (d, new object[] { dtChooseCTY.Rows[i][j].ToString() });
                            }

                            //Set label số thứ tự hộp
                            if (!quytrinh1)
                                if (this.InvokeRequired)
                                {
                                    SetTextCallback d = new SetTextCallback(SetLabelStt);
                                    this.Invoke
                                        (d, new object[] { soau.ToString() });
                                }

                            for (int u = 0; ; u++)
                            {
                                if (choqua == true && result == "OK")
                                    break;
                                else
                                {
                                    result = COMgetData(dtChooseCTY.Rows[soau - 1][j + 1].ToString());
                                    Thread.Sleep(30);
                                }
                            }

                            ////An nut xac nhan
                            //if (this.InvokeRequired)
                            //{
                            //    SetTextCallback d = new SetTextCallback(SetBtXacNhan);
                            //    this.Invoke
                            //        (d, new object[] { soau.ToString() });
                            //}

                            if (result == "OK" && choqua)
                            {
                                if (!quytrinh1)
                                    Total[soau - 1, 1] = (Double.Parse(Total[soau - 1, 1]) + ActualKG).ToString();
                                else
                                    Total[soau - 1, 1] = 0.ToString();
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
                                if (this.InvokeRequired)
                                {
                                    SetTimerCheckAu d = new SetTimerCheckAu(UpdateTemp);
                                    this.Invoke
                                        (d, new object[] { "4" });
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
                                if (soau > int.Parse(dtChoose.Rows[i]["BatchNo"].ToString()))
                                {
                                    dongthung = false;
                                    //Disable dong thung button
                                    if (this.InvokeRequired)
                                    {
                                        SetTextCallback d2 = new SetTextCallback(SetBtDongThungEnable);
                                        this.Invoke
                                            (d2, new object[] { "" });
                                    }

                                    SetTimerCheckAu d = new SetTimerCheckAu(DongThung);
                                    this.Invoke
                                        (d, new object[] { "" });

                                    for (int u = 0; ; u++)
                                    {
                                        if (dongthung == true)
                                            break;

                                    }
                                }
                                else
                                {
                                    //Set màu cho label số hộp
                                    if (this.InvokeRequired)
                                    {
                                        SetTimerCheckAu d = new SetTimerCheckAu(SetStt);
                                        this.Invoke
                                            (d, new object[] { "" });
                                    }
                                    txtKGR.WaterMark = dtChooseCTY.Rows[soau - 1][j + 1].ToString(); //set số lượng cần
                                    result = "";
                                    choqua = false;
                                }
                            }
                            else
                            {
                                choqua = false;
                            }
                        }
                       
                        if (quytrinh1)
                        {
                            string tenthung = "";
                            DataRow[] r = dtSortMateiral.Select("MaterialName = '" + dtChooseCTY.Rows[i][j].ToString() + "'");
                            foreach (var item in r)
                                tenthung = item["AuNumber"].ToString();
                            dtPrintQT1 = SQL_Conn.CheckPrintQT1(dtChoose.Rows[0]["ManufactureOrderNo"].ToString(), dtChoose.Rows[0]["SoThe"].ToString(), tenthung);
                            if (dtPrintQT1.Rows.Count > 0)
                            {
                                PrintDocument printDocument = new PrintDocument(); //add the document to the dialog box..
                                printDocument.DefaultPageSettings.PaperSize.RawKind = 119;
                                printDocument.PrinterSettings.DefaultPageSettings.PaperSize.RawKind = 119;
                                printDocument.DefaultPageSettings.Landscape = false;
                                printDocument.PrinterSettings.PrinterName = printerName;
                                makeo = dtChoose.Rows[0]["ChemicalOrderCode"].ToString() + dtChoose.Rows[0]["ChemicalOrderCode2"].ToString();
                                thoigianin = DateTime.Now.ToString("HH:mm:ss");
                                ngayin = DateTime.Now.ToString("dd-MM-yyyy");
                                soauhientai = int.Parse(tenthung);
                                r = dtSortMateiral.Select("AuNumber = MAX(AuNumber)");
                                foreach (var item in r)
                                    tongau = int.Parse(item["AuNumber"].ToString());

                                if (double.Parse(dtChoose.Rows[0]["Weight"].ToString()) >= 25)
                                    makeokg = 25.ToString();
                                else
                                {
                                    makeokg = dtChoose.Rows[0]["Weight"].ToString();
                                }

                                printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(CreateReceipt_quytrinh1); //add an event handler that will do the printing
                                                                                                                                       //on a till you will not want to ask the user where to print but this is fine for the test envoironment.
                                printDocument.Print();
                            }
                        }
                       
                        choqua = false;
                        soau = 1; //Reset âu để cân hóa chất tiếp theo
                        if (!quytrinh1)
                            if (this.InvokeRequired)
                            {
                                SetTextCallback d = new SetTextCallback(SetLabelStt);
                                this.Invoke
                                    (d, new object[] { soau.ToString() });
                            }

                        Thread.Sleep(3000);
                        //Close plc
                        try
                        {
                            cJ2Compolet1.ForceCancel(OMRON.Compolet.CIP.CJ2Compolet.ForceMemoryTypes.CIO, int.Parse(open[0]), Int16.Parse(open[1]));
                            cJ2Compolet1.ForceCancel(OMRON.Compolet.CIP.CJ2Compolet.ForceMemoryTypes.CIO, int.Parse(light[0]), Int16.Parse(light[1]));
                        }
                        catch
                        {

                        }
                    }

                    j++;
                    l++;
                    result = "";
                }
            }
            for (int i = 0; i < dtBuckets.Rows.Count; i++)
            {
                try
                {
                    open = dtBuckets.Rows[i][2].ToString().Split('.');
                    light = dtBuckets.Rows[i][3].ToString().Split('.');
                    cJ2Compolet1.ForceCancel(OMRON.Compolet.CIP.CJ2Compolet.ForceMemoryTypes.CIO, int.Parse(open[0]), Int16.Parse(open[1]));
                    cJ2Compolet1.ForceCancel(OMRON.Compolet.CIP.CJ2Compolet.ForceMemoryTypes.CIO, int.Parse(light[0]), Int16.Parse(light[1]));
                }
                catch { }
            }

            bool b = SQL_Conn.updateMachine(dtChooseCTY.Rows[0][0].ToString(), 1); //Cập nhật hoàn thành lệnh

            //Kiểm tra cân trạm 2
            if (this.InvokeRequired)
            {
                SetTimerCheckAu d = new SetTimerCheckAu(SetTimerCheckTram2);
                this.Invoke
                    (d, new object[] { "" });
            }
        }

        //Set dữ liệu lên form khi dùng thread
        delegate void SetTextCallback(string text);
        delegate void SetTimerCheckAu(string text);
        private void SetTimer(string text)
        {
            this.timer3.Enabled = true;
        }

        private void SetTimerCheckTram2(string text)
        {
            this.timer6.Enabled = true;
        }
        private void DongThung(string text)
        {
            timer8.Enabled = true;
        }
        private void SetLabelStt(string text)
        {
            this.lbStt.Text = "Hộp thứ: " + text;
        }
        private void SetLabelNL(string text)
        {
            if (quytrinh1)
            {
                DataRow[] r = dtSortMateiral.Select("MaterialName = '" + text + "'");
                foreach (var item in r)
                    this.lbStt.Text = "Thùng " + item["AuNumber"].ToString();
            }

            this.lbNguyenLieu.Text = "Hóa chất " + text;
        }
        private void SetBtXacNhan(string text)
        {
            timer9.Enabled = true;
        }
        private void SetBtXacNhanEnable(string text)
        {
            btXacNhan.Enabled = true;
        }
        private void SetBtXacNhanDisable(string text)
        {
            btXacNhan.Enabled = false;
        }
        private void SetBtDongThungEnable(string text)
        {
            btDongThung.Enabled = true;
        }
        private void SetBtDongThungDisable(string text)
        {
            btDongThung.Enabled = false;
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
            if (text == "1") //Update tạm bì hộp
                SQL_Conn.updateAuTemp(txtQRCode.Text.ToString(), txtCanAu.WaterMark, txtQRAu.Text, stt.ToString(), 1);
            if (text == "2") //Update tạm số hộp
                SQL_Conn.updateAuTotalTemp(txtQRCode.Text, Total[soau - 1, 1], soau.ToString(), 1);
            if (text == "3") //Update tạp số thứ tự hóa chất
                SQL_Conn.updateSttTemp(txtQRCode.Text, soau.ToString(), 1);
            if (text == "4") //Update chi tiết hóa chất
                SQL_Conn.updateTempDetail(txtQRCode.Text, Total[soau - 1, 0], lbNguyenLieu.Text.Substring(8, lbNguyenLieu.Text.Length - 8).Trim(), ActualKG, 1, username);
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
            dgCty.Refresh();
            dgCty2.Refresh();
            dgCty3.Refresh();
            dgCty4.Refresh();

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
            if (!quytrinh1)
                while (stt <= int.Parse(dtChoose.Rows[0]["BatchNo"].ToString()))
                {
                    timeout = 0;
                    //Đúng chuẩn mã QR Âu
                    if (txtQRAu.Text.Length >= 4)
                    {
                        //Kiểm tra âu lẻ khác 25kg có kí tự cuối là A
                        if (dtChooseCTY.Rows[stt - 1][3].ToString() == "25.000" && txtQRAu.Text.Substring(1, 1) == "U" ||
                            (dtChooseCTY.Rows[stt - 1][3].ToString() != "25.000" && txtQRAu.Text.Substring(1, 1) == "A"))
                        {
                            //Kiểm tra quét trùng Âu
                            for (int i = 0; i < int.Parse(dtChoose.Rows[0]["BatchNo"].ToString()); i++)
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
                                    Thread.Sleep(70); //Dừng 0.2s
                                }

                                //Đủ 10s
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
            else
                while (stt <= int.Parse(dtChoose.Rows[0]["BatchNo"].ToString()))
                {
                    Au[stt - 1, 0] = stt.ToString(); //Gán QR vào mảng Au
                    Au[stt - 1, 1] = "0"; //Gán khối lượng bì của âu
                    Total[stt - 1, 0] = stt.ToString(); //Gán QR vào mảng Total
                    Total[stt - 1, 1] = "0"; //Set khối lượng bằng 0

                    if (this.InvokeRequired)
                    {
                        SetTimerCheckAu d = new SetTimerCheckAu(UpdateTemp);
                        this.Invoke
                            (d, new object[] { "1" });
                    }
                    stt++;
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
        string makeo2;
        string makeokg;
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (stoptime) //Cân thành công 
                {
                    showmsg = false;

                    if(!quytrinh1)
                    {
                        DataTable dtPrint = SQL_Conn.GetPrint(dtChoose.Rows[0][0].ToString());
                        dtTyLe = SQL_Conn.GetTyLe(dtChoose.Rows[0][0].ToString());

                        PrintDocument printDocument = new PrintDocument(); //add the document to the dialog box..
                        printDocument.DefaultPageSettings.PaperSize.RawKind = 119;
                        printDocument.PrinterSettings.DefaultPageSettings.PaperSize.RawKind = 119;
                        printDocument.DefaultPageSettings.Landscape = false;
                        printDocument.PrinterSettings.PrinterName = printerName;
                        makeo = dtChoose.Rows[0]["ChemicalOrderCode"].ToString() + dtChoose.Rows[0]["ChemicalOrderCode2"].ToString();
                        for (int i = 0; i < dtPrint.Rows.Count; i++)
                        {
                            thoigianin = DateTime.Now.ToString("HH:mm:ss");
                            ngayin = DateTime.Now.ToString("dd-MM-yyyy");
                            soauhientai = i + 1;
                            tongau = dtPrint.Rows.Count;
                            maau = dtPrint.Rows[i][2].ToString();
                            dtPrintQT2 = SQL_Conn.SelectCompTram1(dtChoose.Rows[0]["ManufactureOrderNo"].ToString(), maau);
                            tenhop = dtPrint.Rows[i][1].ToString();
                            if (maau.Substring(0, 2) == "AU")
                                makeokg = 25.ToString();
                            else
                            {
                                makeokg = (double.Parse(dtChoose.Rows[0]["Weight"].ToString()) - ((tongau - 1) * 25)).ToString();
                            }
                            khoiluong = dtPrint.Rows[i][3].ToString();
                            printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(CreateReceipt); //add an event handler that will do the printing
                                                                                                                         //on a till you will not want to ask the user where to print but this is fine for the test envoironment.
                            printDocument.Print();
                            Thread.Sleep(1000);
                        }
                    }
                   

                    //Show msgbox
                    mo = "";
                    Au = null;
                    Total = null;
                    soau = 1;
                    Total_Temp = null;
                    Stt_Temp = 0;
                    Stt_Can_Temp = 0;
                    Stt_Au_Temp = 1;
                    QRCode = "";
                    quytrinh1 = false;
                    autrung = false;
                    aule = false;
                    inlai = false;
                    checkAu = false;
                    choqua = false;
                    stoptime = false;
                    btXacNhan.Visible = false;
                    lbSoKG.Visible = false;
                    show = 0;
                    for (int k = 0; k < dgCty.Columns.Count; k++)
                    {
                        dgCty.Columns[k].Visible = true;
                    }

                    for (int k = 0; k < dgCty2.Columns.Count; k++)
                    {
                        dgCty2.Columns[k].Visible = true;
                    }

                    for (int k = 0; k < dgCty3.Columns.Count; k++)
                    {
                        dgCty3.Columns[k].Visible = true;
                    }

                    for (int k = 0; k < dgCty4.Columns.Count; k++)
                    {
                        dgCty4.Columns[k].Visible = true;
                    }
                    dgCty.DataSource = null;
                    dgCty2.DataSource = null;
                    dgCty3.DataSource = null;
                    dgCty4.DataSource = null;

                    COMClose();
                    PLCClose();

                    a.Abort();
                    msg m = new msg("Quá trình cân đã hoàn thành");
                    UCCanBanTuDong_Load(this, null);
                    m.TopMost = true;
                    m.Show();

                    txtQRCode.Text = "";
                    txtQRCode.Focus();
                    txtQRCode.SelectAll();
                    //Tắt timer
                    timer1.Enabled = false;
                }
                else
                {                   
                    if (quytrinh1 && timer6.Enabled == false)
                    {
                        var dtPrintT2 = SQL_Conn.CheckCompTram2(dtChoose.Rows[0]["ManufactureOrderNo"].ToString().Trim());

                        if (dtPrintT2.Rows.Count > 0)
                        {
                            string tenthung = "";
                            DataRow[] r = dtSortMateiral.Select("MaterialName = '" + dtPrintT2.Rows[0]["MaterialCode"].ToString() + "'");
                            foreach (var item in r)
                                tenthung = item["AuNumber"].ToString();

                            if (dtPrintT2.Rows[0]["MaterialCode"].ToString() == "VM56" || dtPrintT2.Rows[0]["MaterialCode"].ToString() == "AS-150")
                            {
                                var countMaterialT2 = SQL_Conn.CountMaterialT2(dtChoose.Rows[0]["ManufactureOrderNo"].ToString(), dtPrintT2.Rows[0]["MaterialCode"].ToString());
                                countLast_hoachat = countMaterialT2.Rows.Count - 1;
                                if (countMaterialT2.Rows.Count > 0)
                                    last_hoachat = countMaterialT2.Rows[0]["MaterialCode"].ToString();
                            }
                            dtDataPrintQT1 = SQL_Conn.SelectCompTram2(dtChoose.Rows[0]["ManufactureOrderNo"].ToString().Trim(), tenthung);

                            PrintDocument printDocument = new PrintDocument(); //add the document to the dialog box..
                            printDocument.DefaultPageSettings.PaperSize.RawKind = 119;
                            printDocument.PrinterSettings.DefaultPageSettings.PaperSize.RawKind = 119;
                            printDocument.DefaultPageSettings.Landscape = false;
                            printDocument.PrinterSettings.PrinterName = printerName;
                            makeo = dtChoose.Rows[0]["ChemicalOrderCode"].ToString() + dtChoose.Rows[0]["ChemicalOrderCode2"].ToString();
                            thoigianin = DateTime.Now.ToString("HH:mm:ss");
                            ngayin = DateTime.Now.ToString("dd-MM-yyyy");
                            soauhientai = int.Parse(tenthung);
                            r = dtSortMateiral.Select("AuNumber = MAX(AuNumber)");
                            foreach (var item in r)
                                tongau = int.Parse(item["AuNumber"].ToString());

                            if (double.Parse(dtChoose.Rows[0]["Weight"].ToString()) >= 25)
                                makeokg = 25.ToString();
                            else
                            {
                                makeokg = dtChoose.Rows[0]["Weight"].ToString();
                            }

                            printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(CreateReceipt_quytrinh1_T2); //add an event handler that will do the printing
                                                                                                                                      //on a till you will not want to ask the user where to print but this is fine for the test envoironment.
                            printDocument.Print();
                        }
                    }
                    //Chỉnh màu cho ô khối lượng
                    if (result != "OK") // Nếu chưa đúng trọng lượng
                    {
                        txtKG.BackColor = Mau[iDem];
                        iDem++;
                        if (iDem == 6) iDem = 0;
                    }
                    else
                    {
                        txtKG.BackColor = Color.DarkGreen;
                    }
                }
            }
            catch
            {
                mo = "";
                Au = null;
                Total = null;
                soau = 1;
                Total_Temp = null;
                Stt_Temp = 0;
                Stt_Can_Temp = 0;
                Stt_Au_Temp = 1;
                QRCode = "";
                quytrinh1 = false;
                autrung = false;
                aule = false;
                inlai = false;
                checkAu = false;
                choqua = false;
                stoptime = false;
                btXacNhan.Visible = false;
                lbSoKG.Visible = false;
                show = 0;
                for (int k = 0; k < dgCty.Columns.Count; k++)
                {
                    dgCty.Columns[k].Visible = true;
                }

                for (int k = 0; k < dgCty2.Columns.Count; k++)
                {
                    dgCty2.Columns[k].Visible = true;
                }

                for (int k = 0; k < dgCty3.Columns.Count; k++)
                {
                    dgCty3.Columns[k].Visible = true;
                }

                for (int k = 0; k < dgCty4.Columns.Count; k++)
                {
                    dgCty4.Columns[k].Visible = true;
                }
                dgCty.DataSource = null;
                dgCty2.DataSource = null;
                dgCty3.DataSource = null;
                dgCty4.DataSource = null;

                COMClose();
                PLCClose();

                a.Abort();
                msg m = new msg("In tem lỗi.");
                UCCanBanTuDong_Load(this, null);
                m.TopMost = true;
                m.Show();

                txtQRCode.Text = "";
                txtQRCode.Focus();
                txtQRCode.SelectAll();
                //Tắt timer
                timer1.Enabled = false;
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
            string top = dtChoose.Rows[0]["ChemicalOrderCode"].ToString() + "_" + dtTyLe.Rows[0]["TyLeMK1"].ToString() + "%";
            graphic.DrawString(top, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 20;
            if (dtChoose.Rows[0]["ChemicalOrderCode2"].ToString() != "")
            {
                string top2 = dtChoose.Rows[0]["ChemicalOrderCode2"].ToString() + "_" + dtTyLe.Rows[0]["TyLeMK2"].ToString() + "%";
                graphic.DrawString(top2, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
                offset = offset + 20;
            }
            graphic.DrawString(makeokg + "kg", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            for(int i = 0; i < dtPrintQT2.Rows.Count; i++)
            {
                string tenchat = dtPrintQT2.Rows[i]["MaterialCode"].ToString();
                string khoiluong = dtPrintQT2.Rows[i]["Weight"].ToString();
                offset = offset + 20;
                graphic.DrawString(tenchat + " - " + khoiluong + "g", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);              
            }
            offset = offset + (int)fontHeight; //make the spacing consistent
            graphic.DrawString("----------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 20; //make some room so that the total stands out.
            graphic.DrawString("Số hộp: " + soauhientai + "/" + tongau, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            if (makeokg != 25.ToString())
                graphic.DrawString("LẺ", new Font("Segoe UI", 30, FontStyle.Bold), new SolidBrush(Color.Black), startX + 150, startY + offset);
            offset = offset + 30;
            graphic.DrawString("Tên hộp: " + tenhop, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            if (inlai == false)
                graphic.DrawString("Mã lệnh: " + txtQRCode.Text, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            else
                graphic.DrawString("Mã lệnh: " + txtQRCode.Text, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Tổng khối lượng: " + khoiluong + "g", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Giờ cân: " + thoigianin, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset); ;
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Ngày cân: " + ngayin, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("----------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30;
            QRCodeGenerator _qrCode = new QRCodeGenerator();
            string qrcode = ""; 
            if (dtChoose.Rows[0]["ChemicalOrderCode2"].ToString() == null || dtChoose.Rows[0]["ChemicalOrderCode2"].ToString() == "")
                qrcode = dtChoose.Rows[0]["ChemicalOrderCode"].ToString() + "," + DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("HHmmss");
            else
                qrcode = dtChoose.Rows[0]["ChemicalOrderCode"].ToString() + dtChoose.Rows[0]["ChemicalOrderCode2"].ToString() + "," + DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("HHmmss");
            QRCodeData _qrCodeData = _qrCode.CreateQrCode(qrcode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(_qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            graphic.DrawImage(qrCodeImage, startX + 60, startY + offset, 100, 100);
            bool b = SQL_Conn.updateQRCode(txtQRCode.Text.ToString(), soauhientai, qrcode, ngayin, thoigianin, khoiluong, 1); //Cập nhật QR code
        }

        string last_hoachat = "";
        int countLast_hoachat = 0;
        string tenhoachatT2 = "";
        string sokgT2 = "";
        string intimeCan = "";
        public void CreateReceipt_quytrinh1_T2(object sender, System.Drawing.Printing.PrintPageEventArgs e)
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
            string top = "";
            if(!inlai)
                top = dtChoose.Rows[0]["ChemicalOrderCode"].ToString() + "_" + dtTyLe.Rows[0]["TyLeMK1"].ToString() + "%"; 
            else
                top = CheckPrint.Rows[0]["ChemicalOrderCode"].ToString() + "_" + dtTyLe.Rows[0]["TyLeMK1"].ToString() + "%";
            graphic.DrawString(top, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 20;
            if (!inlai)
            {
                if (dtChoose.Rows[0]["ChemicalOrderCode2"].ToString() != "")
                {
                    string top2 = dtChoose.Rows[0]["ChemicalOrderCode2"].ToString() + "_" + dtTyLe.Rows[0]["TyLeMK2"].ToString() + "%"; ;
                    graphic.DrawString(top2, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
                    offset = offset + 20;
                }
            }

            else
                if (CheckPrint.Rows[0]["ChemicalOrderCode2"].ToString() != "")
                {
                    string top2 = CheckPrint.Rows[0]["ChemicalOrderCode2"].ToString() + "_" + dtTyLe.Rows[0]["TyLeMK2"].ToString() + "%"; ;
                    graphic.DrawString(top2, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
                    offset = offset + 20;
                }
            graphic.DrawString(makeokg + "kg", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            for(int i = 0; i < dtDataPrintQT1.Rows.Count; i++)
            {
                offset = offset + 20;
                tenhoachatT2 = dtDataPrintQT1.Rows[i]["MaterialCode"].ToString();
                sokgT2 = dtDataPrintQT1.Rows[i]["Weight"].ToString();
                intimeCan = dtDataPrintQT1.Rows[i]["intime"].ToString();
                graphic.DrawString(tenhoachatT2 + " - " + sokgT2 + "g", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            }
           
            if (tenhoachatT2 == last_hoachat)
                countLast_hoachat++;
            else
            {
                last_hoachat = tenhoachatT2;
                countLast_hoachat = 1;
            }
            offset = offset + (int)fontHeight; //make the spacing consistent
            graphic.DrawString("----------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 20; //make some room so that the total stands out.
            if (countLast_hoachat == 1)
                graphic.DrawString("Số thùng: " + soauhientai + "/" + tongau, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            else
                graphic.DrawString("Số thùng: " + soauhientai + "-" + countLast_hoachat.ToString() + "/" + tongau, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            if (makeokg != 25.ToString())
                graphic.DrawString("LẺ", new Font("Segoe UI", 30, FontStyle.Bold), new SolidBrush(Color.Black), startX + 150, startY + offset);
            offset = offset + 30;
            //graphic.DrawString("Tên hộp: " + tenhop, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            //offset = offset + 30; //make some room so that the total stands out.
            if(!inlai)
                graphic.DrawString("Mã lệnh: " + txtQRCode.Text, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            else
                graphic.DrawString("Mã lệnh: " + txtInLai.Text, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            //graphic.DrawString("Tổng khối lượng: " + khoiluong + "g", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            //offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Giờ cân: " + thoigianin, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset); ;
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Ngày cân: " + ngayin, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("----------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30;
            QRCodeGenerator _qrCode = new QRCodeGenerator();
            string qrcode = "";
            if (!inlai)
                if (dtChoose.Rows[0]["ChemicalOrderCode2"].ToString() == null || dtChoose.Rows[0]["ChemicalOrderCode2"].ToString() == "")
                    qrcode = dtChoose.Rows[0]["ChemicalOrderCode"].ToString() + "," + DateTime.Now.ToString("yyyyMMdd") + txtQRCode.Text.Trim().Substring(10, 3);
                else
                    qrcode = dtChoose.Rows[0]["ChemicalOrderCode"].ToString() + dtChoose.Rows[0]["ChemicalOrderCode2"].ToString() + "," + txtQRCode.Text.Trim().Substring(10, 3);
            else
                qrcode = QRCode;

            QRCodeData _qrCodeData = _qrCode.CreateQrCode(qrcode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(_qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            graphic.DrawImage(qrCodeImage, startX + 60, startY + offset, 100, 100);
            if (!inlai)
            {
                bool b = SQL_Conn.updateQRCode(txtQRCode.Text.ToString(), soauhientai, qrcode, ngayin, thoigianin, 0.ToString(), 2); //Cập nhật QR code
                for (int i = 0; i < dtDataPrintQT1.Rows.Count; i++)
                {
                    tenhoachatT2 = dtDataPrintQT1.Rows[i]["MaterialCode"].ToString();
                    sokgT2 = dtDataPrintQT1.Rows[i]["Weight"].ToString();
                    intimeCan = dtDataPrintQT1.Rows[i]["intime"].ToString();
                    b = SQL_Conn.updateCompTram2(txtQRCode.Text.ToString(), tenhoachatT2, double.Parse(sokgT2), intimeCan);
                }
                    
            }
        }
        public void CreateReceipt_quytrinh1(object sender, System.Drawing.Printing.PrintPageEventArgs e)
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
            string top = dtChoose.Rows[0]["ChemicalOrderCode"].ToString() + "_" + dtTyLe.Rows[0]["TyLeMK1"].ToString() + "%"; ;
            graphic.DrawString(top, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 20;
            if (dtChoose.Rows[0]["ChemicalOrderCode2"].ToString() != "")
            {
                string top2 = dtChoose.Rows[0]["ChemicalOrderCode2"].ToString() + "_" + dtTyLe.Rows[0]["TyLeMK2"].ToString() + "%"; ;
                graphic.DrawString(top2, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
                offset = offset + 20;
            }
            graphic.DrawString(makeokg + "kg", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);

            for (int i = 0; i < dtPrintQT1.Rows.Count; i++)
            {
                offset = offset + 20;
                graphic.DrawString(dtPrintQT1.Rows[i]["MaterialName"].ToString() + " - " + dtPrintQT1.Rows[i]["Weight"].ToString() + "g", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            }
            offset = offset + (int)fontHeight; //make the spacing consistent
            graphic.DrawString("----------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 20; //make some room so that the total stands out.
            graphic.DrawString("Số thùng: " + soauhientai + "/" + tongau, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);

            if (makeokg != 25.ToString())
                graphic.DrawString("LẺ", new Font("Segoe UI", 30, FontStyle.Bold), new SolidBrush(Color.Black), startX + 150, startY + offset);
            offset = offset + 30;
            //graphic.DrawString("Tên hộp: " + tenhop, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            //offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Mã lệnh: " + txtQRCode.Text, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            //graphic.DrawString("Tổng khối lượng: " + khoiluong + "g", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            //offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Giờ cân: " + thoigianin, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset); ;
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Ngày cân: " + ngayin, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("----------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30;
            QRCodeGenerator _qrCode = new QRCodeGenerator();
            string qrcode = "";
            if (dtChoose.Rows[0]["ChemicalOrderCode2"].ToString() == null || dtChoose.Rows[0]["ChemicalOrderCode2"].ToString() == "")
                qrcode = dtChoose.Rows[0]["ChemicalOrderCode"].ToString() + "," + DateTime.Now.ToString("yyyyMMdd") + txtQRCode.Text.Trim().Substring(10, 3);
            else
                qrcode = dtChoose.Rows[0]["ChemicalOrderCode"].ToString() + dtChoose.Rows[0]["ChemicalOrderCode2"].ToString() + "," + txtQRCode.Text.Trim().Substring(10, 3);
            QRCodeData _qrCodeData = _qrCode.CreateQrCode(qrcode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(_qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            graphic.DrawImage(qrCodeImage, startX + 60, startY + offset, 100, 100);
            if(!inlai)
            {
                bool b = SQL_Conn.updateQRCode(txtQRCode.Text.ToString(), soauhientai, qrcode, ngayin, thoigianin, 0.ToString(), 1); //Cập nhật QR code
                b = SQL_Conn.updateCompTram1(txtQRCode.Text.ToString(), soauhientai.ToString());
            }
        }

        public void CreateReceipt_InLai(object sender, System.Drawing.Printing.PrintPageEventArgs e)
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


            string top = CheckPrint.Rows[0]["ChemicalOrderCode"].ToString() + "_" + dtTyLe.Rows[0]["TyLeMK1"].ToString() + "%";
            graphic.DrawString(top, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 20;
            if (CheckPrint.Rows[0]["ChemicalOrderCode2"].ToString() != "")
            {
                string top2 = CheckPrint.Rows[0]["ChemicalOrderCode2"].ToString() + "_" + dtTyLe.Rows[0]["TyLeMK2"].ToString() + "%";
                graphic.DrawString(top2, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
                offset = offset + 20;
            }

            graphic.DrawString(makeokg + "kg", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            for (int i = 0; i < dtPrintQT2.Rows.Count; i++)
            {
                string tenchat = dtPrintQT2.Rows[i]["MaterialCode"].ToString();
                string khoiluong = dtPrintQT2.Rows[i]["Weight"].ToString();
                offset = offset + 20;
                graphic.DrawString(tenchat + " - " + khoiluong + "g", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            }

            offset = offset + (int)fontHeight; //make the spacing consistent
            graphic.DrawString("----------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 20; //make some room so that the total stands out.
            graphic.DrawString("Số hộp: " + soauhientai + "/" + tongau, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            if (makeokg != 25.ToString())
                graphic.DrawString("LẺ", new Font("Segoe UI", 30, FontStyle.Bold), new SolidBrush(Color.Black), startX + 150, startY + offset);
            offset = offset + 30;
            graphic.DrawString("Tên hộp: " + tenhop, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Mã lệnh: " + txtInLai.Text, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Tổng khối lượng: " + khoiluong + "g", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Giờ cân: " + thoigianin, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset); ;
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Ngày cân: " + ngayin, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("----------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30;
            QRCodeGenerator _qrCode = new QRCodeGenerator();
            string qrcode = QRCode;

            QRCodeData _qrCodeData = _qrCode.CreateQrCode(qrcode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(_qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            graphic.DrawImage(qrCodeImage, startX + 60, startY + offset, 100, 100);
            //bool b = SQL_Conn.updateQRCode(txtQRCode.Text.ToString(), soauhientai, qrcode, ngayin, thoigianin, khoiluong, 1); //Cập nhật QR code
        }
        public void CreateReceipt_TEst(object sender, System.Drawing.Printing.PrintPageEventArgs e)
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
            string top = "ABCD";
            graphic.DrawString(top, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 20;
            graphic.DrawString("50kg", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)fontHeight; //make the spacing consistent
            graphic.DrawString("----------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 20; //make some room so that the total stands out.
            graphic.DrawString("Số hộp: " + soauhientai + "/" + tongau, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            graphic.DrawString("LẺ", new Font("Segoe UI", 30, FontStyle.Bold), new SolidBrush(Color.Black), startX + 150, startY + offset);
            offset = offset + 30;
            graphic.DrawString("Mã hộp: 1", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Mã lệnh: ABCDEF", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Tổng khối lượng: 100 g", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Giờ cân: ", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset); ;
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Ngày cân: ", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("----------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30;
            QRCodeGenerator _qrCode = new QRCodeGenerator();
            string qrcode = "";
            qrcode = "NAMTEST";
            QRCodeData _qrCodeData = _qrCode.CreateQrCode(qrcode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(_qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            graphic.DrawImage(qrCodeImage, startX + 60, startY + offset, 100, 100);
            //bool b = SQL_Conn.updateQRCode(txtQRCode.Text.ToString(), soauhientai, qrcode, ngayin, thoigianin, khoiluong, 1); //Cập nhật QR code
        }

        int demau = 1;
        private void timer3_Tick(object sender, EventArgs e)
        {
            demau++;
            if (demau == 8)
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
                lbNguyenLieu.BackColor = Color.DarkGreen;
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
                lbStt.BackColor = Color.DarkGreen;
            }
        }

        private void btHuy_Click(object sender, EventArgs e)
        {
            pPrint.Visible = false;
        }

        private void btIn_Click(object sender, EventArgs e)
        {
            pPrint.Visible = true;

        }

        private void btIn2_Click(object sender, EventArgs e)
        {
            DataTable dtPrint = SQL_Conn.GetPrint(txtInLai.Text.Trim());
            QRPrint = SQL_Conn.GetQRPrint(txtInLai.Text.Trim());
            dtTyLe = SQL_Conn.GetTyLe(txtInLai.Text.Trim());

            PrintDocument printDocument = new PrintDocument(); //add the document to the dialog box..
            printDocument.DefaultPageSettings.PaperSize.RawKind = 119;
            printDocument.PrinterSettings.DefaultPageSettings.PaperSize.RawKind = 119;
            printDocument.DefaultPageSettings.Landscape = false;
            printDocument.PrinterSettings.PrinterName = printerName;
            //printDocument.PrinterSettings.PrinterName = "Microsoft Print to PDF";
            //printDocument.PrinterSettings.PrintToFile = true;
            makeo = CheckPrint.Rows[0]["ChemicalOrderCode"].ToString().Trim() + CheckPrint.Rows[0]["ChemicalOrderCode2"].ToString().Trim();
            inlai = true;
            if(!quytrinh1)
                for (int i = 0; i < dtPrint.Rows.Count; i++)
                {
                    soauhientai = i + 1;
                    tongau = dtPrint.Rows.Count;
                    tenhop = dtPrint.Rows[i][1].ToString();
                    maau = dtPrint.Rows[i][2].ToString();
                    dtPrintQT2 = SQL_Conn.SelectCompTram1(CheckPrint.Rows[0]["ManufactureOrderNo"].ToString(), maau);
                    if (maau.Substring(0, 2) == "AU")
                        makeokg = 25.ToString();
                    else
                    {
                        makeokg = (double.Parse(CheckPrint.Rows[0]["Weight"].ToString()) - ((tongau - 1) * 25)).ToString();
                    }
                    khoiluong = dtPrint.Rows[i][3].ToString();
                    var qrPrinted = QRPrint.Select("AuNumber = " + dtPrint.Rows[i][0].ToString());
                    foreach (var item in qrPrinted)
                    {
                        QRCode = item[1].ToString();
                        thoigianin = item[2].ToString();
                        ngayin = item[3].ToString();
                        var dtime = DateTime.ParseExact(ngayin, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        ngayin = dtime.ToString("dd-MM-yyyy");
                    }

                    printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(CreateReceipt_InLai); //add an event handler that will do the printing                                                                                                                       //on a till you will not want to ask the user where to print but this is fine for the test envoironment.
                    printDocument.Print();
                }
            else
            {
                string tenthung = "";
                dtDataPrintQT1 = SQL_Conn.SelectCompTram2Print(CheckPrint.Rows[0]["ManufactureOrderNo"].ToString(), cbChonThung.Text);
                dtSortMateiral = SQL_Conn.GetSortMaterial(CheckPrint.Rows[0]["SoThe"].ToString());
                DataRow[] r = dtSortMateiral.Select("MaterialName = '" + dtDataPrintQT1.Rows[0]["MaterialCode"].ToString() + "'");
                foreach (var item in r)
                    tenthung = item["AuNumber"].ToString();
                makeo = CheckPrint.Rows[0]["ChemicalOrderCode"].ToString().Trim() + CheckPrint.Rows[0]["ChemicalOrderCode2"].ToString().Trim();
                thoigianin = CheckPrint.Rows[0]["intime"].ToString().Trim();
                ngayin = CheckPrint.Rows[0]["indat"].ToString().Trim();
                var dtime = DateTime.ParseExact(ngayin, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                ngayin = dtime.ToString("dd-MM-yyyy");
                soauhientai = int.Parse(tenthung);
                r = dtSortMateiral.Select("AuNumber = MAX(AuNumber)");
                QRCode = QRPrint.Rows[0]["QRCode"].ToString();
                foreach (var item in r)
                    tongau = int.Parse(item["AuNumber"].ToString());

                if (double.Parse(CheckPrint.Rows[0]["Weight"].ToString()) >= 25)
                    makeokg = 25.ToString();
                else
                {
                    makeokg = CheckPrint.Rows[0]["Weight"].ToString();
                }
                printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(CreateReceipt_quytrinh1_T2); //add an event handler that will do the printing                                                                                                                       //on a till you will not want to ask the user where to print but this is fine for the test envoironment.
                printDocument.Print();
            }

            msg a = new msg("In lại thành công");
            a.Show();
            inlai = false;
            txtInLai.Focus();
            txtInLai.Text = "";
            txtInLai.Clear();
            btIn2.Enabled = false;
            countLast_hoachat = 0;
            lbChonThung.Visible = false;
            cbChonThung.Visible = false;
        }

        private void txtQRCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtQRCode.Text.Length >= 13)
                {
                    if (a.IsAlive)
                    {
                        a.Abort();
                    }

                    var AllBuckets = SQL_Conn.SelectRunMO();
                    PLCOpen();
                    for (int i = 0; i < AllBuckets.Rows.Count; i++)
                    {
                        try
                        {
                            open = AllBuckets.Rows[i][2].ToString().Split('.');
                            light = AllBuckets.Rows[i][3].ToString().Split('.');
                            cJ2Compolet1.ForceCancel(OMRON.Compolet.CIP.CJ2Compolet.ForceMemoryTypes.CIO, int.Parse(open[0]), Int16.Parse(open[1]));
                            cJ2Compolet1.ForceCancel(OMRON.Compolet.CIP.CJ2Compolet.ForceMemoryTypes.CIO, int.Parse(light[0]), Int16.Parse(light[1]));
                        }
                        catch { }
                    }

                    dtChoose = SQL_Conn.SelectMO(txtQRCode.Text.ToString());
                    if (dtChoose.Rows.Count == 0)
                    {
                        msg error = new msg("Lệnh sản xuất đã hoàn thành.");
                        error.TopMost = true;
                        error.Show();
                        txtQRCode.Text = "";
                        txtQRCode.Clear();
                        txtQRCode.Focus();
                        txtQRCode.SelectAll();
                    }
                    else
                    {
                        if (!comport.IsOpen)
                            COMOpen();
                        PLCOpen();

                        DataRow[] r = dtChoose.Select(@"SoThe like '%_1'");
                        if (r.Length > 0)
                        {
                            dtSortMateiral = SQL_Conn.GetSortMaterial(dtChoose.Rows[0]["SoThe"].ToString());
                            quytrinh1 = true;
                            dtTyLe = SQL_Conn.GetTyLe(dtChoose.Rows[0][0].ToString());
                        }
                        else
                            quytrinh1 = false;
                        Au = null;
                        Total = null;
                        soau = 1;
                        Total_Temp = null;
                        Stt_Temp = 0;
                        Stt_Can_Temp = 0;
                        Stt_Au_Temp = 1;
                        QRCode = "";
                        autrung = false;
                        aule = false;
                        inlai = false;
                        checkAu = false;
                        stoptime = false;
                        username = Error.User;
                        for (int k = 0; k < dgCty.Columns.Count; k++)
                        {
                            dgCty.Columns[k].Visible = true;     //Hides Column
                        }

                        for (int k = 0; k < dgCty2.Columns.Count; k++)
                        {
                            dgCty2.Columns[k].Visible = true;     //Hides Column
                        }

                        for (int k = 0; k < dgCty3.Columns.Count; k++)
                        {
                            dgCty3.Columns[k].Visible = true;     //Hides  Column
                        }

                        for (int k = 0; k < dgCty4.Columns.Count; k++)
                        {
                            dgCty4.Columns[k].Visible = true;
                        }
                        dgCty.DataSource = null;
                        dgCty2.DataSource = null;
                        dgCty3.DataSource = null;
                        dgCty4.DataSource = null;

                        mo = txtQRCode.Text;
                        btDongThung.Visible = true;
                        btXacNhan.Visible = true;
                        lbMaKeo.Visible = true;
                        lbSoMe.Visible = true;
                        lbSoKG.Visible = true;
                        panelAu.Visible = true; //Hiện panel Âu để cân âu
                        txtQRAu.SelectAll();

                        //Lấy thông tin của lệnh vừa được chọn
                        //var expression = "ManufactureOrderNo = '" + txtQRCode + "'";
                        dtBuckets = SQL_Conn.SelectBuckets(txtQRCode.Text.ToString());


                        dtChooseCTY = SQL_Conn.SelectCommandCTY(txtQRCode.Text.ToString(), 1);
                        MaterialTemp = SQL_Conn.SelectCommandTemp(txtQRCode.Text.ToString(), 1);
                        Au = new string[int.Parse(dtChoose.Rows[0]["BatchNo"].ToString()), 2]; //Khai báo số Âu theo lệnh điều động
                        Total = new string[int.Parse(dtChoose.Rows[0]["BatchNo"].ToString()), 2]; //Khai báo tổng số lượng của từng âu


                        //Gán dữ liệu để quét tiếp nếu bị lỗi cân khi đang quét
                        for (int i = 0; i < MaterialTemp.Rows.Count; i++)
                        {
                            Au[i, 0] = MaterialTemp.Rows[i][1].ToString();
                            Au[i, 1] = MaterialTemp.Rows[i][4].ToString();
                            Total[i, 0] = MaterialTemp.Rows[i][1].ToString();
                            if(quytrinh1)
                                Total[i, 1] = 0.ToString();
                            else
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
                                if (AuNumber == int.Parse(dtChoose.Rows[0]["BatchNo"].ToString()))
                                {
                                    Stt_Temp += 1;
                                    Stt_Can_Temp = 0;
                                }
                                else
                                    Stt_Can_Temp = AuNumber;
                            }
                        }

                        //Gán thông tin keo mà tổng số âu
                        lbSoKG.Text = "Số KG: " + dtChoose.Rows[0]["Weight"].ToString();
                        lbMaKeo.Text = "Mã keo 1: " + dtChoose.Rows[0]["ChemicalOrderCode"].ToString();
                        if (dtChoose.Rows[0]["ChemicalOrderCode2"].ToString() != "")
                        {
                            lbMaKeo2.Visible = true;
                            lbMaKeo2.Text = "Mã keo 2: " + dtChoose.Rows[0]["ChemicalOrderCode2"].ToString();
                        }

                        lbSoMe.Text = "Tổng số hộp: " + dtChoose.Rows[0]["BatchNo"].ToString();

                        //Mở timer 2 kiểm tra Âu
                        timer2.Enabled = true;

                        //Chạy thread để lấy dữ liệu từ cân
                        a = new Thread(checkQRAu);
                        a.IsBackground = true;
                        a.Start();
                    }
                }
            }
            catch { }
        }


        int show = 0;
        msg m;
        private void timer6_Tick(object sender, EventArgs e)
        {
            try
            {
                if (show == 0)
                    m = new msg("Chờ cân trạm 2");
                timeout = 0;
                btDongThung.Visible = false;

                if(quytrinh1)
                {
                    var dtPrintT2 = SQL_Conn.CheckCompTram2(dtChoose.Rows[0]["ManufactureOrderNo"].ToString().Trim());

                    if (dtPrintT2.Rows.Count > 0)
                    {
                        string tenthung = "";
                        DataRow[] r = dtSortMateiral.Select("MaterialName = '" + dtPrintT2.Rows[0]["MaterialCode"].ToString() + "'");
                        foreach (var item in r)
                            tenthung = item["AuNumber"].ToString();

                        if (dtPrintT2.Rows[0]["MaterialCode"].ToString() == "VM56" || dtPrintT2.Rows[0]["MaterialCode"].ToString() == "AS-150")
                        {
                            var countMaterialT2 = SQL_Conn.CountMaterialT2(dtChoose.Rows[0]["ManufactureOrderNo"].ToString(), dtPrintT2.Rows[0]["MaterialCode"].ToString());
                            countLast_hoachat = countMaterialT2.Rows.Count - 1;
                            if (countMaterialT2.Rows.Count > 0)
                                last_hoachat = countMaterialT2.Rows[0]["MaterialCode"].ToString();
                        }
                        dtDataPrintQT1 = SQL_Conn.SelectCompTram2(dtChoose.Rows[0]["ManufactureOrderNo"].ToString().Trim(), tenthung);

                        PrintDocument printDocument = new PrintDocument(); //add the document to the dialog box..
                        printDocument.DefaultPageSettings.PaperSize.RawKind = 119;
                        printDocument.PrinterSettings.DefaultPageSettings.PaperSize.RawKind = 119;
                        printDocument.DefaultPageSettings.Landscape = false;
                        printDocument.PrinterSettings.PrinterName = printerName;
                        makeo = dtChoose.Rows[0]["ChemicalOrderCode"].ToString() + dtChoose.Rows[0]["ChemicalOrderCode2"].ToString();
                        thoigianin = DateTime.Now.ToString("HH:mm:ss");
                        ngayin = DateTime.Now.ToString("dd-MM-yyyy");
                        soauhientai = int.Parse(tenthung);
                        r = dtSortMateiral.Select("AuNumber = MAX(AuNumber)");
                        foreach (var item in r)
                            tongau = int.Parse(item["AuNumber"].ToString());

                        if (double.Parse(dtChoose.Rows[0]["Weight"].ToString()) >= 25)
                            makeokg = 25.ToString();
                        else
                        {
                            makeokg = dtChoose.Rows[0]["Weight"].ToString();
                        }

                        printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(CreateReceipt_quytrinh1_T2); //add an event handler that will do the printing
                                                                                                                                  //on a till you will not want to ask the user where to print but this is fine for the test envoironment.
                        printDocument.Print();
                    }
                }
               
                
                DataTable dt = SQL_Conn.CheckAll(txtQRCode.Text.ToString());
                if (dt.Rows.Count == 0)
                {
                    try
                    {
                        if (!m.IsDisposed)
                            m.Close();
                    }
                    catch { }

                    lbMaKeo2.Visible = false;
                    lbMaKeo.Visible = false;
                    lbSoMe.Visible = false;
                    panelAu.Visible = false;
                    panelCan.Visible = false;
                    SQL_Conn.updateCommand(txtQRCode.Text); //Cập nhật hoàn thành 
                    timer6.Enabled = false;
                    quytrinh1 = false;
                    stoptime = true; //Dừng cân                    
                }
                else
                {
                    if (show == 0)
                    {
                        m.TopMost = true;
                        m.Show();
                        show++;
                    }
                }


            }
            catch { }

        }

        private void UCCanBanTuDong_MouseHover(object sender, EventArgs e)
        {
            timeout = 0;
        }

        private void timer7_Tick(object sender, EventArgs e)
        {
            timeout = timeout + 5;
            if (timeout >= 700)
            {
                timeout = 0;
                MetroLink link = (Form1.Instance.Controls["mlBack"] as MetroLink);
                link.PerformClick();
            }

            if (FormUsed.FormNow == "Main")
            {
                DisableAll();
                timer7.Enabled = false;
                timeout = 0;
            }
        }

        bool dongthung = false;
        private void btDongThung_Click(object sender, EventArgs e)
        {
            try
            {
                dongthung = true;
            }
            catch { }
        }

        int f = 0;
        private void timer8_Tick(object sender, EventArgs e)
        {
            if (dongthung == true || f >= 20)
            {
                f = 0;
                dongthung = true;
                timer8.Enabled = false;

            }
            else
                f = f + 1;
        }

        private void btXacNhan_Click(object sender, EventArgs e)
        {
            choqua = true;
            btXacNhan.Enabled = false;
            Thread.Sleep(3000);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PrintDocument printDocument = new PrintDocument(); //add the document to the dialog box..
            printDocument.DefaultPageSettings.PaperSize.RawKind = 119;
            printDocument.PrinterSettings.DefaultPageSettings.PaperSize.RawKind = 119;
            printDocument.DefaultPageSettings.Landscape = false;
            printDocument.PrinterSettings.PrinterName = "Microsoft Print to PDF";
            printDocument.PrinterSettings.PrintToFile = true;
            printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(CreateReceipt_TEst); //add an event handler that will do the printing
                                                                                                              //on a till you will not want to ask the user where to print but this is fine for the test envoironment.
            printDocument.Print();
        }

        private void txtInLai_TextChanged(object sender, EventArgs e)
        {
            if (txtInLai.Text.Length >= 13)
            {
                CheckPrint = SQL_Conn.CheckPrint(txtInLai.Text);
                if (CheckPrint.Rows.Count > 0)
                {
                    DataRow[] r = CheckPrint.Select(@"SoThe like '%_1'");
                    if (r.Length > 0)
                    {
                        dtSortMateiral = SQL_Conn.GetSortMaterial(CheckPrint.Rows[0]["SoThe"].ToString());
                        quytrinh1 = true;
                        r = dtSortMateiral.Select("AuNumber = MAX(AuNumber)");
                        int sothung = 0;
                        foreach (var item in r)
                            sothung = int.Parse(item["AuNumber"].ToString());
                        for (int i = 1; i <= sothung; i++)
                            cbChonThung.Items.Add(i.ToString());
                        lbChonThung.Visible = true;
                        cbChonThung.Visible = true;
                    }

                   
                    btIn2.Enabled = true;
                }                  
                else
                {
                    msg mes = new msg("Mã lệnh này chưa hoàn thành hoặc không phải hôm nay");
                    mes.Show();
                    txtInLai.Focus();
                    txtInLai.Text = "";
                    txtInLai.Clear();
                    btIn2.Enabled = false;
                }

            }
        }

        private void btInLai_Click(object sender, EventArgs e)
        {
            pPrint.Visible = true;
            txtInLai.Focus();
        }


        int demXacNhan = 0;
        private void timer9_Tick(object sender, EventArgs e)
        {
            btXacNhan.Enabled = false;
            demXacNhan++;
            if (demXacNhan == 5)
            {
                btXacNhan.Enabled = true;
                demXacNhan = 0;
                timer9.Enabled = false;
            }
            else
                btXacNhan.Enabled = false;
        }

        public void DisableAll()
        {
            if (a.IsAlive)
            {
                a.Abort();
            }

            Au = null;
            Total = null;
            soau = 1;
            Total_Temp = null;
            Stt_Temp = 0;
            Stt_Can_Temp = 0;
            Stt_Au_Temp = 1;
            QRCode = "";
            autrung = false;
            aule = false;
            inlai = false;
            checkAu = false;
            stoptime = false;
            timer1.Enabled = false;
            timer2.Enabled = false;
            timer3.Enabled = false;
            timer4.Enabled = false;
            timer5.Enabled = false;
            timer6.Enabled = false;
            timer7.Enabled = false;
            timer8.Enabled = false;
        }
    }
}
