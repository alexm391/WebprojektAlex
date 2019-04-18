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
            if(!IsLoggedIn())
            {
                User u = new User();
                return View(u);
            }
            else
            {
                return View("Message", new Message("URL Fehler", "Die eingegeben URL ist ungültig"));
            }

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
                        Session["id"] = user.ID;
                    }
                    else
                    {
                        Session["isRegisteredUser"] = true;
                        Session["name"] = user.Firstname + " " + user.Lastname;
                        Session["id"] = user.ID;
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
            if (IsLoggedIn())
            {
                Session["isAdmin"] = null;
                Session["isRegisteredUser"] = null;
                Session["name"] = null;
                Session["id"] = null;
                Session["roomNr"] = null;
                Session["booking"] = null;
                return View("Message", new Message("Abmeldung", "Sie wurden erfolgreich abgemeldet"));
            }
            else
            {
                return View("Message", new Message("URL Fehler", "Die eingegebene URL ist ungültig"));
            }


        }

        [HttpGet]
        public ActionResult ChangeUserData()
        {
            if (IsLoggedIn())
            {
                try
                {
                    userRepository = new UserRepositoryDB();
                    userRepository.Open();
                    if(Session["id"] != null)
                    {
                        User user = userRepository.GetUser(Convert.ToInt32(Session["id"]));
                        return View(user);
                    }
                    else
                    {
                        return View("Message", new Message("Datenänderung", "Es gab eine Fehler bei der Datenänderung"));
                    }
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
            else
            {
                return View("Message", new Message("URL Fehler", "Die eingegebene URL ist ungültig"));
            }
        }
        [HttpPost]
        public ActionResult ChangeUserData(User newUserDataForm)
        {
            if(newUserDataForm == null)
            {
                return RedirectToAction("ChangeUserData");
            }

            ValidateData(newUserDataForm);

            if (ModelState.IsValid)
            {
                try
                {
                    userRepository = new UserRepositoryDB();
                    userRepository.Open();
                    if(Session["id"] != null)
                    {
                        if (userRepository.ChangeUserData(Convert.ToInt32(Session["id"]), newUserDataForm))
                        {
                            if ((Session["isRegisteredUser"] != null) && (Convert.ToBoolean(Session["isRegisteredUser"]) == true))
                            {
                                Session["name"] = newUserDataForm.Firstname + " " + newUserDataForm.Lastname;
                            }
                            return View("Message", new Message("Datenänderung", "Die Daten wurden erfolgreich geändert"));
                        }
                        else
                        {
                            return View("Message", new Message("Datenänderung", "Die Daten konnten nicht geändert werden"));
                        }
                    }
                    else
                    {
                        return View("Message", new Message("Datenänderung", "Die Daten konnten nicht geändert werden"));
                    }
                }
                catch (Exception ex)
                {
                    return View("Message", new Message("Datenänderung", "Es gab einen Fehler bei der Verarbeitung ihrer Daten"));
                }
                finally
                {
                    userRepository.Close();
                }
            }
            else
            {
                return RedirectToAction("ChangeUserData", newUserDataForm);
            }
        }
        
        [HttpGet]
        public ActionResult ChangePassword()
        {
            if (IsLoggedIn())
            {
                User u = new User();
                return View(u);
            }
            else
            {
                return View("Message", new Message("URL Fehler", "Die eingegebene URL ist ungültig"));
            }

        }
        [HttpPost]
        public ActionResult ChangePassword(User newPasswordForm)
        {
            if (newPasswordForm == null)
            {
                return RedirectToAction("ChangePassword");
            }

            ValidatePassword(newPasswordForm);

            if (ModelState.IsValid)
            {
                try
                {
                    userRepository = new UserRepositoryDB();
                    userRepository.Open();
                    if (Session["id"] != null)
                    {
                        bool newPwd = userRepository.ChangePassword(Convert.ToInt32(Session["id"]), newPasswordForm);
                        if (newPwd == true)
                        {
                            return View("Message", new Message("Passwortänderung", "Das Passwort wurde erfolgreich geändert"));
                        }
                        else
                        {
                            ModelState.AddModelError("NewPassword", "Das alte Passwort ist nicht richtig");
                            return View();
                        }
                    }
                    else
                    {
                        return View("Message", new Message("Passwortänderung", "Das Passwort konnte nicht geändert werden"));
                    }
                }
                catch (Exception ex)
                {
                    return View("Message", new Message("Passwortänderung", "Das Passwort konnte nicht geändert werden"));
                }
                finally
                {
                    userRepository.Close();
                }
            }
            else
            {
                return View();
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
            if((user.Birthdate == null) || (user.Birthdate > DateTime.Now))
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
            if ((user.NewPassword == null) || (user.NewPassword.Length < 8)
                        || (user.NewPassword.IndexOfAny(new char[] { '!', '?', '%', '&' }) == -1))
            {
                ModelState.AddModelError("NewPassword", "Das Passwort muss mindestens 8 Zeichen lang sein ");
                ModelState.AddModelError("NewPassword", "Das Passwort muss mindestend ein Sonderzeichen einthalten");
            }
            if (user.PasswordConfirmation != user.NewPassword)
            {
                ModelState.AddModelError("NewPassword", "Die neuen Passwörter stimmen nicht überein");
            }
        }

        private bool IsLoggedIn()
        {
            if (((Session["isAdmin"] != null) && (Convert.ToBoolean(Session["isAdmin"]) == true)) ||
                            ((Session["isRegisteredUser"] != null) && (Convert.ToBoolean(Session["isRegisteredUser"]) == true)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }



    }
}