using DreamyDayWeddingPlanningWeb.Areas.Identity.Data;
using DreamyDayWeddingPlanningWeb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace DreamyDayWeddingPlanningWeb.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ConfirmEmailModel> _logger;

        public ConfirmEmailModel(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, ILogger<ConfirmEmailModel> logger)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        [BindProperty(SupportsGet = true)]
        public string UserId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Code { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        public bool IsConfirmed { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            _logger.LogInformation("ConfirmEmail page accessed with UserId: {UserId}, Code: {Code}", UserId, Code);

            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Code))
            {
                _logger.LogWarning("Invalid email confirmation attempt: UserId or Code is empty.");
                IsConfirmed = false;
                return Page();
            }

            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                _logger.LogWarning("User not found for Id: {UserId}", UserId);
                IsConfirmed = false;
                return Page();
            }

            var result = await _userManager.ConfirmEmailAsync(user, Code);
            if (result.Succeeded)
            {
                _logger.LogInformation("Email confirmed successfully for user {UserName} (Id: {UserId})", user.UserName, UserId);
                await _dbContext.SaveChangesAsync(); // Explicitly save changes
                IsConfirmed = true;
            }
            else
            {
                _logger.LogError("Failed to confirm email for user {UserName} (Id: {UserId}). Errors: {Errors}",
                    user.UserName, UserId, string.Join(", ", result.Errors.Select(e => e.Description)));
                IsConfirmed = false;
            }

            return Page();
        }
    }
}