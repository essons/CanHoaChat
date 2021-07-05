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
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using SimpleTCP;
using System.Linq.Expressions;
using System.Windows.Documents;
using System.IO;
using MetroFramework.Controls;

namespace CanHoaChat
{
    public partial class UCCanBanTuDong2 : UserControl
    {
        SimpleTcpClient client;
        public static SerialPort comport = new SerialPort();
        public Thread a = new Thread(openThread); //Tạo 1 thread dùng để chạy ẩn
        public UCCanBanTuDong2()
        {
            InitializeComponent();

        }

        public static void openThread()
        { }

        string moactive = "";
        DataTable dtBucket = new DataTable();
        DataTable dtChoose = new DataTable();
        DataTable dtChooseCTY = new DataTable();
        DataTable MaterialTemp = new DataTable();
        DataTable dtSortMaterial = new DataTable();

        bool quytrinh1 = false;
        string[] sChiaThung = new string[3];
        string ComName = "";
        string[,] Au;
        string[,] Total;
        int Stt_Temp = 0;
        int Stt_Can_Temp = 0;
        int Stt_Au_Temp = 1;
        int soau = 1;
        int run = 0;
        string result = "";
        bool can = false;
        bool choqua = false;
        double ActualKG = 0;
        private void CanBanTuDong2_Load(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader("ComCan2.txt");
            ComName = sr.ReadLine().Trim();
            sr.Close();

            this.MaximumSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            this.Size = this.MaximumSize;

            COMOpen();

            //client = new SimpleTcpClient();
            //client.StringEncoder = Encoding.UTF8;
            //client.DataReceived += Client_DataReceived;
        }

        private void Client_DataReceived(object sender, SimpleTCP.Message e)
        {
            //try
            //{
            //    txtTest.Invoke((MethodInvoker)delegate ()
            //    {
            //        MO = e.MessageString.Substring(0,13);
            //        e.ReplyLine("GetMO");
            //        client.Disconnect();
            //    });
            //}
            //catch { }
        }

