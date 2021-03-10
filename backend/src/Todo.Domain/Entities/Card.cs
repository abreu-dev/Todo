using System;
using Todo.Domain.Validators;

namespace Todo.Domain.Entities
{
    public class Card : Entity
    {
        public string Title { get; private set; }
        public Guid ColumnId { get; private set; }
        public int Priority { get; private set; }

        // EF Rel.
        public Column Column { get; set; }

        public Card(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new Exception(UserMessages.RequiredField.Format("Title").Message);
            }

            Title = title;
        }

        internal void LinkColumn(Guid columnId, int priority)
        {
            ColumnId = columnId;
            Priority = priority;
        }

        internal void UnlinkColumn()
        {
            ColumnId = Guid.Empty;
            Priority = 0;
        }

        internal void DefinePriority(int newPriority)
        {
            Priority = newPriority;
        }
    }
}
