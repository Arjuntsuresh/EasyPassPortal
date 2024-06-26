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
        public ActionResult LoginUserDetail(string returnURL)
        {
            return View();
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult LoginUserDetail(LoginModel loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserDetails userDetails = new UserDetails();
                    if(userDetails.VerifyUser(loginModel.Email,loginModel.Password))
                    {
                        Session["UserEmail"] = loginModel.Email;
                        return RedirectToAction("LoginUserDetail", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid login details");
                    }
                }
                return View(loginModel);
               
            }
            catch
            {
                return View();
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
