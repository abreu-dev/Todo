using Todo.Domain.Commands.BoardCommands;
using FluentValidation;

namespace Todo.Domain.Validators.BoardValidators
{
    public class AddCardToColumnCommandValidator : CommandValidator<AddCardToColumnCommand>
    {
        public AddCardToColumnCommandValidator()
        {
            RuleFor(x => x.BoardId)
                .NotEmpty()
                .WithMessage(UserMessages.RequiredField.Format("BoardId").Message);

            RuleFor(x => x.ColumnId)
                .NotEmpty()
                .WithMessage(UserMessages.RequiredField.Format("ColumnId").Message);

            RuleFor(x => x.CardTitle)
                .NotEmpty()
                .WithMessage(UserMessages.RequiredField.Format("CardTitle").Message);
        }
    }
}
