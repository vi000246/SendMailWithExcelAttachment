using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingDetailsReport
{
    public class Model
    {
        /// <summary>
        /// 撈出的Excel資料
        /// </summary>
        public class BillingColumn {
            public string OrderIdGN { get; set; }
            public string OrderIdOther { get; set; }
            public string GNId { get; set; }
            public string OrderDate { get; set; }
            public string OtherDate { get; set; }
            public string AddGPDate { get; set; }
            public string PayWay2 { get; set; }
            public string PayWayStr { get; set; }
            public string CardId { get; set; }
            public string Cash { get; set; }

            public string TestFlag { get; set; }

            public string UserCancelFg { get; set; }
            public string UserCancelDate { get; set; }
        }
    }
}
