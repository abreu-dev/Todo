using Todo.Application.DTOs.BoardDTOs;
using Todo.Domain.Entities;
using AutoMapper;

namespace Todo.Application.AutoMapper
{
    public class DomainToDTOMappingProfile : Profile
    {
        public DomainToDTOMappingProfile()
        {
            CreateMap<Board, BoardDTO>();
            CreateMap<Column, ColumnDTO>();
            CreateMap<Card, CardDTO>();
        }
    }
}
