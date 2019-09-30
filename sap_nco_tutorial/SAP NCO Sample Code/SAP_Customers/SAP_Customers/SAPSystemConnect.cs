using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAP.Middleware.Connector;

namespace SAP_Customers
{
    class SAPSystemConnect:IDestinationConfiguration
    {

        public bool ChangeEventsSupported()
        {
            throw new NotImplementedException();
        }

        public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;

        public RfcConfigParameters GetParameters(string destinationName)
        {
            RfcConfigParameters parms = new RfcConfigParameters();
            if ("Dev".Equals(destinationName))
            {
               
                parms.Add(RfcConfigParameters.AppServerHost, "000.000.000.000");
                parms.Add(RfcConfigParameters.SystemNumber, "95");
                parms.Add(RfcConfigParameters.User, "sap_user");
                parms.Add(RfcConfigParameters.Password, "xxxxxxxxxx");
                parms.Add(RfcConfigParameters.Client, "10");
                parms.Add(RfcConfigParameters.Language, "EN");
                parms.Add(RfcConfigParameters.PoolSize, "5");
                parms.Add(RfcConfigParameters.MaxPoolSize, "10");
                parms.Add(RfcConfigParameters.IdleTimeout, "600");
                
            }
            return parms;
        }
    }
}
