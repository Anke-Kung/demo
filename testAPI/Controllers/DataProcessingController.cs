using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using testAPI.Models;

namespace testAPI.Controllers
{
    public class DataProcessingController : ApiController
    {
        #region 功能
        /// <summary>
        /// 取得PDF資料(僅練習未實作)
        /// </summary>
        [HttpPost]
        [ActionName("getPDF")]
        public void getPDF()
        {
            Models.DataProcessing data = new Models.DataProcessing();
            data.getPDF();
        }

        /// <summary>
        /// 個股交易歷史資料寫入資料庫
        /// </summary>
        /// <param name="stock_Symbol"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("insertData")]
        public DataProcessing.reportList insertData_C(int stock_Symbol)
        {
            return new DataProcessing().insertData_M(stock_Symbol);
        }
        #endregion
    }
}
