using System;

namespace Todo.Application.DTOs.BoardDTOs
{
    public class AddCardToColumnDTO : DTO
    {
        public Guid BoardId { get; set; }
        public Guid ColumnId { get; set; }
        public string CardTitle { get; set; }
    }
}
