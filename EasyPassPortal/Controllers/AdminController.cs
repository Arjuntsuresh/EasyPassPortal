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


        /// <summary>
        /// admin login page
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Admin home page
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminHomePage()
        {
            return View();
        }
        /// <summary>
        /// get all pssport reg users details for admin.
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAccountDetails()
        {
            try
            {
                AdminDetails adminDetails = new AdminDetails();
                ModelState.Clear();
                return View(adminDetails.GetAllDetails());
            }catch (Exception exception)
            {
                ViewBag.Message = "Error occurred w!" + exception.Message;
                return View();
            }
        }
        /// <summary>
        /// This is to get all the user details for the admin.
        /// </summary>
        /// <returns>user detail</returns>
        public ActionResult GetUserDetails()
        {
            try
            {
                AdminDetails adminDetails = new AdminDetails();
                ModelState.Clear();
                return View(adminDetails.GetAllUserDetails());
            }
            catch (Exception exception)
            {
                ViewBag.Message = "Error occurred w!" + exception.Message;
                return View();
            }
           
        }
    }
}
