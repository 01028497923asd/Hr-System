using HumanResource.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResource.Models
{
    public enum EmployeeType
    {
        Male,
        Female
    }
    public class Employee
    {
        
        [Required]
        [RegularExpression(@"[0-9]{14}", ErrorMessage = "Invalid National ID Number")]
        [Key]
        public string SSN { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "Min charachter is 10")]
        public string Name { get; set; }

        [Required]
        [MinLength(5,ErrorMessage ="Min Length is 5 charachter")]
        public string Address { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"[0-9]{11}", ErrorMessage = "Phone Number must be 11 Integer")]
        public string PhoneNumber { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Date Of Birth")]
        
        [Required]
        [Minimum20Years]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Employee Type")]
        public EmployeeType EmployeeType { get; set; }

        
        [Display(Name = "Date Of Contract")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
        //       ApplyFormatInEditMode = true)]
        [Min2008Contract]
        [Required]
        public DateTime DateOfContract { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Start Time")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        [StartTimes]
        public TimeSpan StartTime { get; set; }

        [Required]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        [DataType(DataType.Time)]
        [Display(Name = "End Time")]
        [EndTimes]
        public TimeSpan EndTime { get; set; }

        [Required]
        [Range(1000, float.MaxValue, ErrorMessage = "Cannot Contain Invalid Integer")]
        public decimal Salary { get; set; }

        public string Nationality { get; set; }

        [NotMapped]
        public SelectList Countries { get; set; }

    }
}
