using Company.G02.DAL.Models;
using Company.G02.DAL.Models.Dtos;
using Company.G02.PL.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Security.Claims;

namespace Company.G02.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ISMSService _smsService;
        private readonly IMailSettingsWorkshop _mailSettings;

        public AccountController(UserManager<AppUser> userManager
            , SignInManager<AppUser> signInManager
            , ISMSService smsService
            , IMailSettingsWorkshop mailSettingsWorkshop)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _smsService = smsService;
            _mailSettings = mailSettingsWorkshop;
        }

        #region SignUp

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDto model)
        {
            if (ModelState.IsValid) //Server Side Validation
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user is null)
                {
                    user = await _userManager.FindByEmailAsync(model.Email);

                    if (user is null)
                    {
                        //Register
                        user = new AppUser()
                        {
                            UserName = model.UserName,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            IsAgree = model.IsAgree,
                        };
                        var result = await _userManager.CreateAsync(user, model.Password);

                        if (result.Succeeded)
                        {
                            return RedirectToAction("SignIn");
                        }
                        foreach (var error in result.Errors)
                        {

                            ModelState.AddModelError("", error.Description);

                        }
                    }
                }

                ModelState.AddModelError("", "Invalid SignUp !!");

            }
            return View(model);
        }

        #endregion

        #region SignIn

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);

                    if (flag)
                    {
                        //Sign-In
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");

                        }
                    }

                }

                ModelState.AddModelError("", "Invalid Login");
            }

            return View();
        }

        public IActionResult GoogleLogin()
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            };

            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded)
                return RedirectToAction("SignIn");

            var email = result.Principal.FindFirst(claim => claim.Type == ClaimTypes.Email)?.Value;
            var fullName = result.Principal.Identity?.Name;
            var names = fullName?.Split(' ') ?? new[] { "GoogleUser" };

            var firstName = names.FirstOrDefault() ?? "Google";
            var lastName = names.Skip(1).FirstOrDefault() ?? "User";

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new AppUser
                {
                    Email = email,
                    UserName = email,
                    FirstName = firstName,
                    LastName = lastName,
                    EmailConfirmed = true
                };

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    ModelState.AddModelError("", "Failed to create user from Google login.");
                    return RedirectToAction("SignIn");
                }
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult FacebookLogin()
        {
            var prop = new AuthenticationProperties()
            {
                RedirectUri = Url.Action("FacebookResponse")
            };
            return Challenge(prop, FacebookDefaults.AuthenticationScheme);
        }

       public async Task<IActionResult> FacebookResponse()
{
    var result = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme); // Fix to use Facebook's scheme

    if (!result.Succeeded)
        return RedirectToAction("SignIn");

    var email = result.Principal.FindFirst(claim => claim.Type == ClaimTypes.Email)?.Value;
    var fullName = result.Principal.Identity?.Name;
    var names = fullName?.Split(' ') ?? new[] { "FacebookUser" };

    var firstName = names.FirstOrDefault() ?? "Facebook";
    var lastName = names.Skip(1).FirstOrDefault() ?? "User";

    var user = await _userManager.FindByEmailAsync(email);

    if (user == null)
    {
        user = new AppUser
        {
            Email = email,
            UserName = email,
            FirstName = firstName,
            LastName = lastName,
            EmailConfirmed = true
        };

        var createResult = await _userManager.CreateAsync(user);
        if (!createResult.Succeeded)
        {
            ModelState.AddModelError("", "Failed to create user from Facebook login.");
            return RedirectToAction("SignIn");
        }
    }

    await _signInManager.SignInAsync(user, isPersistent: false);
    return RedirectToAction("Index", "Home");
}
        #endregion

        #region SignOut

        [HttpGet]
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }

        #endregion

        #region ForgetPassword

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    //Generate Token
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    //Create Url https://localhost:44347/Account/ResetPassword
                    var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token }, Request.Scheme);

                    //Create email
                    //var email = new Email()
                    //{ 
                    //   To = model.Email,
                    //   Subject="Reset Password",
                    //   Body=url
                    //};

                    //Send Email

                    //var flag = EmailSettings.SendEmail(email);

                    //if (flag) 
                    //{
                    //    return RedirectToAction("CheckYourInbox");
                    //}

                    var email = new Email()
                    {
                        To = model.Email,
                        Subject = "Reset Your Password",
                        Body = url
                    };
                    _mailSettings.SendEmail(email);

                    return RedirectToAction("CheckYourInbox");
                }
            }
            ModelState.AddModelError("", "Invalid Reset Password ");
            return View("resetPassword", model);
        }
        [HttpGet]
        public IActionResult CheckYourInbox()
        {
            return View();
        }

        #endregion

        #region ResetPassowrd

        [HttpPost]
        public async Task<IActionResult> SendSMS(ForgetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    //Generate Token
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    //Create Url https://localhost:44347/Account/ResetPassword
                    var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token }, Request.Scheme);

                    var sms = new SMSMessage()
                    {
                        PhoneNumber = user.PhoneNumber,
                        Body = url
                    };

                    _smsService.SendSMS(sms);

                    return Ok("Check Your Phone");

                }
            }
            ModelState.AddModelError("", "Invalid Reset Password ");
            return View("ForgetPassword", model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }

        [HttpPost] 
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;

                if (email is null || token is null) return BadRequest("Invalid Operation");

                var user = await _userManager.FindByEmailAsync(email);
                if (user is not null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("SignIn");
                    }
                }
                ModelState.AddModelError("", "Invalid reset password");
            }
            return View();
        }

        #endregion

        #region AccessDenied

        public IActionResult AccessDenied()
        {
            return View();
        }

        #endregion
    }
}
