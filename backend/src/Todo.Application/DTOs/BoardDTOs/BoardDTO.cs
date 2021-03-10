using System;
using System.Collections.Generic;

namespace Todo.Application.DTOs.BoardDTOs
{
    public class BoardDTO : DTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<ColumnDTO> Columns { get; set; }
    }
}
