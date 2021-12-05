using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HumanResource.Models
{
    public class ClaimsStore
    {
        public static List<Claim> AllClaims = new List<Claim>()
        {
            new Claim("Employee" , "Employee"),
            new Claim("GeneralSetting" , "GeneralSetting"),
            new Claim("Permissions" , "Permissions"),
            new Claim("AddNewGroup" , "AddNewGroup"),
            new Claim("Attendance" , "Attendance"),
            new Claim("Salary" , "Salary"),
            new Claim("Users" , "Users")
            //new Claim("Show Employee" , "Show Employee"),
            //new Claim("Add Employee" , "Add Employee"),
            //new Claim("Edit Employee" , "Edit Employee"),
            //new Claim("Delete Employee" , "Delete Employee"),

            //new Claim("Show GeneralSetting" , "Show GeneralSetting"),
            //new Claim("Add GeneralSetting" , "Add GeneralSetting"),
            //new Claim("Edit GeneralSetting" , "Edit GeneralSetting"),
            //new Claim("Delete GeneralSetting" , "Delete GeneralSetting"),

            //new Claim("Show Permissions" , "Show Permissions"),
            //new Claim("Add Permissions" , "Add Permissions"),
            //new Claim("Edit Permissions" , "Edit Permissions"),
            //new Claim("Delete Permissions" , "Delete Permissions"),

            //new Claim("Show AddNewGroup" , "Show AddNewGroup"),
            //new Claim("Add AddNewGroup" , "Add AddNewGroup"),
            //new Claim("Edit AddNewGroup" , "Edit AddNewGroup"),
            //new Claim("Delete AddNewGroup" , "Delete AddNewGroup"),

            //new Claim("Show Attendance" , "Show Attendance"),
            //new Claim("Add Attendance" , "Add Attendance"),
            //new Claim("Edit Attendance" , "Edit Attendance"),
            //new Claim("Delete Attendance" , "Delete Attendance"),

            //new Claim("Show Salary" , "Show Salary"),
            //new Claim("Add Salary" , "Add Salary"),
            //new Claim("Edit Salary" , "Edit Salary"),
            //new Claim("Delete Salary" , "Delete Salary"),

            //new Claim("Show Users" , "Show Users"),
            //new Claim("Add Users" , "Add Users"),
            //new Claim("Edit Users" , "Edit Users"),
            //new Claim("Delete Users" , "Delete Users")



        };
    }
}
