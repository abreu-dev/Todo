using Todo.Domain.Validators.BoardValidators;
using System;

namespace Todo.Domain.Commands.BoardCommands
{
    public class UpdateBoardTitleCommand : Command
    {
        public Guid BoardId { get; }
        public string NewBoardTitle { get; }

        public UpdateBoardTitleCommand(Guid boardId, string newBoardTitle) : base(Guid.Empty)
        {
            BoardId = boardId;
            NewBoardTitle = newBoardTitle;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateBoardTitleCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
