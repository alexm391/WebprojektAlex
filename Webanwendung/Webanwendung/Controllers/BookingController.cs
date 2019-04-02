using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webanwendung.Models.db;
using Webanwendung.Models;

namespace Webanwendung.Controllers
{
    public class BookingController : Controller
    {
        private IBookingRepository bookingRepository;

        // GET: Booking
        // only for testing
        public ActionResult Index()
        {
            try
            {
                DateTime st = new DateTime(2019, 04, 21);
                DateTime en = new DateTime(2019, 04, 27);
                bookingRepository = new BookingRepositoryDB();
                bookingRepository.Open();
                int i = bookingRepository.CheckAvailability(st, en, 1);
                if (i > 0)
                {
                    return View("Message", new Message("frei", "kein Fehler {0}", i.ToString()));
                }
                else
                {
                    return View("Message", new Message("nicht frei", "kein Fehler {0}", i.ToString()));
                }

            }
            catch (Exception ex)
            {
                return View("Message", new Message("Fehler", "Fehler"));
            }
            finally
            {
                bookingRepository.Close();
            }




        }

        [HttpGet]
        public ActionResult Booking()
        {
            Booking b = new Booking();
            return View(b);
        }

        [HttpPost]
        public ActionResult Booking(Booking bookingDataForm)
        {
            if (bookingDataForm == null)
            {
                return RedirectToAction("Booking");
            }

            ValidateData(bookingDataForm);

            if (ModelState.IsValid)
            {
                try
                {
                    bookingRepository = new BookingRepositoryDB();
                    bookingRepository.Open();
                    int roomNr = bookingRepository.CheckAvailability(bookingDataForm.StartDate, bookingDataForm.EndDate, Convert.ToInt32(bookingDataForm.Beds));
                    if (roomNr > 0)
                    {
                        return RedirectToAction("BookinConfirmation");
                    }
                    else
                    {
                        string s = "Zu diesem Zeitpunk ist leider kein " + bookingDataForm.Beds + "-Bett Zimmer frei";
                        if (bookingDataForm.Beds == Beds.eins)
                        {
                            s = "Zu diesem Zeitpunk ist leider kein ein-Bett Zimmer frei";
                        }
                        ModelState.AddModelError("StartDate", s);
                        return View(bookingDataForm);
                    }
                }
                catch (Exception)
                {
                    return View("Message", new Message("Verfügbarkeit prüfen", 
                        "Bei der Verarbeitung ihrer Daten ist ein Fehler aufgetreten", "Versuchen Sie es später nochmal"));
                }
                finally
                {
                    bookingRepository.Close();
                }
            }
            else
            {
                return View(bookingDataForm);
            }
        }

        public ActionResult BookingConfirmation(Booking booking)
        {

        }




        private void ValidateData(Booking booking)
        {
            if((booking.StartDate == null) || (booking.StartDate < DateTime.Today))
            {
                ModelState.AddModelError("StartDate", "Bitte geben Sie ein Datum für den Beginn ihrer Buchung ein");
            }
            if ((booking.EndDate == null) || (booking.StartDate > booking.EndDate))
            {
                ModelState.AddModelError("EndDate", "Bitte geben Sie ein Datum für das Ende ihrer Buchung ein");
            }
            if ((booking.StartDate != null) && (booking.StartDate > new DateTime(2030, 01, 01)))
            {
                ModelState.AddModelError("StartDate", "Sie können nicht so weit im vorhinein buchen");
            }
            if(booking.Beds == Beds.nichtAngegeben)
            {
                ModelState.AddModelError("Beds", "Bitte geben Sie die Betten an");
            }
            
        }

    }
}