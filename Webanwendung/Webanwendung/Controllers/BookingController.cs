﻿using System;
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
            Session["roomNr"] = null;

            if ((Session["isRegisteredUser"] != null) && (Convert.ToBoolean(Session["isRegisteredUser"]) == true))
            {
                Booking b = new Booking();
                return View(b);
            }
            else
            {
                TempData["Message"] = "login";
                return View("Message", new Message("Zimmer buchen", "Bitte melden Sie sich zuerst an um ein Zimmer zu buchen"));
                //TempData["Message"] = "Sie müssen sich anmelden um ein Zimmer zu buchen";
                //return RedirectToAction("Login", "User");      
            }
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
                    Session["roomNr"] = roomNr;
                    if (roomNr > 0)
                    {
                        return RedirectToAction("BookingConfirmation", bookingDataForm);
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

        [HttpGet]
        public ActionResult BookingConfirmation(Booking booking)
        {
            if((Session["roomNr"] != null) && (Convert.ToInt32(Session["roomNr"]) > 0))
            {
                Session["booking"] = booking;
                return View(booking);
            }
            else
            {
                return View("Message", new Message("URL Fehler", "Die angegebene URL ist ungültig"));
            }
        }

        [HttpPost]
        public ActionResult BookingConfirmation()
        {
            try
            {
                //Session["roomNr"] = null;
                Booking booking = Session["booking"] as Booking;
                booking.IdUser = Convert.ToInt32(Session["id"]);
                booking.RoomNr = Convert.ToInt32(Session["roomNr"]);
                Session["roomNr"] = null;
                bookingRepository = new BookingRepositoryDB();
                bookingRepository.Open();
                if (bookingRepository.Insert(booking))
                {
                    return View("Message", new Message("Buchung", "Die Buchung war erfolgreich, Sie bekommen in kürze ein email als Bestätigung"));
                }
                else
                {
                    return View("Message", new Message("Buchung", "Es ist ein Fehler bei ihrer Buchung aufgetreten", "Versuchen Sie es später nocheinmal"));
                }
            }
            catch (Exception ex)
            {
                return View("Message", new Message("Buchung", "Es ist ein Fehler bei ihrer Buchung aufgetreten"));
            }
            finally
            {
                bookingRepository.Close();
            }


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