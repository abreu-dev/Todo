using System;
using Todo.Domain.Validators.BoardValidators;

namespace Todo.Domain.Commands.BoardCommands
{
    public class UpdateCardPriorityInColumnCommand : Command
    {
        public Guid BoardId { get; }
        public Guid ColumnId { get; }
        public Guid CardId { get; }
        public int NewCardPriorityInColumn { get; }

        public UpdateCardPriorityInColumnCommand(Guid boardId, Guid columnId, Guid cardId, int newCardPriorityInColumn) : base(Guid.Empty)
        {
            BoardId = boardId;
            ColumnId = columnId;
            CardId = cardId;
            NewCardPriorityInColumn = newCardPriorityInColumn;    
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateCardPriorityInColumnCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
