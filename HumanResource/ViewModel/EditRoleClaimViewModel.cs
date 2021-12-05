using HumanResource.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResource.ViewModel
{
    public class EditRoleClaimViewModel
    {
        public EditRoleClaimViewModel()
        {
            Claims = new List<RoleClaim>();
        }
        public string Id { get; set; }
        [Required]
        public string RoleName { get; set; }
        public List<RoleClaim> Claims { get; set; }
    }
}
