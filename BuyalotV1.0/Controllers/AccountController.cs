using BuyalotV1._0.DbConnection;
using BuyalotV1._0.Models;
using BuyalotV1._0.Provider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BuyalotV1._0.Controllers
{
    public class AccountController : Controller
    {
        private DataContext Context { get; set; }
        private bool _DisposeContext = false;


        public AccountController()
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
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

         public ActionResult Login()
        {
            return View();
        }

         [HttpPost]
         [AllowAnonymous]
         public ActionResult Login(CustomerModel model)
         {
           
            if(model.isValid(model.email, Cipher.Encrypt(model.password)))
            {
                //var dataItem = db.CustomerModelSet.Where(x => x.email == model.email && x.password == model.password).ToList();
                var dataItem = (from c in Context.CustomerModelSet
                                where c.email == model.email
                                select c).ToList();
                foreach (var cus in dataItem)
                {
                    Session["userID"] = cus.customerID;
                    Session["cusName"] = cus.lastName;

                }
                FormsAuthentication.SetAuthCookie(model.email, false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.errorLogin = "Invalid userName/Password";
                return RedirectToAction("Login", "Account");
            }
            
        }
        public ActionResult Logout()
        {
            var response = new HttpStatusCodeResult(HttpStatusCode.Created);
            FormsAuthentication.SignOut();

            Session["cusName"] = null;
            Session.Abandon();

            return RedirectToAction("Login", "Account");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(CustomerAddressModel cus)
        {
            
            var errors = ModelState
                   .Where(x => x.Value.Errors.Count > 0)
                   .Select(x => new { x.Key, x.Value.Errors })
                    .ToArray();

            if (ModelState.IsValid)
            {
                //CustomerModel cusModel = new CustomerModel();
                //AddressModel addModel = new AddressModel();

                string password = Cipher.Encrypt(cus.password);
                string confirmPassword = Cipher.Encrypt(cus.confirmPassword);


                CustomerModel customerModel = new CustomerModel();
                customerModel.firstName = cus.firstName;
                customerModel.lastName = cus.lastName;
                customerModel.phone = cus.phone;
                customerModel.email = cus.email;
                customerModel.password = password;
                customerModel.confirmPassword = confirmPassword;
                customerModel.state = "Active";

                AddressModel addressModel = new AddressModel();
                addressModel.address = cus.address;
                addressModel.city = cus.city;
                addressModel.postalCode = cus.postalCode;

                Context.CustomerModelSet.Add(customerModel);
                Context.AddressModelSet.Add(addressModel);
                Context.SaveChanges();


                MailMessage message = new MailMessage();
                message.From = new System.Net.Mail.MailAddress("mazibujo19@gmail.com");
                message.To.Add(new System.Net.Mail.MailAddress(cus.email));
                message.Subject = "WELCOME TO BUYALOT!!";
                message.Body = string.Format("Welcome {0} ,<br /><br />Enjoy your Shopping :) <br />Your password is: {1} .<br /><br />Thank You. <br /> Regards, <br /> Buyalot DevTeam", cus.firstName, cus.password);
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

                return RedirectToAction("Login", "Account");
            }

            return View(cus);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Success()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ForgotPassword(CustomerModel model)
        {
            string tmpPass = Membership.GeneratePassword(10, 4);
            var getPass = (from p in Context.CustomerModelSet
                           where p.email == model.email
                           select p).ToList();
            string tempPassword = "";
            foreach (var p in getPass)
            {
                tempPassword = Cipher.Decrypt(p.password);
            }

            var pd = (from e in Context.CustomerModelSet
                      where e.email == model.email
                      select e).ToList();
            string pass = "";
            string name = "";
            foreach (var item in pd)
            {
                pass = Cipher.Decrypt(item.password);
                name = item.firstName;
            }
            MailMessage message = new MailMessage();
            message.From = new System.Net.Mail.MailAddress("mazibujo19@gmail.com");
            message.To.Add(new System.Net.Mail.MailAddress(model.email));
            message.Subject = "Password Recovery";
            message.Body = string.Format("Hi "+ name + ",<br /><br />Your password is: " + pass + "<br /><br />Thank You. <br /> Regards, <br /> Buyalot DevTeam  <img src=cid:buyalot.PNG/>");

            message.IsBodyHtml = true;           

            //add our attachment
            Attachment imgAtt = new Attachment(Server.MapPath(@"/Images/buyalot.PNG"));
            //give it a content id that corresponds to the src we added in the body img tag
            imgAtt.ContentId = "buyalot.PNG";
            //add the attachment to the email
            message.Attachments.Add(imgAtt);
            
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            NetworkCredential NetworkCred = new NetworkCredential();
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = NetworkCred;
            smtp.Port = 587;

            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Send(message);

            return View("Success");
        }


    }
}