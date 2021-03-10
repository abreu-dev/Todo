using Todo.Domain.Commands.BoardCommands;
using FluentValidation;

namespace Todo.Domain.Validators.BoardValidators
{
    public class UpdateCardPriorityInColumnCommandValidator : CommandValidator<UpdateCardPriorityInColumnCommand>
    {
        public UpdateCardPriorityInColumnCommandValidator()
        {
            RuleFor(x => x.BoardId)
                .NotEmpty()
                .WithMessage(UserMessages.RequiredField.Format("BoardId").Message);

            RuleFor(x => x.ColumnId)
                .NotEmpty()
                .WithMessage(UserMessages.RequiredField.Format("ColumnId").Message);

            RuleFor(x => x.CardId)
                .NotEmpty()
                .WithMessage(UserMessages.RequiredField.Format("CardId").Message);

            RuleFor(x => x.NewCardPriorityInColumn)
                .GreaterThan(0)
                .WithMessage(UserMessages.MustBeGreatherThan.Format("NewCardPriorityInColumn", 0).Message);
        }
    }
}
