using FluentValidation;

namespace DreamyDayWeddingPlanningWeb.Areas.Identity.Pages.Account
{
    public class RegisterInputModelValidator : AbstractValidator<RegisterModel.InputModel>
    {
        public RegisterInputModelValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .Length(3, 50).WithMessage("Username must be between 3 and 50 characters.")
                .Matches("^[a-zA-Z0-9_]+$").WithMessage("Username can only contain letters, numbers, and underscores.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"^[^\s]*$").WithMessage("Password cannot contain spaces.")
                .Matches(@"[a-zA-Z].*[a-zA-Z]").WithMessage("Password must contain at least two letters.")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one number.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Passwords do not match.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.")
                .Must(role => new[] { "Couple", "Planner", "Admin" }.Contains(role))
                .WithMessage("Role must be one of: Couple, Planner, or Admin.");

            RuleFor(x => x.ContactNumber)
                .NotEmpty().WithMessage("Contact number is required.")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.");
        }
    }
}