using Todo.Domain.Validators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Todo.Domain.Entities
{
    public class Board : Entity
    {
        public string Title { get; private set; }

        private readonly List<Column> _columns;
        public IReadOnlyCollection<Column> Columns => _columns;

        public Guid UserId { get; private set; }

        // EF Rel.
        public ApplicationUser User { get; set; }

        public Board(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new Exception(UserMessages.RequiredField.Format("Title").Message);
            }

            Title = title;
            _columns = new List<Column>();
        }

        public bool ColumnExists(Guid columnId)
        {
            if (columnId == Guid.Empty)
            {
                throw new Exception(UserMessages.RequiredField.Format("ColumnId").Message);
            }

            return _columns.Any(x => x.Id == columnId);
        }

        public void AddColumn(Column column)
        {
            if (column == null)
            {
                throw new Exception(UserMessages.RequiredField.Format("Column").Message);
            }

            if (ColumnExists(column.Id))
            {
                throw new Exception("That Column already is in the Board.");
            }

            column.LinkBoard(Id, _columns.Count + 1);
            _columns.Add(column);
        }

        public void UpdateTitle(string newTitle)
        {
            if (string.IsNullOrEmpty(newTitle))
            {
                throw new Exception(UserMessages.RequiredField.Format("NewTitle").Message);
            }

            Title = newTitle;
        }

        public void UpdateColumnPositionInBoard(Guid columnId, int newPositionInBoard)
        {
            if (newPositionInBoard < 1)
            {
                throw new Exception(UserMessages.MustBeGreatherThan.Format("PositionInBoard", 0).Message);
            }

            if (!ColumnExists(columnId))
            {
                throw new Exception(UserMessages.NotFound.Format("Column").Message);
            }

            var columnToUpdate = _columns.Single(x => x.Id == columnId);
            if (columnToUpdate.PositionInBoard == newPositionInBoard)
            {
                return;
            }

            if (newPositionInBoard > _columns.Count)
            {
                columnToUpdate.DefinePositionInBoard(_columns.Count);
                return;
            }

            var columnsInHigherPosition = _columns.Where(x => x.PositionInBoard > columnToUpdate.PositionInBoard && x.PositionInBoard <= newPositionInBoard);
            foreach (var column in columnsInHigherPosition)
            {
                column.DefinePositionInBoard(column.PositionInBoard - 1);
            }

            var columnsInLowerPosition = _columns.Where(x => x.PositionInBoard < columnToUpdate.PositionInBoard && x.PositionInBoard >= newPositionInBoard);
            foreach (var column in columnsInLowerPosition)
            {
                column.DefinePositionInBoard(column.PositionInBoard + 1);
            }

            columnToUpdate.DefinePositionInBoard(newPositionInBoard);
        }

        public void UpdateColumnTitle(Guid columnId, string columnNewTitle)
        {
            if (string.IsNullOrEmpty(columnNewTitle))
            {
                throw new Exception(UserMessages.RequiredField.Format("ColumnNewTitle").Message);
            }

            if (!ColumnExists(columnId))
            {
                throw new Exception(UserMessages.NotFound.Format("Column").Message);
            }

            var columnToUpdate = _columns.Single(x => x.Id == columnId);
            columnToUpdate.UpdateTitle(columnNewTitle);
        }

        public void AddCardToColumn(Guid columnId, Card card)
        {
            if (!ColumnExists(columnId))
            {
                throw new Exception(UserMessages.NotFound.Format("Column").Message);
            }

            var column = _columns.Single(x => x.Id == columnId);
            column.AddCard(card);
        }

        public void UpdateCardPriorityInColumn(Guid columnId, Guid cardId, int newCardPriorityInColumn)
        {
            if (newCardPriorityInColumn < 1)
            {
                throw new Exception(UserMessages.MustBeGreatherThan.Format("Priority", 0).Message);
            }

            if (!ColumnExists(columnId))
            {
                throw new Exception(UserMessages.NotFound.Format("Column").Message);
            }

            var column = _columns.Single(x => x.Id == columnId);

            column.UpdateCardPriorityInColumn(cardId, newCardPriorityInColumn);
        }

        public bool CardExistsInColumn(Guid columnId, Guid cardId)
        {
            if (!ColumnExists(columnId))
            {
                throw new Exception(UserMessages.NotFound.Format("Column").Message);
            }

            var column = _columns.Single(x => x.Id == columnId);
            return column.CardExists(cardId);
        }

        public void MoveCardBetweenColumns(Guid cardId, Guid fromColumnId, Guid toColumnId, int cardPriorityInColumn)
        {
            if (cardPriorityInColumn < 1)
            {
                throw new Exception(UserMessages.MustBeGreatherThan.Format("Priority", 0).Message);
            }

            if (!ColumnExists(fromColumnId))
            {
                throw new Exception(UserMessages.NotFound.Format("FromColumn").Message);
            }

            if (!ColumnExists(toColumnId))
            {
                throw new Exception(UserMessages.NotFound.Format("ToColumn").Message);
            }

            if (!CardExistsInColumn(fromColumnId, cardId)) 
            {
                throw new Exception(UserMessages.NotFound.Format("Card").Message);
            }

            var fromColumn = _columns.Single(x => x.Id == fromColumnId);
            var card = fromColumn.Cards.Single(x => x.Id == cardId);

            fromColumn.RemoveCard(cardId);

            var toColumn = _columns.Single(x => x.Id == toColumnId);
            toColumn.AddCard(card);
            toColumn.UpdateCardPriorityInColumn(card.Id, cardPriorityInColumn);
        }

        public void LinkUser(Guid userId)
        {

            if (userId == Guid.Empty)
            {
                throw new Exception(UserMessages.RequiredField.Format("UserId").Message);
            }

            UserId = userId;
        }
    }
}
