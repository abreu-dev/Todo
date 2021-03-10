using Todo.Domain.Validators.BoardValidators;
using System;

namespace Todo.Domain.Commands.BoardCommands
{
    public class AddColumnToBoardCommand : Command
    {
        public Guid BoardId { get; }
        public string ColumnTitle { get; }

        public AddColumnToBoardCommand(Guid boardId, string columnTitle) : base(Guid.Empty)
        {
            BoardId = boardId;
            ColumnTitle = columnTitle;
        }

        public override bool IsValid()
        {
            ValidationResult = new AddColumnToBoardCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
