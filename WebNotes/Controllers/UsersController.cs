using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using WebNotesDataBase.DAL;
using WebNotesDataBase.Models;

namespace WebNotes.Controllers
{
    public class UsersController : Controller
    {
        private UserRepository uowUser;
        public GenericRepository<User> userRepository;

        public UsersController()
        {
            uowUser = new UserRepository();
            userRepository = uowUser.unitOfWork.EntityRepository;
        }

        // GET: Users
        public ActionResult Registration()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public ActionResult LogOut()
        {
            if(Request.Cookies["login"] != null)
            {
                var cookie = new HttpCookie("login");
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
            return RedirectToAction("../Home/Index");
        }

        [HttpPost]
        public ActionResult Login(string email, string pass)
        {
            User auth = userRepository.Get(u => u.Email == email).SingleOrDefault();
            if(auth != null && auth.Pass == pass)
            {
                var cookie = new HttpCookie("login")
                {
                    Value = auth.UserId.ToString(),
                    Expires = DateTime.Now.AddMonths(1)
                };
                Response.SetCookie(cookie);
                return RedirectToAction("../Notes/Index");
            }
            return View("../Shared/Error");
        }

        // POST: Users/Create
            //registration
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,NameAuthor,Birthday,Email,Pass")] User user)
        {
            if (ModelState.IsValid)
            {
                userRepository.Insert(user);
                userRepository.Save();
                return RedirectToAction("Login");
            }
            return View("../Shared/Error");
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public ActionResult Get(int? id)
        {
            User loginUser = null;
            if(ModelState.IsValid)
            {
                loginUser = userRepository.GetByID(id);
                userRepository.Save();
                return View("Index", loginUser);
            }
            return RedirectToAction("Login");
        }

        //GET: Users/Details/5
        public ActionResult Details()
        {
            User user = null;
            if (Request.Cookies["login"] != null)
            {
                int id = Convert.ToInt32(Request.Cookies["login"].Value);
                user = userRepository.GetByID(id);
            }
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = userRepository.GetByID(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,NameAuthor,Birthday,Pass")] User user)
        {
            if (ModelState.IsValid)
            {
                userRepository.Update(user);
                userRepository.Save();
            }
            return View(user);
        }

    }
}
