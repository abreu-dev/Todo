using System;

namespace Todo.Application.DTOs.BoardDTOs
{
    public class UpdateColumnPositionInBoardDTO : DTO
    {
        public Guid BoardId { get; set; }
        public Guid ColumnId { get; set; }
        public int NewColumnPositionInBoard { get; set; }
    }
}
