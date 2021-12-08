using HumanResource.Data;
using HumanResource.Models;
using HumanResource.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResource.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext context;
        public EmployeesController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            var employees = context.Employees.ToList();
            return View(employees);
        }
       
        [HttpGet]
        public IActionResult AddEmployee()
        {
            Employee model = new Employee();
            model.Countries = new SelectList(GetCountries(),"Name","Name");
            model.DateOfBirth = DateTime.Now;
            model.DateOfContract = DateTime.Now;
            
            
            return View(model);
        }

        [HttpPost]
        public IActionResult AddEmployee(Employee employee)
        {
            if (ModelState.IsValid)
            {
                
                context.Employees.Add(employee);
                context.SaveChanges();

                return RedirectToAction("Index", "Employees");
            }
            else
            {
                employee.Countries = new SelectList(GetCountries(), "Name", "Name");
                return View(employee);
            }
            
           
        }
       
        public IEnumerable<Countries> GetCountries()
        {
            return CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                 .Select(x => new Countries
                 {
                     Id = new RegionInfo(x.LCID).Name,
                     Name = new RegionInfo(x.LCID).EnglishName
                 }).GroupBy(c => c.Id)
                   .Select(c => c.First())
                   .OrderBy(x => x.Name);
        }

    }
}
