using Todo.Domain.Validators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Todo.Domain.Entities
{
    public class Column : Entity
    {
        public string Title { get; private set; }
        public Guid BoardId { get; private set; }
        public int PositionInBoard { get; private set; }

        private readonly List<Card> _cards;
        public IReadOnlyCollection<Card> Cards => _cards;

        // EF Rel.
        public Board Board { get; set; }

        public Column(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new Exception(UserMessages.RequiredField.Format("Title").Message);
            }

            Title = title;
            _cards = new List<Card>();
        }

        internal void LinkBoard(Guid boardId, int positionInBoard)
        {
            BoardId = boardId;
            PositionInBoard = positionInBoard;
        }

        internal void DefinePositionInBoard(int newPositionInBoard)
        {
            PositionInBoard = newPositionInBoard;
        }

        internal void UpdateTitle(string newTitle)
        {
            Title = newTitle;
        }

        internal bool CardExists(Guid cardId)
        {
            if (cardId == Guid.Empty)
            {
                throw new Exception(UserMessages.RequiredField.Format("CardId").Message);
            }

            return _cards.Any(x => x.Id == cardId);
        }

        internal void AddCard(Card card)
        {
            if (card == null)
            {
                throw new Exception(UserMessages.RequiredField.Format("Card").Message);
            }

            if (CardExists(card.Id))
            {
                throw new Exception("That Card already is in the Column.");
            }

            card.LinkColumn(Id, _cards.Count + 1);
            _cards.Add(card);
        }

        internal void UpdateCardPriorityInColumn(Guid cardId, int newCardPriorityInColumn)
        {
            if (newCardPriorityInColumn < 1)
            {
                throw new Exception(UserMessages.MustBeGreatherThan.Format("Priority", 0).Message);
            }

            if (!CardExists(cardId))
            {
                throw new Exception(UserMessages.NotFound.Format("Card").Message);
            }

            var cardToUpdate = _cards.Single(x => x.Id == cardId);
            if (cardToUpdate.Priority == newCardPriorityInColumn)
            {
                return;
            }

            if (newCardPriorityInColumn > _cards.Count)
            {
                cardToUpdate.DefinePriority(_cards.Count);
                return;
            }

            var cardsInHigherPosition = _cards.Where(x => x.Priority > cardToUpdate.Priority && x.Priority <= newCardPriorityInColumn);
            foreach (var card in cardsInHigherPosition)
            {
                card.DefinePriority(card.Priority - 1);
            }

            var cardsInLowerPosition = _cards.Where(x => x.Priority < cardToUpdate.Priority && x.Priority >= newCardPriorityInColumn);
            foreach (var card in cardsInLowerPosition)
            {
                card.DefinePriority(card.Priority + 1);
            }

            cardToUpdate.DefinePriority(newCardPriorityInColumn);
        }

        internal void RemoveCard(Guid cardId)
        {
            if (!CardExists(cardId))
            {
                throw new Exception(UserMessages.NotFound.Format("Card").Message);
            }

            var cardToRemove = _cards.Single(x => x.Id == cardId);

            var cardsInHigherPosition = _cards.Where(x => x.Priority > cardToRemove.Priority);
            foreach (var card in cardsInHigherPosition)
            {
                card.DefinePriority(card.Priority - 1);
            }

            cardToRemove.UnlinkColumn();
            _cards.Remove(cardToRemove);
        }
    }
}
