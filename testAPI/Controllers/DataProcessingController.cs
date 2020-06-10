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
        /// 個股交易歷史資料寫入資料庫(單一個股)
        /// </summary>
        /// <param name="stock_Symbol"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("insertData")]
        public DataProcessing.reportList insertData_C(int stock_Symbol)
        {
            return new DataProcessing().insertData_M(stock_Symbol);
        }

        /// <summary>
        /// 個股交易歷史資料寫入資料庫(處理多個資料)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("insertData2")]
        public DataProcessing.reportList insertData2_C()
        {
            return new DataProcessing().insertData2_M();
        }

        /// <summary>
        /// 取得個股交易資料
        /// </summary>
        /// <param name="stock_Symbol">股票代號</param>
        /// <param name="year">交易年份(民國)</param>
        /// <param name="month">交易月份</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("getData")]
        public DataTable getData_C(int stock_Symbol, string year, string month)
        {
            return new DataProcessing().getData_M(stock_Symbol, year, month);
        }

        #endregion
    }
}
