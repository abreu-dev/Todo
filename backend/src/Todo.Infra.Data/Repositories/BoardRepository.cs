using Todo.Domain.Entities;
using Todo.Domain.Interfaces;
using Todo.Infra.Data.Accessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todo.Infra.Data.Repositories
{
    public class BoardRepository : IBoardRepository
    {
        private readonly TodoContext _todoContext;
        private readonly IUserAccessor _user;

        public BoardRepository(TodoContext todoContext, IUserAccessor user)
        {
            _todoContext = todoContext;
            _user = user;
        }

        public IUnitOfWork UnitOfWork => _todoContext;

        public async Task<IEnumerable<Board>> GetAll()
        {
            return await _todoContext.Boards
                .Where(x => x.UserId == _user.GetUserId())
                .ToListAsync();
        }

        public async Task<Board> GetById(Guid id)
        {
            var board = await _todoContext.Boards
                .Where(x => x.UserId == _user.GetUserId())
                .FirstOrDefaultAsync(x => x.Id == id);

            if (board == null) return null;

            await _todoContext.Entry(board)
                .Collection(x => x.Columns).LoadAsync();

            foreach (var column in board.Columns)
            {
                await _todoContext.Entry(column)
                    .Collection(x => x.Cards).LoadAsync();
            }

            return board;
        }

        public async Task Add(Board board)
        {
            board.LinkUser(_user.GetUserId());
            await _todoContext.Boards.AddAsync(board);
        }

        public Task Update(Board board)
        {
            _todoContext.Boards.Update(board);
            return Task.CompletedTask;
        }

        public async Task AddColumn(Column column)
        {
            await _todoContext.Columns.AddAsync(column);
        }

        public async Task AddCard(Card card)
        {
            await _todoContext.Cards.AddAsync(card);
        }
    }
}
