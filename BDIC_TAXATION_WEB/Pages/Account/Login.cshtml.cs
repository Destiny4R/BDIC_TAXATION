using BDIC_TAXATION_ACCESS.Repository.IRepository;
using BDIC_TAXATION_MODELS.Models;
using BDIC_TAXATION_MODELS.ViewModels;
using BDIC_TAXATION_UTILITIES;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BDIC_TAXATION_WEB.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IUnitOfWork unitOfWork;

        [BindProperty]
        public LoginVM  Inputs { get; set; }
        [BindProperty]
        public string? ReturnUrl { get; set; }
        public LoginModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.unitOfWork = unitOfWork;
        }
        public void OnGet(string? returnUrl)
        {
            Addrole();
            var getdata = userManager.Users.Count();
            if (getdata < 1)
            {
                AddAdmin();
            }
            ReturnUrl = returnUrl;
        }
        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == Inputs.Username || u.Email == Inputs.Username || u.PhoneNumber == Inputs.Username);
                    if (user != null)
                    {
                        //USER USING EMAIL TO AUTHENTICATE
                        if (user.Email.Equals(Inputs.Username))
                        {
                            if(!await userManager.IsEmailConfirmedAsync(user))
                            {
                                ModelState.AddModelError("", "Email not confirmed");
                            }
                            else
                            {
                                var signin = await signInManager.PasswordSignInAsync(user, Inputs.Password, Inputs.Rememberme, false);
                                if (signin.Succeeded)
                                {
                                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                                    {
                                        if (User.IsInRole("Admin"))
                                        {
                                            if (ReturnUrl == "/")
                                            {
                                                return RedirectToPage("/Index");
                                            }
                                            return Redirect(ReturnUrl);
                                        }
                                        else if (User.IsInRole(SD.Role_Individual))
                                        {
                                            return RedirectToPage("/Index-Payer");
                                        }
                                    } else
                                        return RedirectToPage("/Index-Payer");
                                }
                                ModelState.AddModelError("", "Wrong Email/Phone number or Password");
                                return Page();
                            }
                        }
                        //USER USING PHONE NUMBER TO AUTHENTICATE
                        if (user.PhoneNumber.Equals(Inputs.Username))
                        {
                            if (!await userManager.IsPhoneNumberConfirmedAsync(user))
                            {
                                ModelState.AddModelError("", "Phone not confirmed");
                                return Page();
                            }
                            else
                            {
                                var signin = await signInManager.PasswordSignInAsync(user, Inputs.Password, Inputs.Rememberme, false);
                                if (signin.Succeeded)
                                {
                                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                                    {
                                        if (User.IsInRole(SD.Role_Admin))
                                        {
                                            if (ReturnUrl == "/")
                                            {
                                                return RedirectToPage("/Index");
                                            }
                                            else if (User.IsInRole(SD.Role_Individual))
                                            {
                                                return RedirectToPage("/Tax-Payer/Index");
                                            }
                                            return Redirect(ReturnUrl);
                                        }
                                        
                                    }
                                }
                                ModelState.AddModelError("", "Wrong Email/Phone number or Password");
                                return Page();
                            }
                        }
                        ModelState.AddModelError("", "Wrong Email/Phone number or Password");
                        return Page();
                    }
                }
                catch (Exception ex)
                {
                    return Page();
                }
            }
            ModelState.AddModelError("", "Provide your login details");
            return Page();
        }

        private void AddAdmin()
        {
            Addrole();
            string username = $"admin@mail.com";

            var user = new ApplicationUser();

            user.UserName = username;
            user.Email = username;
            user.EmailConfirmed = true;
            string password = "123";

            var result = userManager.CreateAsync(user, password).GetAwaiter().GetResult();
            userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
        }
        private void Addrole()
        {
            if (!roleManager.RoleExistsAsync(SD.Role_Consultant).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole(SD.Role_Consultant)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(SD.Role_SubAdmin)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(SD.Role_Individual)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(SD.Role_Business)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(SD.Role_MDAs)).GetAwaiter().GetResult();
            }
        }
    }
}
