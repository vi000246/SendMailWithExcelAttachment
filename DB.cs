using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;

namespace BillingDetailsReport
{
    /// <summary>
    /// 從資料庫撈取資料
    /// </summary>
    public class DB
    {
        //撈取儲值資料的SQL
        private static readonly string SQLQuery = @"select L.OrderIdGN , L.OrderIdOther , L.GNId , L.OrderDate, L.OtherDate, L.AddGPDate
                                                    , L.PayWay2
                                                    , Case L.PayWay2
                                                     when 'card_2' then '點卡-一般通路'
                                                     when 'card_4' then '點卡-超商'
                                                     when 'card_8' then '點卡-外海新價'
                                                     when 'card_18' then '點卡-全家'
                                                     when 'card_19' then '點卡-除全家外的超商'
                                                     when 'card_36' then '點卡-外海新價ver17.06'
                                                     when 'APP-GOOGLE_TW' then 'Google Play'
                                                     when 'APP-IOS_TW' then 'App Store'
                                                     else
                                                      D.ProdName
                                                     end 
                                                     as PayWayStr
 
                                                    , L.CardId  , L.Cash, isnull(L.TestFlag, '')as  TestFlag, isnull(L.UserCancelFg, '') as  UserCancelFg
                                                    , L.UserCancelDate 
                                                    from BillingLog L
                                                    left join [dbo].[GNProductDetail] D on D.ProdId = L.ProdId and D.GameId = L.GameId 
                                                    where 
                                                          DATEPART(year, L.OtherDate) = {0}
                                                          and DATEPART(month, L.OtherDate) = {1}
                                                          and L.BStatus = '1' and L.GameId ='{2}' 
                                                    order by L.TestFlag, L.UserCancelFg, L.PayWay2, L.OtherDate";

        public List<Model.BillingColumn> GetBillingDataByGame(string gameId) {
            //取得config檔 指定撈取的月份

            int year = 0;
            if (!int.TryParse(ConfigurationManager.AppSettings["Year"], out year))
                throw new ArgumentException("config中的year參數必須為數字");
            int month = 0;
            if (!int.TryParse(ConfigurationManager.AppSettings["Month"], out month))
                throw new ArgumentException("config中的month參數必須為數字");

            //如果config檔沒指定年月 就預設撈取上個月的資料
            if (year==0 && month==0) {
                year = DateTime.Now.Year;
                month = (DateTime.Now.Month - 1);
            }

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            List<Model.BillingColumn> result = new List<Model.BillingColumn>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                result = conn.Query<Model.BillingColumn>(string.Format(SQLQuery,year,month,gameId)).ToList();
            }
            return result;
        }

    }
}
