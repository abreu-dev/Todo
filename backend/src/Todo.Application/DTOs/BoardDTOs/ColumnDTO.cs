using System;
using System.Collections.Generic;

namespace Todo.Application.DTOs.BoardDTOs
{
    public class ColumnDTO : DTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int PositionInBoard { get; set; }
        public IEnumerable<CardDTO> Cards { get; set; }
        public Guid BoardId { get; set; }
    }
}
