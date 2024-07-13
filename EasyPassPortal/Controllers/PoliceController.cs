using EasyPassPortal.Models;
using EasyPassPortal.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyPassPortal.Controllers
{
    public class PoliceController : Controller
    {
        // GET: Police
        public ActionResult PoliceLoginDetail()
        {
            return View();
        }

        // GET: Police/Details/5
        [HttpPost]
        public ActionResult PoliceLoginDetail(PoliceDept policeDept)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    PoliceDetail policeDetail = new PoliceDetail();
                    var adminLoggedIn = policeDetail.VerifyPolice(policeDept.Email, policeDept.Password);

                    if (adminLoggedIn)
                    {
                        ViewBag.message = "loggedin";
                        ViewBag.triedOnce = "yes";
                        Session["PoliceEmail"] = policeDept.Email;
                        return RedirectToAction("PoliceHomePage", "Police");
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
                return View(policeDept);
            }
        }


        public ActionResult PoliceHomePage()
        {
            string policeEmail = Session["PoliceEmail"] as string;
            if (string.IsNullOrEmpty(policeEmail))
            {
                return RedirectToAction("PoliceLoginDetail", "Police");
            }

            try
            {
               
                PoliceDetail policeDetail = new PoliceDetail();
                ModelState.Clear();
                return View(policeDetail.GetAllDetails());
            }
            catch (Exception exception)
            {
                ViewBag.Message = "Error occurred: " + exception.Message;
                return View();
            }
        }

        public ActionResult PolicePassportManagement(string email)
        {
            string adminEmail = Session["PoliceEmail"] as string;
            if (string.IsNullOrEmpty(adminEmail))
            {
                return RedirectToAction("PoliceLoginDetail", "Police");
            }

            try
            {
                UserDetails userDetails = new UserDetails();
                UserPassportDetails userPassportDetails = userDetails.GetUserPassportDetails(email);
                if (userPassportDetails == null)
                {
                    return RedirectToAction("PoliceHomePage");
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
        public ActionResult PolicePassportManagement(UserPassportDetails userPassportDetails)
        {
            try
            {
                PoliceDetail policeDetail = new PoliceDetail();
                policeDetail.EditAccountDetails(userPassportDetails);
                return RedirectToAction("PoliceHomePage");
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error occurred while editing!" + ex.Message;
                return View();
            }
        }

        public ActionResult LogOutPolice()
        {
            Session.Abandon();
            return RedirectToAction("HomePageDetails", "User");
        }


    }
}
