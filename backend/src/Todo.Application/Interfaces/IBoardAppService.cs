using Todo.Application.DTOs.BoardDTOs;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Todo.Application.Interfaces
{
    public interface IBoardAppService
    {
        Task<IEnumerable<BoardDTO>> GetAllBoards();
        Task<BoardDTO> GetBoardById(Guid id);

        Task<ValidationResult> AddBoard(AddBoardDTO addBoardDTO);
        Task<ValidationResult> UpdateBoardTitle(UpdateBoardTitleDTO updateBoardTitleDTO);

        Task<ValidationResult> AddColumnToBoard(AddColumnToBoardDTO addColumnToBoardDTO);
        Task<ValidationResult> UpdateColumnTitle(UpdateColumnTitleDTO updateColumnTitleDTO); 
        Task<ValidationResult> UpdateColumnPositionInBoard(UpdateColumnPositionInBoardDTO updateColumnPositionInBoardDTO);

        Task<ValidationResult> AddCardToColumn(AddCardToColumnDTO addCardToColumnDTO);
        Task<ValidationResult> UpdateCardPriorityInColumn(UpdateCardPriorityInColumnDTO updateCardPriorityInColumnDTO);
        Task<ValidationResult> MoveCardBetweenColumns(MoveCardBetweenColumnsDTO moveCardBetweenColumnsDTO);
    }
}
