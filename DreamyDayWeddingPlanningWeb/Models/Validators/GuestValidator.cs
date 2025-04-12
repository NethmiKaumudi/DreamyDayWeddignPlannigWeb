using FluentValidation;

namespace DreamyDayWeddingPlanningWeb.Models.Validators
{
    public class GuestValidator : AbstractValidator<Guest>
    {
        public GuestValidator()
        {
            RuleFor(x => x.WeddingId)
                .GreaterThan(0).WithMessage("Wedding ID must be a valid number.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Guest name is required.")
                .Length(1, 100).WithMessage("Guest name must be between 1 and 100 characters.");

            RuleFor(x => x.MealPreference)
                .Length(0, 50).WithMessage("Meal preference must be less than 50 characters.")
                .When(x => !string.IsNullOrEmpty(x.MealPreference));

            RuleFor(x => x.SeatingArrangement)
                .Length(0, 50).WithMessage("Seating arrangement must be less than 50 characters.")
                .When(x => !string.IsNullOrEmpty(x.SeatingArrangement));
        }

    }
}
