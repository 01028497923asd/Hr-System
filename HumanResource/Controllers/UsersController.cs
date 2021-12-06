using HumanResource.Models;
using HumanResource.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HumanResource.Controllers
{
    //[Authorize(Roles="Admin,Super Admin")]
   [Authorize]
    public class UsersController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public UsersController(RoleManager<IdentityRole> roleManager,
                             UserManager<ApplicationUser> userManager,
                             SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }
        [Authorize(Policy = "UsersShow")]
        public async Task<IActionResult> IndexAsync()
        {
            List<ListUserRolesViewModel> model = new List<ListUserRolesViewModel>();

            var users = userManager.Users.ToList();
            var role = roleManager.Roles.ToList();
            for (int i=0; i < users.Count; i++)
            {
               for(int j = 0; j < role.Count; j++)
                {
                    if(await userManager.IsInRoleAsync(users[i] , role[j].Name))
                    {
                        ListUserRolesViewModel test = new ListUserRolesViewModel
                        {
                            RoleName = role[j].Name,
                            UserId = users[i].Id,
                            UserName = users[i].UserName
                        };
                        model.Add(test);

                    }
                }
            }
            return View(model);
        }
       
        [HttpGet]
        [Authorize(Policy = "PermissionsShow")]
        public IActionResult Permissions()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }
        
        [HttpGet]
        [Authorize(Policy = "PermissionsAdd")]
        public IActionResult AddNewGroup()
        {
            var model = new RoleClaimViewModel
            {
                RoleName = ""
            };
            foreach (Claim claim in ClaimsStore.AllClaims)
            {
                RoleClaim roleClaim = new RoleClaim
                {
                    CliamType = claim.Type,
                    Add = false,
                    Edit = false,
                    Delete = false,
                    Show = true
                };
                model.Claims.Add(roleClaim);
            }
            return View(model);
        }
       
        [HttpPost]
        [Authorize(Policy = "PermissionsAdd")]
        public async Task<IActionResult> AddNewGroup(RoleClaimViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole()
                {
                    Name = model.RoleName
                };
                IdentityResult result = await roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    var role = await roleManager.FindByNameAsync(model.RoleName);
                    var claims = await roleManager.GetClaimsAsync(role);

                    for (int i = 0; i < model.Claims.Count; i++)
                    {
                        var testresult = await roleManager.AddClaimAsync(role, new Claim(model.Claims[i].CliamType + "Show", model.Claims[i].Show ? "true" : "false"));
                        testresult = await roleManager.AddClaimAsync(role, new Claim(model.Claims[i].CliamType + "Add", model.Claims[i].Add ? "true" : "false"));
                        testresult = await roleManager.AddClaimAsync(role, new Claim(model.Claims[i].CliamType + "Edit", model.Claims[i].Edit ? "true" : "false"));
                        testresult = await roleManager.AddClaimAsync(role, new Claim(model.Claims[i].CliamType + "Delete", model.Claims[i].Delete ? "true" : "false"));
                    }
                    //if (testresult.Succeeded)
                    //{
                    //    return RedirectToAction("Permissions", "Users");
                    //}
                    return RedirectToAction("Permissions", "Users");
                }
                else
                {
                    ModelState.AddModelError("", "Cannot Add Role");
                    return View(model);
                }

            }
            else
            {
                var models = new RoleClaimViewModel
                {
                    RoleName = ""
                };
                foreach (Claim claim in ClaimsStore.AllClaims)
                {
                    RoleClaim roleClaim = new RoleClaim
                    {
                        CliamType = claim.Type,
                        Add = false,
                        Edit = false,
                        Delete = false,
                        Show = false
                    };
                    models.Claims.Add(roleClaim);
                }
                return View(models);
            }
        }
       
        [HttpPost]
        [Authorize(Policy = "PermissionsDelete")]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role With Id{roleId} cannot be found";
                return View("NotFound");
            }

            try
            {
                var users =  userManager.Users.ToList();
                for(int i = 0; i < users.Count; i++)
                {
                    if(await userManager.IsInRoleAsync(users[i], role.Name))
                    {
                        ViewBag.ErrorTitle = "Cannot Delete Role";
                        ViewBag.ErrorMessage = $"There is {users[i].UserName} in Use";
                        return View("Error");
                    }
                }

                var result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Permissions", "Users");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);

                }
                return View("Permissions");

            }
            catch (Exception ex)
            {
                ViewBag.ErrorTitle = $"{role.Name} is in use";
                
                ViewBag.ErrorMessage = $"{role.Name} {ex.Message} role cannot be deleted  as there are users us this role" +
                    $"if you want to delete this role Remove users from this role";
                return View("Error");
            }


        }
       
        [HttpGet]
        [Authorize(Policy = "PermissionsEdit")]
        public async Task<IActionResult> EditRole(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role With Id{roleId} cannot be found";
                return View("NotFound");
            }
            try
            {
                var existingRoleClaims = await roleManager.GetClaimsAsync(role);
                if (existingRoleClaims == null)
                {
                    ViewBag.ErrorMessage = $"Claims  cannot be found";
                    return View("NotFound");
                }

                EditRoleClaimViewModel model = new EditRoleClaimViewModel
                {
                    RoleName = role.Name,
                    Id = role.Id
                };
                foreach(Claim claim in ClaimsStore.AllClaims)
                {
                    RoleClaim roleClaim = new RoleClaim
                    {
                        CliamType = claim.Type
                    };
                    // check show 
                    if (existingRoleClaims.Any(c => c.Type == claim.Type + "Show" && c.Value == "true"))
                    {
                        
                        roleClaim.Show = true;
                    }
                    else
                    {
                        roleClaim.Show = false;
                    }
                    // check Add
                    if (existingRoleClaims.Any(c => c.Type == claim.Type + "Add" && c.Value == "true"))
                    {

                        roleClaim.Add = true;
                    }
                    else
                    {
                        roleClaim.Add = false;
                    }
                    // check Edit
                    if (existingRoleClaims.Any(c => c.Type == claim.Type + "Edit" && c.Value == "true"))
                    {

                        roleClaim.Edit = true;
                    }
                    else
                    {
                        roleClaim.Edit = false;
                    }
                    // check Delete
                    if (existingRoleClaims.Any(c => c.Type == claim.Type + "Delete" && c.Value == "true"))
                    {

                        roleClaim.Delete = true;
                    }
                    else
                    {
                        roleClaim.Delete = false;
                    }
                    model.Claims.Add(roleClaim);
                }

                return View(model);

            }
            catch(Exception ex)
            {
                ViewBag.ErrorTitle = $"{role.Name} is in use";
                ViewBag.ErrorMessage = $"{role.Name} role cannot be deleted  as there are users us this role" +
                    $"if you want to delete this role Remove users from this role";
                return View("Error");
            }
            
        }

        [HttpPost]
        [Authorize(Policy = "PermissionsEdit")]
        public async Task<IActionResult> EditRole(EditRoleClaimViewModel model , string roleid)
        {
            var role = await roleManager.FindByIdAsync(roleid);
            if(role == null)
            {
                ViewBag.ErrorMessage = $"Claims  cannot be found";
                return View("NotFound");
            }
            var existClaims = await roleManager.GetClaimsAsync(role);
            role.Name = model.RoleName;
            var result = await roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                for (int i = 0; i < existClaims.Count; i++)
                {
                    var testresult = await roleManager.RemoveClaimAsync(role, new Claim(existClaims[i].Type , existClaims[i].Value));
                }
                for (int i = 0; i < model.Claims.Count; i++)
                {
                    await roleManager.AddClaimAsync(role, new Claim(model.Claims[i].CliamType + "Show", model.Claims[i].Show ? "true" : "false"));
                    await roleManager.AddClaimAsync(role, new Claim(model.Claims[i].CliamType + "Add", model.Claims[i].Add ? "true" : "false"));
                    await roleManager.AddClaimAsync(role, new Claim(model.Claims[i].CliamType + "Edit", model.Claims[i].Edit ? "true" : "false"));
                    await roleManager.AddClaimAsync(role, new Claim(model.Claims[i].CliamType + "Delete", model.Claims[i].Delete ? "true" : "false"));
                }
                var users = userManager.Users.ToList();
                for(int i = 0; i < users.Count; i++)
                {
                    if(await userManager.IsInRoleAsync(users[i] , role.Name))
                    {
                        var claims = await userManager.GetClaimsAsync(users[i]);
                        for (int k = 0; k < claims.Count; k++)
                        {
                            await userManager.RemoveClaimAsync(users[i], new Claim(claims[k].Type, claims[k].Value));
                        }
                        var AddClaims = await roleManager.GetClaimsAsync(role);
                        for (int k = 0; k < AddClaims.Count; k++)
                        {
                            await userManager.AddClaimAsync(users[i], new Claim(AddClaims[k].Type, AddClaims[k].Value));
                        }
                        //testresult = await roleManager.RemoveClaimAsync(role, new Claim(existClaims[i].Type + "", existClaims[i].Value));

                    }
                }
                
                
                return RedirectToAction("Permissions");
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Policy = "UsersAdd")]
        public IActionResult AddUser() 
        {
            var roles = roleManager.Roles.ToList();

            List<Role> allroles = new List<Role>();
            for(int i = 0; i < roles.Count(); i++)
            {
                Role role = new Role
                {
                    Id = roles[i].Id,
                    Name = roles[i].Name
                };
                allroles.Add(role);
            }
            AllRolesViewModel model = new AllRolesViewModel
            {
                allRoles = allroles
            };
            return View( model);
        }
        
        [HttpPost]
        [Authorize(Policy = "UsersAdd")]
        public async Task<IActionResult> AddUser(AllRolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.registerViewModel.UserName,
                    Email = model.registerViewModel.Email,
                    Name = model.registerViewModel.UserName
                };
                var resuult = await userManager.CreateAsync(user, model.registerViewModel.Password);
                if (resuult.Succeeded)
                {
                    var role = await roleManager.FindByIdAsync(model.registerViewModel.RoleId);
                    if(role == null)
                    {
                        ModelState.AddModelError("", "Role is not exist");
                        return View( model);
                    }
                     await userManager.AddToRoleAsync(user, role.Name);
                    var claims = await roleManager.GetClaimsAsync(role);
                    for (int i = 0; i < claims.Count; i++)
                    {
                        await userManager.AddClaimAsync(user, new Claim(claims[i].Type, claims[i].Value));
                    }
                    return RedirectToAction("Index");
                }
                
                foreach(var error in resuult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            var roles = roleManager.Roles.ToList();

            List<Role> allroles = new List<Role>();
            for (int i = 0; i < roles.Count(); i++)
            {
                Role role = new Role
                {
                    Id = roles[i].Id,
                    Name = roles[i].Name
                };
                allroles.Add(role);
            }
            AllRolesViewModel models = new AllRolesViewModel
            {
                allRoles = allroles,
                registerViewModel = model.registerViewModel
            };

            return View( models);
        }
        
        [HttpGet]
        [Authorize(Policy = "UsersEdit")]
        public async Task<IActionResult> EditUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            
            if(user == null)
            {
                ModelState.AddModelError("", "User Not Exist");
                return View("NotFound");
            }
            var roles = roleManager.Roles.ToList();
            var roleId = "";
            List<Role> allroles = new List<Role>();
            for (int i = 0; i < roles.Count(); i++)
            {
                Role role = new Role
                {
                    Id = roles[i].Id,
                    Name = roles[i].Name
                };
                allroles.Add(role);
                if(await userManager.IsInRoleAsync(user , roles[i].Name))
                {
                    roleId = roles[i].Id;
                }
            }
            EditUserViewModel model = new EditUserViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                UserId = user.Id,
                RoleId = roleId,
                Roles =allroles
               
            };

            return View(model);

        }
        
        [HttpPost]
        [Authorize(Policy = "UsersEdit")]
        public async Task<IActionResult> EditUser(EditUserViewModel model , string UserId)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    return View("NotFound");
                }
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.Name = model.UserName;
                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    var role = await roleManager.FindByIdAsync(model.RoleId);
                    if (await userManager.IsInRoleAsync(user , role.Name))
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        var roles = roleManager.Roles.ToList();
                        for (int i = 0; i < roles.Count; i++)
                        {
                            if (await userManager.IsInRoleAsync(user, roles[i].Name))
                            {
                                 var claims = await roleManager.GetClaimsAsync(roles[i]);
                                for (int k = 0; k < claims.Count; k++)
                                {
                                    await userManager.RemoveClaimAsync(user, new Claim(claims[k].Type, claims[k].Value));
                                }
                                await userManager.RemoveFromRoleAsync(user, roles[i].Name);

                            }
                        }

                        result = await userManager.AddToRoleAsync(user, role.Name);
                        var claimstouser = await roleManager.GetClaimsAsync(role);
                        for (int k = 0; k < claimstouser.Count; k++)
                        {
                            await userManager.AddClaimAsync(user, new Claim(claimstouser[k].Type, claimstouser[k].Value));
                        }
                        return RedirectToAction("Index");

                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            
            var roleList = roleManager.Roles.ToList();
            List<Role> allroles = new List<Role>();
            for (int i = 0; i < roleList.Count(); i++)
            {
                Role rolee = new Role
                {
                    Id = roleList[i].Id,
                    Name = roleList[i].Name
                };
                allroles.Add(rolee);

            }
            EditUserViewModel models = new EditUserViewModel
            {
                UserName = model.UserName,
                Email = model.Email,
                UserId = model.UserId,
                RoleId = model.RoleId,
                Roles = allroles

            };

            return View(models);

            
  
        }
        
        [HttpPost]
        [Authorize(Policy = "UsersDelete")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if(user == null)
            {
                ViewBag.ErrorMessage = "User Not Found";
                return View("NotFound");
            }
            var claims = await userManager.GetClaimsAsync(user);
            for (int k = 0; k < claims.Count; k++)
            {
                await userManager.RemoveClaimAsync(user, new Claim(claims[k].Type, claims[k].Value));
            }
            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Users");
            }
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);

            }
            return View("Index");
        }
        
        
        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} In use ");
            }

        }
    }
}
