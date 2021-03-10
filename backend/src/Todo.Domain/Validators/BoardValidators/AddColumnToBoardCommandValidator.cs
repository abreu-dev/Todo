using Todo.Domain.Commands.BoardCommands;
using FluentValidation;

namespace Todo.Domain.Validators.BoardValidators
{
    public class AddColumnToBoardCommandValidator : CommandValidator<AddColumnToBoardCommand>
    {
        public AddColumnToBoardCommandValidator()
        {
            RuleFor(x => x.BoardId)
                .NotEmpty()
                .WithMessage(UserMessages.RequiredField.Format("BoardId").Message);

            RuleFor(x => x.ColumnTitle)
                .NotEmpty()
                .WithMessage(UserMessages.RequiredField.Format("ColumnTitle").Message);
        }
    }
}
