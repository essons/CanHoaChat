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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace CanHoaChat
{
    public partial class UCCanTuDong : MetroFramework.Controls.MetroUserControl
    {

        public static SerialPort comport = new SerialPort();
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

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;
            else
            {
                return true;
            }
        }

        private void UCCanTuDong_Load(object sender, EventArgs e)
        {
            timeout = 0;
            PLCOpen();
            COMOpen();
            txtQRCode.Focus();



            //for(int i = 0; ;i++)
            //{
            //    var strData = comport.ReadExisting().ToString();
            //    var length = strData.Length;
            //    if (length > 17)
            //    {
            //        var data2 = strData.Substring(length - 17, 10);
            //        break;
            //    }
            //    Thread.Sleep(1000);
            //}

            //cJ2Compolet1.Active = true;



            ServicePointManager.ServerCertificateValidationCallback =
                    new RemoteCertificateValidationCallback(ValidateServerCertificate);

            //var tenders = response.Content.ReadAsAsync<tenders>().Result;
        }

        public void COMOpen()
        {
            try
            {
                string strCOMname = "COM4";
                comport.PortName = strCOMname.Trim();
                comport.BaudRate = 9600;
                comport.Parity = System.IO.Ports.Parity.None;
                comport.DataBits = 8;
                comport.StopBits = System.IO.Ports.StopBits.One;
                comport.Open();
            }
            catch (Exception ex)
            {

            }
        }

        public void COMClose()
        {
            if (comport.IsOpen == true)
            {
                comport.Close();
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

        private void txtQRCode_TextChanged(object sender, EventArgs e)
        {
            if (txtQRCode.Text.Length >= 1)
            {
                if (!timer2.Enabled)
                    timer2.Enabled = true;
            }
        }


        int timeout = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            timeout = timeout + 5;
            if (timeout >= 700)
            {
                timeout = 0;
                MetroLink link = (Form1.Instance.Controls["mlBack"] as MetroLink);
                link.PerformClick();
            }
        }

        int show = 0;
        private void timer2_Tick(object sender, EventArgs e)
        {
            show++;
            timeout = 0;
            if (show == 1)
            {
                string[] open = new string[2]; //Mo thung
                string[] light = new string[2];
                DataTable dt = SQL_Conn.GetBucketPosition_V2(txtQRCode.Text);

                if (dt.Rows.Count > 0)
                {
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
                    timer2.Enabled = false;
                    show = 0;
                }
                else
                {
                    txtQRCode.Text = "";
                    timer2.Enabled = false;
                    show = 0;
                }
            }

        }

        private void txtJob_TextChanged(object sender, EventArgs e)
        {
            if (txtJob.Text.Length >= 11)
            {
                HttpClientHandler handler = new HttpClientHandler();
                handler.Credentials = new NetworkCredential("test", "testing");
                HttpClient client = new HttpClient(handler);
                client.BaseAddress = new Uri("https://srv-epi-app2/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.
                    MediaTypeWithQualityHeaderValue("application/json"));
                var byteArray = Encoding.ASCII.GetBytes($"{"manager"}:{"Manager"}");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress + "ERP_TEST/api/v1/Erp.BO.IssueReturnSvc/PerformMaterialMovement");

                var response = client.GetAsync("ERP_TEST/api/v1/BaqSvc/ESS_CheckOnHand?JobNum=" + txtJob.Text).Result;
                DataTable dt = GetDataTableFromJsonString(response.Content.ReadAsStringAsync().Result);

                string CHENUM = "";
                string LOTNO = "";
                string NEXTCHE = "";
                double REQCHE = 0;
                double REALONHAND = 0;
                double REALISSUE = 0;
                double SUMISSUE = 0;
                string BINNUM = "";
                string WAREHOUSECODE = "";
                string Issued = "";
                int JobMtl_MtlSeq = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    NEXTCHE = dt.Rows[i]["JobMtl_PartNum"].ToString();
                    if (NEXTCHE != CHENUM)
                    {
                        CHENUM = NEXTCHE;
                        REQCHE = 0;
                        REALONHAND = 0;
                        REALISSUE = 0;
                        SUMISSUE = 0;
                        JobMtl_MtlSeq = int.Parse(dt.Rows[i]["JobMtl_MtlSeq"].ToString());
                        REQCHE = double.Parse(dt.Rows[i]["JobMtl_RequiredQty"].ToString());
                        REALONHAND = double.Parse(dt.Rows[i]["PartBin_OnhandQty"].ToString());
                        LOTNO = dt.Rows[i]["PartBin_LotNum"].ToString();
                        BINNUM = dt.Rows[i]["PartBin_BinNum"].ToString();
                        WAREHOUSECODE = dt.Rows[i]["PartBin_WarehouseCode"].ToString();
                        Issued = dt.Rows[i]["Calculated_SumQty"].ToString();
                        if (Issued != "")
                            REQCHE = REQCHE - double.Parse(Issued);
                    }
                                                                             

                    if (SUMISSUE < REQCHE && REQCHE > 0)
                        {
                            if (REALONHAND >= REQCHE)
                            {
                                REALISSUE = REQCHE;
                                SUMISSUE += REALISSUE;
                            }
                            else
                            {
                                REALISSUE = REALONHAND;
                                REQCHE = REQCHE - REALISSUE;
                                SUMISSUE += REALISSUE;
                            }

                            var json = @"{
                                ""plNegQtyAction"": true,
                                ""ds"": {
                                ""IssueReturn"": [
                                  {
                                    ""Company"": ""ES168899"",
                                    ""TranDate"": """ + DateTime.Now.ToString("yyyy-MM-dd") + @"T00:00:00+07:00"",
                                    ""PartNum"": """ + NEXTCHE + @""",
                                    ""TranQty"": """ + REALISSUE.ToString() + @""",
                                    ""DimCode"": ""KG"",
                                    ""LotNum"": """+ LOTNO + @""",
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
                                    ""ToJobNum"": """ + dt.Rows[i]["JobMtl_JobNum"].ToString() + @""",
                                    ""ToAssemblySeq"": 0,
                                    ""ToJobSeq"": "+ JobMtl_MtlSeq + @",
                                    ""ToWarehouseCode"": """ + WAREHOUSECODE + @""",
                                    ""ToBinNum"": """ + BINNUM + @""",
                                    ""ToJobPartNum"": """ + dt.Rows[0]["JobHead_PartNum"].ToString() + @""",
                                    ""ToAssemblyPartNum"": """ + dt.Rows[0]["JobHead_PartNum"].ToString() + @""",
                                    ""ToJobSeqPartNum"": """ + NEXTCHE + @""",
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
                                    ""TransYear"": 2022,
                                    ""TransYearSuffix"": """",
                                    ""DspTransYear"": ""2022"",
                                    ""ShowDspTransYear"": false,
                                    ""Prefix"": ""STW"",
                                    ""PrefixList"": """",
                                    ""NumberSuffix"": """",
                                    ""EnablePrefix"": false,
                                    ""EnableSuffix"": false,
                                    ""NumberOption"": ""Manual"",
                                    ""DocumentDate"": ""2022-03-31T00:00:00+07:00"",
                                    ""GenerationType"": ""system"",
                                    ""Description"": ""Issue Materials"",
                                    ""TransPeriod"": 3,
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

                        request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress + "ERP_TEST/api/v1/Erp.BO.IssueReturnSvc/PerformMaterialMovement");
                        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                            response = client.SendAsync(request).Result;
                        }
                    else
                        continue;

                }

            }
        }
    }
}
