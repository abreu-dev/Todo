using Todo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Todo.Domain.Interfaces
{
    public interface IBoardRepository
    {
        IUnitOfWork UnitOfWork { get; }

        Task<IEnumerable<Board>> GetAll();
        Task<Board> GetById(Guid id);

        Task Add(Board board);
        Task Update(Board board);

        Task AddColumn(Column column);

        Task AddCard(Card card);
    }
}
