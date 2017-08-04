using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingDetailsReport
{
    /// <summary>
    /// 負責Excel的產生邏輯
    /// </summary>
    public class ExcelGenerater
    {
        private DB db = new DB();
        private Utility utility = new Utility();
        /// <summary>
        /// 產生Excel
        /// </summary>
        /// <returns></returns>
        public MemoryStream GenerateExcel() {
            //回傳outputStream讓Email夾帶
            MemoryStream outputStream = new MemoryStream();

            //取得要撈取的遊戲編號
            string configGameID = ConfigurationManager.AppSettings["GameID"];
            if (string.IsNullOrEmpty(configGameID))
                throw new ArgumentException("config的GameID不得為空");
            string[] GameIDs = configGameID.Split(',');
            

            using (ExcelPackage pck = new ExcelPackage(outputStream))
            {
                foreach (var ID in GameIDs)
                {
                    string SheetName = db.GetGameNameById(ID);
                    //如果抓不到對應的遊戲中文名稱 就將GameName設為GameId
                    if (string.IsNullOrEmpty(SheetName))
                        SheetName = ID;
                    //取得儲值資料
                    var data = db.GetBillingDataByGame(ID);

                    //加入一個Sheet
                    pck.Workbook.Worksheets.Add(SheetName);
                    //取得剛剛加入的Sheet
                    ExcelWorksheet sheet1 = pck.Workbook.Worksheets[SheetName];//取得Sheet1 

                    //Format the header
                    using (ExcelRange rng = sheet1.Cells["A1:BZ1"])
                    {
                        rng.Style.Font.Bold = true;
                        rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
                        rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));  //Set color to dark blue
                        rng.Style.Font.Color.SetColor(Color.White);
                    }

                    //建立表頭
                    List<string> headers = utility.GetExcelHeader();
                    for (int i = 0; i < headers.Count(); i++)
                    {
                        sheet1.Cells[1, i + 1].Value = headers[i];
                    }
                    //載入資料
                    if (data.Count() > 0)
                    {
                        sheet1.Cells["A2"].LoadFromCollection(data);
                    }
                    //自動修改欄寬 
                    sheet1.Cells.AutoFitColumns();
                }



                string ExcelFileName = utility.GetExcelFileName();

                //如果有開啟存檔
                if (ConfigurationManager.AppSettings["IsGenerateExcelFile"] == "Y")
                {
                    string SaveFolder = "ExcelBackup";
                    string SavePath = SaveFolder + "/" + ExcelFileName;
                    //判斷存檔資料夾是否存在
                    bool exists = Directory.Exists(SaveFolder);
                    if (!exists)
                        Directory.CreateDirectory(SaveFolder);

                    //檔案已存在 
                    if (File.Exists(SavePath))
                    {
                        //刪除檔案
                        File.Delete(SavePath);
                    }

                    //建立檔案串流
                    FileStream OutputFileStream = new FileStream(SavePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                    //把剛剛的Excel物件真實存進檔案裡
                    pck.SaveAs(OutputFileStream);
                    //關閉串流
                    OutputFileStream.Close();
                }

            }
            outputStream.Position = 0;
            return outputStream;
        }
    }
}
