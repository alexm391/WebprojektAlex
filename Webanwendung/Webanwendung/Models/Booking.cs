using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webanwendung.Models
{
    public class Booking
    {
        int ID { get; set; }
        int IdUser { get; set; }
        int RoomNr { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
        // price

        public Booking() : this(-1, 0, 0, DateTime.MaxValue, DateTime.MaxValue) { }
        public Booking(int id, int idUser, int roomNr, DateTime startDate, DateTime endDate)
        {
            this.ID = id;
            this.IdUser = idUser;
            this.RoomNr = roomNr;
            this.StartDate = startDate;
            this.EndDate = endDate;
        }

    }
}