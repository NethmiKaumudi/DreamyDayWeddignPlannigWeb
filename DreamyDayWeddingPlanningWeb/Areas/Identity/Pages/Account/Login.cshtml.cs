using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using DreamyDayWeddingPlanningWeb.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using FluentValidation;

namespace DreamyDayWeddingPlanningWeb.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IValidator<InputModel> _validator;

        public LoginModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<LoginModel> logger,
            IValidator<InputModel> validator)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _validator = validator;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Username")]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    ModelState.AddModelError(string.Empty, ErrorMessage);
                }

                returnUrl ??= Url.Content("~/");

                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

                ReturnUrl = returnUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while loading the login page.");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
            }
        }

       public async Task<IActionResult> OnPostAsync(string returnUrl = null)
{
    returnUrl ??= Url.Content("~/");

    try
    {
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        var validationResult = await _validator.ValidateAsync(Input);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return Page();
        }

        var user = await _userManager.FindByNameAsync(Input.UserName);
        if (user == null)
        {
            _logger.LogWarning($"User {Input.UserName} not found.");
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }

        _logger.LogInformation($"Attempting login for user {Input.UserName}. EmailConfirmed: {user.EmailConfirmed}");

        var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            _logger.LogInformation("User logged in.");
            var roles = await _userManager.GetRolesAsync(user);
            _logger.LogInformation($"Roles for user {Input.UserName}: {string.Join(", ", roles)}");
            if (roles.Contains("Admin"))
            {
                return RedirectToAction("AdminDashboard", "Admin");
            }
            else if (roles.Contains("Planner"))
            {
                return RedirectToAction("PlannerDashboard", "Planner");
            }
            else if (roles.Contains("Couple"))
            {
                        return RedirectToAction("Dashboard", "Couple");
                    }
            return LocalRedirect(returnUrl);
        }
        if (result.RequiresTwoFactor)
        {
            _logger.LogInformation("Two-factor authentication required.");
            return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
        }
        if (result.IsLockedOut)
        {
            _logger.LogWarning("User account locked out.");
            return RedirectToPage("./Lockout");
        }
        else
        {
            _logger.LogWarning($"Login failed for user {Input.UserName}.");
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "An error occurred during login.");
        ModelState.AddModelError(string.Empty, "An unexpected error occurred during login. Please try again later.");
        return Page();
    }
}
    }
}