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
        int CheckAvailability(DateTime startDate, DateTime endDate, int beds);
        List<int> GetPrices();
        List<Booking> GetBookings(int userId);
        bool Delete(int idToDelete);
        List<Booking> GetAllBookings();

    }
}
