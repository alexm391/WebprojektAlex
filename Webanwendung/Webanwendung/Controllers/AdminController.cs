using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webanwendung.Models.db;
using Webanwendung.Models;

namespace Webanwendung.Controllers
{
    public class AdminController : BasisController
    {
        private IUserRepository userRepository;
        private IBookingRepository bookingRepository;

        // only for testing
        //public ActionResult Index()
        //{
        //    try
        //    {
        //        userRepository = new UserRepositoryDB();
        //        userRepository.Open();
        //        List<User> users = userRepository.GetAllUsers();
        //        return View("Message", new Message("kein Fehler", "kein Fehler"));
        //    }
        //    catch (Exception)
        //    {
        //        return View("Message", new Message("Fehler", "Fehler"));
        //    }   
        //}

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

        [HttpPost]
        public ActionResult DeleteUser(int idToDelete)
        {
            if (IsAdmin())
            {
                try
                {
                    userRepository = new UserRepositoryDB();
                    userRepository.Open();
                    if (userRepository.Delete(idToDelete))
                    {
                        return View("Message", new Message("Löschen", "Erfolgreich gelöscht"));
                    }
                    else
                    {
                        return View("Message", new Message("Löschen", "Löschen nicht erfolgreich"));
                    }
                }
                catch (Exception)
                {
                    return View("Message", new Message("Löschen", "Beim Löschen ist ein Fehler aufgetreten"));
                }
                finally
                {
                    userRepository.Close();
                }
            }
            else
            {
                return View("Message", new Message("URL Fehler", "Die angegebene URL ist ungültig"));
            }
            
        }

        //[HttpPost]
        public ActionResult ShowBookings(int? userId)
        {
            if (IsAdmin() && (userId != null))
            {
                try
                {
                    bookingRepository = new BookingRepositoryDB();
                    bookingRepository.Open();
                    List<Booking> bookings;
                    if(userId == -1)
                    {
                        bookings = bookingRepository.GetAllBookings();
                    }
                    else
                    {
                        TempData["Message"] = "oneUser";
                        bookings = bookingRepository.GetBookingsOneUser(Convert.ToInt32(userId));
                    }
                    return View(bookings);
                }
                catch (Exception)
                {
                    return View("Message", new Message("Alle Buchungen anzeigen", "Beim Anzeigen der Buchungen ist ein Fehler aufgetreten"));
                }
                finally
                {
                    bookingRepository.Close();
                }
            }
            else
            {
                return View("Message", new Message("URL Fehler", "Die angegebene URL ist ungültig"));
            }
        }

        [HttpPost]
        public ActionResult DeleteBooking(int idToDelete)
        {
            if (IsAdmin())
            {
                try
                {
                    bookingRepository = new BookingRepositoryDB();
                    bookingRepository.Open();
                    if (bookingRepository.Delete(idToDelete))
                    {
                        return View("Message", new Message("Buchung Löschen", "Die Buchung wurde erfolgreich gelöscht"));
                    }
                    else
                    {
                        return View("Message", new Message("Buchung Löschen", "Die Buchung konnte nicht gelöscht werden"));
                    }
                }
                catch (Exception)
                {
                    return View("Message", new Message("Buchung Löschen", "Die Buchung konnte nicht gelöscht werden"));
                }
                finally
                {
                    bookingRepository.Close();
                }
            }
            else
            {
                return View("Message", new Message("URL Fehler", "Die angegebene URL ist ungültig"));
            }
        }

        [HttpGet]
        public ActionResult SetPrices()
        {
            if (IsAdmin())
            {
                try
                {
                    bookingRepository = new BookingRepositoryDB();
                    bookingRepository.Open();
                    List<decimal> prices = bookingRepository.GetPrices();
                    return View(prices);
                }
                catch (Exception)
                {
                    return View("Message", new Message("Preise ändern", "Fehler beim Verarbeiten der Daten"));
                }
                finally
                {
                    bookingRepository.Close();
                }
            }
            else
            {
                return View("Message", new Message("URL Fehler", "Die angegebene URL ist ungültig"));
            }
        }
        [HttpPost]
        public ActionResult SetPrices(List<decimal> prices)
        {
            ValidatePrices(prices);
            if (ModelState.IsValid)
            {
                try
                {
                    bookingRepository = new BookingRepositoryDB();
                    bookingRepository.Open();
                    if (bookingRepository.SetPrices(prices))
                    {
                        return View("Message", new Message("Preise ändern", "Die Preise wurden erfolgreich geändert"));
                    }
                    else
                    {
                        return View("Message", new Message("Preise ändern", "Die Preise konnten nicht geändert werden"));
                    }
                }
                catch (Exception)
                {
                    return View("Message", new Message("Preise ändern", "Die Preise konnten nicht geändert werden"));
                }
                finally
                {
                    bookingRepository.Close();
                }
            }
            else
            {
                return View();
            }
            
        }


        private void ValidatePrices(List<decimal> prices)
        {
            for (int i = 0; i <= 2; i++)
            {
                if (prices[i] < 0.0m)
                {
                    ModelState.AddModelError("prices[0]", "Die Preise müssen größer null sein");
                }
            }                   
        }

    }
}