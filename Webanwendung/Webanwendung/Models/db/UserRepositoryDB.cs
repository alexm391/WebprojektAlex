using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MySql.Data.MySqlClient;


namespace Webanwendung.Models.db
{
    public class UserRepositoryDB : RepositoryDB, IUserRepository
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
                cmd.Parameters.AddWithValue("pwd", userToInsert.NewPassword);

                return cmd.ExecuteNonQuery() == 1;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public User Authenticate(Login login)
        {
            if((login.UsernameOrEmail == "") || (login.Password == ""))
            {
                return null;
            }

            User user;
        
            try
            {
                MySqlCommand cmd = this._connection.CreateCommand();
                cmd.CommandText = "select * from users where ((passwrd = sha1(@password)) AND (username = @emailOrUsername) OR ((passwrd = sha1(@password)) AND (email = @emailOrUsername)))";
                cmd.Parameters.AddWithValue("emailOrUsername", login.UsernameOrEmail);
                cmd.Parameters.AddWithValue("password", login.Password);

                using(MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        user = new User
                        {
                            ID = Convert.ToInt32(reader["id"]),
                            Firstname = Convert.ToString(reader["firstname"]),
                            Lastname = Convert.ToString(reader["lastname"]),
                            Birthdate = Convert.ToDateTime(reader["birthdate"]),
                            Gender = (Gender)Convert.ToInt32(reader["gender"]),
                            Username = Convert.ToString(reader["username"]),
                            Email = Convert.ToString(reader["email"]),
                        };
                        if (Convert.ToBoolean(reader["isAdmin"]))
                        {
                            user.UserRole = UserRole.admin;
                        }
                        else
                        {
                            user.UserRole = UserRole.registeredUser;
                        }
                        return user;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool ChangeUserData(int userIdToChange, User newUserData)
        {
            if((userIdToChange == 0) || (newUserData == null))
            {
                return false;
            }

            try
            {
                MySqlCommand cmd = this._connection.CreateCommand();
                cmd.CommandText = "UPDATE users SET firstname = @firstname, lastname = @lastname, birthdate = @birthdate, gender = @gender, username = @username, email = @email where id = @id";
                cmd.Parameters.AddWithValue("firstname", newUserData.Firstname);
                cmd.Parameters.AddWithValue("lastname", newUserData.Lastname);
                cmd.Parameters.AddWithValue("birthdate", newUserData.Birthdate);
                cmd.Parameters.AddWithValue("gender", newUserData.Gender);
                cmd.Parameters.AddWithValue("username", newUserData.Username);
                cmd.Parameters.AddWithValue("email", newUserData.Email);
                cmd.Parameters.AddWithValue("id", userIdToChange);

                return cmd.ExecuteNonQuery() == 1;

            }
            catch (Exception ex)
            {
                throw;
            }


        }

        public bool ChangePassword(int userIdToChange, User user)
        {
            if((userIdToChange == 0) || (user.NewPassword == ""))
            {
                return false;
            }

            try
            {
                MySqlCommand cmd = this._connection.CreateCommand();
                cmd.CommandText = "UPDATE users SET passwrd = sha1(@newPassword) where (id = @id) and (passwrd = sha1(@oldPassword))";
                cmd.Parameters.AddWithValue("newPassword", user.NewPassword);
                cmd.Parameters.AddWithValue("oldPassword", user.OldPassword);
                cmd.Parameters.AddWithValue("id", userIdToChange);

                return cmd.ExecuteNonQuery() == 1;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public User GetUser(int userIdToGet)
        {
            if(userIdToGet == 0)
            {
                return null;
            }

            User user;

            try
            {
                MySqlCommand cmd = this._connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM users where id = @id";
                cmd.Parameters.AddWithValue("id", userIdToGet);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        user = new User
                        {
                            ID = Convert.ToInt32(reader["id"]),
                            Firstname = Convert.ToString(reader["firstname"]),
                            Lastname = Convert.ToString(reader["lastname"]),
                            Birthdate = Convert.ToDateTime(reader["birthdate"]),
                            Gender = (Gender)Convert.ToInt32(reader["gender"]),
                            Username = Convert.ToString(reader["username"]),
                            Email = Convert.ToString(reader["email"]),
                        };
                        return user;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            int i = 0;

            try
            {
                MySqlCommand cmd = this._connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM users";

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                ID = Convert.ToInt32(reader["id"]),
                                Firstname = Convert.ToString(reader["firstname"]),
                                Lastname = Convert.ToString(reader["lastname"]),
                                Birthdate = Convert.ToDateTime(reader["birthdate"]),
                                Gender = (Gender)Convert.ToInt32(reader["gender"]),
                                Username = Convert.ToString(reader["username"]),
                                Email = Convert.ToString(reader["email"]),
                            }
                            );
                            if (Convert.ToBoolean(reader["isAdmin"]))
                            {
                                users[i].UserRole = UserRole.admin;
                            }
                            else
                            {
                                users[i].UserRole = UserRole.registeredUser;
                            }
                            i++;
                        }
                        return users;
                    }
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