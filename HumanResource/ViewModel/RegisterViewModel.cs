using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResource.ViewModel
{
    public class RegisterViewModel
    {
        

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        //[Remote(action: "IsEmailInUse" , controller:"Users")]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="Confirm Password")]
        [Compare("Password" , ErrorMessage ="Password and Confirmation Password Not Match")]
        public string ConfirmPassword { get; set; }
        
        [ForeignKey("RoleId")]
        public string RoleId { get; set; }
        public Role Role { get; set; }
    }
}
