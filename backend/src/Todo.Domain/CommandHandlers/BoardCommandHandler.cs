using Todo.Domain.Commands.BoardCommands;
using Todo.Domain.Entities;
using Todo.Domain.Interfaces;
using Todo.Domain.Validators;
using FluentValidation.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Todo.Domain.CommandHandlers
{
    public class BoardCommandHandler : CommandHandler,
        IRequestHandler<AddBoardCommand, ValidationResult>,
        IRequestHandler<AddColumnToBoardCommand, ValidationResult>,
        IRequestHandler<UpdateColumnPositionInBoardCommand, ValidationResult>,
        IRequestHandler<UpdateBoardTitleCommand, ValidationResult>,
        IRequestHandler<UpdateColumnTitleCommand, ValidationResult>,
        IRequestHandler<AddCardToColumnCommand, ValidationResult>,
        IRequestHandler<UpdateCardPriorityInColumnCommand, ValidationResult>,
        IRequestHandler<MoveCardBetweenColumnsCommand, ValidationResult>
    {
        private readonly IBoardRepository _boardRepository;

        public BoardCommandHandler(IBoardRepository boardRepository)
        {
            _boardRepository = boardRepository;
        }

        public async Task<ValidationResult> Handle(AddBoardCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                return request.ValidationResult;
            }

            await _boardRepository.Add(new Board(request.BoardTitle));

            return await Commit(_boardRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(AddColumnToBoardCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                return request.ValidationResult;
            }

            var board = await _boardRepository.GetById(request.BoardId);
            if (board == null)
            {
                AddError(UserMessages.NotFound.Format("Board").Message);
                return ValidationResult;
            }

            var column = new Column(request.ColumnTitle);

            board.AddColumn(column);

            await _boardRepository.AddColumn(column);

            return await Commit(_boardRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(UpdateColumnPositionInBoardCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                return request.ValidationResult;
            }

            var board = await _boardRepository.GetById(request.BoardId);

            if (board == null)
            {
                AddError(UserMessages.NotFound.Format("Board").Message);
                return ValidationResult;
            }

            if (!board.ColumnExists(request.ColumnId))
            {
                AddError(UserMessages.NotFound.Format("Column").Message);
                return ValidationResult;
            }

            board.UpdateColumnPositionInBoard(request.ColumnId, request.NewColumnPositionInBoard);

            return await Commit(_boardRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(UpdateBoardTitleCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                return request.ValidationResult;
            }

            var board = await _boardRepository.GetById(request.BoardId);
            if (board == null)
            {
                AddError(UserMessages.NotFound.Format("Board").Message);
                return ValidationResult;
            }

            board.UpdateTitle(request.NewBoardTitle);

            return await Commit(_boardRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(UpdateColumnTitleCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                return request.ValidationResult;
            }

            var board = await _boardRepository.GetById(request.BoardId);

            if (board == null)
            {
                AddError(UserMessages.NotFound.Format("Board").Message);
                return ValidationResult;
            }

            if (!board.ColumnExists(request.ColumnId))
            {
                AddError(UserMessages.NotFound.Format("Column").Message);
                return ValidationResult;
            }

            board.UpdateColumnTitle(request.ColumnId, request.NewColumnTitle);

            return await Commit(_boardRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(AddCardToColumnCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                return request.ValidationResult;
            }

            var board = await _boardRepository.GetById(request.BoardId);
            if (board == null)
            {
                AddError(UserMessages.NotFound.Format("Board").Message);
                return ValidationResult;
            }

            if (!board.ColumnExists(request.ColumnId))
            {
                AddError(UserMessages.NotFound.Format("Column").Message);
                return ValidationResult;
            }

            var card = new Card(request.CardTitle);

            board.AddCardToColumn(request.ColumnId, card);

            await _boardRepository.AddCard(card);

            return await Commit(_boardRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(UpdateCardPriorityInColumnCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                return request.ValidationResult;
            }

            var board = await _boardRepository.GetById(request.BoardId);
            if (board == null)
            {
                AddError(UserMessages.NotFound.Format("Board").Message);
                return ValidationResult;
            }

            if (!board.ColumnExists(request.ColumnId))
            {
                AddError(UserMessages.NotFound.Format("Column").Message);
                return ValidationResult;
            }

            if (!board.CardExistsInColumn(request.ColumnId, request.CardId))
            {
                AddError(UserMessages.NotFound.Format("Card").Message);
                return ValidationResult;
            }

            board.UpdateCardPriorityInColumn(request.ColumnId, request.CardId, request.NewCardPriorityInColumn);

            return await Commit(_boardRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(MoveCardBetweenColumnsCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                return request.ValidationResult;
            }

            var board = await _boardRepository.GetById(request.BoardId);
            if (board == null)
            {
                AddError(UserMessages.NotFound.Format("Board").Message);
                return ValidationResult;
            }

            if (!board.ColumnExists(request.FromColumnId))
            {
                AddError(UserMessages.NotFound.Format("FromColumn").Message);
                return ValidationResult;
            }

            if (!board.ColumnExists(request.ToColumnId))
            {
                AddError(UserMessages.NotFound.Format("ToColumn").Message);
                return ValidationResult;
            }

            if (!board.CardExistsInColumn(request.FromColumnId, request.CardId))
            {
                AddError(UserMessages.NotFound.Format("Card").Message);
                return ValidationResult;
            }

            board.MoveCardBetweenColumns(request.CardId, request.FromColumnId, request.ToColumnId, request.CardPriorityInColumn);

            return await Commit(_boardRepository.UnitOfWork);
        }
    }
}
