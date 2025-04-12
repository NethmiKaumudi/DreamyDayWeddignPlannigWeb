using FluentValidation;

namespace DreamyDayWeddingPlanningWeb.Areas.Identity.Pages.Account
{
    public class LoginInputModelValidator : AbstractValidator<LoginModel.InputModel>
    {
        public LoginInputModelValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}