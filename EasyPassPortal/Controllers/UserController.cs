using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EasyPassPortal.Models;
using EasyPassPortal.Repository;

namespace EasyPassPortal.Controllers
{
    public class UserController : Controller
    {
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
            catch (Exception ex)
            {
                ViewBag.Message = "Error occurred while inserting!" + ex.Message;
                return View();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnURL"></param>
        /// <returns></returns>
        public ActionResult LoginUserDetail()
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult LoginUserDetail(LoginModel user)
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
            catch (Exception ex)
            {
                ViewBag.triedOnce = "yes";
                ViewBag.message = "An error occurred: " + ex.Message;
                return View(user);
            }
        }


        public ActionResult HomePageDetails()
        {
            return View();
        }

        public ActionResult AddUserPassportDetail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddUserPassportDetail(UserPassportDetails passportDetails)
        {
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        UserDetails userDetails = new UserDetails();
                        if (userDetails.AddPassportDetails(passportDetails))
                        {
                            ViewBag.Message = "Data Inset SuccessFully.";
                        }
                    }
                    return View();

                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Error occurred while inserting!" + ex.Message;
                    return View();
                }
            }
        }

        public ActionResult About()
        {
            return View();
        }


        /// <summary>
        /// userDetails page
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUserDetail()
        {
            // Retrieve the email from the session
            string userEmail = Session["UserEmail"] as string;
            if (string.IsNullOrEmpty(userEmail))
            {
                // Handle the case where the email is not found in the session
                return RedirectToAction("LoginUserDetail", "User"); // Redirect to login if email is not found
            }

            UserDetails userDetails = new UserDetails();
            UserModel user = userDetails.GetUserByEmail(userEmail);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

























        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
