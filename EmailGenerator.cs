using NLog;
using System;
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
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public void SendMail() {
            try
            {
                using (SmtpClient mySmtp = new SmtpClient())
                {
                    mySmtp.SendCompleted += new SendCompletedEventHandler(SendCompleted);
                    string Account = "gnjoytw";
                    string password = "ixjnrsi@366";
                    string host = "mail.gravity.co.kr";
                    int port = 25;
                    //設定smtp帳密
                    mySmtp.Credentials = new System.Net.NetworkCredential(Account, password);
                    mySmtp.Port = port;
                    mySmtp.Host = host; //SMTP主機名稱
                    mySmtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    mySmtp.UseDefaultCredentials = false;
                    mySmtp.EnableSsl = true;    //開啟SSL驗證
                                                //信件內容
                    string pcontect = ConfigurationManager.AppSettings["EmailContent"];
                    //設定mail內容
                    MailMessage msgMail = new MailMessage();

                    //寄件者
                    msgMail.From = new MailAddress("gnjoytw@gravity.co.kr", "GNJOY遊戲平台");
                    //收件者
                    string recevier = ConfigurationManager.AppSettings["SendAddress"];

                    logger.Info("Email recevier: " + recevier);

                    if (string.IsNullOrEmpty(recevier))
                        throw new ArgumentException("The parameter \"Recevier\" in config file cannot be empty");
                    string[] receviers = recevier.Split(',');
                    foreach (var mail in receviers)
                    {
                        msgMail.To.Add(mail);
                    }

                    //主旨
                    string title = ConfigurationManager.AppSettings["EmailTitle"];
                    DateTime configDate = new Utility().GetConfigYearMonth();
                    title = string.Format(title, configDate.ToString("yyyyMM"));
                    logger.Info("Email title: " + title);


                    msgMail.Subject = title;
                    //信件內容(含HTML時)
                    AlternateView alt = AlternateView.CreateAlternateViewFromString(pcontect, null, "text/html");
                    msgMail.AlternateViews.Add(alt);

                    var FileName = new Utility().GetExcelFileName();
                    logger.Info("Attached Excel fileName: " + FileName);
                    //夾帶Excel
                    var FileStream = new ExcelGenerater().GenerateExcel();

                    Attachment attachment = new Attachment(FileStream, FileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    msgMail.Attachments.Add(attachment);
                    //寄mail
                    mySmtp.Send(msgMail);
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Failed to send Email: " + ex.Message);
                logger.Error("Failed to send Email: " + ex.Message);
            }

        }

        public void SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e) {
            logger.Info("The Email was sent successfully");
        }


    }
}
