using EasyPassPortal.Models;
using EasyPassPortal.Repository;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Principal;
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
        /// <summary>
        /// AdminLoginDetail post method.
        /// </summary>
        /// <param name="adminModel"></param>
        /// <returns></returns>
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
                        ViewBag.message = "Email or password is incorrect!";
                        ViewBag.triedOnce = "yes";
                        return View();
                    }
                }
                return View();
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
            string adminEmail = Session["AdminEmail"] as string;
            if (string.IsNullOrEmpty(adminEmail))
            {
                return RedirectToAction("AdminLoginDetail", "Admin");
            }
            TotalUserCount();
            TotalUserPassportRequestCount();
            return View();
        }
        /// <summary>
        /// get all pssport reg users details for admin.
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAccountDetails()
        {
            string adminEmail = Session["AdminEmail"] as string;
            if (string.IsNullOrEmpty(adminEmail))
            {
                return RedirectToAction("AdminLoginDetail", "Admin");
            }

            try
            {
                TotalUserCount();
                TotalUserPassportRequestCount();
                AdminDetails adminDetails = new AdminDetails();
                ModelState.Clear();
                return View(adminDetails.GetAllDetails());
            }
            catch (Exception exception)
            {
                ViewBag.Message = "Error occurred: " + exception.Message;
                return View();
            }
        }

        /// <summary>
        /// This is to get all the user details for the admin.
        /// </summary>
        /// <returns>user detail</returns>
        public ActionResult GetUserDetails()
        {
            string adminEmail = Session["AdminEmail"] as string;
            if (string.IsNullOrEmpty(adminEmail))
            {
                return RedirectToAction("AdminLoginDetail", "Admin");
            }

            try
            {
                TotalUserCount();
                TotalUserPassportRequestCount();
                AdminDetails adminDetails = new AdminDetails();
                ModelState.Clear();
                return View(adminDetails.GetAllUserDetails());
            }
            catch (Exception exception)
            {
                ViewBag.Message = "Error occurred: " + exception.Message;
                return View();
            }
        }

        /// <summary>
        /// Add new admin view page 
        /// </summary>
        /// <returns></returns>
        public ActionResult AddNewAdmin()
        {
            TotalUserCount();
            TotalUserPassportRequestCount();
            string adminEmail = Session["AdminEmail"] as string;
            if (string.IsNullOrEmpty(adminEmail))
            {
                return RedirectToAction("AdminLoginDetail", "Admin");
            }

            return View();
        }

        /// <summary>
        /// Add new admin post method.
        /// </summary>
        /// <param name="adminModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddNewAdmin(AdminModel adminModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AdminDetails adminDetails = new AdminDetails();
                    if (adminDetails.AddNewAdmin(adminModel))
                    {
                        ViewBag.Message = "New Admin added successfully.";
                        return RedirectToAction("AdminLoginDetail", "Admin");
                    }
                }
                return View();
            }
            catch (Exception exception)
            {
                ViewBag.Message = "Error occured while inserting!" + exception.Message;
                return View();
            }
        }
        /// <summary>
        /// Edit admin password
        /// </summary>
        /// <returns></returns>
        public ActionResult EditAdminPassword()
        {
            try
            {
                TotalUserCount();
                TotalUserPassportRequestCount();
                string userEmail = Session["AdminEmail"] as string;
                AdminDetails adminDetails = new AdminDetails();
                AdminModel adminModel = adminDetails.GetAdminByEmail(userEmail);
                if (adminModel == null)
                {
                    return RedirectToAction("AdminLoginDetail");
                }
                return View(adminModel);
            }
            catch
            {
                return RedirectToAction("AdminLoginDetail");
            }
        }

        /// <summary>
        /// Edit admin password 
        /// </summary>
        /// <param name="adminModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditAdminPassword(AdminModel adminModel)
        {
            try
            {
                AdminDetails adminDetails = new AdminDetails();
                adminDetails.EditPassword(adminModel);
                return RedirectToAction("AdminHomePage");
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error occurred while editing!" + ex.Message;
                return View();
            }


        }
        /// <summary>
        /// Delete user from admin.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteUserDetail(int id)
        {
            try
            {
                AdminDetails adminDetails = new AdminDetails();
                if (adminDetails.DeleteUserFromAdmin(id))
                {
                    ViewBag.AlertMessage = ("User detail deleted successfully.");
                }
                return RedirectToAction("GetUserDetails");
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error occurred while deleting!" + ex.Message;
                return View();
            }
        }
        /// <summary>
        /// This is the method for deleting user requested passport .
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeletePassportDetail(int id)
        {
            try
            {
                AdminDetails adminDetails = new AdminDetails();
                if (adminDetails.DeletePassportDetailFromAdmin(id))
                {
                    ViewBag.AlertMessage = ("Passport  deleted successfully.");
                }
                return RedirectToAction("GetAccountDetails");
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error occurred while deleting!" + ex.Message;
                return View();
            }

        }
        /// <summary>
        /// Get total users count.
        /// </summary>
        /// <returns></returns>
        public ActionResult TotalUserCount()
        {
            try
            {
                AdminDetails adminDetails = new AdminDetails();
                ModelState.Clear();
                int totalCount = adminDetails.TotalCountOfUser();
                ViewBag.TotalUserCount = totalCount;
                return View();
            }
            catch (Exception exception)
            {
                ViewBag.Message = "Error occurred: " + exception.Message;
                return View();
            }
        }

        public ActionResult TotalUserPassportRequestCount()
        {
            try
            {
                AdminDetails adminDetails = new AdminDetails();
                ModelState.Clear();
                int totalCount = adminDetails.TotalPassportRequestCountOfUser();
                ViewBag.TotalPassportCount = totalCount;
                return View();
            }
            catch (Exception exception)
            {
                ViewBag.Message = "Error occurred: " + exception.Message;
                return View();
            }
        }

        public ActionResult AdminPassportManagement(string email)
        {
            string adminEmail = Session["AdminEmail"] as string;
            if (string.IsNullOrEmpty(adminEmail))
            {
                return RedirectToAction("AdminLoginDetail", "Admin");
            }

            try
            {
                UserDetails userDetails = new UserDetails();
                UserPassportDetails userPassportDetails = userDetails.GetUserPassportDetails(email);
                if (userPassportDetails == null)
                {
                    return RedirectToAction("GetAccountDetails");
                }
                return View(userPassportDetails);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error occurred while editing!" + ex.Message;
                return View();
            }
        }

        [HttpPost]
        public ActionResult AdminPassportManagement(UserPassportDetails userPassportDetails)
        {
            try
            {
                AdminDetails adminDetails = new AdminDetails();
                adminDetails.EditAccountDetails(userPassportDetails);
                return RedirectToAction("GetAccountDetails");
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error occurred while editing!" + ex.Message;
                return View();
            }
        }

        /// <summary>
        /// Admin logout function
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOutAdmin()
        {
            Session.Abandon();
            return RedirectToAction("HomePageDetails", "User");
        }
    }
}
