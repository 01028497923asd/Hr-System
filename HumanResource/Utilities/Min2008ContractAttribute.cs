using HumanResource.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResource.Utilities
{
    public class Min2008ContractAttribute :ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var employee = (Employee)validationContext.ObjectInstance;
            var year = employee.DateOfContract.Year;
            var existYear = DateTime.Today.Year;
            return (year >= 2008 && year <= existYear) ? ValidationResult.Success : new ValidationResult("Contract Year must be between 2008 and Time of Now");

        }
    }
}
