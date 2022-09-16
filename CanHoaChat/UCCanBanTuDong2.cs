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
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace CanHoaChat
{
    public partial class UCCanBanTuDong2 : UserControl
    {
        public static SerialPort comport = new SerialPort();
        public Thread a = new Thread(openThread); //Tạo 1 thread dùng để chạy ẩn
        HttpClientHandler handler = new HttpClientHandler();
        HttpClient client;
        HttpResponseMessage response;
        string tenthung = "";
        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;
            else
            {
                return true;
            }
        }

        public DataTable GetDataTableFromJsonString(string json)
        {
            var jsonLinq = JObject.Parse(json);

            // Find the first array using Linq
            var srcArray = jsonLinq.Descendants().Where(d => d is JArray).First();
            var trgArray = new JArray();
            foreach (JObject row in srcArray.Children<JObject>())
            {
                var cleanRow = new JObject();
                foreach (JProperty column in row.Properties())
                {
                    // Only include JValue types
                    if (column.Value is JValue)
                    {
                        cleanRow.Add(column.Name, column.Value);
                    }
                }
                trgArray.Add(cleanRow);
            }

            return JsonConvert.DeserializeObject<DataTable>(trgArray.ToString());
        }
        public UCCanBanTuDong2()
        {
            InitializeComponent();
        }

        public static void openThread()
        { }

        string moactive = "";
        DataTable dtBucket = new DataTable();
        DataTable dtChoose = new DataTable();
        DataTable dtTarget = new DataTable();
        DataTable dtChooseCTY = new DataTable();
        DataTable dtMaterialDetail = new DataTable();
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
        int aunumber = 0;
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

            ServicePointManager.ServerCertificateValidationCallback =
                   new RemoteCertificateValidationCallback(ValidateServerCertificate);

            ServicePointManager.ServerCertificateValidationCallback =
                    new RemoteCertificateValidationCallback(ValidateServerCertificate);
            try
            {
                handler.Credentials = new NetworkCredential("test", "testing");
            }
            catch { }

            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://srv-epi-app/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.
                MediaTypeWithQualityHeaderValue("application/json"));
            var byteArray = Encoding.ASCII.GetBytes($"{"itdev"}:{"P@ssw0rd123"}");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            COMOpen();
        }

        //msg m = new msg("Không kết nối được cân điện");
        public void COMOpen()
        {
            if (comport.IsOpen == true)
                comport.Close();

            try
            {
                string strCOMname = ComName;
                comport.PortName = strCOMname;
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

        DataTable dtCheckOnHand = new DataTable();
        public void Issue_Material(string CheNum, double REALISSUE)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress + "ERP_LIVE/api/v1/Erp.BO.IssueReturnSvc/PerformMaterialMovement");
            string LOTNO = "";
            double REQCHE = 0;
            double REALONHAND = 0;
            string BINNUM = "";
            string WAREHOUSECODE = "";
            string Issued = "";
            int JobMtl_MtlSeq = 0;
            DataRow[] r = dtCheckOnHand.Select("JobMtl_PartNum = '" + CheNum + "'", "PartBin_OnhandQty ASC");
            Issued = r[0]["Calculated_SumQty"].ToString();
            REQCHE = double.Parse(r[0]["JobMtl_RequiredQty"].ToString());
            JobMtl_MtlSeq = int.Parse(r[0]["JobMtl_MtlSeq"].ToString());
            foreach (var ir in r)
            {
                REALONHAND = double.Parse(ir["PartBin_OnhandQty"].ToString());
                LOTNO = ir["PartBin_LotNum"].ToString();
                BINNUM = ir["PartBin_BinNum"].ToString();
                WAREHOUSECODE = ir["PartBin_WarehouseCode"].ToString();

                var json = @"{
                                ""plNegQtyAction"": true,
                                ""ds"": {
                                ""IssueReturn"": [
                                  {
                                    ""Company"": ""ES168899"",
                                    ""TranDate"": """ + DateTime.Now.ToString("yyyy-MM-dd") + @"T00:00:00+07:00"",
                                    ""PartNum"": """ + CheNum + @""",
                                    ""TranQty"": """ + REALISSUE.ToString() + @""",
                                    ""DimCode"": ""KG"",
                                    ""LotNum"": """ + LOTNO + @""",
                                    ""ReasonCode"": """",
                                    ""OrderNum"": 0,
                                    ""OrderLine"": 0,
                                    ""OrderRel"": 0,
                                    ""FromJobNum"": """",
                                    ""FromAssemblySeq"": 0,
                                    ""FromJobSeq"": 0,
                                    ""FromWarehouseCode"": """ + WAREHOUSECODE + @""",
                                    ""FromBinNum"": """ + BINNUM + @""",
                                    ""OnHandQty"": """ + REALONHAND.ToString() + @""",
                                    ""QtyRequired"": """ + REQCHE.ToString() + @""",
                                    ""IssuedComplete"": false,
                                    ""ToJobNum"": """ + ir["JobMtl_JobNum"].ToString() + @""",
                                    ""ToAssemblySeq"": 0,
                                    ""ToJobSeq"": " + JobMtl_MtlSeq + @",
                                    ""ToWarehouseCode"": """ + WAREHOUSECODE + @""",
                                    ""ToBinNum"": """ + BINNUM + @""",
                                    ""ToJobPartNum"": """ + ir["JobHead_PartNum"].ToString() + @""",
                                    ""ToAssemblyPartNum"": """ + ir["JobHead_PartNum"].ToString() + @""",
                                    ""ToJobSeqPartNum"": """ + CheNum + @""",
                                    ""MtlQueueRowId"": ""00000000-0000-0000-0000-000000000000"",
                                    ""TranType"": ""STK-MTL"",
                                    ""DimConvFactor"": ""1"",
                                    ""UM"": ""KG"",
                                    ""FromJobPlant"": ""MfgSys"",
                                    ""ToJobPlant"": ""MfgSys"",
                                    ""DummyKeyField"": ""itdev"",
                                    ""RequirementUOM"": ""KG"",
                                    ""RequirementQty"": """ + REALISSUE.ToString() + @""",
                                    ""ProcessID"": ""IssueMaterial"",
                                    ""OnHandUM"": ""KG"",
                                    ""TranDocTypeID"": ""ST-WIP"",
                                    ""Plant"": ""MfgSys"",
                                    ""RowMod"": ""U""
                                  }
                                ],
                                ""LegalNumGenOpts"": [
                                  {
                                    ""Company"": ""ES168899"",
                                    ""LegalNumberID"": ""ST-WIP"",
                                    ""TransYear"": " + DateTime.Now.Year.ToString() + @",
                                    ""TransYearSuffix"": """",
                                    ""DspTransYear"": """ + DateTime.Now.Year.ToString() + @""",
                                    ""ShowDspTransYear"": false,
                                    ""Prefix"": ""STW"",
                                    ""PrefixList"": """",
                                    ""NumberSuffix"": """",
                                    ""EnablePrefix"": false,
                                    ""EnableSuffix"": false,
                                    ""NumberOption"": ""Manual"",
                                    ""DocumentDate"": """ + DateTime.Now.ToString("yyyy-MM-dd") + @"T00:00:00+07:00"",
                                    ""GenerationType"": ""system"",
                                    ""Description"": ""Issue Materials"",
                                    ""TransPeriod"": " + DateTime.Now.Month.ToString() + @",
                                    ""PeriodPrefix"": """",
                                    ""ShowTransPeriod"": false,
                                    ""LegalNumber"": """",
                                    ""TranDocTypeID"": ""StockWIP"",
                                    ""TranDocTypeID2"": ""ST-WIP"",
                                    ""GenerationOption"": ""Save"",
                                    ""SysRowID"": ""00000000-0000-0000-0000-000000000000"",
                                    ""RowMod"": ""A""
                                    }
                                  ]
                                }
                              }";

                request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress + "ERP_LIVE/api/v1/Erp.BO.IssueReturnSvc/PerformMaterialMovement");
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                response = client.SendAsync(request).Result;

                /* Nếu đủ thì trừ onhand chưa đủ thì set onhand lot này = 0 chuyển sang lot tiếp theo */
                if (REALONHAND >= REALISSUE)
                {
                    ir["PartBin_OnhandQty"] = ((REALONHAND - REALISSUE) / 1000).ToString();
                    break;
                }
                else
                {
                    REALISSUE = REALISSUE - REALONHAND;
                }
                /*-------------------------------------------------  */
            }
        }

        msg m2 = new msg("Đang chờ lệnh cân");
        private void timer1_Tick(object sender, EventArgs e)
        {
            //Lấy CBK
            dtChoose = SQL_Conn.SelectMOActive_V2();

            if (run == 0 && dtChoose.Rows.Count > 0)
            {
                try
                {
                    if (!m2.IsDisposed)
                        m2.Close();
                }
                catch { }

                if (!comport.IsOpen)
                    COMOpen();

                PLCOpen();

                timeout = 0;
                run = 1;
                if (a.IsAlive)
                {
                    a.Abort();
                }

                //Coi Job để xem quy trình
                if (dtChoose.Rows[0]["JobNo"].ToString().Substring(0, 3) == "JRR")
                {
                    //Lấy từ epicor
                    quytrinh1 = true;
                }
                else
                {
                    quytrinh1 = false;
                }

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress + "ERP_LIVE/api/v1/Erp.BO.IssueReturnSvc/PerformMaterialMovement");
                response = client.GetAsync("ERP_LIVE/api/v1/BaqSvc/ESN_Scale_COH?JobNum=" + dtChoose.Rows[0]["JobNo"].ToString()).Result;
                while(response.IsSuccessStatusCode == false)
                {
                    Thread.Sleep(2000);
                    response = client.GetAsync("ERP_LIVE/api/v1/BaqSvc/ESN_Scale_COH?JobNum=" + dtChoose.Rows[0]["JobNo"].ToString()).Result;                    
                }
                dtCheckOnHand = GetDataTableFromJsonString(response.Content.ReadAsStringAsync().Result);

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
                //Lấy tất cả thùng to của Job
                response = client.GetAsync("ERP_LIVE/api/v1/BaqSvc/ESN_Scale_AllBinPort2?JobNo=" + dtChoose.Rows[0]["JobNo"].ToString()).Result;
                while (response.IsSuccessStatusCode == false)
                {
                    Thread.Sleep(2000);
                    response = client.GetAsync("ERP_LIVE/api/v1/BaqSvc/ESN_Scale_AllBinPort2?JobNo=" + dtChoose.Rows[0]["JobNo"].ToString()).Result;
                }
                dtBucket = GetDataTableFromJsonString(response.Content.ReadAsStringAsync().Result);

                //Lấy partnum
                response = client.GetAsync("ERP_LIVE/api/v1/BaqSvc/ESN_Scale_GetPartD?PartNum=" + dtChoose.Rows[0]["ChemicalOrderCode"].ToString()).Result;
                while (response.IsSuccessStatusCode == false)
                {
                    Thread.Sleep(2000);
                    response = client.GetAsync("ERP_LIVE/api/v1/BaqSvc/ESN_Scale_GetPartD?PartNum=" + dtChoose.Rows[0]["ChemicalOrderCode"].ToString()).Result;
                }
                DataTable dtGetPartDesc = GetDataTableFromJsonString(response.Content.ReadAsStringAsync().Result);
                var PartDesc = dtGetPartDesc.Rows[0]["Part_PartDescription"].ToString();

                //Lấy dữ liệu xem đã cân chưa
                dtChooseCTY = SQL_Conn.SelectCommandCTY_V2(dtChoose.Rows[0][0].ToString(), 2);
                dtMaterialDetail = SQL_Conn.SelectDetail_V2(dtChoose.Rows[0][0].ToString(), 2);
                MaterialTemp = SQL_Conn.SelectCommandTemp_V2(dtChoose.Rows[0][0].ToString(), 2);

                Au = new string[int.Parse(dtChoose.Rows[0]["BatchNo"].ToString()), 2]; //Khai báo số Âu theo lệnh điều động
                Total = new string[int.Parse(dtChoose.Rows[0]["BatchNo"].ToString()), 2]; //Khai báo tổng số lượng của từng âu

                //Gán dữ liệu để quét tiếp nếu bị lỗi cân khi đang quét
                for (int i = 0; i < MaterialTemp.Rows.Count; i++)
                {
                    Au[i, 0] = MaterialTemp.Rows[i][1].ToString();
                    Au[i, 1] = MaterialTemp.Rows[i][4].ToString();
                }

                //Gán thông tin keo mà tổng số âu
                lbSoKG.Text = "Số kg: " + dtChoose.Rows[0]["Weight"].ToString();

                //Lấy mã keo epicor
                lbMaKeo.Text = "Mã keo 1: " + PartDesc;
                if(!quytrinh1)
                    lbSoMe.Text = "Tổng số hộp: " + dtChoose.Rows[0]["BatchNo"].ToString();
                else
                    lbSoMe.Text = "Tổng số cây: " + dtChoose.Rows[0]["BatchNo"].ToString();

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
                    if (!can)
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
        private string COMgetData(string kg, int i)
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
                    double LastWeight = 0;
                    if (!quytrinh1)
                    {
                        LastWeight = Convert.ToDouble(dtMaterialDetail.Compute("SUM(RealWeight)", "AuNumber = " + aunumber.ToString()));
                    }

                    strData2 = strData.Substring(strData.Length - 12, 7);
                    //strData2 = strData.Substring(7, strData.Length - 7);
                    dResult = Convert.ToDouble(strData2) * 1000;
                    if (dResult >= 0)
                    {
                        //Lấy số lượng hiện tại bằng số lượng cân trừ đi trọng lượng âu và tổng trọng lượng các chát hiện có của Au
                        double slhientai = dResult - LastWeight;
                        slhientai = Math.Round(slhientai, 2);
                        if (slhientai > 0)
                            txtKG.WaterMark = slhientai.ToString();
                        else
                            txtKG.WaterMark = "0";
                    }

                    //Lấy xác suất lệch cho phép là 0.5%
                    var minKG = (Double.Parse(kg) - Double.Parse(kg) / 100);
                    var maxKG = (Double.Parse(kg) + Double.Parse(kg) / 100);

                    //Làm tròn số 371 372 373 thành 370
                    long so, tronDV;
                    so = (long)minKG;
                    tronDV = RoundingTo(so, 10);
                    minKG = Double.Parse(tronDV.ToString());
                    so = (long)maxKG;
                    tronDV = RoundingTo(so, 10);
                    maxKG = Double.Parse(tronDV.ToString());

                    ActualKG = dResult - LastWeight;
                    ActualKG = Math.Round(ActualKG, 2);
                    if (ActualKG >= minKG && ActualKG <= maxKG)
                    {
                        if (this.InvokeRequired)
                        {
                            SetTextCallback d = new SetTextCallback(SetBtXacNhanEnable);
                            this.Invoke
                                (d, new object[] { soau.ToString() });
                        }

                        //choqua = true;
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
        string MaterialCode = "";
        string MaterialName = "";
        int totalAunumber = 0;
        public void getMaterial()
        {
            var totalAu = dtMaterialDetail.Select("AuNumber = Max(AuNumber)");
            totalAunumber = int.Parse(totalAu[0]["AuNumber"].ToString());

            for (int k = 0; k < dtBucket.Rows.Count; k++)
            {
                try
                {
                    light = dtBucket.Rows[k]["UDCodes2_LongDesc"].ToString().Split('.');
                    cJ2Compolet1.ForceSet(OMRON.Compolet.CIP.CJ2Compolet.ForceMemoryTypes.CIO, int.Parse(light[0]), Int16.Parse(light[1]));
                }
                catch { }
            }

            for (int i = 0; i < dtMaterialDetail.Rows.Count; i++)
            {
                if (dtMaterialDetail.Rows[i]["ChemStatus"].ToString() == "1" || dtMaterialDetail.Rows[i]["MachineNo"].ToString() == "1")
                    continue;

                MaterialCode = dtMaterialDetail.Rows[i]["MaterialCode"].ToString().Trim();
                //Set den, mo thung plc    
                try
                {
                    var row = dtBucket.Select("UDCodes_LongDesc = '" + MaterialCode + "'");
                    foreach (var item in row)
                    {
                        open = item["UDCodes2_CodeDesc"].ToString().Split('.');
                        light = item["UDCodes2_LongDesc"].ToString().Split('.');
                    }
                    MaterialName = row[0]["Part_PartDescription"].ToString();
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
                        (d, new object[] { i.ToString() });
                }

                txtKGR.WaterMark = dtMaterialDetail.Rows[i]["Weight"].ToString(); //set số lượng cần

                while (choqua == false)
                {
                    aunumber = int.Parse(dtMaterialDetail.Rows[i]["AuNumber"].ToString());
                    //Set label số thứ tự hộp
                    if (this.InvokeRequired)
                    {
                        SetTextCallback d = new SetTextCallback(SetLabelStt);
                        this.Invoke
                            (d, new object[] { dtMaterialDetail.Rows[i]["AuNumber"].ToString() });
                    }

                    //Set label tên nguyên liệu
                    if (this.InvokeRequired)
                    {
                        SetTextCallback d = new SetTextCallback(SetLabelNL);
                        this.Invoke
                            (d, new object[] { i.ToString() }); 
                    }

                    for (int u = 0; ; u++)
                    {
                        if (choqua == true && result == "OK")
                        {
                            dtMaterialDetail.Rows[i]["RealWeight"] = ActualKG;
                            dtMaterialDetail.Rows[i]["MaterialName"] = MaterialName;
                            dtMaterialDetail.Rows[i]["ChemStatus"] = 1;
                            break;
                        }                          
                        else
                        {
                            result = COMgetData(dtMaterialDetail.Rows[i]["Weight"].ToString(), i);
                            Thread.Sleep(60);
                        }
                    }

                    //Ẩn nút xác nhận
                    //if (this.InvokeRequired)
                    //{
                    //    SetTextCallback d = new SetTextCallback(SetBtXacNhan);
                    //    this.Invoke
                    //        (d, new object[] { "" });
                    //}

                    if (this.InvokeRequired)
                    {
                        SetTextCallback d = new SetTextCallback(SetBtXacNhanDisable);
                        this.Invoke
                            (d, new object[] { soau.ToString() });
                    }

                    //Lấy liệu từ cổng COM
                    if (result == "OK" && choqua)
                    {
                        //Update bảng lưu tạm
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
                                (d, new object[] { i.ToString() });
                        }

                        //Gởi issue đến epicor
                        //Issue_Material(MaterialCode,ActualKG);

                        //Cân đủ khối lượng chuyển sang chất kế tiếp   
                        if ((totalAunumber == dtMaterialDetail.Select("ChemStatus = 1 and MaterialCode = '" + MaterialCode + "'").Length) 
                            || quytrinh1)
                        {
                            result = "OK";
                            if (i == dtMaterialDetail.Rows.Count)
                                break;

                            if (quytrinh1)
                                break;
                        }
                        else
                        {
                            i++;
                            //Set màu cho label số hộp
                            if (this.InvokeRequired)
                            {
                                SetTimerCheckAu d = new SetTimerCheckAu(SetStt);
                                this.Invoke
                                    (d, new object[] { "" });
                            }
                            txtKGR.WaterMark = dtMaterialDetail.Rows[i]["Weight"].ToString(); //set số lượng cần
                            result = "";
                            choqua = false;
                        }
                    }
                    else
                        choqua = false;
                }

                
                //Close plc
                if(quytrinh1)
                {
                   if(dtMaterialDetail.Select("MaterialCode = '" + MaterialCode + "'").Length == dtMaterialDetail.Select("ChemStatus = 1 and MaterialCode = '" + MaterialCode + "'").Length)
                    {
                        try
                        {
                            Thread.Sleep(10000);
                            cJ2Compolet1.ForceCancel(OMRON.Compolet.CIP.CJ2Compolet.ForceMemoryTypes.CIO, int.Parse(open[0]), Int16.Parse(open[1]));
                            cJ2Compolet1.ForceCancel(OMRON.Compolet.CIP.CJ2Compolet.ForceMemoryTypes.CIO, int.Parse(light[0]), Int16.Parse(light[1]));
                        }
                        catch
                        {

                        }
                    }
                }
                else
                {
                    try
                    {
                        Thread.Sleep(10000);
                        cJ2Compolet1.ForceCancel(OMRON.Compolet.CIP.CJ2Compolet.ForceMemoryTypes.CIO, int.Parse(open[0]), Int16.Parse(open[1]));
                        cJ2Compolet1.ForceCancel(OMRON.Compolet.CIP.CJ2Compolet.ForceMemoryTypes.CIO, int.Parse(light[0]), Int16.Parse(light[1]));
                    }
                    catch
                    {

                    }
                }


                result = "";
                choqua = false;
                soau = 1; //Reset âu để cân hóa chất tiếp theo
            }


            result = "";

            bool b = SQL_Conn.updateMachine_V2(dtMaterialDetail.Rows[0][0].ToString(), 2); //Cập nhật hoàn thành lệnh
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

            try
            {
                var row = dtMaterialDetail.Rows[int.Parse(text)]["AuNumber"].ToString();
                this.lbBon.Text = "Bồn: " + row;
            }
            catch
            {
                this.lbBon.Text = "Chưa có";
            }

            if (quytrinh1)
            {
                var row = dtMaterialDetail.Rows[int.Parse(text)]["BinSTT"].ToString();
                if(row == "0")
                {
                    this.lbStt.Text = "Thùng 1";
                    tenthung = 0.ToString() ;
                }
                else
                {
                    this.lbStt.Text = "Thùng " + row;
                    tenthung = row ;
                }
               
            }

            DataRow[] r1 = dtBucket.Select(@"UDCodes_LongDesc = '" + dtMaterialDetail.Rows[int.Parse(text)]["MaterialCode"] + "'");
            if (r1.Length > 0)
            {
                foreach (var item in r1)
                {
                    text = item["Part_PartDescription"].ToString();
                    MaterialName = text;
                }
                   
            }
            this.lbNguyenLieu.Text = "Hóa chất " + text;
        }

        private void UpdateTemp(string text)
        {
            if (text == "4") //Update chi tiết hóa chất
            {
                if (!quytrinh1)
                    SQL_Conn.updateTempDetail_V2(dtChoose.Rows[0][0].ToString(), aunumber, 0, MaterialCode, MaterialName, ActualKG, 2, "");
                else
                    SQL_Conn.updateTempDetail_V2(dtChoose.Rows[0][0].ToString(), aunumber, int.Parse(tenthung), MaterialCode, MaterialName, ActualKG, 2, "");
            }

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
            //Thêm dữ liệu vào datagridview show thông tin hóa chất
            dgCty.Rows.Clear();
            dgCty2.Rows.Clear();
            dgCty.Refresh();
            dgCty2.Refresh();
            //Kiểm tra hóa chất

            dtTarget = dtMaterialDetail.Clone();
            DataRow[] rowsToCopy;
            rowsToCopy = dtMaterialDetail.Select("AuNumber='" + dtMaterialDetail.Rows[int.Parse(text)]["AuNumber"] + "'");
            foreach (DataRow temp in rowsToCopy)
            {
                dtTarget.ImportRow(temp);
            }

            //Thêm dữ liệu vào datagridview show thông tin hóa chất
            dgCty.Rows.Clear();
            dgCty2.Rows.Clear();
            dgCty.Refresh();
            dgCty2.Refresh();


            string[,] dgv = new string[8, 2];

            for (int i = 0; i < dtTarget.Rows.Count; i++)
            {
                DataRow[] r = dtBucket.Select(@"Part_PartNum = '" + dtTarget.Rows[i]["MaterialCode"] + "'");
                if (r.Length > 0)
                { //Part_PartDescription
                    dgv[i, 0] = r[0]["Part_PartDescription"].ToString();
                    dgv[i, 1] = dtTarget.Rows[i]["Weight"].ToString();
                }
            }

            for (int n = 0; n < 8; n++)
            {
                if (dgv[n, 0] is null)
                {
                    dgv[n, 0] = "0";
                }
                if (dgv[n, 1] is null)
                {
                    dgv[n, 1] = "0";
                }
            }

            //Kiểm tra hóa chất        
            dgCty.Rows.Add(dgv[0, 0], dgv[0, 1], dgv[1, 0], dgv[1, 1], dgv[2, 0], dgv[2, 1], dgv[3, 0], dgv[3, 1]);
            dgCty2.Rows.Add(dgv[4, 0], dgv[4, 1], dgv[5, 0], dgv[5, 1], dgv[6, 0], dgv[6, 1], dgv[7, 0], dgv[7, 1]);

            for (int k = 0; k < dgCty.Columns.Count; k++)
            {
                string val = dgCty.Rows[0].Cells[k].Value.ToString();  //check cell value
                if (val == "0")
                {
                    dgCty.Columns[k].Visible = false;     //Hides Column
                }
            }

            for (int k = 0; k < dgCty2.Columns.Count; k++)
            {
                string val = dgCty2.Rows[0].Cells[k].Value.ToString();  //check cell value
                if (val == "0")
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
                if (a.IsAlive)
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
            if (demXacNhan == 15)
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
