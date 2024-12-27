using FluentValidation;
using TaskManagerApp.Models;

namespace TaskManagerApp.Validators
{
    public class TaskItemValidator : AbstractValidator<TaskItem>
    {
        public TaskItemValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title cannot be empty.");
        }
    }
}
