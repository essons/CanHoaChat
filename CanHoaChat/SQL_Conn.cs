using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CanHoaChat
{
    class SQL_Conn
    {
        public static DataTable GetSortMaterial_V2(string sothe)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@SoThe", sothe));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "GetSortMaterial"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable SelectHistory_V2()
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "CheckHistory"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }

        public static DataTable CheckNV_V2(string empid)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@empid", empid));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "CheckNV"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }

        public static DataTable CheckPrint_V2(string mo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@MO", mo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "CheckPrint"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable CheckJobStatus_V2(string JobNo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@JobNo", JobNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "CheckJobStatus"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable SelectRunMO_V2(bool bh2)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@bh2", bh2));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "SelectRunMO"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static bool InsertJob_V2(DataTable dt, bool bh2)
        {          
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            string PartNum = dt.Rows[0]["JobHead_PartNum"].ToString();
            string JobNo = dt.Rows[0]["JobMtl_JobNum"].ToString();

            Double weight = Double.Parse(dt.Rows[0]["JobHead_ProdQty"].ToString()); 
            if (dt.Rows[0]["JobMtl2_QtyPer"].ToString() != "")
                weight = Double.Parse(dt.Rows[0]["JobHead_ProdQty"].ToString()) * (1 - Double.Parse(dt.Rows[0]["JobMtl2_QtyPer"].ToString()));

            Double Qty1Mix = 25;
            if(dt.Rows[0]["UDCodes6_LongDesc"].ToString() != "")
                Qty1Mix = Double.Parse(dt.Rows[0]["UDCodes6_LongDesc"].ToString());

            dt.Columns.Remove("JobHead_ProdQty");
            dt.Columns.Remove("JobMtl_IUM");
            dt.Columns.Remove("JobHead_PartNum");
            dt.Columns.Remove("Part_PartDescription");
            dt.Columns.Remove("JobMtl_Description");
            dt.Columns.Remove("UDCodes6_LongDesc");
            dt.Columns.Remove("JobMtl2_QtyPer");
            dt.Columns.Remove("RowIdent");
            dt.Columns.Add("BinType");
            dt.Columns["JobMtl_JobNum"].ColumnName = "JobNo";
            dt.Columns["JobMtl_PartNum"].ColumnName = "PartNum";
            dt.Columns["JobMtl_RequiredQty"].ColumnName = "weight";
            dt.Columns["UDCodes1_CodeID"].ColumnName = "ZoneNum";
            dt.Columns["Calculated_AuNumber"].ColumnName = "AuNumber";
            dt.AcceptChanges();
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                        new SqlParameter("@ddctpc", dt));
                    cmd.Parameters.Add(
                        new SqlParameter("@MaKeo1", PartNum));
                    cmd.Parameters.Add(
                        new SqlParameter("@JobNo", JobNo));
                    cmd.Parameters.Add(
                       new SqlParameter("@Weight", weight));
                    cmd.Parameters.Add(
                       new SqlParameter("@Qty1Mix", Qty1Mix));

                    if (bh2)
                        cmd.Parameters.Add(
                            new SqlParameter("@BH2", 1));
                    else
                        cmd.Parameters.Add(
                            new SqlParameter("@BH2", 0));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "DieuDong"));

                    int effectedRow = cmd.ExecuteNonQuery();
                    return effectedRow > 0;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        public static bool InsertDetail_V2(string ManufactureOrderNo, string Weight, string QRAu, string AuNumber)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                      new SqlParameter("@QRAu", QRAu));
                    cmd.Parameters.Add(
                      new SqlParameter("@AuNumber", AuNumber));
                    cmd.Parameters.Add(
                       new SqlParameter("@Weight", Weight));
                    cmd.Parameters.Add(
                       new SqlParameter("@MO", ManufactureOrderNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "UpdateAuTemp"));

                    int effectedRow = cmd.ExecuteNonQuery();
                    return effectedRow > 0;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        public static DataTable SelectMOActive_V2(bool bh2)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                      new SqlParameter("@bh2", bh2));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "SelectMOActive"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable SelectMO_V2(string mo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@MO", mo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "SelectMO"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable SelectCommandCTY_V2(string ManufactureOrderNo, int MachineNo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                         new SqlParameter("@MO", ManufactureOrderNo));
                    cmd.Parameters.Add(
                        new SqlParameter("@MachineNo", MachineNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "SelectCTY"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable GetMOFromJob(string JobNo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                         new SqlParameter("@JobNo", JobNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "GetMOFromJob"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable SelectDetail_V2(string ManufactureOrderNo, int MachineNo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                         new SqlParameter("@MO", ManufactureOrderNo));
                    cmd.Parameters.Add(
                        new SqlParameter("@MachineNo", MachineNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "SelectMaterialDetail"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable SelectCommandTemp_V2(string ManufactureOrderNo, int MachineNo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@MO", ManufactureOrderNo));
                    cmd.Parameters.Add(
                     new SqlParameter("@MachineNo", MachineNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "SelectTemp"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }

        public static bool updateMOLocation(string ManufactureOrderNo, bool bh2)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                      new SqlParameter("@MO", ManufactureOrderNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@BH2", bh2));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "UpdateMOLocation"));

                    int effectedRow = cmd.ExecuteNonQuery();
                    return effectedRow > 0;
                }
                catch (Exception ex)
                {

                    return false;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        public static bool updateAuTemp_V2(string ManufactureOrderNo, string AuTareWeight, string QRAu, string soau, int MachineNo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                      new SqlParameter("@MO", ManufactureOrderNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@AuTareWeight", AuTareWeight));
                    cmd.Parameters.Add(
                     new SqlParameter("@QRAu", QRAu));
                    cmd.Parameters.Add(
                      new SqlParameter("@AuNumber", soau));
                    cmd.Parameters.Add(
                     new SqlParameter("@MachineNo", MachineNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "UpdateAuTemp"));

                    int effectedRow = cmd.ExecuteNonQuery();
                    return effectedRow > 0;
                }
                catch (Exception ex)
                {

                    return false;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        public static bool updateAuTotalTemp_V2(string ManufactureOrderNo, string Total, string soau, int MachineNo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                      new SqlParameter("@MO", ManufactureOrderNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@Total", Total));
                    cmd.Parameters.Add(
                        new SqlParameter("@MachineNo", MachineNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@AuNumber", soau));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "UpdateAuTotalTemp"));

                    int effectedRow = cmd.ExecuteNonQuery();
                    return effectedRow > 0;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        public static bool updateSttTemp_V2(string ManufactureOrderNo, string soau, int MachineNo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                      new SqlParameter("@MO", ManufactureOrderNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@AuNumber", soau));
                    cmd.Parameters.Add(
                        new SqlParameter("@MachineNo", MachineNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "UpdateSttTemp"));

                    int effectedRow = cmd.ExecuteNonQuery();
                    return effectedRow > 0;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        public static bool updateTempDetail_V2(string ManufactureOrderNo,int AuNumber, int BinSTT ,string MaterialCode, string MaterialName ,double Weight, int MachineNo, string empid)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                      new SqlParameter("@MO", ManufactureOrderNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@BinSTT", BinSTT));
                    cmd.Parameters.Add(
                      new SqlParameter("@AuNumber", AuNumber));
                    cmd.Parameters.Add(
                      new SqlParameter("@MaterialCode", MaterialCode));
                    cmd.Parameters.Add(
                      new SqlParameter("@PartMtlName", MaterialName));
                    cmd.Parameters.Add(
                      new SqlParameter("@Weight", Weight));
                    cmd.Parameters.Add(
                        new SqlParameter("@MachineNo", MachineNo));
                    cmd.Parameters.Add(
                        new SqlParameter("@empid", empid));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "UpdateTempDetail"));

                    int effectedRow = cmd.ExecuteNonQuery();
                    return effectedRow > 0;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        public static bool updateQRCode_V2(string ManufactureOrderNo, int stt, int binstt ,string QRCode, string ngayin, string thoigianin, string khoiluong, int MachineNo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                      new SqlParameter("@Mo", ManufactureOrderNo));
                    cmd.Parameters.Add(
                     new SqlParameter("@QRCode", QRCode));
                    cmd.Parameters.Add(
                      new SqlParameter("@AuNumber", stt));
                    cmd.Parameters.Add(
                      new SqlParameter("@BinSTT", binstt));
                    cmd.Parameters.Add(
                    new SqlParameter("@PrintTime", thoigianin));
                    cmd.Parameters.Add(
                      new SqlParameter("@PrintDate", ngayin));
                    cmd.Parameters.Add(
                      new SqlParameter("@Total", khoiluong));
                    cmd.Parameters.Add(
                     new SqlParameter("@MachineNo", MachineNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "UpdateQR"));

                    int effectedRow = cmd.ExecuteNonQuery();
                    return effectedRow > 0;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        public static DataTable CountMaterialT2_V2(string mo, string materialcode)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@ManufactureOrderNo", mo));
                    cmd.Parameters.Add(
                    new SqlParameter("@MaterialCode", materialcode));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "CountMaterialTram2"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable CheckPrintQT1_V2(string mo, string sothe, string aunumber)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@ManufactureOrderNo", mo));
                    cmd.Parameters.Add(
                     new SqlParameter("@SoThe", sothe));
                    cmd.Parameters.Add(
                     new SqlParameter("@AuNumber", aunumber));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "CheckPrintQT1"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static bool updateCompTram2_V2(string ManufactureOrderNo, string MaterialCode, double weight, string intime)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                      new SqlParameter("@ManufactureOrderNo", ManufactureOrderNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@MaterialCode", MaterialCode));
                    cmd.Parameters.Add(
                     new SqlParameter("@Weight", weight));
                    cmd.Parameters.Add(
                     new SqlParameter("@intime", intime));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "UpdateCompTram2"));

                    int effectedRow = cmd.ExecuteNonQuery();
                    return effectedRow > 0;
                }
                catch (Exception ex)
                {

                    return false;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        public static bool updateCompTram1_V2(string ManufactureOrderNo, string Aunumber)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                      new SqlParameter("@ManufactureOrderNo", ManufactureOrderNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@AuNumber", Aunumber));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "UpdateCompTram1"));

                    int effectedRow = cmd.ExecuteNonQuery();
                    return effectedRow > 0;
                }
                catch (Exception ex)
                {

                    return false;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        public static DataTable SelectCompTram2_V2(string mo, string Aunumber)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@ManufactureOrderNo", mo));
                    cmd.Parameters.Add(
                     new SqlParameter("@AuNumber", Aunumber));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "SelectCompTram2"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable SelectCompTram2Print_V2(string mo, string Aunumber)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@ManufactureOrderNo", mo));
                    cmd.Parameters.Add(
                     new SqlParameter("@AuNumber", Aunumber));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "SelectCompTram2Print"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable SelectCompTram1_V2(string mo, string qrAu)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@ManufactureOrderNo", mo));
                    cmd.Parameters.Add(
                     new SqlParameter("@QRAu", qrAu));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "SelectCompTram1"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable CheckCompTram2_V2(string mo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@ManufactureOrderNo", mo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "CheckCompTram2"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable GetQRPrint_V2(string mo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@ManufactureOrderNo", mo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "GetQRPrint"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable GetPrint_V2(string mo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@ManufactureOrderNo", mo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "GetPrint"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable GetMO_V2(string JobNo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@JobNo", JobNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "GetMO"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable CheckAll_V2(string ManufactureOrderNo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "CheckAll"));
                    cmd.Parameters.Add(
                      new SqlParameter("@MO", ManufactureOrderNo));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static bool updateCommand_V2(string ManufactureOrderNo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                      new SqlParameter("@MO", ManufactureOrderNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@empid", Error.User));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "UpdateEnd"));

                    int effectedRow = cmd.ExecuteNonQuery();
                    return effectedRow > 0;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        public static bool updateMachine_V2(string ManufactureOrderNo, int MachineNo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                      new SqlParameter("@MO", ManufactureOrderNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@MachineNo", MachineNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "UpdateMachine"));

                    int effectedRow = cmd.ExecuteNonQuery();
                    return effectedRow > 0;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        public static DataTable GetBucketPosition_V2(string MaterialName)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@MaterialName", MaterialName));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "GetPosition"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable SelectTenMaKeo_V2(string mo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@ManufactureOrderNo", mo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "GetTenMaKeo"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable SelectAllBucketActive_V2(string mo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@ManufactureOrderNo", mo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "SelectAllBucket"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }

        public static bool updatePartName(string ManufactureOrderNo, string PartName)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                      new SqlParameter("@MO", ManufactureOrderNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@Partname", PartName));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "UpdatePartName"));

                    int effectedRow = cmd.ExecuteNonQuery();
                    return effectedRow > 0;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }


        public static DataTable SelectCommand()
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "Select"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable SelectHistory()
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "CheckHistory"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable GetSortMaterial(string sothe)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@SoThe", sothe));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "GetSortMaterial"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable CheckPrint(string mo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@ManufactureOrderNo", mo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "CheckPrint"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable CountMaterialT2(string mo, string materialcode)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@ManufactureOrderNo", mo));
                    cmd.Parameters.Add(
                    new SqlParameter("@MaterialCode", materialcode));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "CountMaterialTram2"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable CheckPrintQT1(string mo, string sothe, string aunumber)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@ManufactureOrderNo", mo));
                    cmd.Parameters.Add(
                     new SqlParameter("@SoThe", sothe));
                    cmd.Parameters.Add(
                     new SqlParameter("@AuNumber", aunumber));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "CheckPrintQT1"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static bool updateCompTram2(string ManufactureOrderNo, string MaterialCode, double weight, string intime)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                      new SqlParameter("@ManufactureOrderNo", ManufactureOrderNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@MaterialCode", MaterialCode));
                    cmd.Parameters.Add(
                     new SqlParameter("@Weight", weight));
                    cmd.Parameters.Add(
                     new SqlParameter("@intime", intime));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "UpdateCompTram2"));

                    int effectedRow = cmd.ExecuteNonQuery();
                    return effectedRow > 0;
                }
                catch (Exception ex)
                {

                    return false;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        public static bool updateCompTram1(string ManufactureOrderNo, string Aunumber)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                      new SqlParameter("@ManufactureOrderNo", ManufactureOrderNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@AuNumber", Aunumber));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "UpdateCompTram1"));

                    int effectedRow = cmd.ExecuteNonQuery();
                    return effectedRow > 0;
                }
                catch (Exception ex)
                {

                    return false;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        public static DataTable SelectCompTram2(string mo, string Aunumber)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@ManufactureOrderNo", mo));
                    cmd.Parameters.Add(
                     new SqlParameter("@AuNumber", Aunumber));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "SelectCompTram2"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable SelectCompTram2Print(string mo, string Aunumber)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@ManufactureOrderNo", mo));
                    cmd.Parameters.Add(
                     new SqlParameter("@AuNumber", Aunumber));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "SelectCompTram2Print"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable SelectCompTram1(string mo, string qrAu)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@ManufactureOrderNo", mo));
                    cmd.Parameters.Add(
                     new SqlParameter("@QRAu", qrAu));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "SelectCompTram1"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable CheckCompTram2(string mo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@ManufactureOrderNo", mo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "CheckCompTram2"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable GetQRPrint(string mo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@ManufactureOrderNo", mo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "GetQRPrint"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable GetTyLe(string mo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@ManufactureOrderNo", mo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "GetTyLe"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable SelectAllBucket()
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "SelectAll"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable SelectRunMO()
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "SelectRunMO"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }       
        public static DataTable SelectBuckets(string mo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@ManufactureOrderNo", mo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "SelectBuckets"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable SelectAllBucketActive(string mo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@ManufactureOrderNo", mo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "SelectAllBucket"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable SelectTenMaKeo(string mo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@ManufactureOrderNo", mo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "GetTenMaKeo"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable SelectBuckets2(string mo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@ManufactureOrderNo", mo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "SelectBuckets2"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable SelectMO(string mo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@ManufactureOrderNo", mo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "SelectMO"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable CheckNV(string empid)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@empid", empid));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "CheckNV"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        public static DataTable GetPrint(string mo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@ManufactureOrderNo", mo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "GetPrint"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }

        public static DataTable GetDetailPrint_V2(string mo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien_V2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@MO", mo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "GetDetailPrint"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }

        public static DataTable SelectMOActive()
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //cmd.Parameters.Add(
                    // new SqlParameter("@ManufactureOrderNo", mo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "SelectMOActive"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable GetBucketPosition(string MaterialName)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@MaterialName", MaterialName));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "GetPosition"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable SelectCommandPrint()
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "SelectIn"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable SelectCommandCTY(string ManufactureOrderNo, int MachineNo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                         new SqlParameter("@ManufactureOrderNo", ManufactureOrderNo));
                    cmd.Parameters.Add(
                        new SqlParameter("@MachineNo", MachineNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "SelectCTY"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable SelectCommandTemp(string ManufactureOrderNo, int MachineNo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                     new SqlParameter("@ManufactureOrderNo", ManufactureOrderNo));
                    cmd.Parameters.Add(
                     new SqlParameter("@MachineNo", MachineNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "SelectTemp"));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }
        public static DataTable CheckAll(string ManufactureOrderNo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "CheckAll"));
                    cmd.Parameters.Add(
                      new SqlParameter("@ManufactureOrderNo", ManufactureOrderNo));
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(sdr);
                    return dt;
                }
                catch (Exception ex)
                {
                    return new DataTable();
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }

        }     
        public static bool updateMachine(string ManufactureOrderNo, int MachineNo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                      new SqlParameter("@ManufactureOrderNo", ManufactureOrderNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@MachineNo", MachineNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "UpdateMachine"));

                    int effectedRow = cmd.ExecuteNonQuery();
                    return effectedRow > 0;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        public static bool updateCommand(string ManufactureOrderNo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                      new SqlParameter("@ManufactureOrderNo", ManufactureOrderNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@empid", Error.User));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "Update"));

                    int effectedRow = cmd.ExecuteNonQuery();
                    return effectedRow > 0;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        public static bool updateQRCode(string ManufactureOrderNo, int stt, string QRCode, string ngayin, string thoigianin, string khoiluong, int MachineNo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                      new SqlParameter("@ManufactureOrderNo", ManufactureOrderNo));
                    cmd.Parameters.Add(
                     new SqlParameter("@QRCode", QRCode));
                    cmd.Parameters.Add(
                      new SqlParameter("@AuNumber", stt));
                    cmd.Parameters.Add(
                    new SqlParameter("@PrintTime", thoigianin));
                    cmd.Parameters.Add(
                      new SqlParameter("@PrintDate", ngayin));
                    cmd.Parameters.Add(
                      new SqlParameter("@Total", khoiluong));
                    cmd.Parameters.Add(
                     new SqlParameter("@MachineNo", MachineNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "UpdateQR"));

                    int effectedRow = cmd.ExecuteNonQuery();
                    return effectedRow > 0;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        public static bool updateAuTemp(string ManufactureOrderNo, string AuTareWeight, string QRAu, string soau, int MachineNo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                      new SqlParameter("@ManufactureOrderNo", ManufactureOrderNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@AuTareWeight", AuTareWeight));
                    cmd.Parameters.Add(
                     new SqlParameter("@QRAu", QRAu));
                    cmd.Parameters.Add(
                      new SqlParameter("@AuNumber", soau));
                    cmd.Parameters.Add(
                     new SqlParameter("@MachineNo", MachineNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "UpdateAuTemp"));

                    int effectedRow = cmd.ExecuteNonQuery();
                    return effectedRow > 0;
                }
                catch (Exception ex)
                {
                   
                    return false;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        public static bool updateAuTotalTemp(string ManufactureOrderNo, string Total, string soau, int MachineNo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                      new SqlParameter("@ManufactureOrderNo", ManufactureOrderNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@Total", Total));
                    cmd.Parameters.Add(
                        new SqlParameter("@MachineNo", MachineNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@AuNumber", soau));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "UpdateAuTotalTemp"));

                    int effectedRow = cmd.ExecuteNonQuery();
                    return effectedRow > 0;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        public static bool updateSttTemp(string ManufactureOrderNo, string soau, int MachineNo)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                      new SqlParameter("@ManufactureOrderNo", ManufactureOrderNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@AuNumber", soau));
                    cmd.Parameters.Add(
                        new SqlParameter("@MachineNo", MachineNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "UpdateSttTemp"));

                    int effectedRow = cmd.ExecuteNonQuery();
                    return effectedRow > 0;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
        public static bool updateTempDetail(string ManufactureOrderNo, string QRAu, string MaterialCode ,double Weight ,int MachineNo, string empid)
        {
            string ConnectionString = @"Data Source=SRV-DB-02\SQLEXPRESS;Initial Catalog=CBK;User ID=sa;Password=Es@2020";
            //string ConnectionString = @"Data Source = 198.1.1.95; Initial Catalog = JianDaMES; User ID = kendakv2; Password = kenda123";
            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_CanDien", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(
                      new SqlParameter("@ManufactureOrderNo", ManufactureOrderNo));
                    cmd.Parameters.Add(
                      new SqlParameter("@QRAu", QRAu));
                    cmd.Parameters.Add(
                      new SqlParameter("@MaterialCode", MaterialCode));
                    cmd.Parameters.Add(
                      new SqlParameter("@Weight", Weight));
                    cmd.Parameters.Add(
                        new SqlParameter("@MachineNo", MachineNo));
                    cmd.Parameters.Add(
                        new SqlParameter("@empid", empid));
                    cmd.Parameters.Add(
                      new SqlParameter("@type", "UpdateTempDetail"));

                    int effectedRow = cmd.ExecuteNonQuery();
                    return effectedRow > 0;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
            }
        }
    }
}
