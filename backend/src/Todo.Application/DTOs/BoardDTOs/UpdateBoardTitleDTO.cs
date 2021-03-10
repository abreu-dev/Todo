using System;

namespace Todo.Application.DTOs.BoardDTOs
{
    public class UpdateBoardTitleDTO : DTO
    {
        public Guid BoardId { get; set; }
        public string NewBoardTitle { get; set; }
    }
}
