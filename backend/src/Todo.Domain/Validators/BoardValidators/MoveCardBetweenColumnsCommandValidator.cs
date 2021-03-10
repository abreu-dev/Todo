using Todo.Domain.Commands.BoardCommands;
using FluentValidation;

namespace Todo.Domain.Validators.BoardValidators
{
    public class MoveCardBetweenColumnsCommandValidator : CommandValidator<MoveCardBetweenColumnsCommand>
    {
        public MoveCardBetweenColumnsCommandValidator()
        {
            RuleFor(x => x.BoardId)
                .NotEmpty()
                .WithMessage(UserMessages.RequiredField.Format("BoardId").Message);

            RuleFor(x => x.FromColumnId)
                .NotEmpty()
                .WithMessage(UserMessages.RequiredField.Format("FromColumnId").Message);

            RuleFor(x => x.ToColumnId)
                .NotEmpty()
                .WithMessage(UserMessages.RequiredField.Format("ToColumnId").Message);

            RuleFor(x => x.CardId)
                .NotEmpty()
                .WithMessage(UserMessages.RequiredField.Format("CardId").Message);

            RuleFor(x => x.CardPriorityInColumn)
                .GreaterThan(0)
                .WithMessage(UserMessages.MustBeGreatherThan.Format("CardPriorityInColumn", 0).Message);
        }
    }
}
