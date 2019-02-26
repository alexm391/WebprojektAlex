using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webanwendung.Models
{
    public enum Gender
    {
        notSpecified, male, female
    }
    public enum UserRole
    {
        noUser, admin, registeredUser
    }

    public class User
    {
        public int ID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime? Birthdate { get; set; }
        public Gender Gender { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole UserRole { get; set; }

        public User() : this(-1, "", "", null, Gender.notSpecified, "", "", "", UserRole.noUser) { }
        public User(int id, string firstname, string lastname, DateTime? birthdate, Gender gender, string username, string email, 
            string password, UserRole userRole)
        {
            this.ID = id;
            this.Firstname = firstname;
            this.Lastname = lastname;
            this.Birthdate = birthdate;
            this.Gender = Gender;
            this.Username = username;
            this.Email = email;
            this.Password = password;
            this.UserRole = userRole;
        }

    }
}