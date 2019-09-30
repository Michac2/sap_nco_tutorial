using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAP.Middleware.Connector;

namespace SAP_Customers
{
    class Program
    {
        static void Main(string[] args)
        {
            SAPSystemConnect sapCfg = new SAPSystemConnect();
         

            RfcDestinationManager.RegisterDestinationConfiguration(sapCfg);
            RfcDestination rfcDest=null;

            for (int i = 0; i < args.Length; i++)
            {     
                // arg[i] = Dev
                rfcDest = RfcDestinationManager.GetDestination(args[i]);
            }
          

            Customers customer = new Customers();
            customer.GetCustomerDetails(rfcDest);  
      
            System.Environment.Exit(0);

        }


    }
}