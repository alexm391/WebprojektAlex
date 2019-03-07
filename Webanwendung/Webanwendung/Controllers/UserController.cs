using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webanwendung.Models;
using Webanwendung.Models.db;
using MySql.Data.MySqlClient;


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

            ValidateData(userDataForm);
            ValidatePassword(userDataForm);

            if (ModelState.IsValid)
            {
                try
                {
                    userRepository = new UserRepositoryDB();
                    userRepository.Open();
                    userRepository.Insert(userDataForm);
                    return View("Message", new Message("Registrierung", "Sie wurden erfolgreich registriert"));
                }
                catch(Exception ex)
                {
                    return View("Message", new Message("Registrierung", "Sie konnten nicht registriert werden", "Versuchen Sie es später nocheinmal"));
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

        [HttpGet]
        public ActionResult Login()
        {
            Login l = new Login();
            return View(l);

        }
        [HttpPost]
        public ActionResult Login(Login loginDataForm)
        {
            if(loginDataForm == null)
            {
                return RedirectToAction("Login");
            }

            try
            {
                userRepository = new UserRepositoryDB();
                userRepository.Open();
                User user = userRepository.Authenticate(loginDataForm);

                if(user != null)
                {
                    if (user.UserRole == UserRole.admin)
                    {
                        Session["isAdmin"] = true;
                        Session["name"] = user.Firstname;
                        Session["userID"] = user.ID;
                    }
                    else
                    {
                        Session["isRegisteredUser"] = true;
                        Session["name"] = user.Firstname + " " + user.Lastname;
                        Session["userID"] = user.ID;
                    }
                    return View("Message", new Message("Anmeldung", "Sie wurden erfolgreich angemeldet"));

                }
                else
                {
                    ModelState.AddModelError("Password", "Benutzername/Passwort Kombination ist falsch");
                    return View(loginDataForm);
                }

            }
            catch(Exception ex)
            {
                return View("Message", new Message("Anmeldung", "Es bei der Anmeldung einen Fehler", "Versuchen Sie es später nocheinmal"));
            }
            finally
            {
                userRepository.Close();
            }
        }

        public ActionResult Logout()
        {
            Session["isAdmin"] = null;
            Session["isRegisteredUser"] = null; 
            Session["name"] = null;
            Session["userID"] = null;
            return View("Message", new Message("Abmeldung", "Sie wurden erfolgreich abgemeldet"));

        }

        [HttpGet]
        public ActionResult ChangeUserData(int userId)
        {
            try
            {
                userRepository = new UserRepositoryDB();
                userRepository.Open();
                User user = userRepository.GetUser(userId);
                return View(user);
            }
            catch (Exception ex)
            {
                return View("Message", new Message("Datenänderung", "Es gab eine Fehler bei der Datenänderung"));
            }
            finally
            {
                userRepository.Close();
            }
        }
        [HttpPost]
        public ActionResult ChangeUserData(int userId, User newUserDataForm)
        {
            if(newUserDataForm == null)
            {
                return RedirectToAction("ChangeUserData(userId)");
            }

            ValidateData(newUserDataForm);

            if (ModelState.IsValid)
            {
                try
                {
                    userRepository = new UserRepositoryDB();
                    userRepository.Open();
                    userRepository.ChangeUserData(userId, newUserDataForm);
                    Session["name"] = newUserDataForm.Firstname + " " + newUserDataForm.Lastname;
                    return View("Message", new Message("Datenänderung", "Die Daten wurden erfolgreich geändert"));
                }
                catch (Exception ex)
                {
                    return View("Message", new Message("Datenänderung", "Die Daten konnten nicht geändert werden"));
                }
                finally
                {
                    userRepository.Close();
                }
            }
            else
            {
                return RedirectToAction("ChangeUserData(userId, newUserDataForm)");
            }
        }




        private void ValidateData(User user)
        {
            if((user.Firstname == null) || (user.Firstname.Trim().Length < 3))
            {
                ModelState.AddModelError("Firstname", "Der Vorname muss mindestens 3 Zeichen lang sein");
            }
            if((user.Lastname == null) || (user.Lastname.Trim().Length < 3))
            {
                ModelState.AddModelError("Lastname", "Der Nachname muss mindestens 3 Zeichen lang sein");
            }
            if(user.Birthdate > DateTime.Now)
            {
                ModelState.AddModelError("Birthdate", "Bitte Geburtsdatum angeben");
            }
            if(user.Gender == Gender.notSpecified)
            {
                ModelState.AddModelError("Gender", "Das Geschlecht muss angegeben werden");
            }
            if((user.Username == null) || (user.Username.Trim().Length < 8))
            {
                ModelState.AddModelError("Username", "Der Username muss mindestens 8 Zeichen lang sein");
            }
            if((user.Email == null) || (!user.Email.Contains('@')))
            {
                ModelState.AddModelError("Email", "Bitte Email-Adresse angeben");
            }
        }
        private void ValidatePassword(User user)
        {
            if ((user.Password == null) || (user.Password.Length < 8)
                        || (user.Password.IndexOfAny(new char[] { '!', '?', '%', '&' }) == -1))
            {
                ModelState.AddModelError("Password", "Das Passwort muss mindestens 8 Zeichen lang sein und mindestend ein Sonderzeichen einthalten");
            }
            if (user.PasswordConfirmation != user.Password)
            {
                ModelState.AddModelError("Password", "Die Passwörter stimmen nicht überein");
            }
        }

    }
}