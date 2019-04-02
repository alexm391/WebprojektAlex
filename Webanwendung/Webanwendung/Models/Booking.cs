using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webanwendung.Models
{
    public enum Beds
    {
        nichtAngegeben, eins, zwei, drei
    }

    public class Booking
    {
        private decimal _price1;
        private decimal _price2;
        private decimal _price3;

        public int ID { get; set; }
        public int IdUser { get; set; }
        public int RoomNr { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        // price + beds
        public Beds Beds { get; set; }
        //public int Beds { get; set; }
        public decimal Price1
        {
            get { return this._price1; }
            set
            {
                if(value >= 0.0m)
                {
                    this._price1 = value;
                }
            }
        }
        public decimal Price2
        {
            get { return this._price2; }
            set
            {
                if (value >= 0.0m)
                {
                    this._price2 = value;
                }
            }
        }
        public decimal Price3
        {
            get { return this._price3; }
            set
            {
                if (value >= 0.0m)
                {
                    this._price3 = value;
                }
            }
        }

        public Booking() : this(-1, 0, 0, DateTime.MaxValue, DateTime.MaxValue, 0, 0m, 0m, 0m) { }
        public Booking(int id, int idUser, int roomNr, DateTime startDate, DateTime endDate, Beds beds, decimal price1, decimal price2, decimal price3)
        {
            this.ID = id;
            this.IdUser = idUser;
            this.RoomNr = roomNr;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.Beds = beds;
            this.Price1 = price1;
            this.Price2 = price2;
            this.Price3 = price3;
        }

    }
}