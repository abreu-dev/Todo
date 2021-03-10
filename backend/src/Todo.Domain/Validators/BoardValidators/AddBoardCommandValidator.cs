using Todo.Domain.Commands.BoardCommands;
using FluentValidation;

namespace Todo.Domain.Validators.BoardValidators
{
    public class AddBoardCommandValidator : CommandValidator<AddBoardCommand>
    {
        public AddBoardCommandValidator()
        {
            RuleFor(x => x.BoardTitle)
                .NotEmpty()
                .WithMessage(UserMessages.RequiredField.Format("BoardTitle").Message);
        }
    }
}
