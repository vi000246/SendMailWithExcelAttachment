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
                new EmailGenerator().SendMail();
                Console.Write("執行完畢");
            }
            catch (Exception ex) {
                Console.Write(ex.Message);
            }
            Console.ReadLine();
        }
    }
}