        //msg m = new msg("Không kết nối được cân điện");
        public void COMOpen()
        {
            if (comport.IsOpen == true)
                comport.Close();

            try
            {
                string strCOMname = ComName;
                comport.PortName = strCOMname.Trim();
                comport.BaudRate = 9600;
                comport.Parity = System.IO.Ports.Parity.None;
                comport.DataBits = 8;
                comport.StopBits = System.IO.Ports.StopBits.One;
                comport.Open();
                can = true;
            }
            catch (Exception ex)
            {
                can = false;
            }
        }
        public void COMClose()
        {
            if (comport.IsOpen == true)
                comport.Close();
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

        msg m2 = new msg("Đang chờ lệnh cân");
        //msg m3 = new msg("Không kết nối được cân trạm 1");
        private void timer1_Tick(object sender, EventArgs e)
        {            
            dtChoose = SQL_Conn.SelectMOActive();
            
            if (run == 0 && dtChoose.Rows.Count > 0 )
            {
                try
                {
                    if (!m2.IsDisposed)
                        m2.Close();
                }
                catch { }

                if(!comport.IsOpen)
                    COMOpen();

                PLCOpen();

                timeout = 0;
                run = 1;
                if (a.IsAlive)
                {
                    a.Abort();
                }

                DataRow[] r = dtChoose.Select(@"SoThe like '%_1'");
                if (r.Length > 0)
                {
                    dtSortMaterial = SQL_Conn.GetSortMaterial(dtChoose.Rows[0]["SoThe"].ToString());
                    quytrinh1 = true;
                }
                else
                    quytrinh1 = false;

                Au = null;
                Total = null;
                soau = 1;
                Stt_Temp = 0;
                Stt_Can_Temp = 0;
                Stt_Au_Temp = 1;
                btXacNhan.Visible = true;
                lbMaKeo.Visible = true;
                lbSoMe.Visible = true;
                lbSoKG.Visible = true;
                lbMaLenh.Visible = true;
                for (int k = 0; k < dgCty.Columns.Count; k++)
                {
                        dgCty.Columns[k].Visible = true;     //Hides Column
                }

                for (int k = 0; k < dgCty2.Columns.Count; k++)
                {
                        dgCty2.Columns[k].Visible = true;     //Hides Column
                }
                dgCty.DataSource = null;
                dgCty2.DataSource = null; 

                lbMaLenh.Text = "Mã lệnh: " + dtChoose.Rows[0][0].ToString();
                dtBucket = SQL_Conn.SelectBuckets2(dtChoose.Rows[0][0].ToString());
                dtChooseCTY = SQL_Conn.SelectCommandCTY(dtChoose.Rows[0][0].ToString(), 2);
                MaterialTemp = SQL_Conn.SelectCommandTemp(dtChoose.Rows[0][0].ToString(), 2);
                Au = new string[int.Parse(dtChoose.Rows[0]["BatchNo"].ToString()), 2]; //Khai báo số Âu theo lệnh điều động
                Total = new string[int.Parse(dtChoose.Rows[0]["BatchNo"].ToString()), 2]; //Khai báo tổng số lượng của từng âu

                //Gán dữ liệu để quét tiếp nếu bị lỗi cân khi đang quét
                for (int i = 0; i < MaterialTemp.Rows.Count; i++)
                {
                    Au[i, 0] = MaterialTemp.Rows[i][1].ToString();
                    Au[i, 1] = MaterialTemp.Rows[i][4].ToString();
                    Total[i, 0] = MaterialTemp.Rows[i][1].ToString();
                    Total[i, 1] = MaterialTemp.Rows[i][5].ToString();

                    int ChemicalStt = int.Parse(MaterialTemp.Rows[i][3].ToString());
                    int AuNumber = int.Parse(MaterialTemp.Rows[i][2].ToString());
                    if (AuNumber >= Stt_Au_Temp && double.Parse(Total[i, 1]) != 0)
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
                lbSoKG.Text = "Số kg: " + dtChoose.Rows[0]["Weight"].ToString();
                lbMaKeo.Text = "Mã keo 1: " + dtChoose.Rows[0]["ChemicalOrderCode"].ToString();
                if(dtChoose.Rows[0]["ChemicalOrderCode2"].ToString() != "")
                {
                    lbMaKeo2.Text = "Mã keo 2: " + dtChoose.Rows[0]["ChemicalOrderCode2"].ToString();
                    lbMaKeo2.Visible = true;
                }
                
                lbSoMe.Text = "Tổng số hộp: " + dtChoose.Rows[0]["BatchNo"].ToString();

                lbNguyenLieu.Visible = true;
                lbStt.Visible = true;
                txtKG.Visible = true;
                txtKGR.Visible = true;
                panelCan.Visible = true;

                soau = 1; //Set số âu về 1 để cân hóa chất
                a = new Thread(getMaterial);
                a.IsBackground = true;
                a.Start();
                //Mở timer 1 cân hóa chất
                // timer1.Enabled = true;
                timer4.Enabled = true;
                timer1.Enabled = false;
                timer1.Interval = 60000;
            }
            else
            {
                COMClose();
                try
                {
                    m2.Dispose();
                }
                catch { }

                try
                {
                    if(!can)
                    {
                        m2 = new msg("Không kết nối được cân điện");
                        m2.TopMost = true;
                        m2.Show();
                    }
                    else
                    {
                        m2 = new msg("Đang chờ lệnh cân");
                        m2.TopMost = true;
                        m2.Show();
                    }
                }
                catch
                {

                }
                timer1.Interval = 60000;
            }            
        }

        long RoundingTo(long myNum, long roundTo)
        {
            if (roundTo <= 0) return myNum;
            return (myNum + roundTo / 2) / roundTo * roundTo;
        }

        long RoundingTo(long myNum, int roundTo)
        {
            return RoundingTo(myNum, (long)roundTo);
        }

        int RoundingTo(int myNum, int roundTo)
        {
            return (int)RoundingTo((long)myNum, (long)roundTo);
        }
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
                if (strData.Length > 0)
                {
                    strData2 = strData.Substring(strData.Length - 12, 7);
                    //strData2 = strData.Substring(7, strData.Length - 7);
                    dResult = Convert.ToDouble(strData2) * 1000;
                    if (dResult >= 0)
                    {
                        //Lấy số lượng hiện tại bằng số lượng cân trừ đi trọng lượng âu và tổng trọng lượng các chát hiện có của Au
                        double slhientai = dResult - Double.Parse(Total[soau - 1, 1]);
                        slhientai = Math.Round(slhientai, 2);
                        if (slhientai > 0)
                            txtKG.WaterMark = slhientai.ToString();
                        else
                            txtKG.WaterMark = "0";
                    }
                    //Lấy xác suất lệch cho phép là 1%
                    var minKG = Double.Parse(kg) - Double.Parse(kg) / 100;
                    var maxKG = Double.Parse(kg) + Double.Parse(kg) / 100;

                    //Làm tròn số 371 372 373 thành 370
                    long so, tronDV;
                    so = (long)minKG;
                    tronDV = RoundingTo(so, 10); 
                    minKG = Double.Parse(tronDV.ToString());
                    so = (long)maxKG;
                    tronDV = RoundingTo(so, 10); 
                    maxKG = Double.Parse(tronDV.ToString());

                    ActualKG = dResult - Double.Parse(Total[soau - 1, 1]);
                    ActualKG = Math.Round(ActualKG, 2);
                    if (ActualKG >= minKG && ActualKG <= maxKG)
                    {
                        if (this.InvokeRequired)
                        {
                            SetTextCallback d = new SetTextCallback(SetBtXacNhanEnable);
                            this.Invoke
                                (d, new object[] { soau.ToString() });
                        }
                        //Total[soau - 1, 1] = (Double.Parse(Total[soau - 1, 1]) + ActualKG).ToString(); //Gán chất vừa cân được vào khối lượng tổng của âu
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

        string[] open = new string[2]; //Mo thung
        string[] light = new string[2]; //Mo den
        public void getMaterial()
        {
            int chiathung = 0;
            int demthung = 0;
            int countLastHoaChat = 0;
            int j = 5;
            soau = Stt_Can_Temp + 1;
            if (Stt_Temp > 0)
            {
                j = 5 + ((Stt_Temp - 1) * 2);
            }

            for (int i = 0; i < dtChooseCTY.Rows.Count; i++)
            {
                for (int k = 0; k < dtBucket.Rows.Count; k++)
                {
                    try
                    {
                        light = dtBucket.Rows[k][3].ToString().Split('.');
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
                            var row = dtBucket.Select("MaterialName = '" + dtChooseCTY.Rows[i][j].ToString().Trim() + "'");
                            foreach (var item in row)
                            {
                                open = item[2].ToString().Split('.');
                                light = item[3].ToString().Split('.');
                            }
                        }
                        catch { }


                        try
                        {
                            cJ2Compolet1.ForceSet(OMRON.Compolet.CIP.CJ2Compolet.ForceMemoryTypes.CIO, int.Parse(open[0]), Int16.Parse(open[1]));
                        }
                        catch { }

                        timeout = 0;
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
                        if(!quytrinh1)
                            while (choqua == false && soau <= int.Parse(dtChoose.Rows[i]["BatchNo"].ToString()))
                        {
                            //Set label số thứ tự hộp
                            if (this.InvokeRequired)
                            {
                                SetTextCallback d = new SetTextCallback(SetLabelStt);
                                this.Invoke
                                    (d, new object[] { soau.ToString() });
                            }

                                //Set label tên nguyên liệu
                                if (this.InvokeRequired)
                                {
                                    SetTextCallback d = new SetTextCallback(SetLabelNL);
                                    this.Invoke
                                        (d, new object[] { dtChooseCTY.Rows[i][j].ToString() });
                                }

                                for (int u = 0; ; u++)
                            {
                                if (choqua == true && result == "OK")
                                    break;
                                else
                                {
                                    result = COMgetData(dtChooseCTY.Rows[soau - 1][j + 1].ToString());
                                    Thread.Sleep(60);
                                }
                            }

                            //Ẩn nút xác nhận
                            if (this.InvokeRequired)
                            {
                                SetTextCallback d = new SetTextCallback(SetBtXacNhan);
                                this.Invoke
                                    (d, new object[] { "" });
                            }

                            //Lấy liệu từ cổng COM
                            if (result == "OK" && choqua)
                            {
                                Total[soau - 1, 1] = (Double.Parse(Total[soau - 1, 1]) + ActualKG).ToString();
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
                                    //result = "OK";
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
                                choqua = false;
                        }
                        else
                        {
                            if (dtChooseCTY.Rows[i][j].ToString().Trim() == "AS-150" || dtChooseCTY.Rows[i][j].ToString().Trim() == "VM56")
                            {
                                if(dtChooseCTY.Rows[i][j].ToString().Trim() == "AS-150")
                                {
                                    var countMaterialT2 = SQL_Conn.CountMaterialT2(dtChoose.Rows[0]["ManufactureOrderNo"].ToString(), dtChooseCTY.Rows[i][j].ToString().Trim());
                                    countLastHoaChat = countMaterialT2.Rows.Count;

                                    if (double.Parse(dtChooseCTY.Rows[soau - 1][j + 1].ToString()) < 4000)
                                    {
                                        chiathung = 1;
                                        sChiaThung[0] = dtChooseCTY.Rows[soau - 1][j + 1].ToString();
                                    }

                                    if (double.Parse(dtChooseCTY.Rows[soau - 1][j + 1].ToString()) < 8000 && double.Parse(dtChooseCTY.Rows[soau - 1][j + 1].ToString()) > 4000)
                                    {
                                        chiathung = 2;
                                        sChiaThung[0] = "4000";
                                        sChiaThung[1] = (double.Parse(dtChooseCTY.Rows[soau - 1][j + 1].ToString()) - 4000).ToString();
                                    }
                                    
                                    if (double.Parse(dtChooseCTY.Rows[soau - 1][j + 1].ToString()) >= 8000)
                                    {
                                        chiathung = 3;
                                        sChiaThung[0] = "4000";
                                        sChiaThung[1] = "4000";
                                        sChiaThung[2] = (double.Parse(dtChooseCTY.Rows[soau - 1][j + 1].ToString()) - 8000).ToString();
                                    }                                    
                                }

                                if (dtChooseCTY.Rows[i][j].ToString().Trim() == "VM56")
                                {
                                    var countMaterialT2 = SQL_Conn.CountMaterialT2(dtChoose.Rows[0]["ManufactureOrderNo"].ToString(), dtChooseCTY.Rows[i][j].ToString().Trim());
                                    countLastHoaChat = countMaterialT2.Rows.Count;
                                    if (double.Parse(dtChooseCTY.Rows[soau - 1][j + 1].ToString()) <= 6400)
                                    {
                                        chiathung = 1;
                                        sChiaThung[0] = dtChooseCTY.Rows[soau - 1][j + 1].ToString();
                                    }

                                    if (double.Parse(dtChooseCTY.Rows[soau - 1][j + 1].ToString()) < 12800 && double.Parse(dtChooseCTY.Rows[soau - 1][j + 1].ToString()) > 6400)
                                    {
                                        chiathung = 2;
                                        sChiaThung[0] = "6400";
                                        sChiaThung[1] = (double.Parse(dtChooseCTY.Rows[soau - 1][j + 1].ToString()) - 6400).ToString();
                                    }

                                    if (double.Parse(dtChooseCTY.Rows[soau - 1][j + 1].ToString()) >= 12800)
                                    {
                                        chiathung = 3;
                                        sChiaThung[0] = "6400";
                                        sChiaThung[1] = "6400";
                                        sChiaThung[2] = (double.Parse(dtChooseCTY.Rows[soau - 1][j + 1].ToString()) - 12800).ToString();
                                    }

                                }
                                demthung = countLastHoaChat;
                                txtKGR.WaterMark = sChiaThung[demthung];
                            }
                            
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
                                if(!quytrinh1)
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
                                        result = COMgetData(txtKGR.WaterMark);
                                        Thread.Sleep(60);
                                    }
                                }

                                //Ẩn nút xác nhận
                                if (this.InvokeRequired)
                                {
                                    SetTextCallback d = new SetTextCallback(SetBtXacNhan);
                                    this.Invoke
                                        (d, new object[] { "" });
                                }

                                //Lấy liệu từ cổng COM
                                if (result == "OK" && choqua)
                                {
                                    Total[soau - 1, 1] = "0";
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
                                            (d, new object[] { "4" });
                                    }
                                    //Thay đổi khổi lượng gridview của từng hộp
                                    if (this.InvokeRequired)
                                    {
                                        SetTextCallback d = new SetTextCallback(setDgv);
                                        this.Invoke
                                            (d, new object[] { (soau - 1).ToString() });
                                    }

                                    if (dtChooseCTY.Rows[i][j].ToString().Trim() == "AS-150" || dtChooseCTY.Rows[i][j].ToString().Trim() == "VM56")
                                    {
                                        demthung++;
                                        if (chiathung == demthung)
                                        {
                                            countLastHoaChat = 1;
                                            if (this.InvokeRequired)
                                            {
                                                SetTimerCheckAu d = new SetTimerCheckAu(UpdateTemp);
                                                this.Invoke
                                                    (d, new object[] { "3" });
                                            }
                                            break;
                                        }                                           
                                    }    
                                    else
                                    {
                                        if (this.InvokeRequired)
                                        {
                                            SetTimerCheckAu d = new SetTimerCheckAu(UpdateTemp);
                                            this.Invoke
                                                (d, new object[] { "3" });
                                        }
                                        soau++;
                                    }
                                        
                                    //Cân đủ khối lượng chuyển sang chất kế tiếp   
                                    if (soau > int.Parse(dtChoose.Rows[i]["BatchNo"].ToString()))
                                    {
                                        //result = "OK";
                                    }
                                    else
                                    {
                                        //Set màu cho label số hộp
                                        if (!quytrinh1)
                                        {
                                            if (this.InvokeRequired)
                                            {
                                                SetTimerCheckAu d = new SetTimerCheckAu(SetStt);
                                                this.Invoke
                                                    (d, new object[] { "" });
                                            }

                                            txtKGR.WaterMark = dtChooseCTY.Rows[soau - 1][j + 1].ToString(); //set số lượng cần
                                        }
                                        else
                                        {
                                            if (dtChooseCTY.Rows[i][j].ToString().Trim() == "AS-150" || dtChooseCTY.Rows[i][j].ToString().Trim() == "VM56")
                                                txtKGR.WaterMark = sChiaThung[demthung];
                                            else
                                                txtKGR.WaterMark = dtChooseCTY.Rows[soau - 1][j + 1].ToString();
                                        }
                                            
                                        result = "";
                                        choqua = false;
                                    }
                                }
                                else
                                    choqua = false;
                            }
                        }

                        choqua = false;
                        chiathung = 0;
                        demthung = 0;
                        soau = 1; //Reset âu để cân hóa chất tiếp theo
                        if(!quytrinh1)
                            if (this.InvokeRequired)
                            {
                                SetTextCallback d = new SetTextCallback(SetLabelStt);
                                this.Invoke
                                    (d, new object[] { soau.ToString() });
                            }

                        Thread.Sleep(3000);
                        //Close plc
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
                    result = "";
                }
                
            }

            bool b = SQL_Conn.updateMachine(dtChooseCTY.Rows[0][0].ToString(), 2); //Cập nhật hoàn thành lệnh
            quytrinh1 = false;
            if (this.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetEnd);
                this.Invoke
                    (d, new object[] { "" });
            }
        }

