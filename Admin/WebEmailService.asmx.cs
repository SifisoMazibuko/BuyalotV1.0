using Admin.DbConnection;
using Admin.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Services;

namespace Admin
{
    /// <summary>
    /// Send Email to all the Subscribers of buyalot WebEmailService
    /// </summary>
   // [WebService(Namespace = "http://tempuri.org/")]
    [WebService(Namespace = "http://microsoft.com/webservices/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebEmailService : System.Web.Services.WebService
    {
        private System.Windows.Forms.Button button1;

        [WebMethod]
        public string Subscribe()
        {
            DataContext Context = new DataContext();
            SubscribeModel subscribeModel = new SubscribeModel();
            var sub = (from s in Context.SubscribeModelSet
                       select s).ToList();

            foreach(var e in sub)
            {
                MailMessage message = new MailMessage();
                message.From = new System.Net.Mail.MailAddress("mazibujo19@gmail.com");
                message.To.Add(new System.Net.Mail.MailAddress(e.email));
                message.Subject = "New Deals";
                message.Body = string.Format("Hi Shopper ,<br /><br /> We have a variety of new deals to choose from, click on link below <br /><br />Thank You. <br /> Regards, <br /> Buyalot DevTeam");
                message.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential();
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;

                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                client.Send(message);
            }
                     

            return "Newsletter sent to every one";
        }

        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Send Email To Subscribers";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
        }

        private void button1_Click(object sender, EventArgs e)
        {
           string bb =  Subscribe();
        }
    }
}
