using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAP.Middleware.Connector;

namespace SAP_Customers
{
    class Customers
    {
        public string CustomerNo;
        public string CustomerName;
        public string Address;
        public string City;
        public string StateProvince;
        public string CountryCode;
        public string PostalCode;
        public string Region;
        public string Industry;
        public string District;
        public string SalesOrg;
        public string DistributionChannel;
        public string Division;





        public void GetCustomerDetails(RfcDestination destination)
        {


            try
            {
                RfcRepository repo = destination.Repository;
                IRfcFunction customerBapi = repo.CreateFunction("BAPI_CUSTOMER_GETLIST");


                customerBapi.Invoke(destination);

                IRfcTable idRange = customerBapi.GetTable("IdRange");
                idRange.SetValue("SIGN", "I");
                idRange.SetValue("OPTION", "BT");
                idRange.SetValue("LOW", "");//customer number range - smallest value
                idRange.SetValue("HIGH", "999999");//customer number range - largest value


                //add selection range to customerBapi function to search for all customers
                customerBapi.SetValue("idrange", idRange);


                IRfcTable addressData = customerBapi.GetTable("AddressData");
                customerBapi.Invoke(destination);//execute query
               
                for (int cuIndex = 0; cuIndex < addressData.RowCount; cuIndex++)
                {
                   
                    addressData.CurrentIndex = cuIndex;
                    IRfcFunction customerHierachy = repo.CreateFunction("BAPI_CUSTOMER_GETSALESAREAS");
                    IRfcFunction customerDetail1 = repo.CreateFunction("BAPI_CUSTOMER_GETDETAIL1");
                    IRfcFunction customerDetail2 = repo.CreateFunction("BAPI_CUSTOMER_GETDETAIL2");
                    Customers cust = new Customers(); ;
                  
                      


                        cust.CustomerNo = addressData.GetString("Customer");
                        cust.CustomerName = addressData.GetString("Name");
                        cust.Address = addressData.GetString("Street");
                        cust.City = addressData.GetString("City");
                        cust.StateProvince = addressData.GetString("Region");
                        cust.CountryCode = addressData.GetString("CountryISO");
                        cust.PostalCode = addressData.GetString("Postl_Cod1");
                    
                        customerDetail2.SetValue("CustomerNo", cust.CustomerNo);
                        customerDetail2.Invoke(destination);
                        IRfcStructure generalDetail = customerDetail2.GetStructure("CustomerGeneralDetail");

                        cust.Region = generalDetail.GetString("Reg_Market");
                        cust.Industry = generalDetail.GetString("Industry");
                      

                        customerDetail1.Invoke(destination);
                        IRfcStructure detail1 = customerDetail1.GetStructure("PE_CompanyData");
                        cust.District = detail1.GetString("District");
                       

                        customerHierachy.Invoke(destination);
                        customerHierachy.SetValue("CustomerNo", cust.CustomerNo);
                        customerHierachy.Invoke(destination);

                        IRfcTable otherDetail = customerHierachy.GetTable("SalesAreas");

                        if (otherDetail.RowCount > 0)
                        {
                            cust.SalesOrg = otherDetail.GetString("SalesOrg");
                            cust.DistributionChannel = otherDetail.GetString("DistrChn");
                            cust.Division = otherDetail.GetString("Division");
                        }

                    customerHierachy = null;
                    customerDetail1 = null;
                    customerDetail2 = null;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();



                }


            }
            catch (RfcCommunicationException e)
            {

            }
            catch (RfcLogonException e)
            {
                // user could not logon...
            }
            catch (RfcAbapRuntimeException e)
            {
                // serious problem on ABAP system side...
            }
            catch (RfcAbapBaseException e)
            {
                // The function module returned an ABAP exception, an ABAP message
                // or an ABAP class-based exception...
            }

        }
    }
}
