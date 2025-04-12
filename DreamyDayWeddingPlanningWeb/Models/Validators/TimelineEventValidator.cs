using FluentValidation;

namespace DreamyDayWeddingPlanningWeb.Models.Validators
{
    public class TimelineEventValidator : AbstractValidator<TimelineEvent>
    {
        public TimelineEventValidator()
        {
            RuleFor(x => x.WeddingId)
                .GreaterThan(0).WithMessage("Wedding ID must be a valid number.");

            RuleFor(x => x.EventName)
                .NotEmpty().WithMessage("Event name is required.")
                .Length(1, 100).WithMessage("Event name must be between 1 and 100 characters.");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Start time is required.");

            RuleFor(x => x.EndTime)
                .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time.")
                .When(x => x.EndTime != default(DateTime));

            RuleFor(x => x.Description)
                .Length(0, 200).WithMessage("Description must be less than 200 characters.")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }

    }
}
