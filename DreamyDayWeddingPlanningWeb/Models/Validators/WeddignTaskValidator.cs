using FluentValidation;
namespace DreamyDayWeddingPlanningWeb.Models.Validators
{
    public class WeddignTaskValidator : AbstractValidator<WeddingTask>
    {
        public WeddignTaskValidator()
        {
            RuleFor(x => x.TaskName)
                .NotEmpty().WithMessage("Task name is required.")
                .Length(1, 100).WithMessage("Task name must be between 1 and 100 characters.");

            RuleFor(x => x.Deadline)
                .NotEmpty().WithMessage("Deadline is required.")
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Deadline cannot be in the past.");
        }
    }


}
