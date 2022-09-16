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
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace CanHoaChat
{
    public partial class UCCanBanTuDong : MetroFramework.Controls.MetroUserControl
    {
        string mo = "";
        string ComName = "";
        //Tao 1 thể hiện của SerialPort;
        public static SerialPort comport = new SerialPort();
        public Thread a = new Thread(openThread); //Tạo 1 thread dùng để chạy ẩn
        HttpClientHandler handler = new HttpClientHandler();
        HttpClient client;
        HttpResponseMessage response;
        DataTable dtCheckOnHand = new DataTable();
        public UCCanBanTuDong()
        {
            InitializeComponent();
        }


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
                //REALISSUE = REALISSUE / 1000;
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
                                    ""UM"": ""G"",
                                    ""FromJobPlant"": ""MfgSys"",
                                    ""ToJobPlant"": ""MfgSys"",
                                    ""DummyKeyField"": ""itdev"",
                                    ""RequirementUOM"": ""G"",
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
                    ir["PartBin_OnhandQty"] = ((REALONHAND * 1000 - REALISSUE)).ToString();
                    break;
                }
                else
                {
                    REALISSUE = REALISSUE - REALONHAND * 1000;
                }
                /*-------------------------------------------------  */
            }
        }

        public static void openThread()
        { }

        DataTable dtBuckets = new DataTable();
        DataTable AllBuckets = new DataTable();
        DataTable dtAllBucketActive = new DataTable();
        DataTable CheckPrint = new DataTable();
        DataTable dtChoose = new DataTable();
        DataTable dtChooseCTY = new DataTable();
        DataTable MaterialTemp = new DataTable();
        DataTable dtMaterialDetail = new DataTable();
        DataTable QRPrint = new DataTable();
        DataTable dtPrintQT1 = new DataTable();
        DataTable dtPrintQT2 = new DataTable();
        DataTable dtDataPrintQT1 = new DataTable();
        DataTable dtTyLe = new DataTable();
        DataTable dtDetail = new DataTable();
        DataTable dtSortMateiral = new DataTable();

        string[,] Au;
        string[,] Total;
        string[,] Total_Temp;
        int Stt_Temp = 0;
        int Stt_Can_Temp = 0;
        int Stt_Au_Temp = 1;

        int totalAunumber = 0;
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
        string PartNum = "";
        string MaterialName = "";
        string tenthung = "";
        int aunumber = 0;
        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;
            else
            {
                return true;
            }
        }
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

            cbMO.Text = "";
            txtJob.Focus();
            txtJob.SelectAll();
        }

        double ActualKG = 0;
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
                if (strData.Length >= 17)
                {
                    //Lấy trọng lượng hóa chất đã cân trong âu và trọng lượng âu
                    double LastWeight = 0;
                    double AuWeight = 0;
                    if (!quytrinh1)
                    {
                        LastWeight = Convert.ToDouble(dtMaterialDetail.Compute("SUM(RealWeight)", "AuNumber = " + aunumber.ToString()));
                        AuWeight = Convert.ToDouble(Au[aunumber - 1, 1]);
                    }

                    strData2 = strData.Substring(strData.Length - 17, 10);
                    //strData2 = strData.Substring(7, strData.Length - 7);
                    dResult = Convert.ToDouble(strData2);
                    if (dResult >= 0)
                    {
                        //Lấy số lượng hiện tại bằng số lượng cân trừ đi trọng lượng âu và tổng trọng lượng các chát hiện có của Au
                        double slhientai = dResult - AuWeight - LastWeight;
                        slhientai = Math.Round(slhientai, 3);
                        if (slhientai > 0)
                            txtKG.WaterMark = slhientai.ToString();
                        else
                            txtKG.WaterMark = "0";
                    }
                    //Lấy xác suất lệch cho phép là 0.5%
                    var minKG = (Double.Parse(kg) - (Double.Parse(kg) / 100 / 2));
                    var maxKG = (Double.Parse(kg) + (Double.Parse(kg) / 100 / 2));
                    ActualKG = dResult - AuWeight - LastWeight;
                    ActualKG = Math.Round(ActualKG, 3);
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

        bool showmsg = false;
        string MaterialCode = "";
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

            var totalAu = dtMaterialDetail.Select("AuNumber = Max(AuNumber)");
            totalAunumber = int.Parse(totalAu[0]["AuNumber"].ToString());
            for (int i = 0; i < AllBuckets.Rows.Count; i++)
            {
                try
                {
                    open = AllBuckets.Rows[i]["UDCodes_CodeDesc"].ToString().Split('.');
                    light = AllBuckets.Rows[i]["UDCodes_LongDesc"].ToString().Split('.');
                    cJ2Compolet1.ForceCancel(OMRON.Compolet.CIP.CJ2Compolet.ForceMemoryTypes.CIO, int.Parse(open[0]), Int16.Parse(open[1]));
                    cJ2Compolet1.ForceCancel(OMRON.Compolet.CIP.CJ2Compolet.ForceMemoryTypes.CIO, int.Parse(light[0]), Int16.Parse(light[1]));
                }
                catch { }
            }

            for (int i = 0; i < dtMaterialDetail.Rows.Count; i++)
            {
                //Nếu cân rồi thì chuyển sang chất tiếp theo
                if (dtMaterialDetail.Rows[i]["ChemStatus"].ToString() == "1" || dtMaterialDetail.Rows[i]["MachineNo"].ToString() == "2")
                    continue;

                for (int k = 0; k < dtAllBucketActive.Rows.Count; k++)
                {
                    try
                    {
                        light = dtAllBucketActive.Rows[k]["UDCodes_LongDesc"].ToString().Split('.');
                        cJ2Compolet1.ForceSet(OMRON.Compolet.CIP.CJ2Compolet.ForceMemoryTypes.CIO, int.Parse(light[0]), Int16.Parse(light[1]));
                    }
                    catch { }
                }

                MaterialCode = dtMaterialDetail.Rows[i]["MaterialCode"].ToString();
                //Set den, mo thung plc    
                try
                {
                    var row = dtAllBucketActive.Select("Part_PartNum = '" + MaterialCode + "'");
                    MaterialName = row[0]["Part_PartDescription"].ToString();
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
                        (d, new object[] { i.ToString() });
                }

                //Cân đủ số âu thì chuyển sang chất khác
                while (choqua == false)
                {
                    aunumber = int.Parse(dtMaterialDetail.Rows[i]["AuNumber"].ToString());
                    txtKGR.WaterMark = dtMaterialDetail.Rows[i]["Weight"].ToString(); //set số lượng cần
                    //Set label tên nguyên liệu                                                                  
                    if (this.InvokeRequired)
                    {
                        string TenHoaChatCan = dtMaterialDetail.Rows[i]["MaterialCode"].ToString();
                        SetTextCallback d = new SetTextCallback(SetLabelNL);
                        //Lấy từ lại theo bảng của epicor
                        DataRow[] r = dtBuckets.Select(@"JobMtl_PartNum = '" + TenHoaChatCan + "'");
                        if (r.Length > 0)
                        {
                            foreach (var item in r)
                                TenHoaChatCan = item["JobMtl_Description"].ToString();
                        }
                        this.Invoke
                            (d, new object[] { TenHoaChatCan });
                    }

                    //Set label số thứ tự hộp
                    if (this.InvokeRequired)
                    {
                        SetTextCallback d = new SetTextCallback(SetLabelStt);
                        this.Invoke
                            (d, new object[] { dtMaterialDetail.Rows[i]["AuNumber"].ToString() });
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
                            Thread.Sleep(30);
                        }
                    }

                    //An nut xac nhan
                    if (this.InvokeRequired)
                    {
                        SetTextCallback d = new SetTextCallback(SetBtXacNhan);
                        this.Invoke
                            (d, new object[] { soau.ToString() });
                    }

                    if (result == "OK" && choqua)
                    {
                        tenthung = dtMaterialDetail.Rows[i]["BinSTT"].ToString();
                        if (this.InvokeRequired)
                        {
                            SetTimerCheckAu d = new SetTimerCheckAu(UpdateTemp);
                            this.Invoke
                                (d, new object[] { "4" });
                        }

                        //Gửi issue tới epicor
                        //Issue_Material(MaterialCode, ActualKG);

                        //Thay đổi khổi lượng gridview của từng hộp
                        if (this.InvokeRequired)
                        {
                            SetTextCallback d = new SetTextCallback(setDgv);
                            this.Invoke
                                (d, new object[] { i.ToString() });
                        }

                        //Cân đủ khối lượng chuyển sang chất kế tiếp 
                        if ((totalAunumber == dtMaterialDetail.Select("ChemStatus = 1 and MaterialCode = '" + MaterialCode + "'").Length)
                            || quytrinh1)
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

                            choqua = true;
                            //Cân xong toàn bộ chất
                            if (i == dtMaterialDetail.Rows.Count)
                                break;
                            if (quytrinh1)
                                break;
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
                            txtKGR.WaterMark = dtMaterialDetail.Rows[i + 1]["Weight"].ToString(); //set số lượng cần
                            result = "";
                            i++;
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
                    //Lấy từ bảng của epicor
                    tenthung = dtMaterialDetail.Rows[i]["AuNumber"].ToString();
                    dtTarget = dtMaterialDetail.Clone();
                    DataRow[] rowsToCopy;
                    rowsToCopy = dtMaterialDetail.Select("AuNumber=" + tenthung + " and ChemStatus = 0");
                    if (rowsToCopy.Length == 0)
                    {
                        rowsToCopy = dtMaterialDetail.Select("AuNumber=" + tenthung + " and ChemStatus = 1");
                        foreach (DataRow temp in rowsToCopy)
                        {
                            dtTarget.ImportRow(temp);
                        }

                        PrintDocument printDocument = new PrintDocument(); //add the document to the dialog box..
                        printDocument.DefaultPageSettings.PaperSize.RawKind = 119;
                        printDocument.PrinterSettings.DefaultPageSettings.PaperSize.RawKind = 119;
                        printDocument.DefaultPageSettings.Landscape = false;
                        printDocument.PrinterSettings.PrinterName = printerName;
                        thoigianin = DateTime.Now.ToString("HH:mm:ss");
                        ngayin = DateTime.Now.ToString("dd-MM-yyyy");
                        soauhientai = int.Parse(tenthung);
                        tongau = rowsToCopy.Length;

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
                result = "";
            }

            for (int i = 0; i < AllBuckets.Rows.Count; i++)
            {
                try
                {
                    open = AllBuckets.Rows[i]["UDCodes_CodeDesc"].ToString().Split('.');
                    light = AllBuckets.Rows[i]["UDCodes_LongDesc"].ToString().Split('.');
                    cJ2Compolet1.ForceCancel(OMRON.Compolet.CIP.CJ2Compolet.ForceMemoryTypes.CIO, int.Parse(open[0]), Int16.Parse(open[1]));
                    cJ2Compolet1.ForceCancel(OMRON.Compolet.CIP.CJ2Compolet.ForceMemoryTypes.CIO, int.Parse(light[0]), Int16.Parse(light[1]));
                }
                catch { }
            }

            bool b = SQL_Conn.updateMachine_V2(dtMaterialDetail.Rows[0]["ManufactureOrderNo"].ToString(), 1); //Cập nhật hoàn thành lệnh
            canquytrinh1_1 = true;
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
        {          this.timer6.Enabled = true;
        }
        private void DongThung(string text)
        {
            timer8.Enabled = true;
        }
        private void SetLabelStt(string text)
        {
            if (quytrinh1)
            {
                this.lbStt.Text = "Thùng: " + text;
            }
            else
                this.lbStt.Text = "Hộp thứ: " + text;
        }
        private void SetLabelNL(string text)
        {
            this.lbNguyenLieu.Text = "Hóa chất " + text;
            MaterialName = text;
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
                SQL_Conn.updateAuTemp_V2(cbMO.Text.ToString(), txtCanAu.WaterMark, txtQRAu.Text, stt.ToString(), 1);
            if (text == "4") //Update chi tiết hóa chất
            {
                if (!quytrinh1)
                    SQL_Conn.updateTempDetail_V2(cbMO.Text, aunumber, 0, MaterialCode, MaterialName, ActualKG, 1, username);
                else
                    SQL_Conn.updateTempDetail_V2(cbMO.Text, aunumber, int.Parse(tenthung), MaterialCode, MaterialName, ActualKG, 1, username);
            }

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
            dgCty3.Rows.Clear();
            dgCty4.Rows.Clear();
            dgCty.Refresh();
            dgCty2.Refresh();
            dgCty3.Refresh();
            dgCty4.Refresh();

            string[,] dgv = new string[14, 2];

            for (int i = 0; i < dtTarget.Rows.Count; i++)
            {
                DataRow[] r = dtAllBucketActive.Select(@"Part_PartNum = '" + dtTarget.Rows[i]["MaterialCode"] + "'");
                if (r.Length > 0)
                { //Part_PartDescription
                    dgv[i, 0] = r[0]["Part_PartDescription"].ToString();
                    dgv[i, 1] = dtTarget.Rows[i]["Weight"].ToString();
                }
            }


            for (int n = 0; n < 14; n++)
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
            dgCty3.Rows.Add(dgv[8, 0], dgv[8, 1], dgv[9, 0], dgv[9, 1], dgv[10, 0], dgv[10, 1], dgv[11, 0], dgv[11, 1]);
            dgCty4.Rows.Add(dgv[12, 0], dgv[12, 1], dgv[13, 0], dgv[13, 1]);

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

            for (int k = 0; k < dgCty3.Columns.Count; k++)
            {
                string val = dgCty3.Rows[0].Cells[k].Value.ToString();  //check cell value
                if (val == "0")
                {
                    dgCty3.Columns[k].Visible = false;     //Hides Column
                }
            }

            for (int k = 0; k < dgCty4.Columns.Count; k++)
            {
                string val = dgCty4.Rows[0].Cells[k].Value.ToString();  //check cell value
                if (val == "0")
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

                    //Update thêm vào bảng Detail từ QRAu

                    stt++;
                }
            checkAu = true; //Kiểm tra âu thành công
        }

        bool stoptime = false;
        bool canquytrinh1_1 = false;
        int soauhientai;
        int tongau;
        string maau;
        string khoiluong;
        string thoigianin;
        string ngayin;
        string makeo;
        string makeo2;
        string makeokg;
        string khoiluongau;
        string tongkhoiluong;
        DataTable dtTarget = new DataTable();
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (stoptime) //Cân thành công 
                {
                    showmsg = false;

                    if (!quytrinh1)
                    {
                        PrintDocument printDocument = new PrintDocument(); //add the document to the dialog box..
                        printDocument.DefaultPageSettings.PaperSize.RawKind = 119;
                        printDocument.PrinterSettings.DefaultPageSettings.PaperSize.RawKind = 119;
                        printDocument.DefaultPageSettings.Landscape = false;
                        printDocument.PrinterSettings.PrinterName = printerName;

                        dtMaterialDetail = SQL_Conn.GetDetailPrint_V2(cbMO.Text.ToString());

                        DataRow[] dr = dtMaterialDetail.Select("AuNumber = MAX(AuNumber)");
                        tongau = int.Parse(dr[0]["AuNumber"].ToString());
                        for (int i = 1; i <= tongau; i++)
                        {
                            dtTarget = dtMaterialDetail.Clone();
                            DataRow[] rowsToCopy;
                            rowsToCopy = dtMaterialDetail.Select("AuNumber='" + i + "'");
                            foreach (DataRow temp in rowsToCopy)
                            {
                                dtTarget.ImportRow(temp);
                            }

                            thoigianin = DateTime.Now.ToString("HH:mm:ss");
                            ngayin = DateTime.Now.ToString("dd-MM-yyyy");
                            soauhientai = i;
                            maau = dtMaterialDetail.Rows[i - 1]["QRAu"].ToString();
                            tenhop = maau;
                            if (maau.Substring(0, 2) == "AU")
                                makeokg = 25.ToString();
                            else
                            {
                                makeokg = (double.Parse(dtChoose.Rows[0]["Weight"].ToString()) % 25).ToString();
                            }
                            khoiluongau = Au[i - 1, 1];
                            khoiluong = dtTarget.Compute("SUM(RealWeight)", "").ToString();
                            tongkhoiluong = (double.Parse(khoiluongau) + double.Parse(khoiluong)).ToString();
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

                    string job = txtJob.Text.Substring(0, 11);
                    DataTable dtMO = SQL_Conn.GetMOFromJob(job);
                    if (dtMO.Rows.Count > 0)
                    {
                        cbMO.DataSource = dtMO;
                        cbMO.DisplayMember = "ManufactureOrderNo";
                        cbMO.ValueMember = "ManufactureOrderNo";
                    }
                    else
                    {
                        MessageBox.Show("Job đã hoàn thành lệnh cân");
                        txtJob.Focus();
                        txtJob.SelectAll();
                    }
                    //Tắt timer
                    timer1.Enabled = false;
                }
                else
                {
                    if (quytrinh1 && timer6.Enabled == false && canquytrinh1_1)
                    {
                        //In thùng cân 2
                        PrintDocument printDocument = new PrintDocument(); //add the document to the dialog box..
                        printDocument.DefaultPageSettings.PaperSize.RawKind = 119;
                        printDocument.PrinterSettings.DefaultPageSettings.PaperSize.RawKind = 119;
                        printDocument.DefaultPageSettings.Landscape = false;
                        printDocument.PrinterSettings.PrinterName = printerName;
                        thoigianin = DateTime.Now.ToString("HH:mm:ss");
                        ngayin = DateTime.Now.ToString("dd-MM-yyyy");
                        soauhientai = int.Parse(tenthung);

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

                cbMO.Text = "";
                cbMO.Focus();
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
            string top = makeo + "_";
            graphic.DrawString(top, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 20;
            graphic.DrawString(makeokg + "kg", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            for (int i = 0; i < dtTarget.Rows.Count; i++)
            {
                string tenchat = dtTarget.Rows[i]["MaterialName"].ToString();
                string khoiluong = dtTarget.Rows[i]["RealWeight"].ToString();
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
                graphic.DrawString("Mã lệnh: " + cbMO.Text, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            else
                graphic.DrawString("Mã lệnh: " + cbMO.Text, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Số Job: " + dtChoose.Rows[0]["JobNo"].ToString(), new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Khối lượng HC: " + khoiluong + "g", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Khối lượng hộp: " + khoiluongau + "g", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Tổng khối lượng: " + tongkhoiluong + "g", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Giờ cân: " + thoigianin, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset); ;
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Ngày cân: " + ngayin, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("----------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30;
            graphic.DrawString("QC xác nhận", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 100;
            graphic.DrawString("----------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30;
            QRCodeGenerator _qrCode = new QRCodeGenerator();
            string qrcode = "";
            //qrcode = dtChoose.Rows[0]["ChemicalOrderCode"].ToString() + "," + DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("HHmmss");
            qrcode = PartNum;
            QRCodeData _qrCodeData = _qrCode.CreateQrCode(qrcode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(_qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            graphic.DrawImage(qrCodeImage, startX + 60, startY + offset, 100, 100);
            bool b = SQL_Conn.updateQRCode_V2(cbMO.Text.ToString(), soauhientai, 0, qrcode, ngayin, thoigianin, khoiluong, 1); //Cập nhật QR code
            while (b != true)
            {
                b = SQL_Conn.updateQRCode_V2(cbMO.Text.ToString(), soauhientai, 0, qrcode, ngayin, thoigianin, khoiluong, 1); //Cập nhật QR code
                Thread.Sleep(3000);
            }
        }

        public void CreateReceipt_PhieuCan(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //this prints the reciept
            Graphics graphic = e.Graphics;
            Font font = new Font("Segoe UI", 12); //must use a mono spaced font as the spaces need to line up

            float fontHeight = font.GetHeight();

            int startX = 10;
            int startY = 10;
            int offset = 30;

            graphic.DrawString("PHIẾU CÂN / QC HÀNG HÓA", new Font("Segoe UI", 16, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY);
            offset = offset + 20;
            string Time = DateTime.Now.ToString("HH:mm:ss");
            string Date = DateTime.Now.ToString("MM-dd-yyyy");
            graphic.DrawString("Time: " + Time, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            graphic.DrawString("Date: " + Date, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX + 130, startY + offset);
            offset = offset + 20;
            string Employee = "3890";
            graphic.DrawString("Employee ID: " + Employee, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 20;
            string Customer = "SWY";
            graphic.DrawString("Customer: " + Customer, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 20; //make some room so that the total stands out.
            graphic.DrawString("SO: 8888", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 20;
            graphic.DrawString("PartNum: RUB00002212", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 20;
            graphic.DrawString("Batch Number: DHE_8888_0321", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 20;
            graphic.DrawString("ResourceID: MayDH_01", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 20;
            graphic.DrawString("Department: Định hình", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 20;
            graphic.DrawString("PO: 1234566", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 20;
            graphic.DrawString("JOB: 01234", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 20;
            graphic.DrawString("Root Material: PA", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 20;
            graphic.DrawString("Material: SRU00000123", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 20;
            graphic.DrawString("Weight: 50kg", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30;
            graphic.DrawString("----------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30;
            graphic.DrawString("Type/Loại", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            string boxnonChecked = "☐";
            FontFamily family = new FontFamily("Segoe UI");//change the text size by font size
            Font font2 = new System.Drawing.Font(family, 9, FontStyle.Bold);//ch
            Point DrawLocation = new Point(startX + 100, startY + offset + 5); // point of text location
            graphic.DrawString(boxnonChecked + " " + "Chưa kiểm", font2, new SolidBrush(Color.Black), DrawLocation);
            offset = offset + 40;
            DrawLocation = new Point(startX, startY + offset);
            graphic.DrawString(boxnonChecked + " " + "Hàng OK", font2, new SolidBrush(Color.Black), DrawLocation);
            DrawLocation = new Point(startX + 100, startY + offset);
            graphic.DrawString(boxnonChecked + " " + "Nguyên liệu", font2, new SolidBrush(Color.Black), DrawLocation);
            offset = offset + 40;
            DrawLocation = new Point(startX, startY + offset);
            graphic.DrawString(boxnonChecked + " " + "Hàng NG", font2, new SolidBrush(Color.Black), DrawLocation);
            DrawLocation = new Point(startX + 100, startY + offset);
            graphic.DrawString(boxnonChecked + " " + "Phế liệu", font2, new SolidBrush(Color.Black), DrawLocation);
            offset = offset + 40;
            graphic.DrawString("QC Check", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30;
            DrawLocation = new Point(startX, startY + offset);
            graphic.DrawString(boxnonChecked + " " + "Pass", font2, new SolidBrush(Color.Black), DrawLocation);
            offset = offset + 30;
            DrawLocation = new Point(startX, startY + offset);
            graphic.DrawString(boxnonChecked + " " + "Fail", font2, new SolidBrush(Color.Black), DrawLocation);
            offset = offset + 30;
            graphic.DrawString("----------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30;
            graphic.DrawString("QR Code", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX + 70, startY + offset);
            offset = offset + 20;
            QRCodeGenerator _qrCode = new QRCodeGenerator();
            string qrcode = "DHE_8888_0321";

            QRCodeData _qrCodeData = _qrCode.CreateQrCode(qrcode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(_qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            graphic.DrawImage(qrCodeImage, startX + 60, startY + offset, 100, 100);
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
            top = makeo;
            graphic.DrawString(top, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 20;

            graphic.DrawString(makeokg + "kg", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);

            if (!inlai)
            {
                offset = offset + 20;
                DataRow[] r = AllBuckets.Select(@"Part_PartNum = '" + tenhoachatT2 + "'");
                if (r.Length > 0)
                {
                    foreach (var item in r)
                        tenhoachatT2 = item["Part_PartDescription"].ToString();
                }
                graphic.DrawString(tenhoachatT2 + " - " + sokgT2 + "g", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            }
            else
            {
                for (int i = 0; i < dtTarget.Rows.Count; i++)
                {
                    if (dtTarget.Rows[i]["BinSTT"].ToString() != "0")
                    {
                        if (dtTarget.Rows[i]["BinSTT"].ToString() == BinSTT)
                        {
                            offset = offset + 20;
                            tenhoachatT2 = dtTarget.Rows[i]["MaterialName"].ToString();
                            sokgT2 = dtTarget.Rows[i]["RealWeight"].ToString();
                            graphic.DrawString(tenhoachatT2 + " - " + sokgT2 + "g", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
                            countLast_hoachat = int.Parse(BinSTT);
                            break;
                        }
                    }
                    else
                    {
                        offset = offset + 20;
                        tenhoachatT2 = dtTarget.Rows[i]["MaterialName"].ToString();
                        sokgT2 = dtTarget.Rows[i]["RealWeight"].ToString();
                        graphic.DrawString(tenhoachatT2 + " - " + sokgT2 + "g", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
                    }
                }
            }

            offset = offset + (int)fontHeight; //make the spacing consistent
            graphic.DrawString("----------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 20; //make some room so that the total stands out.
            if (countLast_hoachat > 0)
                graphic.DrawString("Số thùng: " + soauhientai + "-" + countLast_hoachat.ToString(), new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            else
                graphic.DrawString("Số thùng: " + soauhientai, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);

            if (makeokg != 25.ToString())
                graphic.DrawString("LẺ", new Font("Segoe UI", 30, FontStyle.Bold), new SolidBrush(Color.Black), startX + 150, startY + offset);
            offset = offset + 30;
            //graphic.DrawString("Tên hộp: " + tenhop, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            //offset = offset + 30; //make some room so that the total stands out.
            if (!inlai)
                graphic.DrawString("Mã lệnh: " + cbMO.Text, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            else
                graphic.DrawString("Mã lệnh: " + txtInLai.Text.Substring(0,11), new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
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
            string qrcode = PartNum;

            QRCodeData _qrCodeData = _qrCode.CreateQrCode(qrcode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(_qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            graphic.DrawImage(qrCodeImage, startX + 60, startY + offset, 100, 100);
            if (!inlai)
            {
                bool b = SQL_Conn.updateQRCode_V2(cbMO.Text.ToString(), soauhientai, countLast_hoachat, qrcode, ngayin, thoigianin, 0.ToString(), 2); //Cập nhật QR code
                while (b != true)
                {
                    b = SQL_Conn.updateQRCode_V2(cbMO.Text.ToString(), soauhientai, countLast_hoachat, qrcode, ngayin, thoigianin, 0.ToString(), 2);
                    Thread.Sleep(3000);
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
            string top = makeo;
            graphic.DrawString(top, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 20;
            graphic.DrawString(makeokg + "kg", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);

            for (int i = 0; i < dtTarget.Rows.Count; i++)
            {
                offset = offset + 20;
                string tenchat = dtTarget.Rows[i]["MaterialCode"].ToString();
                DataRow[] r = AllBuckets.Select(@"Part_PartNum = '" + tenchat + "'");
                if (r.Length > 0)
                {
                    foreach (var item in r)
                        tenchat = item["Part_PartDescription"].ToString();
                }
                graphic.DrawString(tenchat + " - " + dtTarget.Rows[i]["RealWeight"].ToString() + "g", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            }
            offset = offset + (int)fontHeight; //make the spacing consistent
            graphic.DrawString("----------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 20; //make some room so that the total stands out.
            graphic.DrawString("Số thùng: " + soauhientai, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);

            if (makeokg != 25.ToString())
                graphic.DrawString("LẺ", new Font("Segoe UI", 30, FontStyle.Bold), new SolidBrush(Color.Black), startX + 150, startY + offset);
            offset = offset + 30;
            //graphic.DrawString("Tên hộp: " + tenhop, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            //offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Mã lệnh: " + cbMO.Text, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Số Job: " + txtInLai.Text.Substring(0,11), new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
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
            string qrcode = PartNum;
            //qrcode = dtChoose.Rows[0]["ChemicalOrderCode"].ToString() + "," + DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("HHmmss");

            QRCodeData _qrCodeData = _qrCode.CreateQrCode(qrcode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(_qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            graphic.DrawImage(qrCodeImage, startX + 60, startY + offset, 100, 100);
            bool b = SQL_Conn.updateQRCode_V2(cbMO.Text.ToString(), soauhientai, 0, qrcode, ngayin, thoigianin, 0.ToString(), 1); //Cập nhật QR code
            while (b != true)
            {
                b = SQL_Conn.updateCompTram1_V2(cbMO.Text.ToString(), soauhientai.ToString());
                Thread.Sleep(3000);
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


            string top = makeo;
            graphic.DrawString(top, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 20;
            graphic.DrawString(makeokg + "kg", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            dtTarget = CheckPrint.Clone();
            DataRow[] rowsToCopy;
            rowsToCopy = CheckPrint.Select("AuNumber = " + soauhientai);
            if (rowsToCopy.Length > 1)
            {
                foreach (DataRow temp in rowsToCopy)
                {
                    dtTarget.ImportRow(temp);
                }
                for (int i = 0; i < dtTarget.Rows.Count; i++)
                {
                    string khoiluong = dtTarget.Rows[i]["RealWeight"].ToString();
                    string tenchat = dtTarget.Rows[i]["MaterialName"].ToString(); ;
                    offset = offset + 20;
                    graphic.DrawString(tenchat + " - " + khoiluong + "g", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
                }

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
            graphic.DrawString("Mã lệnh: " + cbMO.Text, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Job number: " + JobNo, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Khối lượng HC: " + khoiluong + "g", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Khối lượng hộp: " + khoiluongau + "g", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Tổng khối lượng: " + tongkhoiluong + "g", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Giờ cân: " + thoigianin, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset); ;
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("Ngày cân: " + ngayin, new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30; //make some room so that the total stands out.
            graphic.DrawString("----------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30;
            graphic.DrawString("QC xác nhận", new Font("Segoe UI", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 100;
            graphic.DrawString("----------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + 30;
            QRCodeGenerator _qrCode = new QRCodeGenerator();
            QRCodeData _qrCodeData = _qrCode.CreateQrCode(QRCode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(_qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            graphic.DrawImage(qrCodeImage, startX + 60, startY + offset, 100, 100);
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
            txtInLai.Text = "";
            pPrint.Visible = false;
        }

        private void btIn_Click(object sender, EventArgs e)
        {
            pPrint.Visible = true;

        }

        string JobNo = "";
        double khoiluongcongdon = 0;
        string BinSTT = "";
        private void btIn2_Click(object sender, EventArgs e)
        {
            inlai = true;
            PrintDocument printDocument = new PrintDocument(); //add the document to the dialog box..
            printDocument.DefaultPageSettings.PaperSize.RawKind = 119;
            printDocument.PrinterSettings.DefaultPageSettings.PaperSize.RawKind = 119;
            printDocument.DefaultPageSettings.Landscape = false;
            printDocument.PrinterSettings.PrinterName = printerName;

            if (!quytrinh1)
                for (int i = 0; i < CheckPrint.Rows.Count; i++)
                {
                    soauhientai = int.Parse(CheckPrint.Rows[i]["AuNumber"].ToString());
                    var tongaumax = CheckPrint.Select("AuNumber = Max(AuNumber)");
                    tongau = int.Parse(tongaumax[0]["AuNumber"].ToString());
                    tenhop = CheckPrint.Rows[i]["QRAu"].ToString();
                    maau = CheckPrint.Rows[i]["QRAu"].ToString();
                    makeo = CheckPrint.Rows[i]["ChemicalOrderName"].ToString();
                    if (maau.Substring(0, 2) == "AU")
                        makeokg = 25.ToString();
                    else
                    {
                        makeokg = CheckPrint.Rows[i]["COWeight"].ToString();
                    }
                    khoiluongau = CheckPrint.Rows[i]["AuTareWeight"].ToString();
                    if (i < CheckPrint.Rows.Count - 1)
                    {
                        if (maau == CheckPrint.Rows[i + 1]["QRAu"].ToString())
                        {
                            khoiluongcongdon += double.Parse(CheckPrint.Rows[i]["RealWeight"].ToString());
                            continue;
                        }
                        else
                            khoiluongcongdon += double.Parse(CheckPrint.Rows[i]["RealWeight"].ToString());
                    }
                    else
                    {
                        khoiluongcongdon += double.Parse(CheckPrint.Rows[i]["RealWeight"].ToString());
                    }

                    khoiluong = khoiluongcongdon.ToString();
                    tongkhoiluong = (double.Parse(khoiluongau) + khoiluongcongdon).ToString(); ;
                    var qrPrinted = CheckPrint.Select("AuNumber = " + CheckPrint.Rows[i]["AuNumber"].ToString());
                    QRCode = qrPrinted[0]["QRCode"].ToString();
                    thoigianin = qrPrinted[0]["PrintTime"].ToString();
                    ngayin = qrPrinted[0]["PrintDate"].ToString();
                    var dtime = DateTime.ParseExact(ngayin, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    ngayin = dtime.ToString("dd-MM-yyyy");

                    printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(CreateReceipt_InLai); //add an event handler that will do the printing                                                                                                                       //on a till you will not want to ask the user where to print but this is fine for the test envoironment.
                    printDocument.Print();
                    khoiluongcongdon = 0;
                }
            else
            {
                string tenthung = cbChonThung.Text;
                dtTarget = CheckPrint.Clone();
                DataRow[] rowsToCopy;
                rowsToCopy = CheckPrint.Select("AuNumber = " + tenthung);
                if (rowsToCopy.Length > 1)
                {
                    foreach (DataRow temp in rowsToCopy)
                    {
                        dtTarget.ImportRow(temp);
                    }
                }

                for (int i = 0; i < dtTarget.Rows.Count; i++)
                {
                    makeo = dtTarget.Rows[i]["ChemicalOrderName"].ToString();
                    soauhientai = int.Parse(tenthung);
                    QRCode = dtTarget.Rows[0]["QRCode"].ToString();

                    if (double.Parse(dtTarget.Rows[i]["COWeight"].ToString()) >= 25)
                        makeokg = 25.ToString();
                    else
                    {
                        makeokg = dtTarget.Rows[i]["COWeight"].ToString();
                    }

                    if (i < dtTarget.Rows.Count - 1)
                    {
                        if (dtTarget.Rows[i]["BinSTT"].ToString() == "0")
                        {
                            khoiluongcongdon += double.Parse(dtTarget.Rows[i]["RealWeight"].ToString());
                            continue;
                        }
                        else
                        {
                            BinSTT = dtTarget.Rows[i]["BinSTT"].ToString();
                            khoiluongcongdon += double.Parse(dtTarget.Rows[i]["RealWeight"].ToString());
                        }
                    }
                    else
                    {
                        BinSTT = dtTarget.Rows[i]["BinSTT"].ToString();
                        khoiluongcongdon += double.Parse(dtTarget.Rows[i]["RealWeight"].ToString());
                    }

                    khoiluong = khoiluongcongdon.ToString();
                    thoigianin = dtTarget.Rows[i]["intime"].ToString().Trim();
                    ngayin = dtTarget.Rows[i]["indat"].ToString().Trim();
                    var dtime = DateTime.ParseExact(ngayin, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    ngayin = dtime.ToString("dd-MM-yyyy");

                    printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(CreateReceipt_quytrinh1_T2); //add an event handler that will do the printing                                                                                                                       //on a till you will not want to ask the user where to print but this is fine for the test envoironment.
                    printDocument.Print();
                    khoiluongcongdon = 0;
                }
            }

            msg a = new msg("In lại thành công");
            a.Show();
            inlai = false;
            txtInLai.SelectAll();
            txtInLai.Focus();
            btIn2.Enabled = false;
            countLast_hoachat = 0;
            lbChonThung.Visible = false;
            cbChonThung.Visible = false;
        }

        private void txtQRCode_TextChanged(object sender, EventArgs e)
        {
            
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

                if (quytrinh1)
                {
                    //Lấy từ CBK
                    var dtPrintT2 = SQL_Conn.SelectDetail_V2(dtChoose.Rows[0]["ManufactureOrderNo"].ToString().Trim(), 2);
                    PrintDocument printDocument = new PrintDocument(); //add the document to the dialog box..
                    printDocument.DefaultPageSettings.PaperSize.RawKind = 119;
                    printDocument.PrinterSettings.DefaultPageSettings.PaperSize.RawKind = 119;
                    printDocument.DefaultPageSettings.Landscape = false;
                    printDocument.PrinterSettings.PrinterName = printerName;

                    if (dtPrintT2.Rows.Count > 0)
                    {
                        dtTarget = dtPrintT2.Clone();
                        DataRow[] rowsToCopy;
                        rowsToCopy = dtPrintT2.Select("ChemStatus = 1 and intime = '' and BinSTT = 0");
                        if (rowsToCopy.Length > 1)
                        {
                            foreach (DataRow temp in rowsToCopy)
                            {
                                dtTarget.ImportRow(temp);
                            }

                            if (dtTarget.Rows.Count == dtMaterialDetail.Select("AuNumber = " + dtTarget.Rows[0]["AuNumber"].ToString()).Length)
                            {
                                tenthung = dtTarget.Rows[0]["AuNumber"].ToString();
                                thoigianin = DateTime.Now.ToString("HH:mm:ss");
                                ngayin = DateTime.Now.ToString("dd-MM-yyyy");
                                soauhientai = int.Parse(tenthung);
                                tongau = 1;

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

                        dtTarget = dtPrintT2.Clone();
                        rowsToCopy = dtMaterialDetail.Select("ChemStatus = 1 and intime = '' and BinSTT > 0");
                        if (rowsToCopy.Length > 0)
                        {
                            foreach (DataRow temp in rowsToCopy)
                            {
                                dtTarget.ImportRow(temp);
                            }

                            for (int i = 0; i < dtTarget.Rows.Count; i++)
                            {
                                tenhoachatT2 = dtTarget.Rows[i]["MaterialCode"].ToString();
                                sokgT2 = dtTarget.Rows[i]["RealWeight"].ToString();
                                tenthung = dtTarget.Rows[i]["AuNumber"].ToString();
                                thoigianin = DateTime.Now.ToString("HH:mm:ss");
                                ngayin = DateTime.Now.ToString("dd-MM-yyyy");
                                soauhientai = int.Parse(tenthung);
                                countLast_hoachat = int.Parse(dtTarget.Rows[i]["BinSTT"].ToString());
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
                    }
                }

                DataTable dt = SQL_Conn.CheckAll_V2(cbMO.Text.ToString());
                if (dt.Rows.Count == 0)
                {
                    try
                    {
                        if (!m.IsDisposed)
                            m.Close();
                    }
                    catch { }

                    lbMaKeo.Visible = false;
                    lbSoMe.Visible = false;
                    panelAu.Visible = false;
                    panelCan.Visible = false;
                    SQL_Conn.updateCommand_V2(cbMO.Text); //Cập nhật hoàn thành 
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
            // printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(CreateReceipt_TEst); //add an event handler that will do the printing
            //on a till you will not want to ask the user where to print but this is fine for the test envoironment.
            printDocument.Print();
        }

        private void txtInLai_TextChanged(object sender, EventArgs e)
        {
            if (txtInLai.Text.Length >= 15)
            {
                //Get MO
                JobNo = txtInLai.Text.Substring(0, 11);
                if (JobNo.Substring(0, 3) == "JRR")
                    quytrinh1 = true;
                else
                    quytrinh1 = false;

                DataTable dtMO = SQL_Conn.GetMO_V2(JobNo);
                cbMO.DataSource = dtMO;
                cbMO.DisplayMember = "ManufactureOrderNo";
                cbMO.ValueMember = "ManufactureOrderNo";

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



        private void txtJob_TextChanged(object sender, EventArgs e)
        {
            if (txtJob.Text.Trim().Length >= 15)
            {
                string job = txtJob.Text.Substring(0, 11);
                HttpResponseMessage response = client.GetAsync("ERP_LIVE/api/v1/BaqSvc/ESN_Scale_JobInfo?JobNo=" + job).Result;
                while (response.IsSuccessStatusCode == false)
                {
                    Thread.Sleep(3000);
                    response = client.GetAsync("ERP_LIVE/api/v1/BaqSvc/ESN_Scale_JobInfo?JobNo=" + job).Result;
                }

                DataTable dt = GetDataTableFromJsonString(response.Content.ReadAsStringAsync().Result);
                if (txtJob.Text.Trim().Substring(0, 3) == "JRR" || txtJob.Text.Trim().Substring(0, 3) == "JCB" || txtJob.Text.Trim().Substring(0, 3) == "JOB")
                {
                    DataTable dtJob = new DataTable();
                    dtJob = SQL_Conn.CheckJobStatus_V2(txtJob.Text.Trim());
                    if (dtJob.Rows.Count == 0)
                    {
                        try
                        {
                            bool b = SQL_Conn.InsertJob_V2(dt);
                            if (b)
                            {
                                DataTable dtMO = SQL_Conn.GetMOFromJob(job); 
                                if(dtMO.Rows.Count > 0)
                                {
                                    cbMO.DataSource = dtMO;
                                    cbMO.DisplayMember = "ManufactureOrderNo";
                                    cbMO.ValueMember = "ManufactureOrderNo";
                                    MessageBox.Show("Load mã cân thành công");
                                }
                                else
                                {
                                    MessageBox.Show("Job đã hoàn thành lệnh cân");
                                    txtJob.Focus();
                                    txtJob.SelectAll();
                                }
                            }
                        }
                        catch
                        {
                            txtJob.Text = "";
                            MessageBox.Show("Lỗi kết nối tới server");
                        }

                    }
                }
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            PrintDocument printDocument = new PrintDocument(); //add the document to the dialog box..
            printDocument.DefaultPageSettings.PaperSize.RawKind = 119;
            printDocument.PrinterSettings.DefaultPageSettings.PaperSize.RawKind = 119;
            printDocument.DefaultPageSettings.Landscape = false;
            printDocument.PrinterSettings.PrinterName = "Microsoft Print to PDF";
            printDocument.DocumentName = "file.pdf";

            printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(CreateReceipt_PhieuCan); //add an event handler that will do the printing
                                                                                                                  //on a till you will not want to ask the user where to print but this is fine for the test envoironment.
            printDocument.Print();
        }

        private void txtInLai_Click(object sender, EventArgs e)
        {

        }

        private void cbMO_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Get all bin
            try
            {
                cbChonThung.DataSource = null;
                cbChonThung.Items.Clear();
                cbChonThung.ResetText();
                if (cbMO.Text.Substring(0, 3) == "1MO")
                {
                    CheckPrint = SQL_Conn.CheckPrint_V2(cbMO.Text);
                    if (CheckPrint.Rows.Count > 0)
                    {
                    
                        //Check lại epicor
                        if (quytrinh1)
                        {
                            var r = CheckPrint.Select("AuNumber = MAX(AuNumber)");

                            for (int i = 1; i <= int.Parse(r[0]["AuNumber"].ToString()); i++)
                                cbChonThung.Items.Add(i.ToString());
                            lbChonThung.Visible = true;
                            cbChonThung.Visible = true;
                        }
                        btIn2.Enabled = true;
                    }
                    else
                    {
                        msg mes = new msg("Mã lệnh này chưa hoàn thành");
                        mes.Show();
                        btIn2.Enabled = false;
                    }
                }

            }
            catch { }
        }

        private void btStartCan_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbMO.Text.Length >= 13)
                {
                    if (a.IsAlive)
                    {
                        a.Abort();
                    }
                    //Get từ epicor từ số job lấy bin đang mở để đóng lại chạy job mới
                    PLCOpen();

                    var JobNoTB = SQL_Conn.SelectRunMO_V2();
                    if (JobNoTB.Rows.Count > 0)
                    {
                        response = client.GetAsync("ERP_LIVE/api/v1/BaqSvc/ESN_Scale_AllBinInJob?JobNo=" + JobNoTB.Rows[0][0].ToString()).Result;

                        while (response.IsSuccessStatusCode == false)
                        {
                            Thread.Sleep(3000);
                            response = client.GetAsync("ERP_LIVE/api/v1/BaqSvc/ESN_Scale_AllBinInJob?JobNo=" + JobNoTB.Rows[0][0].ToString()).Result;
                        }


                        AllBuckets = GetDataTableFromJsonString(response.Content.ReadAsStringAsync().Result);
                        for (int i = 0; i < AllBuckets.Rows.Count; i++)
                        {
                            try
                            {
                                open = AllBuckets.Rows[i]["UDCodes_CodeDesc"].ToString().Split('.');
                                light = AllBuckets.Rows[i]["UDCodes_LongDesc"].ToString().Split('.');
                                cJ2Compolet1.ForceCancel(OMRON.Compolet.CIP.CJ2Compolet.ForceMemoryTypes.CIO, int.Parse(open[0]), Int16.Parse(open[1]));
                                cJ2Compolet1.ForceCancel(OMRON.Compolet.CIP.CJ2Compolet.ForceMemoryTypes.CIO, int.Parse(light[0]), Int16.Parse(light[1]));
                            }
                            catch { }
                        }
                    }

                    //-----------------------------------------------------------------                                                        
                    //Lấy thông tin lệnh cân. K cần epicor
                    dtChoose = SQL_Conn.SelectMO_V2(cbMO.Text.ToString());
                    PartNum = dtChoose.Rows[0]["ChemicalOrderCode"].ToString();
                    if (dtChoose.Rows.Count == 0)
                    {
                        msg error = new msg("Lệnh sản xuất đã hoàn thành.");
                        error.TopMost = true;
                        error.Show();
                        cbMO.Text = "";
                        cbMO.Focus();
                        cbMO.SelectAll();
                    }
                    else
                    {
                        if (!comport.IsOpen)
                            COMOpen();

                        PLCOpen();
                        if (dtChoose.Rows[0]["JobNo"].ToString().Substring(0, 3) == "JRR")
                        {
                            //* Lấy từ epicor                          
                            //response = client.GetAsync("ERP_LIVE/api/v1/BaqSvc/ESN_Scale_RRCSTT?PartNum=" + PartNum).Result;
                            //dtSortMateiral = GetDataTableFromJsonString(response.Content.ReadAsStringAsync().Result);

                            quytrinh1 = true;
                        }
                        else
                            quytrinh1 = false;

                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress + "ERP_LIVE/api/v1/Erp.BO.IssueReturnSvc/PerformMaterialMovement");
                        response = client.GetAsync("ERP_LIVE/api/v1/BaqSvc/ESN_Scale_COH?JobNum=" + dtChoose.Rows[0]["JobNo"].ToString()).Result;
                        while (response.IsSuccessStatusCode == false)
                        {
                            Thread.Sleep(3000);
                            response = client.GetAsync("ERP_LIVE/api/v1/BaqSvc/ESN_Scale_COH?JobNum=" + dtChoose.Rows[0]["JobNo"].ToString()).Result;
                        }
                        dtCheckOnHand = GetDataTableFromJsonString(response.Content.ReadAsStringAsync().Result);

                        Au = null;
                        Total = null;
                        soau = 1;
                        Total_Temp = null;
                        Stt_Temp = 0;
                        Stt_Can_Temp = 0;
                        Stt_Au_Temp = 1;
                        QRCode = "";
                        canquytrinh1_1 = false;
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

                        mo = cbMO.Text;
                        btDongThung.Visible = true;
                        btXacNhan.Visible = true;
                        lbMaKeo.Visible = true;
                        lbSoMe.Visible = true;
                        lbSoKG.Visible = true;
                        panelAu.Visible = true; //Hiện panel Âu để cân âu
                        txtQRAu.SelectAll();

                        //Lấy thông tin của lệnh vừa được chọn
                        //Thêm epicor
                        response = client.GetAsync("ERP_LIVE/api/v1/BaqSvc/ESN_Scale_JobInfo?JobNo=" + dtChoose.Rows[0]["JobNo"].ToString()).Result;
                        while (response.IsSuccessStatusCode == false)
                        {
                            Thread.Sleep(3000);
                            response = client.GetAsync("ERP_LIVE/api/v1/BaqSvc/ESN_Scale_JobInfo?JobNo=" + dtChoose.Rows[0]["JobNo"].ToString()).Result;
                        }
                        dtBuckets = GetDataTableFromJsonString(response.Content.ReadAsStringAsync().Result);

                        //Lấy part description
                        response = client.GetAsync("ERP_LIVE/api/v1/BaqSvc/ESN_Scale_GetPartD?PartNum=" + PartNum).Result;
                        while (response.IsSuccessStatusCode == false)
                        {
                            Thread.Sleep(3000);
                            response = client.GetAsync("ERP_LIVE/api/v1/BaqSvc/ESN_Scale_GetPartD?PartNum=" + PartNum).Result;
                        }
                        DataTable dtGetPartDesc = GetDataTableFromJsonString(response.Content.ReadAsStringAsync().Result);
                        makeo = dtGetPartDesc.Rows[0]["Part_PartDescription"].ToString();
                        SQL_Conn.updatePartName(cbMO.Text, makeo);

                        //Lấy các thùng ở cân trạm 1 của Job này
                        //Thêm epicor
                        //dtAllBucketActive = SQL_Conn.SelectAllBucketActive(txtQRCode.Text.ToString());
                        response = client.GetAsync("ERP_LIVE/api/v1/BaqSvc/ESN_Scale_AllBinPort1?JobNo=" + dtChoose.Rows[0]["JobNo"].ToString()).Result;
                        while (response.IsSuccessStatusCode == false)
                        {
                            Thread.Sleep(3000);
                            response = client.GetAsync("ERP_LIVE/api/v1/BaqSvc/ESN_Scale_AllBinPort1?JobNo=" + dtChoose.Rows[0]["JobNo"].ToString()).Result;
                        }
                        dtAllBucketActive = GetDataTableFromJsonString(response.Content.ReadAsStringAsync().Result);

                        //Lấy thông tin đang cân. Không cần epicor
                        dtChooseCTY = SQL_Conn.SelectCommandCTY_V2(cbMO.Text.ToString(), 1);
                        dtMaterialDetail = SQL_Conn.SelectDetail_V2(cbMO.Text.ToString(), 1);
                        MaterialTemp = SQL_Conn.SelectCommandTemp_V2(cbMO.Text.ToString(), 1);

                        Au = new string[int.Parse(dtChoose.Rows[0]["BatchNo"].ToString()), 2]; //Khai báo số Âu theo lệnh điều động
                        Total = new string[int.Parse(dtChoose.Rows[0]["BatchNo"].ToString()), 2]; //Khai báo tổng số lượng của từng âu

                        //Gán dữ liệu để quét tiếp nếu bị lỗi cân khi đang quét
                        for (int i = 0; i < MaterialTemp.Rows.Count; i++)
                        {
                            Au[i, 0] = MaterialTemp.Rows[i][1].ToString();
                            Au[i, 1] = MaterialTemp.Rows[i][4].ToString();
                            Total[i, 0] = MaterialTemp.Rows[i][1].ToString();
                            if (quytrinh1)
                                Total[i, 1] = 0.ToString();
                            else
                                Total[i, 1] = MaterialTemp.Rows[i][5].ToString();

                            int AuNumber = int.Parse(MaterialTemp.Rows[i][2].ToString());
                            string QRAu = MaterialTemp.Rows[i][1].ToString();
                            if (AuNumber >= Stt_Au_Temp && QRAu != "")
                            {
                                Stt_Au_Temp = AuNumber + 1;
                            }
                        }

                        //Gán thông tin keo mà tổng số âu
                        lbSoKG.Text = "Số KG: " + dtChoose.Rows[0]["Weight"].ToString();
                        lbMaKeo.Text = "Mã keo 1: " + makeo;

                        if (!quytrinh1)
                            lbSoMe.Text = "Tổng số hộp: " + dtChoose.Rows[0]["BatchNo"].ToString();
                        else
                            lbSoMe.Text = "Tổng số cây: " + dtChoose.Rows[0]["BatchNo"].ToString();

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
    }
}
