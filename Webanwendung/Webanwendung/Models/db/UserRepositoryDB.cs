using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Webanwendung.Models.db
{
    public class UserRepositoryDB : IUserRepository
    {
        private string _connectionString = "Server=localhost;Database=webProject;Uid=root;Pwd=iscoregoals"; // pw isg
        private SqlConnection _connection;

        public void Open()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public bool Insert(User userToInsert)
        {
            throw new NotImplementedException();
        }


    }
}