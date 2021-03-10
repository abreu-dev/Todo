using Todo.Domain.Commands.BoardCommands;
using FluentValidation;

namespace Todo.Domain.Validators.BoardValidators
{
    public class UpdateColumnPositionInBoardCommandValidator : CommandValidator<UpdateColumnPositionInBoardCommand>
    {
        public UpdateColumnPositionInBoardCommandValidator()
        {
            RuleFor(x => x.BoardId)
                .NotEmpty()
                .WithMessage(UserMessages.RequiredField.Format("BoardId").Message);

            RuleFor(x => x.ColumnId)
                .NotEmpty()
                .WithMessage(UserMessages.RequiredField.Format("ColumnId").Message);

            RuleFor(x => x.NewColumnPositionInBoard)
                .GreaterThan(0)
                .WithMessage(UserMessages.MustBeGreatherThan.Format("NewColumnPositionInBoard", 0).Message);
        }
    }
}
