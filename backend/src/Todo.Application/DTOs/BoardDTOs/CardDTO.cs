using System;

namespace Todo.Application.DTOs.BoardDTOs
{
    public class CardDTO : DTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Priority { get; set; }
        public Guid ColumnId { get; set; }
    }
}
