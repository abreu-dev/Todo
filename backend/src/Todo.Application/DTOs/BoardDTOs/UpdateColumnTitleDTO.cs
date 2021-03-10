using System;

namespace Todo.Application.DTOs.BoardDTOs
{
    public class UpdateColumnTitleDTO : DTO
    {
        public Guid BoardId { get; set; }
        public Guid ColumnId { get; set; }
        public string NewColumnTitle { get; set; }
    }
}
