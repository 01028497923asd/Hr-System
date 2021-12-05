using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResource.ViewModel
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Roles = new List<Role>();
        }
        public string UserId { get; set; }

        [Required]
        public string UserName { get; set; }
        
        [Required]
        [EmailAddress]
        //[Remote(action: "IsEmailInUse", controller: "Users")]
        public string Email { get; set; }
        public List<Role> Roles { get; set; }

        public string RoleId { get; set; }
    }
}
