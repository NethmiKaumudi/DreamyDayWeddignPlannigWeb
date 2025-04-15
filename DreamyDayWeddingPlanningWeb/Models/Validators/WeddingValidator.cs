using FluentValidation;

namespace DreamyDayWeddingPlanningWeb.Models.Validators
{
    public class WeddingValidator : AbstractValidator<Wedding>
    {
        public WeddingValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Couple ID is required.");

            RuleFor(x => x.WeddingDate)
                .NotEmpty().WithMessage("Wedding date is required.")
                .GreaterThan(DateTime.Now).WithMessage("Wedding date must be in the future.");

            RuleFor(x => x.TotalBudget)
                .GreaterThanOrEqualTo(0).WithMessage("Total budget cannot be negative.");

            RuleFor(x => x.SpentBudget)
                .GreaterThanOrEqualTo(0).WithMessage("Spent budget cannot be negative.")
                .LessThanOrEqualTo(x => x.TotalBudget).WithMessage("Spent budget cannot exceed total budget.");

            RuleFor(x => x.Progress)
                .InclusiveBetween(0, 100).WithMessage("Progress must be between 0 and 100.");
        }
    }

}

