using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace BillingDetailsReport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.Write("Start!");
                new EmailGenerator().SendMail();
                Console.Write("The process has been complete.See log");
            }
            catch (Exception ex) {
                Console.Write(ex.Message);
            }
            Console.ReadLine();
        }
    }
}
