using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webanwendung.Models.db;
using Webanwendung.Models;

namespace Webanwendung.Controllers
{
    public class AdminController : Controller
    {
        private IUserRepository userRepository;
        private IBookingRepository bookingRepository;

        // only for testing
        public ActionResult Index()
        {
            try
            {
                userRepository = new UserRepositoryDB();
                userRepository.Open();
                List<User> users = userRepository.GetAllUsers();
                return View("Message", new Message("kein Fehler", "kein Fehler"));
            }
            catch (Exception)
            {
                return View("Message", new Message("Fehler", "Fehler"));
            }   
        }

        public ActionResult ShowUsers()
        {
            if (IsAdmin())
            {
                try
                {
                    userRepository = new UserRepositoryDB();
                    userRepository.Open();
                    List<User> users = userRepository.GetAllUsers();
                    return View(users);
                }
                catch (Exception)
                {
                    return View("Message", new Message("Alle User anzeigen", "Beim anzeigen aller User ist ein Fehler aufgetreten"));
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


        private bool IsAdmin()
        {
            if ((Session["isAdmin"] != null) && (Convert.ToBoolean(Session["isAdmin"]) == true))
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