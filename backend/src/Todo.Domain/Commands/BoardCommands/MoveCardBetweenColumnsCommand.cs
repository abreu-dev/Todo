using Todo.Domain.Validators.BoardValidators;
using System;

namespace Todo.Domain.Commands.BoardCommands
{
    public class MoveCardBetweenColumnsCommand : Command
    {
        public Guid BoardId { get; }
        public Guid FromColumnId { get; }
        public Guid ToColumnId { get; }
        public Guid CardId { get; }
        public int CardPriorityInColumn { get; }

        public MoveCardBetweenColumnsCommand(Guid boardId, Guid fromColumnId, Guid toColumnId, Guid cardId, int cardPriorityInColumn) : base(Guid.Empty)
        {
            BoardId = boardId;
            FromColumnId = fromColumnId;
            ToColumnId = toColumnId;
            CardId = cardId;
            CardPriorityInColumn = cardPriorityInColumn;
        }

        public override bool IsValid()
        {
            ValidationResult = new MoveCardBetweenColumnsCommandValidator().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
