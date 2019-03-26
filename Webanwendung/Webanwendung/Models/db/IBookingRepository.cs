using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webanwendung.Models.db
{
    interface IBookingRepository
    {
        void Open();
        void Close();
        bool Insert(Booking bookingToInsert);
        // bool Delete();
        bool CheckAvailability(DateTime startDate, DateTime endDate, int beds);

    }
}
