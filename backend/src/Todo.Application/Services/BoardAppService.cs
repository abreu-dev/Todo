using Todo.Application.DTOs.BoardDTOs;
using Todo.Application.Interfaces;
using Todo.Domain.Commands.BoardCommands;
using Todo.Domain.Interfaces;
using Todo.Domain.Mediator;
using AutoMapper;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todo.Application.Services
{
    public class BoardAppService : IBoardAppService
    {
        private readonly IBoardRepository _boardRepository;
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediatorHandler;

        public BoardAppService(IBoardRepository boardRepository, 
                               IMapper mapper, 
                               IMediatorHandler mediatorHandler)
        {
            _boardRepository = boardRepository;
            _mapper = mapper;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<IEnumerable<BoardDTO>> GetAllBoards()
        {
            return _mapper.Map<IEnumerable<BoardDTO>>(await _boardRepository.GetAll());
        }

        public async Task<BoardDTO> GetBoardById(Guid id)
        {
            var board = _mapper.Map<BoardDTO>(await _boardRepository.GetById(id));

            if (board == null) return null;

            board.Columns = board.Columns.OrderBy(x => x.PositionInBoard);

            foreach (var column in board.Columns)
            {
                column.Cards = column.Cards.OrderBy(x => x.Priority);
            }

            return board;
        }

        public async Task<ValidationResult> AddBoard(AddBoardDTO addBoardDTO)
        {
            return await _mediatorHandler.SendCommand(new AddBoardCommand(addBoardDTO.BoardTitle));
        }

        public async Task<ValidationResult> UpdateBoardTitle(UpdateBoardTitleDTO updateBoardTitleDTO)
        {
            return await _mediatorHandler.SendCommand(new UpdateBoardTitleCommand(updateBoardTitleDTO.BoardId, updateBoardTitleDTO.NewBoardTitle));
        }

        public async Task<ValidationResult> AddColumnToBoard(AddColumnToBoardDTO addColumnToBoardDTO)
        {
            return await _mediatorHandler.SendCommand(new AddColumnToBoardCommand(addColumnToBoardDTO.BoardId, addColumnToBoardDTO.ColumnTitle));
        }

        public async Task<ValidationResult> UpdateColumnTitle(UpdateColumnTitleDTO updateColumnTitleDTO)
        {
            return await _mediatorHandler.SendCommand(new UpdateColumnTitleCommand(updateColumnTitleDTO.BoardId, updateColumnTitleDTO.ColumnId, updateColumnTitleDTO.NewColumnTitle));
        }

        public async Task<ValidationResult> UpdateColumnPositionInBoard(UpdateColumnPositionInBoardDTO updateColumnPositionInBoardDTO)
        {
            return await _mediatorHandler.SendCommand(new UpdateColumnPositionInBoardCommand(updateColumnPositionInBoardDTO.BoardId, updateColumnPositionInBoardDTO.ColumnId, updateColumnPositionInBoardDTO.NewColumnPositionInBoard));
        }

        public async Task<ValidationResult> AddCardToColumn(AddCardToColumnDTO addCardToColumnDTO)
        {
            return await _mediatorHandler.SendCommand(new AddCardToColumnCommand(addCardToColumnDTO.BoardId, addCardToColumnDTO.ColumnId, addCardToColumnDTO.CardTitle));
        }

        public async Task<ValidationResult> UpdateCardPriorityInColumn(UpdateCardPriorityInColumnDTO updateCardPriorityInColumnDTO)
        {
            return await _mediatorHandler.SendCommand(new UpdateCardPriorityInColumnCommand(updateCardPriorityInColumnDTO.BoardId, updateCardPriorityInColumnDTO.ColumnId, updateCardPriorityInColumnDTO.CardId, updateCardPriorityInColumnDTO.NewCardPriorityInColumn));
        }

        public async Task<ValidationResult> MoveCardBetweenColumns(MoveCardBetweenColumnsDTO moveCardBetweenColumnsDTO)
        {
            return await _mediatorHandler.SendCommand(new MoveCardBetweenColumnsCommand(moveCardBetweenColumnsDTO.BoardId, moveCardBetweenColumnsDTO.FromColumnId, moveCardBetweenColumnsDTO.ToColumnId, moveCardBetweenColumnsDTO.CardId, moveCardBetweenColumnsDTO.CardPriorityInColumn));
        }
    }
}
