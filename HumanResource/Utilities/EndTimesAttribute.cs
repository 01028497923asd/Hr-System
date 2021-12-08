using HumanResource.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResource.Utilities
{
    public class EndTimesAttribute :ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var employee = (Employee)validationContext.ObjectInstance;
            var startTime = employee.StartTime.Hours;
                var endTime = employee.EndTime.Hours;
            if(endTime == 0)
            {
                return new ValidationResult("End Time Cannot be Zero ");
            }
            return (endTime > startTime) ? ValidationResult.Success : new ValidationResult("End Time Can not be Less than Star time");
        }
    }
}
