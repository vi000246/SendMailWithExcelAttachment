using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using MySql.Data.MySqlClient;

namespace BillingDetailsReport
{
    /// <summary>
    /// 從資料庫撈取資料
    /// </summary>
    public class DB
    {
        //撈取儲值資料的SQL
        private static readonly string SQLQuery = @"select TOP 10 L.OrderIdGN , L.OrderIdOther , L.GNId , L.OrderDate, L.OtherDate, L.AddGPDate
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
        /// <summary>
        /// 取得儲值的資料
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public List<Model.BillingColumn> GetBillingDataByGame(string gameId) {

            DateTime configDate = new Utility().GetConfigYearMonth();

            string connectionString = ConfigurationManager.AppSettings["BillingConnectionString"];
            List<Model.BillingColumn> result = new List<Model.BillingColumn>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                result = conn.Query<Model.BillingColumn>(string.Format(SQLQuery,configDate.Year,configDate.Month,gameId)).ToList();
            }
            return result;
        }

        /// <summary>
        /// 依據GameId取得遊戲名稱
        /// </summary>
        /// <param name="GameId"></param>
        /// <returns></returns>
        public string GetGameNameById(string GameId) {
            string connectionString = ConfigurationManager.AppSettings["GameConnectionString"];
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    var result = conn.Query<string>(string.Format("SELECT subject FROM gnjoy.game where gametype='{0}'",GameId)).FirstOrDefault();
                }
            }
            catch (Exception ex) {
                throw ex;
            }
            return null;
        }

    }
}
