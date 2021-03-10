using Todo.Domain.Commands.BoardCommands;
using FluentValidation;

namespace Todo.Domain.Validators.BoardValidators
{
    public class UpdateBoardTitleCommandValidator : CommandValidator<UpdateBoardTitleCommand>
    {
        public UpdateBoardTitleCommandValidator()
        {
            RuleFor(x => x.BoardId)
                .NotEmpty()
                .WithMessage(UserMessages.RequiredField.Format("BoardId").Message);

            RuleFor(x => x.NewBoardTitle)
                .NotEmpty()
                .WithMessage(UserMessages.RequiredField.Format("NewBoardTitle").Message);
        }
    }
}
