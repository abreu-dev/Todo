using System;

namespace Todo.Application.DTOs.BoardDTOs
{
    public class UpdateCardPriorityInColumnDTO : DTO
    {
        public Guid BoardId { get; set; }
        public Guid ColumnId { get; set; }
        public Guid CardId { get; set; }
        public int NewCardPriorityInColumn { get; set; }
    }
}
