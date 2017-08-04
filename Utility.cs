﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BillingDetailsReport
{
    public class Utility
    {
        /// <summary>
        /// //取得config檔 指定撈取的月份
        /// </summary>
        /// <returns></returns>
        public DateTime GetConfigYearMonth() {

            try
            {
                int year = 0;
                if (!int.TryParse(ConfigurationManager.AppSettings["Year"], out year))
                    throw new ArgumentException("config中的year參數必須為數字");
                int month = 0;
                if (!int.TryParse(ConfigurationManager.AppSettings["Month"], out month))
                    throw new ArgumentException("config中的month參數必須為數字");

                //如果config檔沒指定年月 就預設撈取上個月的資料
                if (year == 0 && month == 0)
                {
                    year = DateTime.Now.Year;
                    month = (DateTime.Now.Month - 1);
                }
                if (year == 0 || month == 0)
                    throw new ArgumentException("config中的year和month必須皆為0，或皆不為0");

                return new DateTime(year, month, 1);
            }
            catch (Exception ex) {
                throw ex;
            }
            
        }

        /// <summary>
        /// 依據Class的ColumnName Attribute 取得Excel檔案的Headers
        /// </summary>
        /// <returns></returns>
        public List<string> GetExcelHeader() {
            List<string> Headers = new List<string>();
            //取得class的Column Name Attribute
            var t = typeof(Model.BillingColumn);
            var Headings = t.GetProperties();
            foreach (PropertyInfo prop in Headings)
            {
                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    ColumnNameAttribute authAttr = attr as ColumnNameAttribute;
                    if (authAttr != null)
                    {
                        Headers.Add(authAttr.Description);
                    }
                }
            }
            return Headers;
        }

        /// <summary>
        /// 取得Excel檔名
        /// </summary>
        /// <returns></returns>
        public string GetExcelFileName() {
            DateTime configDate = new Utility().GetConfigYearMonth();
            return string.Format("{0}-{1}.xlsx", configDate.ToString("yyyyMM"), DateTime.Now.ToString("yyyyMMdd"));
        }
    }
}
