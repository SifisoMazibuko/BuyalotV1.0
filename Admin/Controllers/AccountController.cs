using Admin.DbConnection;
using Admin.Models;
using Admin.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Admin.Controllers
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

            public ActionResult Login()
            {
            return View();
            }
            [HttpPost]
            [AllowAnonymous]
            public ActionResult Login(AdminModel model)
            {

                var errors = ModelState
                        .Where(x => x.Value.Errors.Count > 0)
                        .Select(x => new { x.Key, x.Value.Errors })
                         .ToArray();

                if (model.isValid(model.email, model.password))
                {
                    FormsAuthentication.SetAuthCookie(model.email, false);
                    var dataItem = (from c in Context.AdminModelSet
                                    where c.email == model.email
                                    select c).ToList();
                    foreach (var cus in dataItem)
                    {
                        Session["userID"] = cus.adminID;
                        Session["adminName"] = cus.adminName;

                    }


                    return RedirectToAction("Index", "Home");
                }
                else
                    ViewBag.err = "Incorrect Email/Password!Try again!";
                        //return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Incorrect details");
                    return RedirectToAction("Login", "Account");
                }


            public ActionResult Register()
            {
                 return View();
            }

            [HttpPost]
            [AllowAnonymous]
            public ActionResult Register(AdminModel model)
            {

                var errors = ModelState
                  .Where(x => x.Value.Errors.Count > 0)
                  .Select(x => new { x.Key, x.Value.Errors })
                   .ToArray();

                if (ModelState.IsValid)
                {

                    AdminModel admin = new AdminModel();
                    admin.adminName = model.adminName;
                    admin.email = model.email;
                    admin.password = model.password;
                    admin.confirmPassword = model.confirmPassword;
                
                    Context.AdminModelSet.Add(admin);
                    Context.SaveChanges();

                    return RedirectToAction("Login", "Account");
                }

                return View(model);
            }

            public ActionResult Logout()
            {
                var response = new HttpStatusCodeResult(HttpStatusCode.Created);
                FormsAuthentication.SignOut();
                             
                Session["adminName"] = null;
                Session.Abandon();
                return RedirectToAction("Login", "Account");
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
            public ActionResult ForgotPassword(AdminModel model)
            {

                string tmpPass = Membership.GeneratePassword(10, 4);
                var getPass = (from p in Context.AdminModelSet
                               where p.email == model.email
                               select p).ToList();

                string tempPassword = "";
                foreach (var p in getPass)
                {
                    tempPassword = Cipher.Decrypt(p.password);
                }
                MailMessage message = new MailMessage();
                message.From = new System.Net.Mail.MailAddress("mazibujo19@gmail.com");
                message.To.Add(new System.Net.Mail.MailAddress(model.email));
                message.Subject = "Password Recovery";
                message.Body = string.Format("Hi {0} ,<br /><br />Your password is: {0} .<br /><br />Thank You. <br /> Regards, <br /> Buyalot DevTeam", model.adminName, model.password);
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

                return View("Success");
            }
        }
 }
