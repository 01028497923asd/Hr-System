using HumanResource.Models;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResource.Utilities
{
    public class Minimum20YearsAttribute :ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //var _localizationService = (IStringLocalizer)validationContext.GetService(typeof(IStringLocalizer));

            //var localizedError = _localizationService[ErrorMessage];

            var employee = (Employee)validationContext.ObjectInstance;
            var age = DateTime.Today.Year - employee.DateOfBirth.Year;
            Console.WriteLine(age);

            return (age >= 20) ? ValidationResult.Success : new ValidationResult("Age must be more than or equal 20 years");


        }
    }
}
