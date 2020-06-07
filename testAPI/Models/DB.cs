using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace testAPI.Models
{
    public class DB
    {
        public SqlConnection connDB = new SqlConnection(ConfigurationManager.ConnectionStrings["TESTAPI"].ConnectionString);//取得web.config連線物件

        /// <summary>
        /// 取得資料庫資料
        /// </summary>
        /// <param name="sql">SQL語法</param>
        /// <returns></returns>
        public DataTable getDB(string sql)
        {
            if (string.IsNullOrEmpty(sql))
                return null;
            try
            {
                connDB.Open();
            }
            catch (Exception ex)
            {
                connDB.Close();
                throw ex;
            }
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand(sql, connDB);
            try
            {
                da.SelectCommand = cmd;
                da.Fill(ds, "Table");
            }
            catch (SqlException ex)
            {
                connDB.Close();
                cmd.Dispose();
                da.Dispose();
                throw ex;
            }
            DataTable dt = ds.Tables["Table"];
            connDB.Close();
            cmd.Dispose();
            da.Dispose();
            return dt;
        }
        /// <summary>
        /// INSERT、UPDATE、DELETE資料庫，並回傳影響列數
        /// </summary>
        /// <param name="sql">SQL語法</param>
        /// <returns></returns>
        public int executeSql_int(string sql)
        {
            int columns = 0;
            try
            {
                connDB.Open();
            }
            catch
            {
                connDB.Close();
                return columns;
            }
            SqlCommand cmd = new SqlCommand(sql, connDB);
            columns = cmd.ExecuteNonQuery();
            connDB.Close();
            cmd.Dispose();
            return columns;
        }
        /// <summary>
        /// INSERT、UPDATE、DELETE資料庫，並回傳是否成功
        /// </summary>
        /// <param name="sql">SQL語法</param>
        /// <returns></returns>
        public bool executeSql_bool(string sql)
        {
            bool report = true;
            try
            {
                connDB.Open();
            }
            catch
            {
                connDB.Close();
                report = false;
                return report;
            }
            SqlCommand cmd = new SqlCommand(sql, connDB);
            if (cmd.ExecuteNonQuery() > 0)
            {
                report = true;
            }
            else
            {
                report = false;
            }
            return report;
        }
    }
}