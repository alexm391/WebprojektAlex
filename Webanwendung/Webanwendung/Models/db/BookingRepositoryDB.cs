using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MySql.Data.MySqlClient;

namespace Webanwendung.Models.db
{
    public class BookingRepositoryDB : RepositoryDB, IBookingRepository 
    {
        //private string _connectionString = "Server=localhost;Database=webProject;Uid=root;Pwd=iscoregoals";
        //private MySqlConnection _connection;

        //public void Open()
        //{
        //    if (this._connection == null)
        //    {
        //        this._connection = new MySqlConnection(this._connectionString);
        //    }

        //    if (this._connection.State != ConnectionState.Open)
        //    {
        //        this._connection.Open();
        //    }
        //}

        //public void Close()
        //{
        //    if ((this._connection != null) && (this._connection.State == ConnectionState.Open))
        //    {
        //        this._connection.Close();
        //    }
        //}

        public int CheckAvailability(DateTime startDate, DateTime endDate, int beds)
        {
            if((startDate == null) || (endDate == null) || (beds == 0))
            {
                return -1;
            }

            List<int> unavailableRooms = new List<int>();

            try
            {
                List<int> allRooms = GetAllRooms(beds);

                MySqlCommand cmd = this._connection.CreateCommand();
                cmd.CommandText = "select b.roomNr from bookings b inner join rooms r on b.roomNr = r.roomNr where " +
                    "r.beds = @beds and ((@startDate >= b.startDate) and (@startDate <= b.endDate) or (@endDate >= b.startDate) " +
                    "and (@endDate <= b.endDate) or (@startDate <= b.startDate) and (@endDate >= b.endDate))";                
                //cmd.CommandText = "select b.roomNr from bookings b inner join rooms r on b.roomNr = r.roomNr where r.beds = @beds and ((@startDate < b.startDate) and (@endDate > b.startDate) or (@startDate > b.startDate) and (@endDate < b.endDate))";
                cmd.Parameters.AddWithValue("beds", beds);
                cmd.Parameters.AddWithValue("startDate", startDate);
                cmd.Parameters.AddWithValue("endDate", endDate);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {                        
                            {
                                unavailableRooms.Add(Convert.ToInt32(reader["roomNr"]));
                            };
                        }
                        //reader.Close();

                        foreach(int a in allRooms)
                        {
                            if (!unavailableRooms.Contains(a))
                            {
                                return a;
                            }
                        }                   
                        return -1;
                    }
                    return allRooms != null ? allRooms[0] : -1;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool Insert(Booking bookingToInsert)
        {
            if ((bookingToInsert == null) || (bookingToInsert.IdUser < 0) || (bookingToInsert.RoomNr < 0))
            {
                return false;
            }

            try
            {
                MySqlCommand cmd = this._connection.CreateCommand();
                cmd.CommandText = "INSERT into bookings values(null, @idUser, @roomNr, @startDate, @endDate, @price)";
                cmd.Parameters.AddWithValue("idUser", bookingToInsert.IdUser);
                cmd.Parameters.AddWithValue("roomNr", bookingToInsert.RoomNr);
                cmd.Parameters.AddWithValue("startDate", bookingToInsert.StartDate);
                cmd.Parameters.AddWithValue("endDate", bookingToInsert.EndDate);
                cmd.Parameters.AddWithValue("price", bookingToInsert.PriceForStay);

                return cmd.ExecuteNonQuery() == 1;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<int> GetPrice()
        {
            List<int> prices = new List<int>();
            try
            {
                for (int i = 1; i <= 3; i++)
                {
                    MySqlCommand cmd = this._connection.CreateCommand();
                    cmd.CommandText = "select price from rooms where beds = @beds";
                    cmd.Parameters.AddWithValue("beds", i);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            prices.Add(Convert.ToInt32(reader["price"]));                                                     
                        }
                    }
                }
                return prices;

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private List<int> GetAllRooms(int beds)
        {
            if (beds == 0)
            {
                return null;
            }

            List<int> rooms = new List<int>();
            try
            {
                MySqlCommand cmd = this._connection.CreateCommand();
                cmd.CommandText = "select roomNr from rooms where beds = @beds";
                cmd.Parameters.AddWithValue("beds", beds);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            {
                                rooms.Add(Convert.ToInt32(reader["roomNr"]));
                            };
                        }
                        reader.Close();
                        return rooms;
                    }
                    reader.Close();
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}