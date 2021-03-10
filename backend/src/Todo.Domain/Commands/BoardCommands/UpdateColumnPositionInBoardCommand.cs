using Todo.Domain.Validators.BoardValidators;
using System;

namespace Todo.Domain.Commands.BoardCommands
{
    public class UpdateColumnPositionInBoardCommand : Command
    {
        public Guid BoardId { get; }
        public Guid ColumnId { get; }
        public int NewColumnPositionInBoard { get; }

        public UpdateColumnPositionInBoardCommand(Guid boardId, Guid columnId, int newColumnPositionInBoard) : base(Guid.Empty)
        {
            BoardId = boardId;
            ColumnId = columnId;
            NewColumnPositionInBoard = newColumnPositionInBoard; 
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateColumnPositionInBoardCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
