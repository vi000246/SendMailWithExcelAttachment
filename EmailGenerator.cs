﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BillingDetailsReport
{
    /// <summary>
    /// 負責產生Email
    /// </summary>
    public class EmailGenerator
    {
        public void SendMail() {
            using (SmtpClient mySmtp = new SmtpClient()) {
                //設定smtp帳密
                mySmtp.Credentials = new System.Net.NetworkCredential("vi000246", "uish2014");
                mySmtp.Port = 587;
                mySmtp.Host = "smtp.gmail.com"; //SMTP主機名稱
                mySmtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //mySmtp.UseDefaultCredentials = false;
                mySmtp.EnableSsl = true;    //開啟SSL驗證
                //信件內容
                string pcontect = ConfigurationManager.AppSettings["EmailContent"];
                //設定mail內容
                MailMessage msgMail = new MailMessage();
                //寄件者
                msgMail.From = new MailAddress("gnjoytw@gravity.co.kr", "GNJOY遊戲平台");
                //收件者
                string recevier = ConfigurationManager.AppSettings["SendAddress"];
                if (string.IsNullOrEmpty(recevier))
                    throw new ArgumentException("config中的收件者為空");
                string[] receviers = recevier.Split(',');
                foreach (var mail in receviers)
                {
                    msgMail.To.Add(mail);
                }

                //主旨
                string title = ConfigurationManager.AppSettings["EmailTitle"];
                DateTime configDate = new Utility().GetConfigYearMonth();
                msgMail.Subject = string.Format(title, configDate.ToString("yyyyMM"));
                //信件內容(含HTML時)
                AlternateView alt = AlternateView.CreateAlternateViewFromString(pcontect, null, "text/html");
                msgMail.AlternateViews.Add(alt);

                //夾帶Excel
                var FileStream = new ExcelGenerater().GenerateExcel();
                var FileName = new Utility().GetExcelFileName();
                Attachment attachment = new Attachment(FileStream, FileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                msgMail.Attachments.Add(attachment);
                //寄mail
                mySmtp.Send(msgMail);
            }

        }


    }
}
