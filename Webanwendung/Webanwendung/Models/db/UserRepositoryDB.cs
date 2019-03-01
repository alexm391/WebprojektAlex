using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MySql.Data.MySqlClient;


namespace Webanwendung.Models.db
{
    public class UserRepositoryDB : IUserRepository
    {
        private string _connectionString = "Server=localhost;Database=webProject;Uid=root;Pwd=iscoregoals"; 
        private MySqlConnection _connection;

        public void Open()
        {
            if (this._connection == null)
            {
                this._connection = new MySqlConnection(this._connectionString);
            }

            if (this._connection.State != ConnectionState.Open)
            {
                this._connection.Open();
            }
        }

        public void Close()
        {
            if ((this._connection != null) && (this._connection.State == ConnectionState.Open))
            {
                this._connection.Close();
            }
        }

        public bool Insert(User userToInsert)
        {
            if (userToInsert == null)
            {
                return false;
            }

            try
            {
                MySqlCommand cmd = this._connection.CreateCommand();
                cmd.CommandText = "INSERT into users values(null, @firstname, @lastname, @birthdate, @gender, @username, @email, sha1(@pwd), false)";
                cmd.Parameters.AddWithValue("firstname", userToInsert.Firstname);
                cmd.Parameters.AddWithValue("lastname", userToInsert.Lastname);
                cmd.Parameters.AddWithValue("birthdate", userToInsert.Birthdate);
                cmd.Parameters.AddWithValue("gender", userToInsert.Gender);
                cmd.Parameters.AddWithValue("username", userToInsert.Username);
                cmd.Parameters.AddWithValue("email", userToInsert.Email);
                cmd.Parameters.AddWithValue("pwd", userToInsert.Password);

                return cmd.ExecuteNonQuery() == 1;
            }
            catch (Exception ex)
            {
                throw;
            }
        }





    }
}