using System;

namespace Todo.Application.DTOs.BoardDTOs
{
    public class AddColumnToBoardDTO : DTO
    {
        public Guid BoardId { get; set; } 
        public string ColumnTitle { get; set; }
    }
}
