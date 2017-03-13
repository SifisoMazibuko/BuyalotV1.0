//using BuyalotV1._0.Gateway;
//using BuyalotV1._0.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Web;
//using System.Xml;

//namespace Buyalot.Gateway
//{
//    public class OrderQueryWebService
//    {
//        /// <summary>
//        /// Static Reference to the SID Web Services API
//        /// </summary>
//        public static SidapiService _SidApi
//        {
//            get;
//            set;
//        }

//        /// <summary>
//        /// Static Function used in a Console Application
//        /// </summary>
//        /// <param name="args"></param>
//        static void Main(string[] args)
//        {
//            _SidApi = new SidapiService();

//            List<Transaction> Transactions = new List<Transaction>();
//            Transactions.Add(new Transaction("ZA", "ZAR", "106.00", "8"));
//            Transactions.Add(new Transaction("ZA", "ZAR", "106.00", "7"));

//            string Error = string.Empty;
//            List<Transaction> Reply = CreateOrderQueryRequestString("PHONE", "sid@phonewarehouse.co.za", "112233", Transactions, out Error);

//            if (Reply != null)
//            {
//                foreach (Transaction trans in Reply)
//                {
//                    Console.WriteLine(string.Format("Transaction: [{0}] was [{1}]", trans.Reference, trans.Status));
//                }
//            }
//            else
//            {
//                Console.WriteLine(Error);
//            }

//            Console.ReadKey();
//        }

//        /// <summary>
//        /// Pass through the variables required and receive a list of Transaction objects showing transaction status
//        /// </summary>
//        /// <param name="MerchantCode">Your Merchant Code</param>
//        /// <param name="MerchantUsername">Your Merchant Username</param>
//        /// <param name="MerchantPassword">Your Merchant Password</param>
//        /// <param name="TransactionsToConfirm">List of Transaction Object you wish to check</param>
//        /// <param name="ErrorMessage">Will have a value if an error has occurred, or else will be an empty string</param>
//        /// <returns>A List of Transactions, or NULL if there was an error</returns>
//        private static List<Transaction> CreateOrderQueryRequestString(string MerchantCode, string MerchantUsername,
//            string MerchantPassword, List<Transaction> TransactionsToConfirm, out string ErrorMessage)
//        {
//            StringBuilder Builder = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");

//            Builder.Append("<sid_order_query_request>");
//            Builder.Append("<merchant>");
//            Builder.Append(string.Format("<code>{0}</code>", MerchantCode));
//            Builder.Append(string.Format("<uname>{0}</uname>", MerchantUsername));
//            Builder.Append(string.Format("<pword>{0}</pword>", MerchantPassword));
//            Builder.Append("</merchant>");
//            Builder.Append("<orders>");

//            foreach (Transaction _transaction in TransactionsToConfirm)
//            {
//                Builder.Append("<transaction>");
//                Builder.Append(string.Format("<country>{0}</country>", _transaction.Country));
//                Builder.Append(string.Format("<currency>{0}</currency>", _transaction.Currency));
//                Builder.Append(string.Format("<amount>{0}</amount>", _transaction.Amount));
//                Builder.Append(string.Format("<reference>{0}</reference>", _transaction.Reference));
//                Builder.Append("</transaction>");
//            }

//            Builder.Append("</orders>");
//            Builder.Append("</sid_order_query_request>");

//            string ReturnString = _SidApi.sid_order_query(Builder.ToString());

//            List<Transaction> ReturnMe = Returned(ReturnString, out ErrorMessage);

//            return ReturnMe;
//        }

//        /// <summary>
//        /// Internal method to parse the XML from the WebService call in CreateOrderQueryRequestString
//        /// </summary>
//        /// <param name="XMLString">String returned from the WebService</param>
//        /// <param name="ErrorMessage">An Error Message if one has occurred</param>
//        /// <returns>List of Transaction objects</returns>
//        private static List<Transaction> Returned(string XMLString, out string ErrorMessage)
//        {
//            ErrorMessage = string.Empty;
//            XmlDocument doc = new XmlDocument();
//            XmlNode outcome = doc.SelectSingleNode(" ");
//            XmlNode Error = outcome.Attributes["errorcode"];

//            if (Error.InnerText != "0")
//            {

//                XmlNode ErrorDesc = outcome.Attributes["errordescription"];
//                XmlNode ErrorSolu = outcome.Attributes["errorsolution"];
//                ErrorMessage = string.Format("Code: {0}; Description: {1}; Solution: {2}", Error.InnerText, ErrorDesc.InnerText, ErrorSolu.InnerText);

//                return null;
//            }

//            XmlNodeList OrdersList = doc.SelectNodes("./sid_order_query_response/data/orders/transaction");

//            List<Transaction> transList = new List<Transaction>();

//            foreach (XmlNode node in OrdersList)
//            {
//                Transaction trans = new Transaction();

//                trans.Status = node.SelectSingleNode("./status").InnerText;
//                trans.Country = node.SelectSingleNode("./country/code").InnerText;
//                trans.CountryName = node.SelectSingleNode("./country/name").InnerText;

//                trans.Currency = node.SelectSingleNode("./currency/code").InnerText;
//                trans.CurrencyName = node.SelectSingleNode("./currency/name").InnerText;

//                trans.Amount = node.SelectSingleNode("./amount").InnerText;
//                trans.Reference = node.SelectSingleNode("./reference").InnerText;

//                trans.Date_Created = node.SelectSingleNode("./date_created").InnerText;
//                trans.Date_Ready = node.SelectSingleNode("./date_ready").InnerText;
//                trans.Date_Completed = node.SelectSingleNode("./date_completed").InnerText;

//                trans.TnxID = node.SelectSingleNode("./tnxid").InnerText;
//                trans.ReceiptNo = node.SelectSingleNode("./receiptno").InnerText;

//                transList.Add(trans);
//            }

//            return transList;
//        }
//    }
//}