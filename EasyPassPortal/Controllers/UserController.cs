using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using EasyPassPortal.Models;
using EasyPassPortal.Repository;

namespace EasyPassPortal.Controllers
{
    public class UserController : Controller
    {
        /// <summary>
        /// get user detail get method
        /// </summary>
        /// <returns></returns>
        public ActionResult AddUserDetail()
        {
            return View();
        }

        /// <summary>
        /// This method is used to add new users.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddUserDetail(UserModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserDetails userDetails = new UserDetails();
                    if (userDetails.AddAccountDetails(user))
                    {
                        ViewBag.Message = "Data Inset SuccessFully.";
                        return RedirectToAction("LoginUserDetail", "User");
                    }
                }
                return View();
            }
            catch (Exception exception)
            {
                ViewBag.Message = "Error occurred while inserting!" + exception.Message;
                return View();
            }
        }
        /// <summary>
        /// Get login page.
        /// </summary>
        /// <param name="returnURL"></param>
        /// <returns></returns>
        public ActionResult LoginUserDetail()
        {
            return View();
        }

        /// <summary>
        /// Login authentication of user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoginUserDetail(UserModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserDetails userDetails = new UserDetails();
                    var userLoggedIn = userDetails.VerifyUser(user.Email, user.Password);

                    if (userLoggedIn)
                    {
                        ViewBag.message = "loggedin";
                        ViewBag.triedOnce = "yes";
                        Session["UserEmail"] = user.Email;
                        return RedirectToAction("HomePageDetails", "User");
                    }
                    else
                    {
                        ViewBag.triedOnce = "yes";
                        return View();
                    }
                }
                return View(user);
            }
            catch (Exception exception)
            {
                ViewBag.triedOnce = "yes";
                ViewBag.message = "An error occurred: " + exception.Message;
                return View(user);
            }
        }

        /// <summary>
        /// Home page get method
        /// </summary>
        /// <returns></returns>
        public ActionResult HomePageDetails()
        {
            return View();
        }
        /// <summary>
        /// Show passport apply page
        /// </summary>
        /// <returns></returns>
        public ActionResult AddUserPassportDetail()
        {
            string userEmail = Session["UserEmail"] as string;
            if (string.IsNullOrEmpty(userEmail))
            {
                 return RedirectToAction("LoginUserDetail", "User");
               
            }
            return View();
        }
        /// <summary>
        /// Adding a passport request for admin 
        /// </summary>
        /// <param name="passportDetails"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddUserPassportDetail(UserPassportDetails passportDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserDetails userDetails = new UserDetails();
                    if (userDetails.AddPassportDetails(passportDetails))
                    {
                        ViewBag.Message = "Data Inserted Successfully.";
                        return RedirectToAction("StatusEnquiry", "User");
                    }
                }
                return View();
            }
            catch (Exception exception)
            {
                ViewBag.Message = "Error occurred while inserting! " + exception.Message;
                return View();
            }
        }

        /// <summary>
        /// About us page.
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            return View();
        }
        /// <summary>
        /// Contact us page
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            return View();
        }
        /// <summary>
        /// userDetails page
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUserDetail()
        {
            try
            {
                string userEmail = Session["UserEmail"] as string;
                if (string.IsNullOrEmpty(userEmail))
                {
                    return RedirectToAction("LoginUserDetail", "User");
                }
                UserDetails userDetails = new UserDetails();
                UserModel user = userDetails.GetUserByEmail(userEmail);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
            catch (Exception exception)
            {
                ViewBag.Message = "Error occurred while login!" + exception.Message;
                return View();
            }
        }
        /// <summary>
        /// Edit user password view get method.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditUserPassword()
        {
            try
            {
                string userEmail = Session["UserEmail"] as string;
                UserDetails userDetails = new UserDetails();
                UserModel user = userDetails.GetUserByEmail(userEmail);
                if (user == null)
                {
                    return RedirectToAction("LoginUserDetail");
                }
                return View(user);
            }
            catch
            {
               return RedirectToAction("LoginUserDetail");
            }
        }

        /// <summary>
        /// Editing user password.
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditUserPassword(UserModel userModel)
        {
            try
            {
                UserDetails userDetails = new UserDetails();
                userDetails.EditPassword(userModel);
                return RedirectToAction("GetUserDetail");
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error occurred while editing!" + ex.Message;
                return View();
            }
        }
        /// <summary>
        /// This is the view for user enquirey.
        /// </summary>
        /// <returns></returns>
        public ActionResult StatusEnquiry()
        {
            try
            {
                string userEmail = Session["UserEmail"] as string;
                UserDetails userDetails = new UserDetails();
                UserPassportDetails userPassportDetails = userDetails.GetUserPassportDetails(userEmail);
                if (userPassportDetails == null)
                {
                    return RedirectToAction("GetUserDetail");
                }
                return View(userPassportDetails);
            }
            catch(Exception ex) 
            {
                ViewBag.Message = "Error occurred while editing!" + ex.Message;
                return View();
            }
        }
        /// <summary>
        /// Logout for user side.
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOutUser()
        {
            Session.Abandon();
            return RedirectToAction("LoginUserDetail", "User");
        }

    }
}
