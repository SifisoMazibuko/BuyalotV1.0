using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;
using System.Xml;

namespace BuyalotV1._0.Gateway
{
    /// <summary>
    /// Summary description for SidapiService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SidapiService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        public void sid_api()
        {
            var requestXml = new XmlDocument();
            //build xml request
            var httpReq = HttpWebRequest.Create("https://www.sidpayment.com/");
            httpReq.Method = "POST";
            httpReq.ContentType = "text/xml";
            //set headers
            using (var requestStream  = httpReq.GetRequestStream())
            {
                requestXml.Save(requestStream);
            }

            using (var response = (HttpWebResponse)httpReq.GetResponse())
            using(var responseStream = response.GetResponseStream())
            {
                var responseXml = new XmlDocument();
                responseXml.Load(responseStream);
            }
            
        }
    }
}
