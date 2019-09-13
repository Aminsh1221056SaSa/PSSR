
using PSSR.UserSecurity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.UserSecurity.Configuration
{
    public class UserModel
    {
        public UserModel()
        {
            Roles = new List<Role>();
            Persons = new List<Person>();
        }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string PasswordHinit { get; set; }
        [Required]
        public string RoleName { get; set; }
        public int PersonId { get; set; }
        public List<Role> Roles { get;  set; }
        public List<Person> Persons { get;  set; }
    }

    public class Person
    {
        public int Id { get; set; }
        public string NationalId { get; set; }
        public string Name { get; set; }
    }
}
