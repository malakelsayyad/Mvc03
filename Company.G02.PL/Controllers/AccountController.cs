﻿using Company.G02.DAL.Models;
using Company.G02.DAL.Models.Dtos;
using Company.G02.PL.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.G02.PL.Controllers
{
    public class AccountController: Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

                ModelState.AddModelError("","Invalid SignUp !!");

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
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password,model.RememberMe,false);
                        if(result.Succeeded)
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");

                        }
                    }

                }

                ModelState.AddModelError("", "Invalid Login");
            }

            return View();
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
                if(user is not null)
                {
                    //Generate Token
                   var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    //Create Url https://localhost:44347/Account/ResetPassword
                    var url = Url.Action("ResetPassword", "Account", new { email=model.Email,token },Request.Scheme);

                    //Create email
                    var email = new Email()
                    { 
                       To = model.Email,
                       Subject="Reset Password",
                       Body=url
                    };
                    //Send Email
                    var flag = EmailSettings.SendEmail(email);
                    if (flag) 
                    {
                        return RedirectToAction("CheckYourInbox");
                    }
                }
            }
            ModelState.AddModelError("", "Invalid Reset Password ");
            return View("ForgetPassword",model);
        }

        [HttpGet]
        public IActionResult CheckYourInbox()
        {
            return View();
        }

        #endregion
    }
}
