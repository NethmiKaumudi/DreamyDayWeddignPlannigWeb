using FluentValidation;

namespace DreamyDayWeddingPlanningWeb.Models.Validators
{
    public class BudgetValidator : AbstractValidator<Budget>
    {
        public BudgetValidator()
        {
            RuleFor(x => x.WeddingId)
                .GreaterThan(0).WithMessage("Wedding ID must be a valid number.");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("Category is required.")
                .Length(1, 100).WithMessage("Category must be between 1 and 100 characters.");

            RuleFor(x => x.AllocatedAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Allocated amount cannot be negative.");

            RuleFor(x => x.SpentAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Spent amount cannot be negative.")
                .LessThanOrEqualTo(x => x.AllocatedAmount).WithMessage("Spent amount cannot exceed allocated amount.");
        }

    }
}
