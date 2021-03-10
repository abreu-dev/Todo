using Todo.Domain.Validators.BoardValidators;
using System;

namespace Todo.Domain.Commands.BoardCommands
{
    public class UpdateColumnTitleCommand : Command
    {
        public Guid BoardId { get; }
        public Guid ColumnId { get; }
        public string NewColumnTitle { get; }

        public UpdateColumnTitleCommand(Guid boardId, Guid columnId, string newColumnTitle) : base(Guid.Empty)
        {
            BoardId = boardId;
            ColumnId = columnId;
            NewColumnTitle = newColumnTitle;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateColumnTitleCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
