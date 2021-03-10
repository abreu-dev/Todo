using System;
using Todo.Domain.Validators.BoardValidators;

namespace Todo.Domain.Commands.BoardCommands
{
    public class AddCardToColumnCommand : Command
    {
        public Guid BoardId { get; }
        public Guid ColumnId { get; }
        public string CardTitle { get; }

        public AddCardToColumnCommand(Guid boardId, Guid columnId, string cardTitle) : base(Guid.Empty)
        {
            BoardId = boardId;
            ColumnId = columnId;
            CardTitle = cardTitle;
        }

        public override bool IsValid()
        {
            ValidationResult = new AddCardToColumnCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
