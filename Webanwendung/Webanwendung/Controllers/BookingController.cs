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

        //[HttpGet]
        //public ActionResult Booking()
        //{
        //    Booking b = new Booking();
        //    return View(b);
        //}

        //[HttpPost]
        //public ActionResult Booking(Booking booking)
        //{

        //}

        //[HttpPost]
        //public ActionResult Booking(Booking booking, string s)
        //{

        //}

    }
}