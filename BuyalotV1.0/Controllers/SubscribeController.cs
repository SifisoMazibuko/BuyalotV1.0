using BuyalotV1._0.DbConnection;
using BuyalotV1._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace BuyalotV1._0.Controllers
{
    public class SubscribeController : Controller
    {
        private DataContext Context { get; set; }
        private bool _DisposeContext = false;


        public SubscribeController()
        {
            Context = new DataContext();
            _DisposeContext = true;
        }


        protected override void Dispose(bool disposing)
        {
            if (_DisposeContext)
                Context.Dispose();

            base.Dispose(disposing);

        }
        // GET: Subscribe
        public ActionResult Subscribe()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Subscribe([Bind(Include = "subscribeID,firstName,email")] SubscribeModel subscribe)
        {
            if (ModelState.IsValid)
            {
                Context.SubscribeModelSet.Add(subscribe);
                Context.SaveChanges();

                MailMessage message = new MailMessage();
                message.From = new System.Net.Mail.MailAddress(subscribe.email);
                message.To.Add(new System.Net.Mail.MailAddress("mazibujo19@gmail.com"));
                message.Subject = "NEW BUYALOT SUBSCRIBER!!";
                message.Body = string.Format("Hi ,<br /><br /> {0} Just Subscribed to Get Promo Deals <br /> {1} email is: {2} .<br /><br /> ", subscribe.firstName, subscribe.firstName, subscribe.email);
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

                return RedirectToAction("Success", "Subscribe");
            }

            return View(subscribe);
        }
        public ActionResult Success()
        {
            return View();
        }
    }
}