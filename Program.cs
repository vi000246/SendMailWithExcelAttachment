using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using NLog;

namespace BillingDetailsReport
{
    class Program
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            
            try
            {
                Console.WriteLine("Start!");
                logger.Info(">>>>>>>Process Start!!<<<<<<<");
                new EmailGenerator().SendMail();
                Console.WriteLine("The process has been complete.See log");
                logger.Info(">>>>>>>Process complete<<<<<<<");
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                logger.Error("Error occur: "+ex.Message);
            }
        }
    }
}