        delegate void SetTextCallback(string text);
        delegate void SetTimerCheckAu(string text);

        //private void SetButton(string text)
        //{
        //    this.lbStt.Text = "Hộp thứ: " + text;
        //}
        private void SetLabelStt(string text)
        {
            this.lbStt.Text = "Hộp thứ: " + text;
        }
        private void SetLabelNL(string text)
        {
            if (quytrinh1)
            {
                DataRow[] r = dtSortMaterial.Select("MaterialName = '" + text + "'");
                foreach (var item in r)
                    this.lbStt.Text = "Thùng " + item["AuNumber"].ToString();
            }
            this.lbNguyenLieu.Text = "Hóa chất " + text;
            var row = dtBucket.Select("MaterialName = '" + text + "'");
            this.lbBon.Text = "Bồn: " + row[0]["BucketID"].ToString();
        }
        private void UpdateTemp(string text)
        {
            if (text == "2") //Update tạm số hộp
                SQL_Conn.updateAuTotalTemp(dtChoose.Rows[0][0].ToString(), Total[soau - 1, 1], soau.ToString(), 2);
            if (text == "3") //Update tạp số thứ tự hóa chất
                SQL_Conn.updateSttTemp(dtChoose.Rows[0][0].ToString(), soau.ToString(), 2);
            if (text == "4") //Update chi tiết hóa chất
                SQL_Conn.updateTempDetail(dtChoose.Rows[0][0].ToString(), Total[soau - 1, 0], lbNguyenLieu.Text.Substring(8,lbNguyenLieu.Text.Length - 8).Trim(), ActualKG, 2, "");
        }
        private void SetNL(string text)
        {
            timer2.Enabled = true;
        }
        private void SetBtXacNhan(string text)
        {
            timer6.Enabled = true;
        }
        private void SetBtXacNhanEnable(string text)
        {
            btXacNhan.Enabled = true;
        }
        private void SetBtXacNhanDisable(string text)
        {
            btXacNhan.Enabled = false;
        }
        private void SetStt(string text)
        {
            timer3.Enabled = true;
        }
        private void setDgv(string text)
        {
            int i = int.Parse(text);
            //Thêm dữ liệu vào datagridview show thông tin hóa chất
            dgCty.Rows.Clear();
            dgCty2.Rows.Clear();
            dgCty.Refresh();
            dgCty2.Refresh();
            //Kiểm tra hóa chất
            string m1 = dtChooseCTY.Rows[0]["Material01"].ToString();
            string m2 = dtChooseCTY.Rows[0]["Material02"].ToString();
            string m3 = dtChooseCTY.Rows[0]["Material03"].ToString();
            string m4 = dtChooseCTY.Rows[0]["Material04"].ToString();
            string m5 = dtChooseCTY.Rows[0]["Material05"].ToString();
            string m6 = dtChooseCTY.Rows[0]["Material06"].ToString();
            string m7 = dtChooseCTY.Rows[0]["Material07"].ToString();
            string m8 = dtChooseCTY.Rows[0]["Material08"].ToString();


            string w1 = dtChooseCTY.Rows[i]["weight01"].ToString();
            string w2 = dtChooseCTY.Rows[i]["weight02"].ToString();
            string w3 = dtChooseCTY.Rows[i]["weight03"].ToString();
            string w4 = dtChooseCTY.Rows[i]["weight04"].ToString();
            string w5 = dtChooseCTY.Rows[i]["weight05"].ToString();
            string w6 = dtChooseCTY.Rows[i]["weight06"].ToString();
            string w7 = dtChooseCTY.Rows[i]["weight07"].ToString();
            string w8 = dtChooseCTY.Rows[i]["weight08"].ToString();

            dgCty.Rows.Add(m1, w1, m2, w2, m3, w3, m4, w4);
            dgCty2.Rows.Add(m5, w5, m6, w6, m7, w7, m8, w8);

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
        }
        private void SetEnd(string text)
        {
            timer4.Enabled = false;
            timer1.Interval = 60000;
            timer1.Enabled = true;
            panelCan.Visible = false;
            lbMaKeo.Visible = false;
            lbMaKeo2.Visible = false;
            lbSoMe.Visible = false;
            lbMaLenh.Visible = false;
            btXacNhan.Visible = false;
            lbSoKG.Visible = false;
            run = 0;
        }
        private void ShowMsg(string text)
        {
            msgPrintError m = new msgPrintError("Không kết nối được cân điện. Bạn có muốn tiếp tục cân không ?");
            m.Show();
        }

