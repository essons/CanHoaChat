using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;

namespace CanHoaChat
{
    class SQL_Conn
    {
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
        public static DataTable SelectCommandCTY(string ManufactureOrderNo)
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

        public static DataTable SelectCommandTemp(string ManufactureOrderNo)
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
        public static bool updateQRCode(string ManufactureOrderNo, int stt, string QRCode, string ngayin, string thoigianin, string khoiluong)
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
        public static bool updateAuTemp(string ManufactureOrderNo, string AuTareWeight, string QRAu, string soau)
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
        public static bool updateAuTotalTemp(string ManufactureOrderNo, string Total, string soau)
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
        public static bool updateSttTemp(string ManufactureOrderNo, string soau)
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
    }
}
