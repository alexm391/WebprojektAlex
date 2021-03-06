﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webanwendung.Models.db
{
    interface IUserRepository
    {
        void Open();
        void Close();
        bool Insert(User userToInsert);
        User Authenticate(Login login);
        bool ChangeUserData(int userIdToChange, User newUserData);
        bool ChangePassword(int userIdToChange, User user);
        User GetUser(int userIdToGet);
        List<User> GetAllUsers();
        bool Delete(int idToDelete);

    }
}
