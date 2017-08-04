using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingDetailsReport
{
    public class Model
    {

        public class BillingSheet {
            public BillingSheet(string sheetName, List<BillingColumn> sheetData) {
                this.SheetName = SheetName;
                this.SheetData = SheetData;
            }
            public string SheetName { get; set; }
            public List<BillingColumn> SheetData { get; set; }
        }

        /// <summary>
        /// 從DB撈出的儲值資料
        /// </summary>
        public class BillingColumn {
 
            [ColumnName("訂單編號")]
            public string OrderIdGN { get; set; }

            [ColumnName("廠商訂單編號")]
            public string OrderIdOther { get; set; }

            [ColumnName("平台帳號")]
            public string GNId { get; set; }

            [ColumnName("訂單時間")]
            public string OrderDate { get; set; }

            [ColumnName("金流交易日")]
            public string OtherDate { get; set; }

            [ColumnName("給幣時間")]
            public string AddGPDate { get; set; }

            [ColumnName("付費方式(代碼)")]
            public string PayWay2 { get; set; }

            [ColumnName("付費方式(中文)")]
            public string PayWayStr { get; set; }

            [ColumnName("點卡卡號")]
            public string CardId { get; set; }
            [ColumnName("Cash")]
            public string Cash { get; set; }

            [ColumnName("測試註記")]

            public string TestFlag { get; set; }

            [ColumnName("取消註記")]

            public string UserCancelFg { get; set; }

            [ColumnName("取消日期")]
            public string UserCancelDate { get; set; }
        }
    }
    //自訂欄位名稱Attribute
    public class ColumnNameAttribute : Attribute
    {
        public string Description { get; set; }

        public ColumnNameAttribute(string Description)
        {
            this.Description = Description;
        }

        public override string ToString()
        {
            return this.Description.ToString();
        }
    }
}
