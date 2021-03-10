using System;

namespace Todo.Application.DTOs.BoardDTOs
{
    public class MoveCardBetweenColumnsDTO: DTO
    {
        public Guid BoardId { get; set; }
        public Guid FromColumnId { get; set; }
        public Guid ToColumnId { get; set; }
        public Guid CardId { get; set; }
        public int CardPriorityInColumn { get; set; }
    }
}
