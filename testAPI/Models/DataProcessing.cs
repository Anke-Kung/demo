using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;

namespace testAPI.Models
{
    public class DataProcessing
    {
        public void getPDF()
        {
            FileInfo pdffile = new FileInfo(@"C:\Users\yuwei\Desktop\02164042652.033.pdf");
            if (pdffile.Exists)
            {
                FileInfo file = new FileInfo(@"D:\mis2000lab_example.txt");
                pdf2txt(pdffile, file);
            }
        }

        public void pdf2txt(FileInfo file, FileInfo txtfile)
        {
            PDDocument doc = PDDocument.load(file.FullName);
            PDFTextStripper pdfStripper = new PDFTextStripper();
            string text = pdfStripper.getText(doc);
            StreamWriter swPdfChange = new StreamWriter(txtfile.FullName, false, Encoding.GetEncoding("utf-8"));
            swPdfChange.Write(text);
            swPdfChange.Close();
        }
    }
}