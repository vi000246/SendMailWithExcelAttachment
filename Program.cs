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
            var list = new DB().GetBillingDataByGame("ML");
            Console.ReadLine();
        }
    }
}
