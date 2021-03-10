using System;
using System.Collections.Generic;
using Todo.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Todo.Application.DTOs.BoardDTOs;
using Microsoft.AspNetCore.Authorization;

namespace Todo.Api.Controllers
{
    [Authorize]
    public class BoardsController : ApiController
    {
        private readonly IBoardAppService _boardAppService;

        public BoardsController(IBoardAppService boardAppService)
        {
            _boardAppService = boardAppService; 
        }

        [HttpGet]
        public async Task<IEnumerable<BoardDTO>> GetAllBoards()
        {
            return await _boardAppService.GetAllBoards();
        }

        [HttpGet("{boardId:guid}")]
        public async Task<BoardDTO> GetBoardById(Guid boardId)
        {
            return await _boardAppService.GetBoardById(boardId);
        }

        [HttpPost]
        public async Task<IActionResult> AddBoard([FromBody] AddBoardDTO addBoardDTO)
        {
            return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _boardAppService.AddBoard(addBoardDTO));
        }

        [HttpPut("title")]
        public async Task<IActionResult> UpdateBoardTitle([FromBody] UpdateBoardTitleDTO updateBoardTitleDTO)
        {
            return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _boardAppService.UpdateBoardTitle(updateBoardTitleDTO));
        }

        [HttpPost("columns")]
        public async Task<IActionResult> AddColumnToBoard([FromBody] AddColumnToBoardDTO addColumnToBoardDTO)
        {
            return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _boardAppService.AddColumnToBoard(addColumnToBoardDTO));
        }

        [HttpPut("columns/title")]
        public async Task<IActionResult> UpdateColumnTitle([FromBody] UpdateColumnTitleDTO updateColumnTitleDTO)
        {
            return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _boardAppService.UpdateColumnTitle(updateColumnTitleDTO));
        }

        [HttpPut("columns/position-in-board")]
        public async Task<IActionResult> UpdateColumnPositionInBoard([FromBody] UpdateColumnPositionInBoardDTO updateColumnPositionInBoardDTO)
        {
            return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _boardAppService.UpdateColumnPositionInBoard(updateColumnPositionInBoardDTO));
        }

        [HttpPost("cards")]
        public async Task<IActionResult> AddCardToColumn([FromBody] AddCardToColumnDTO addCardToColumnDTO)
        {
            return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _boardAppService.AddCardToColumn(addCardToColumnDTO));
        }

        [HttpPut("cards/priority")]
        public async Task<IActionResult> UpdateCardPriorityInColumn([FromBody] UpdateCardPriorityInColumnDTO updateCardPriorityInColumnDTO)
        {
            return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _boardAppService.UpdateCardPriorityInColumn(updateCardPriorityInColumnDTO));
        }

        [HttpPut("cards/move-to-column")]
        public async Task<IActionResult> MoveCardBetweenColumns([FromBody] MoveCardBetweenColumnsDTO moveCardBetweenColumnsDTO)
        {
            return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _boardAppService.MoveCardBetweenColumns(moveCardBetweenColumnsDTO));
        }
    }
}
