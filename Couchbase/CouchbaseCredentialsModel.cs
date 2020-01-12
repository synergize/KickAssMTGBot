using System;
using System.Collections.Generic;
using System.Text;

namespace Couchbase
{
   public class CouchbaseCredentialsModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public CouchbaseCredentialsModel(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
