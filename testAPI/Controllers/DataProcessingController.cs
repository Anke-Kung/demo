using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace testAPI.Controllers
{
    public class DataProcessingController : ApiController
    {
        public SqlConnection connectionDB = new SqlConnection(ConfigurationManager.ConnectionStrings["TESTAPI"].ConnectionString);//取得web.config連線物件
        //public SqlConnection connectionDB = new SqlConnection("Data Source=LAPTOP-RTF7TICB\\WEBAPI;User ID=sa;Password=test0011;");
        //取得資料庫
        public DataTable getDB(string sql)
        {
            if (string.IsNullOrEmpty(sql))
                return null;
            try
            {
                connectionDB.Open();
            }
            catch (Exception ex)
            {
                connectionDB.Close();
                throw ex;
            }
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand(sql, connectionDB);
            try
            {
                da.SelectCommand = cmd;
                da.Fill(ds, "Table");
            }
            catch (SqlException ex)
            {
                connectionDB.Close();
                cmd.Dispose();
                da.Dispose();
                throw ex;
            }
            DataTable dt = ds.Tables["Table"];
            connectionDB.Close();
            cmd.Dispose();
            da.Dispose();
            return dt;
        }
        #region 功能
        /// <summary>
        /// 連接資料庫測試
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("test")]
        public DataTable test()
        {
            return getDB("SELECT * " +
                         "  FROM [Sales].[dbo].[test]");
        }

        [HttpPost]
        [ActionName("getPDF")]
        public void getPDF()
        {
            Models.DataProcessing data = new Models.DataProcessing();
            data.getPDF();
        }




        #endregion
    }
}
