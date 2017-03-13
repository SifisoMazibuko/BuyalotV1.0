using BuyalotV1._0.DbConnection;
using BuyalotV1._0.Models;
using BuyalotV1._0.Provider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuyalotV1._0.Controllers
{
    public class ProfileController : Controller
    {
        private DataContext Context { get; set; }
        private bool _DisposeContext = false;


        public ProfileController()
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
        // GET: Profile
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult EditProfile(int? id)
        {
            var cus = new CustomerInfoModel();

            var customerModel = Context.CustomerModelSet.Where(c => c.customerID == id).ToList();
            var addressModel = Context.AddressModelSet.Where(a => a.customerID == id).ToList();

            foreach (var cm in customerModel)
            {
                cus.customerID = cm.customerID;
                cus.firstName = cm.firstName;
                cus.lastName = cm.lastName;
                cus.phone = cm.phone;
                cus.email = cm.email;
                cus.password = Cipher.Decrypt(cm.password);
                cus.confirmPassword = Cipher.Decrypt(cm.confirmPassword);

            }

            foreach (var am in addressModel)
            {
                cus.address = am.address;
                cus.city = am.city;
                cus.postalCode = am.postalCode;
            }
            return View(cus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile([Bind(Include = "customerID,firstName,lastName,phone,email,password,confirmPassword,state")] CustomerModel cus)
        {
            if (ModelState.IsValid)
            {
                Context.Entry(cus).State = EntityState.Modified;
                Cipher.Decrypt(cus.password);
                Cipher.Decrypt(cus.confirmPassword);
                Context.SaveChanges();
                return View(cus);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult _EditProfile(int? id)
        {
            var cus = new CustomerInfoModel();

            var customerModel = Context.CustomerModelSet.Where(c => c.customerID == id).ToList();

            var addressModel = (from a in Context.AddressModelSet
                                where a.customerID == id
                                select a).ToList();
            //var addressModel = Context.AddressModelSet.Where(a => a.customerID == id).ToList();

            foreach (var cm in customerModel)
            {
                cus.customerID = cm.customerID;
                cus.firstName = cm.firstName;
                cus.lastName = cm.lastName;
                cus.phone = cm.phone;
                cus.email = cm.email;
                cus.password = Cipher.Decrypt(cm.password);
                cus.confirmPassword = Cipher.Decrypt(cm.confirmPassword);
            }

            foreach (var am in addressModel)
            {
                cus.address = am.address;
                cus.city = am.city;
                cus.postalCode = am.postalCode;
            }
            return View(cus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _EditProfile([Bind(Include = "customerID,firstName,lastName,phone,email,password,confirmPassword,state,address,city,postalCode")] CustomerInfoModel cus)
        {
            int cusID = Convert.ToInt32(Session["userID"]);

             var errors = ModelState
               .Where(x => x.Value.Errors.Count > 0)
               .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();

            if (ModelState.IsValid)
            {

                CustomerModel cusModel = new CustomerModel();
                AddressModel addModel = new AddressModel();

                string password = Cipher.Encrypt(cus.password);
                string confirmPassword = Cipher.Encrypt(cus.confirmPassword);
                try
                {
                    var customerModel = new CustomerModel();
                    customerModel = Context.CustomerModelSet.Find(cusID);
                    customerModel.customerID = cus.customerID;
                    customerModel.firstName = cus.firstName;
                    customerModel.lastName = cus.lastName;
                    customerModel.phone = cus.phone;
                    customerModel.email = cus.email;
                    customerModel.password = password;
                    customerModel.confirmPassword = confirmPassword;



                    var addressModel = new AddressModel();
                    addressModel = Context.AddressModelSet.Find(cusID);
                    addressModel.address = cus.address;
                    addressModel.city = cus.city;
                    addressModel.postalCode = cus.postalCode;


                    Context.Entry(customerModel).State = EntityState.Modified;
                    Context.Entry(addressModel).State = EntityState.Modified;
                }
                catch (Exception)
                {

                    
                }
               

              

                try
                {
                    Context.SaveChanges();
                    ViewBag.result = "Your Data has been Successfully Updated";
                }
                catch (DbUpdateConcurrencyException ex)
                {
                   
                 // Update the values of the entity that failed to save from the store 
                    ex.Entries.Single().Reload();
}

                return View(cus);
            }

            //return RedirectToAction("EditProfile", "Profile");
            return RedirectToAction("Index");
        }
    }
}