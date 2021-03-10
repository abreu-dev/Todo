using Todo.Domain.Validators.BoardValidators;
using System;

namespace Todo.Domain.Commands.BoardCommands
{
    public class AddBoardCommand : Command
    {
        public string BoardTitle { get; }

        public AddBoardCommand(string boardTitle) : base(Guid.Empty)
        {
            BoardTitle = boardTitle;
        }

        public override bool IsValid()
        {
            ValidationResult = new AddBoardCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
