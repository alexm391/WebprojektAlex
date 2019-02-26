using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webanwendung.Models;
using Webanwendung.Models.db;

namespace Webanwendung.Controllers
{
    public class UserController : Controller
    {
        private IUserRepository userRepository;


        [HttpGet]
        public ActionResult Registration()
        {
            User u = new User(); 
            return View(u);
        }

        [HttpPost]
        public ActionResult Registration(User userDataForm)
        {
            if(userDataForm == null)
            {
                return RedirectToAction("Registration");
            }

            ValidateRegistration(userDataForm);

            if (ModelState.IsValid)
            {
                try
                {
                    userRepository = new UserRepositoryDB();
                    userRepository.Open();
                    userRepository.Insert(userDataForm);
                    return View("Index");
                }
                catch(Exception ex)
                {
                    return View("Index");
                }
                finally
                {
                    userRepository.Close();
                }
            }
            else
            {
                return View(userDataForm);
            }
        }

        private void ValidateRegistration(User user)
        {
            if((user.Firstname != null) && (user.Firstname.Trim().Length < 3))
            {
                ModelState.AddModelError("Firstname", "Der Vorname muss mindestens 3 Zeichen lang sein");
            }
            if((user.Lastname != null) && (user.Lastname.Trim().Length < 3))
            {
                ModelState.AddModelError("Lastname", "Der Nachname muss mindestens 3 Zeichen lang sein");
            }
            if(user.Birthdate < DateTime.Now)
            {
                ModelState.AddModelError("Birthdate", "Bitte Geburtsdatum angeben");
            }
            if(user.Gender == Gender.notSpecified)
            {
                ModelState.AddModelError("Gender", "Das Geschlecht muss angegeben werden");
            }
            if((user.Username != null) && (user.Username.Trim().Length < 8))
            {
                ModelState.AddModelError("Username", "Der Username muss mindestens 8 Zeichen lang sein");
            }
            if((user.Email != null) && (!user.Email.Contains('@')))
            {
                ModelState.AddModelError("Email", "Bitte Email-Adresse angeben");
            }
            if ((user.Password == null) || (user.Password.Length < 8)
                    || (user.Password.IndexOfAny(new char[] { '!', '?', '%', '&' }) == -1))
            {
                ModelState.AddModelError("Password", "Das Passwort muss mindestens 8 Zeichen lang sein und mindestend ein Sonderzeichen einthalten");
            }
        }
    }
}