using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EHWM.Models {
    public class AuthenticatedUser {
        [PrimaryKey,AutoIncrement]
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public enum Roles { Admin, Sales};
        public Roles Role = Roles.Admin;
    }
}
