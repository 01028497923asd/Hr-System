using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResource.ViewModel
{
    public class AllRolesViewModel
    {
        public AllRolesViewModel()
        {
            allRoles = new List<Role>();
        }
        public RegisterViewModel registerViewModel { get; set; }
        public IEnumerable<Role> allRoles { get; set; }
    }
}
