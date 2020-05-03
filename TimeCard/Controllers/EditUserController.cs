using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Timecard.vm;
using TimeCard.db;
using TimeCard.repo;

namespace TimeCard.Controllers
{
    public class EditUserController : Controller
    {
        TimeCardEntities _contextuser = new TimeCardEntities();
        // GET: EditUser
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Getallusers()
        {
            Resetpasswordrepo reporesetpswdobj = new Resetpasswordrepo();
            var allusers = reporesetpswdobj.Getallusersrepo();
            return View("Getallusers", allusers);
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public ActionResult Edit()
        {
            int UserId = Convert.ToInt32(RouteData.Values["id"]);
            if (UserId == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            UserVM userdetails = new UserVM();
            userdetails.UserdetailsVM = _contextuser.Users.Find(UserId);
            if (userdetails == null)
            {
                return HttpNotFound();
            }
            return View(userdetails);
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public ActionResult Edit([Bind(Include = "Password,ConfirmPassword")] UserVM userdetails)
        {
            int UserId = Convert.ToInt32(RouteData.Values["id"]);
            ModelState.Remove("Name");
            ModelState.Remove("EmailId");
            ModelState.Remove("Username");
            ModelState.Remove("RoleId");
            if (ModelState.IsValid)
            {
                Resetpasswordrepo repoobj = new Resetpasswordrepo();
                repoobj.ResetPasswordrepo(userdetails, UserId);
                userdetails = null;
                ViewBag.Message = "Password updated successfully";
                return RedirectToAction("Getallusers");
            }
            else
                return View();

        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public ActionResult Delete()
        {
            int UserId = Convert.ToInt32(RouteData.Values["id"]);
            if (UserId==0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserVM userdetails = new UserVM();
            userdetails.UserdetailsVM = _contextuser.Users.Find(UserId);
            if (userdetails == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Deleteconfirm",userdetails);
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public ActionResult Deleteconfirm()
        {
            int UserId = Convert.ToInt32(RouteData.Values["id"]);
            Resetpasswordrepo repoobj = new Resetpasswordrepo();
            repoobj.Deleteuserrepo(UserId);
            UserVM usersdetails = new UserVM();
            usersdetails = null;
            ViewBag.Message = "User deleted successfully";
            return View("Getallusers");
        }

        [HttpPost]
        public ActionResult Back()
        {
            ModelState.Remove("Name");
            ModelState.Remove("EmailId");
            ModelState.Remove("Username");
            ModelState.Remove("RoleId");
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            return View("Getallusers");
        }
    }
}