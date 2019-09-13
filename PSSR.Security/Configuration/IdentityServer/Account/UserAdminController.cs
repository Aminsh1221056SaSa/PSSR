using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using PSSR.UserSecurity.Models;
using PSSR.UserSecurity.Configuration;
using System.Security.Claims;
using PSSR.Security.Helpers;
using System.Data;
using Microsoft.Extensions.Options;

namespace IdentityServer4.Quickstart.UI
{
    [Authorize(Policy = "dataEventRecordsAdmin")]
    public class UserAdminController : Controller
    {
        private UserManager<AppUser> _userManager;
        private RoleManager<Role> _roleManager;
        private IUserValidator<AppUser> _userValidator;
        private IPasswordValidator<AppUser> _passwordValidator;
        private IPasswordHasher<AppUser> _passwordHasher;
        private readonly IDatabaseService _databaseService;
        private readonly IOptions<SqlConnectionHelper> _settings;

        public UserAdminController(UserManager<AppUser> usrMgr,
                 IUserValidator<AppUser> userValid,
                 IPasswordValidator<AppUser> passValid,
                 IPasswordHasher<AppUser> passwordHash, RoleManager<Role> roleMgr, IDatabaseService dbService
            , IOptions<SqlConnectionHelper> settings)
        {
            _userManager = usrMgr;
            _userValidator = userValid;
            _passwordValidator = passValid;
            _passwordHasher = passwordHash;
            _roleManager = roleMgr;
            _databaseService = dbService;
            _settings = settings;
            _databaseService.ConnectionString = _settings.Value.DefaultConnection;
        }

        public ViewResult Credit()
        {
            return View(_userManager.Users.AsEnumerable());
        }

        [HttpPost]
        public async Task<IActionResult> CreditDelete(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Credit");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View("Credit", _userManager.Users);
        }

        [HttpGet]
        public IActionResult CreditInsert()
        {
            var viewModel = new UserModel();
            _roleManager.Roles.ToList().ForEach(r =>
            {
                viewModel.Roles.Add(r);
            });
            string command = "select Id,FirstName,LastName,NationalId from Person.Person";
            using (IDataReader reader = _databaseService.ExecuteReader(command, CommandType.Text, cmdParms: null))
            {
                viewModel.Persons = reader.Select(r => r.FromDataReaderPerson()).ToList();
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreditInsert(UserModel model)
        {
            if (ModelState.IsValid)
            {
                string msg = "";
               
                var role = await _roleManager.FindByIdAsync(model.RoleName);
                if (role == null)
                {
                    msg += "Role Not Found" + Environment.NewLine;
                }

                if (model.PersonId <= 0)
                {
                    msg += "Not valid Person" + Environment.NewLine;
                }

                if(!string.Equals(model.Password,model.PasswordHinit,StringComparison.OrdinalIgnoreCase))
                {
                    msg += "Password and confirm passwrod not equeal." + Environment.NewLine;
                }

                if (msg.Length <= 0)
                {
                    AppUser user = new AppUser
                    {
                        UserName = model.Name,
                        Email = model.Email,
                        PersonId=model.PersonId,
                    };

                    try
                    {
                        IdentityResult result
                            = await _userManager.CreateAsync(user, model.Password);

                        if (result.Succeeded)
                        {
                            var result1 = await _userManager.AddToRoleAsync(user, role.Name);
                            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Name, user.UserName));
                            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Actor, user.PersonId.ToString()));
                            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, user.Email));
                            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, role.Name));

                            return RedirectToAction("Credit");
                        }
                        else
                        {
                            foreach (IdentityError error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    ModelState.AddModelError("", msg);
                }
            }
            _roleManager.Roles.ToList().ForEach(r =>
            {
                model.Roles.Add(r);
            });

            string command = "select Id,FirstName,LastName,NationalId from Person.Person";
            using (IDataReader reader = _databaseService.ExecuteReader(command, CommandType.Text, cmdParms: null))
            {
                model.Persons = reader.Select(r => r.FromDataReaderPerson()).ToList();
            }

            return View(model);
        }

        public async Task<IActionResult> CreditEdit(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles != null)
                {
                    user.RoleName = userRoles.Aggregate((s,x)=>s+" , "+x);
                }
                return View(user);
            }
            else
            {
                return RedirectToAction("Credit");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreditEdit(string id, string email,
                 string password, string roleName)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Email = email;
                IdentityResult validEmail
                    = await _userValidator.ValidateAsync(_userManager, user);
                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                }
                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(password))
                {
                    validPass = await _passwordValidator.ValidateAsync(_userManager,
                    user, password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = _passwordHasher.HashPassword(user,
                        password);
                    }
                    else
                    {
                        AddErrorsFromResult(validPass);
                    }
                }
                if ((validEmail.Succeeded && validPass == null)
                         || (validEmail.Succeeded
                        && password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Credit");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View(user);
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

        }
    }
}