using HumanResource.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HumanResource.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
       
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<IdentityUser>().Ignore()
        //                                       .Ignore(c => c.LockoutEnabled)
                                               
        //                                       .Ignore(c => c.Roles)
        //                                       .Ignore(c => c.TwoFactorEnabled);//and so on...

        //    modelBuilder.Entity<IdentityUser>().ToTable("Users");//to change the name of table.

        //}
    }
}