        int baohieu = 0;
        private void timer2_Tick(object sender, EventArgs e)
        {
            baohieu++;
            if (baohieu == 2)
            {
                lbNguyenLieu.BackColor = Color.White;
                baohieu = 0;
                timer2.Enabled = false;
            }
            else
            {
                lbNguyenLieu.BackColor = Color.DarkGreen;
            }
        }
        private void timer3_Tick(object sender, EventArgs e)
        {
            baohieu++;
            if (baohieu == 2)
            {
                lbStt.BackColor = Color.White;
                baohieu = 0;
                timer3.Enabled = false;
            }
            else
            {
                lbStt.BackColor = Color.DarkGreen;
            }
        }

        Color[] Mau = { Color.DodgerBlue, Color.DodgerBlue, Color.LightSkyBlue, Color.LightSkyBlue, Color.White, Color.White };
        int iDem = 0;
        private void timer4_Tick(object sender, EventArgs e)
        {
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

        int timeout = 0;
        private void timer5_Tick(object sender, EventArgs e)
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
                timer5.Enabled = false;
                timeout = 0;
                if(a.IsAlive)
                    a.Abort();
            }
        }

        public void DisableAll()
        {
            if (a.IsAlive)
            {
                a.Abort();
            }

            timer1.Enabled = false;
            timer2.Enabled = false;
            timer3.Enabled = false;
            timer4.Enabled = false;
            timer5.Enabled = false;
            timer6.Enabled = false;

        }

        private void UCCanBanTuDong2_MouseHover(object sender, EventArgs e)
        {
            timeout = 0;
        }

        private void btXacNhan_Click(object sender, EventArgs e)
        {
            choqua = true;
            btXacNhan.Enabled = false;
            Thread.Sleep(5000);
        }

        int demXacNhan = 0;
        private void timer6_Tick(object sender, EventArgs e)
        {
            btXacNhan.Enabled = false;
            demXacNhan++;
            if (demXacNhan == 10)
            {
                btXacNhan.Enabled = true;
                demXacNhan = 0;
                timer6.Enabled = false;
            }
            else
                btXacNhan.Enabled = false;
        }
    }
}
