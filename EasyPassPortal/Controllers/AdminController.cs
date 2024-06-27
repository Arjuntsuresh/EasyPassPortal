using EasyPassPortal.Models;
using EasyPassPortal.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyPassPortal.Controllers
{
    public class AdminController : Controller
    {


        //Admin Login
        public ActionResult AdminLoginDetail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminLoginDetail(AdminModel adminModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AdminDetails adminDetails = new AdminDetails();
                    var adminLoggedIn = adminDetails.VerifyAdmin(adminModel.Email, adminModel.Password);

                    if (adminLoggedIn)
                    {
                        ViewBag.message = "loggedin";
                        ViewBag.triedOnce = "yes";
                        Session["AdminEmail"] = adminModel.Email;
                        return RedirectToAction("AdminHomePage", "Admin");
                    }
                    else
                    {
                        ViewBag.triedOnce = "yes";
                        return View();
                    }
                }
                return View(adminModel);
            }
            catch (Exception ex)
            {
                ViewBag.triedOnce = "yes";
                ViewBag.message = "An error occurred: " + ex.Message;
                return View(adminModel);
            }
        }

        //Admin Home page
        public ActionResult AdminHomePage()
        {
            return View();
        }
        //Get all Passport registered users
        public ActionResult GetAccountDetails()
        {
            AdminDetails adminDetails = new AdminDetails();
            ModelState.Clear();
            return View(adminDetails.GetAllDetails());
        }
        public ActionResult GetUserDetails()
        {
            AdminDetails adminDetails = new AdminDetails();
            ModelState.Clear();
            return View(adminDetails.GetAllUserDetails());
        }
    }
}
