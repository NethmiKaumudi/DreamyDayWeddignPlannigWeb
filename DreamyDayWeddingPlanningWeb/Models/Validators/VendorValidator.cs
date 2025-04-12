using FluentValidation;

namespace DreamyDayWeddingPlanningWeb.Models.Validators
{
    public class VendorValidator : AbstractValidator<Vendor>
    {
        public VendorValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Vendor name is required.")
                .Length(1, 100).WithMessage("Vendor name must be between 1 and 100 characters.");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("Category is required.")
                .Length(1, 50).WithMessage("Category must be between 1 and 50 characters.");

            RuleFor(x => x.Description)
                .Length(0, 500).WithMessage("Description must be less than 500 characters.")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price cannot be negative.");

            RuleFor(x => x.Reviews)
                .Length(0, 200).WithMessage("Reviews must be less than 200 characters.")
                .When(x => !string.IsNullOrEmpty(x.Reviews));
        }

    }
}
