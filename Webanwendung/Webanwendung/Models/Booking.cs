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
        private decimal _priceOneBed;
        private decimal _priceTwoBeds;
        private decimal _priceThreeBeds;
        private decimal _priceForStay;
        private int _duration;

        public int ID { get; set; }
        public int IdUser { get; set; }
        public int RoomNr { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        // price + beds
        public Beds Beds { get; set; }
        //public int Beds { get; set; }
        public decimal PriceOneBed
        {
            get { return this._priceOneBed; }
            set
            {
                if(value >= 0.0m)
                {
                    this._priceOneBed = value;
                }
            }
        }
        public decimal PriceTwoBeds
        {
            get { return this._priceTwoBeds; }
            set
            {
                if (value >= 0.0m)
                {
                    this._priceTwoBeds = value;
                }
            }
        }
        public decimal PriceThreeBeds
        {
            get { return this._priceThreeBeds; }
            set
            {
                if (value >= 0.0m)
                {
                    this._priceThreeBeds = value;
                }
            }
        }
        public decimal PriceForStay
        {
            get { return this._priceForStay; }
            set
            {
                if(value > 0m)
                {
                    this._priceForStay = value;

                }
            }
        }
        public int Duration
        {
            get { return this._duration; }
            set
            {
                if (value > 0)
                {
                    this._duration = value;
                }
            }
        }

        public Booking() : this(-1, 0, 0, DateTime.MaxValue, DateTime.MaxValue, 0, 0m, 0m, 0m, 0m, 0) { }
        public Booking(int id, int idUser, int roomNr, DateTime startDate, DateTime endDate, Beds beds, 
            decimal price1, decimal price2, decimal price3, decimal priceForStay, int duration)
        {
            this.ID = id;
            this.IdUser = idUser;
            this.RoomNr = roomNr;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.Beds = beds;
            this.PriceOneBed = price1;
            this.PriceTwoBeds = price2;
            this.PriceThreeBeds = price3;
            this.PriceForStay = priceForStay;
            this.Duration = duration;
        }

    }
}