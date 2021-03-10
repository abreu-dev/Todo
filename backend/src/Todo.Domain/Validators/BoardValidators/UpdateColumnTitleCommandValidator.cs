using Todo.Domain.Commands.BoardCommands;
using FluentValidation;

namespace Todo.Domain.Validators.BoardValidators
{
    public class UpdateColumnTitleCommandValidator : CommandValidator<UpdateColumnTitleCommand>
    {
        public UpdateColumnTitleCommandValidator()
        {
            RuleFor(x => x.BoardId)
                .NotEmpty()
                .WithMessage(UserMessages.RequiredField.Format("BoardId").Message);

            RuleFor(x => x.ColumnId)
                .NotEmpty()
                .WithMessage(UserMessages.RequiredField.Format("ColumnId").Message);

            RuleFor(x => x.NewColumnTitle)
                .NotEmpty()
                .WithMessage(UserMessages.RequiredField.Format("NewColumnTitle").Message);
        }
    }
}
