using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using javax.xml.crypto;
using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;

namespace testAPI.Models
{
    public class DataProcessing
    {
        #region 類別
        public class reportList
        {
            public bool report { get; set; }//資料寫入結果
            public string msg { get; set; }//出現錯誤訊息
        }
        #endregion
        #region PDF資料練習
        /// <summary>
        /// 取得PDF資料
        /// </summary>
        public void getPDF()
        {
            FileInfo pdffile = new FileInfo(@"C:\Users\yuwei\Desktop\02164042652.033.pdf");
            if (pdffile.Exists)
            {
                FileInfo file = new FileInfo(@"D:\mis2000lab_example.txt");
                pdf2txt(pdffile, file);
            }
        }
        /// <summary>
        /// 將取得PDF資料讀入文字檔
        /// </summary>
        /// <param name="file"></param>
        /// <param name="txtfile"></param>
        public void pdf2txt(FileInfo file, FileInfo txtfile)
        {
            PDDocument doc = PDDocument.load(file.FullName);
            PDFTextStripper pdfStripper = new PDFTextStripper();
            string text = pdfStripper.getText(doc);
            StreamWriter swPdfChange = new StreamWriter(txtfile.FullName, false, Encoding.GetEncoding("utf-8"));
            swPdfChange.Write(text);
            swPdfChange.Close();
        }
        #endregion
        #region 股市資料讀存
        /// <summary>
        /// 個股交易歷史資料寫入資料庫
        /// </summary>
        /// <param name="stock_Symbol"></param>
        /// <returns></returns>
        public reportList insertData_M(int stock_Symbol)
        {
            string dir = @"D:\stock\" + stock_Symbol.ToString();
            reportList result = new reportList();
            result.msg = "";
            try
            {
                foreach (string folder in Directory.GetDirectories(dir))//查詢子目錄    
                {
                    foreach(string filePath in Directory.GetFiles(folder, "*.csv"))//子目錄檔案處理
                    {
                        if((result.report = insertDailyTrading(getHistorical(filePath), stock_Symbol)) == false)//依據檔案路徑開始儲存資料
                        {
                            result.msg = "傳入資料失敗!";
                            return result;
                        }
                    }
                }
                return result;//寫入成功回傳TRUE，不回傳任何訊息
            }
            catch(Exception ex)
            {
                result.report = false;
                result.msg = ex.Message;
                return result;
            }
        }
        /// <summary>
        /// 個股交易資訊csv檔處理
        /// </summary>
        /// <param name="filePath">檔案位置</param>
        /// <returns></returns>
        public DataTable getHistorical(string filePath)
        {
            FileStream fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);//讀取並建立檔案資料流
            StreamReader sr = new StreamReader(fs, Encoding.Default);//對傳入的資料流進行字元編碼定義
            DataTable dt = new DataTable();
            string strLine = "";
            string[] aryLine = null;
            string[] tableHead = null;
            string strTitle = "";
            int fileColumn = 0;//判斷檔案行數
            int columnCount = 0;//欄位數
            bool IsFirst = true;
            while ((strLine = sr.ReadLine()) != null)
            {
                if(fileColumn == 0)
                {
                    fileColumn = 1;
                    DataColumn dc = new DataColumn("個股資訊");
                    strTitle = strLine.Split(',')[0];
                    dt.Columns.Add(dc);
                }
                else if(fileColumn == 1)
                {
                    fileColumn = 2;
                    tableHead = strLine.Split(',');
                    columnCount = tableHead.Length;
                    for (int i = 0;i< columnCount; i++)
                    {
                        tableHead[i] = tableHead[i].Replace("\"", "");//資料表內容多餘引號處理
                        DataColumn dc = new DataColumn(tableHead[i]);//定義欄位
                        dt.Columns.Add(dc);//將欄位新增至dt裡
                    }
                }
                else
                {
                    MatchCollection mcs = Regex.Matches(strLine, "(?<=^|,)(\"(?:[^\"]|\"\")*\"|[^,]*)");//資料表內容有逗號，需用正則表示式判斷
                    DataRow dr = dt.NewRow();//以dt結構建立新row
                    int i = 0;
                    foreach(Match mc in mcs)
                    {
                        if (IsFirst)//先將title字串加入
                        {
                            IsFirst = false;
                            dr[i] = strTitle;
                            i++;
                        }
                        dr[i] = mc.Value.Replace("\"", ""); ;//將aryLine多餘引號處理，並傳給dr
                        i++;
                    }
                    dt.Rows.Add(dr);//將row資料新增至dt
                    IsFirst = true;
                }
            }
            if (aryLine != null && aryLine.Length > 0)
            {
                dt.DefaultView.Sort = tableHead[1] + " " + "DESC";//對日期做排序
            }
            sr.Close();
            fs.Close();
            return dt;
        }
        /// <summary>
        /// 資料傳入資料庫
        /// </summary>
        /// <param name="dt">要傳入的資料</param>
        /// <param name="stock_Symbol">股票代號</param>
        /// <returns></returns>
        public bool insertDailyTrading(DataTable dt,int stock_Symbol)
        {
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataTable report = new DB().getDB("EXEC [Stock].[dbo].[ups_DailyTrading] '" + stock_Symbol + "'," +
                                                                                             "'" + row["個股資訊"] + "'," +
                                                                                             "'" + row["日期"] + "'," +
                                                                                             "'" + row["成交股數"] + "'," +
                                                                                             "'" + row["成交金額"] + "'," +
                                                                                             row["開盤價"] + "," +
                                                                                             row["最高價"] + "," +
                                                                                             row["最低價"] + "," +
                                                                                             row["收盤價"] + "," +
                                                                                             "'" + row["漲跌價差"] + "'," +
                                                                                             "'" + row["成交筆數"] + "'");//將資料寫入資料庫
                    if(report.Rows.Count == 0)//寫入失敗回傳FALSE
                    {
                        return false;
                    }
                }
                return true;//寫入成功回傳TRUE
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}