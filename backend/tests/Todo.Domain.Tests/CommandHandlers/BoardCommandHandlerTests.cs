using Todo.Domain.CommandHandlers;
using Todo.Domain.Commands.BoardCommands;
using Todo.Domain.Entities;
using Todo.Domain.Interfaces;
using Todo.Domain.Validators;
using Moq;
using Moq.AutoMock;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Todo.Domain.Tests.CommandHandlers
{
    public class BoardCommandHandlerTests
    {
        private readonly AutoMocker _autoMocker;
        private readonly BoardCommandHandler _boardCommandHandler;

        public BoardCommandHandlerTests()
        {
            _autoMocker = new AutoMocker();
            _boardCommandHandler = _autoMocker.CreateInstance<BoardCommandHandler>();
        }

        #region AddBoardCommand
        [Fact]
        public async Task Handle_AddBoardCommand_ShouldFailValidation_WhenEmptyBoardTitle()
        {
            // Arrange
            var command = new AddBoardCommand("");

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.RequiredField.Format("BoardTitle").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_AddBoardCommand_ShouldAddAndCommit_WhenValid()
        {
            // Arrange
            var command = new AddBoardCommand("Personal Board");

            _autoMocker.GetMock<IBoardRepository>()
                .Setup(x => x.UnitOfWork).Returns(_autoMocker.GetMock<IUnitOfWork>().Object);

            _autoMocker.GetMock<IUnitOfWork>()
                .Setup(x => x.Commit()).Returns(Task.FromResult(true));

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            _autoMocker.GetMock<IBoardRepository>()
                .Verify(x => x.Add(It.IsAny<Board>()), Times.Once);

            _autoMocker.GetMock<IUnitOfWork>()
                .Verify(x => x.Commit(), Times.Once);

            Assert.True(validationResult.IsValid);
        }
        #endregion

        #region AddColumnToBoardCommand
        [Fact]
        public async Task Handle_AddColumnToBoardCommand_ShouldFailValidation_WhenEmptyBoardId()
        {
            // Arrange
            var command = new AddColumnToBoardCommand(Guid.Empty, "To Do");

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.RequiredField.Format("BoardId").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_AddColumnToBoardCommand_ShouldFailValidation_WhenEmptyColumnTitle()
        {
            // Arrange
            var command = new AddColumnToBoardCommand(Guid.NewGuid(), "");

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.RequiredField.Format("ColumnTitle").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_AddColumnToBoardCommand_ShouldFailValidation_WhenBoardNotFound()
        {
            // Arrange
            var command = new AddColumnToBoardCommand(Guid.NewGuid(), "To Do");

            _autoMocker.GetMock<IBoardRepository>()
                .Setup(x => x.GetById(command.BoardId)).ReturnsAsync((Board)null);

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            _autoMocker.GetMock<IBoardRepository>()
                .Verify(x => x.GetById(It.Is<Guid>(x => x.Equals(command.BoardId))), Times.Once);

            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.NotFound.Format("Board").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_AddColumnToBoardCommand_ShouldAddAndCommit_WhenValid()
        {
            // Arrange
            var board = new Board("Personal Board");
            var command = new AddColumnToBoardCommand(board.Id, "To Do");

            _autoMocker.GetMock<IBoardRepository>()
                .Setup(x => x.GetById(command.BoardId)).ReturnsAsync(board);

            _autoMocker.GetMock<IBoardRepository>()
                .Setup(x => x.UnitOfWork).Returns(_autoMocker.GetMock<IUnitOfWork>().Object);

            _autoMocker.GetMock<IUnitOfWork>()
                .Setup(x => x.Commit()).Returns(Task.FromResult(true));

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            _autoMocker.GetMock<IBoardRepository>()
                .Verify(x => x.GetById(It.Is<Guid>(x => x.Equals(command.BoardId))), Times.Once);

            _autoMocker.GetMock<IBoardRepository>()
                .Verify(x => x.AddColumn(It.Is<Column>(x => x.BoardId == board.Id && x.PositionInBoard == 1)), Times.Once);

            _autoMocker.GetMock<IUnitOfWork>()
                .Verify(x => x.Commit(), Times.Once);

            Assert.True(validationResult.IsValid);
        }
        #endregion

        #region UpdateBoardTitleCommand
        [Fact]
        public async Task Handle_UpdateBoardTitleCommand_ShouldFailValidation_WhenEmptyBoardId()
        {
            // Arrange
            var command = new UpdateBoardTitleCommand(Guid.Empty, "To do");

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.RequiredField.Format("BoardId").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_UpdateBoardTitleCommand_ShouldFailValidation_WhenEmptyNewBoardTitle()
        {
            // Arrange
            var command = new UpdateBoardTitleCommand(Guid.NewGuid(), "");

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.RequiredField.Format("NewBoardTitle").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_UpdateBoardTitleCommand_ShouldFailValidation_WhenBoardNotFound()
        {
            // Arrange
            var command = new UpdateBoardTitleCommand(Guid.NewGuid(), "To do");

            _autoMocker.GetMock<IBoardRepository>()
                .Setup(x => x.GetById(command.BoardId)).ReturnsAsync((Board)null);

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            _autoMocker.GetMock<IBoardRepository>()
                .Verify(x => x.GetById(It.Is<Guid>(x => x.Equals(command.BoardId))), Times.Once);

            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.NotFound.Format("Board").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_UpdateBoardTitleCommand_ShouldUpdateAndCommit_WhenValid()
        {
            // Arrange
            var board = new Board("Personal Board");

            var command = new UpdateBoardTitleCommand(board.Id, "Work Board");

            _autoMocker.GetMock<IBoardRepository>()
                .Setup(x => x.GetById(command.BoardId)).ReturnsAsync(board);

            _autoMocker.GetMock<IBoardRepository>()
                .Setup(x => x.UnitOfWork).Returns(_autoMocker.GetMock<IUnitOfWork>().Object);

            _autoMocker.GetMock<IUnitOfWork>()
                .Setup(x => x.Commit()).Returns(Task.FromResult(true));

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            _autoMocker.GetMock<IBoardRepository>()
                .Verify(x => x.GetById(It.Is<Guid>(x => x.Equals(command.BoardId))), Times.Once);

            _autoMocker.GetMock<IUnitOfWork>()
                .Verify(x => x.Commit(), Times.Once);

            Assert.True(validationResult.IsValid);
        }
        #endregion

        #region UpdateColumnPositionInBoardCommand
        [Fact]
        public async Task Handle_UpdateColumnPositionInBoardCommand_ShouldFailValidation_WhenEmptyBoardId()
        {
            // Arrange
            var command = new UpdateColumnPositionInBoardCommand(Guid.Empty, Guid.NewGuid(), 1);

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.RequiredField.Format("BoardId").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_UpdateColumnPositionInBoardCommand_ShouldFailValidation_WhenEmptyColumnId()
        {
            // Arrange
            var command = new UpdateColumnPositionInBoardCommand(Guid.NewGuid(), Guid.Empty, 1);

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.RequiredField.Format("ColumnId").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_UpdateColumnPositionInBoardCommand_ShouldFailValidation_WhenNewColumnPositionInBoardLessThanOne()
        {
            // Arrange
            var command = new UpdateColumnPositionInBoardCommand(Guid.NewGuid(), Guid.NewGuid(), 0);

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.MustBeGreatherThan.Format("NewColumnPositionInBoard", 0).Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_UpdateColumnPositionInBoardCommand_ShouldFailValidation_WhenBoardNotFound()
        {
            // Arrange
            var command = new UpdateColumnPositionInBoardCommand(Guid.NewGuid(), Guid.NewGuid(), 1);

            _autoMocker.GetMock<IBoardRepository>()
                .Setup(x => x.GetById(command.BoardId)).ReturnsAsync((Board)null);

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            _autoMocker.GetMock<IBoardRepository>()
                .Verify(x => x.GetById(It.Is<Guid>(x => x.Equals(command.BoardId))), Times.Once);

            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.NotFound.Format("Board").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_UpdateColumnPositionInBoardCommand_ShouldFailValidation_WhenColumnNotFoundInBoard()
        {
            // Arrange
            var board = new Board("Personal Board");

            var command = new UpdateColumnPositionInBoardCommand(board.Id, Guid.NewGuid(), 1);

            _autoMocker.GetMock<IBoardRepository>()
                .Setup(x => x.GetById(command.BoardId)).ReturnsAsync(board);

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            _autoMocker.GetMock<IBoardRepository>()
                .Verify(x => x.GetById(It.Is<Guid>(x => x.Equals(command.BoardId))), Times.Once);

            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.NotFound.Format("Column").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_UpdateColumnPositionInBoardCommand_ShouldUpdateAndCommit_WhenValid()
        {
            // Arrange
            var board = new Board("Personal Board");

            var todoColumn = new Column("To do");
            var doingColumn = new Column("Doing");
            var doneColumn = new Column("Done");

            board.AddColumn(doneColumn);
            board.AddColumn(todoColumn);
            board.AddColumn(doingColumn);

            var command = new UpdateColumnPositionInBoardCommand(board.Id, doneColumn.Id, 3);

            _autoMocker.GetMock<IBoardRepository>()
                .Setup(x => x.GetById(command.BoardId)).ReturnsAsync(board);

            _autoMocker.GetMock<IBoardRepository>()
                .Setup(x => x.UnitOfWork).Returns(_autoMocker.GetMock<IUnitOfWork>().Object);

            _autoMocker.GetMock<IUnitOfWork>()
                .Setup(x => x.Commit()).Returns(Task.FromResult(true));

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            _autoMocker.GetMock<IBoardRepository>()
                .Verify(x => x.GetById(It.Is<Guid>(x => x.Equals(command.BoardId))), Times.Once);

            _autoMocker.GetMock<IUnitOfWork>()
                .Verify(x => x.Commit(), Times.Once);

            Assert.True(validationResult.IsValid);
        }
        #endregion

        #region UpdateColumnTitleCommand
        [Fact]
        public async Task Handle_UpdateColumnTitleCommand_ShouldFailValidation_WhenEmptyBoardId()
        {
            // Arrange
            var command = new UpdateColumnTitleCommand(Guid.Empty, Guid.NewGuid(), "To do");

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.RequiredField.Format("BoardId").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_UpdateColumnTitleCommand_ShouldFailValidation_WhenEmptyColumnId()
        {
            // Arrange
            var command = new UpdateColumnTitleCommand(Guid.NewGuid(), Guid.Empty, "To do");

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.RequiredField.Format("ColumnId").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_UpdateColumnTitleCommand_ShouldFailValidation_WhenEmptyNewColumnTitle()
        {
            // Arrange
            var command = new UpdateColumnTitleCommand(Guid.NewGuid(), Guid.NewGuid(), "");

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.RequiredField.Format("NewColumnTitle").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_UpdateColumnTitleCommand_ShouldFailValidation_WhenBoardNotFound()
        {
            // Arrange
            var command = new UpdateColumnTitleCommand(Guid.NewGuid(), Guid.NewGuid(), "To do");

            _autoMocker.GetMock<IBoardRepository>()
                .Setup(x => x.GetById(command.BoardId)).ReturnsAsync((Board)null);

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            _autoMocker.GetMock<IBoardRepository>()
                .Verify(x => x.GetById(It.Is<Guid>(x => x.Equals(command.BoardId))), Times.Once);

            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.NotFound.Format("Board").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_UpdateColumnTitleCommand_ShouldFailValidation_WhenColumnNotFoundInBoard()
        {
            // Arrange
            var board = new Board("Personal Board");
            var command = new UpdateColumnTitleCommand(board.Id, Guid.NewGuid(), "To do");

            _autoMocker.GetMock<IBoardRepository>()
                .Setup(x => x.GetById(command.BoardId)).ReturnsAsync(board);

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            _autoMocker.GetMock<IBoardRepository>()
                .Verify(x => x.GetById(It.Is<Guid>(x => x.Equals(command.BoardId))), Times.Once);

            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.NotFound.Format("Column").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_UpdateColumnTitleCommand_ShouldUpdateAndCommit_WhenValid()
        {
            // Arrange
            var board = new Board("Personal Board");
            var column = new Column("To do");
            board.AddColumn(column);

            var command = new UpdateColumnTitleCommand(board.Id, column.Id, "Doing");

            _autoMocker.GetMock<IBoardRepository>()
                .Setup(x => x.GetById(command.BoardId)).ReturnsAsync(board);

            _autoMocker.GetMock<IBoardRepository>()
                .Setup(x => x.UnitOfWork).Returns(_autoMocker.GetMock<IUnitOfWork>().Object);

            _autoMocker.GetMock<IUnitOfWork>()
                .Setup(x => x.Commit()).Returns(Task.FromResult(true));

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            _autoMocker.GetMock<IBoardRepository>()
                .Verify(x => x.GetById(It.Is<Guid>(x => x.Equals(command.BoardId))), Times.Once);

            _autoMocker.GetMock<IUnitOfWork>()
                .Verify(x => x.Commit(), Times.Once);

            Assert.True(validationResult.IsValid);
        }
        #endregion

        #region AddCardToColumn
        [Fact]
        public async Task Handle_AddCardToColumnCommand_ShouldFailValidation_WhenEmptyBoardId()
        {
            // Arrange
            var command = new AddCardToColumnCommand(Guid.Empty, Guid.NewGuid(), "Sleep");

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.RequiredField.Format("BoardId").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_AddCardToColumnCommand_ShouldFailValidation_WhenEmptyColumnId()
        {
            // Arrange
            var command = new AddCardToColumnCommand(Guid.NewGuid(), Guid.Empty, "Sleep");

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.RequiredField.Format("ColumnId").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_AddCardToColumnCommand_ShouldFailValidation_WhenEmptyCardTitle()
        {
            // Arrange
            var command = new AddCardToColumnCommand(Guid.NewGuid(), Guid.NewGuid(), "");

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.RequiredField.Format("CardTitle").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_AddCardToColumnCommand_ShouldFailValidation_WhenBoardNotFound()
        {
            // Arrange
            var command = new AddCardToColumnCommand(Guid.NewGuid(), Guid.NewGuid(), "Sleep");

            _autoMocker.GetMock<IBoardRepository>()
                .Setup(x => x.GetById(command.BoardId)).ReturnsAsync((Board)null);

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            _autoMocker.GetMock<IBoardRepository>()
                .Verify(x => x.GetById(It.Is<Guid>(x => x.Equals(command.BoardId))), Times.Once);

            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.NotFound.Format("Board").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_AddCardToColumnCommand_ShouldFailValidation_WhenColumnNotFoundInBoard()
        {
            // Arrange
            var board = new Board("Personal Board");
            var command = new AddCardToColumnCommand(board.Id, Guid.NewGuid(), "Sleep");

            _autoMocker.GetMock<IBoardRepository>()
                .Setup(x => x.GetById(command.BoardId)).ReturnsAsync(board);

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            _autoMocker.GetMock<IBoardRepository>()
                .Verify(x => x.GetById(It.Is<Guid>(x => x.Equals(command.BoardId))), Times.Once);

            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.NotFound.Format("Column").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_AddCardToColumnCommand_ShouldAddAndCommit_WhenValid()
        {
            // Arrange
            var board = new Board("Personal Board");
            var column = new Column("To do");
            board.AddColumn(column);

            var command = new AddCardToColumnCommand(board.Id, column.Id, "Sleep");

            _autoMocker.GetMock<IBoardRepository>()
                .Setup(x => x.GetById(command.BoardId)).ReturnsAsync(board);

            _autoMocker.GetMock<IBoardRepository>()
                .Setup(x => x.UnitOfWork).Returns(_autoMocker.GetMock<IUnitOfWork>().Object);

            _autoMocker.GetMock<IUnitOfWork>()
                .Setup(x => x.Commit()).Returns(Task.FromResult(true));

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            _autoMocker.GetMock<IBoardRepository>()
                .Verify(x => x.GetById(It.Is<Guid>(x => x.Equals(command.BoardId))), Times.Once);

            _autoMocker.GetMock<IBoardRepository>()
                .Verify(x => x.AddCard(It.Is<Card>(x => x.ColumnId == column.Id && x.Priority == 1)), Times.Once);

            _autoMocker.GetMock<IUnitOfWork>()
                .Verify(x => x.Commit(), Times.Once);

            Assert.True(validationResult.IsValid);
        }
        #endregion

        #region UpdateCardPriorityInColumnCommand
        [Fact]
        public async Task Handle_UpdateCardPriorityInColumnCommand_ShouldFailValidation_WhenEmptyBoardId()
        {
            // Arrange
            var command = new UpdateCardPriorityInColumnCommand(Guid.Empty, Guid.NewGuid(), Guid.NewGuid(), 2);

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.RequiredField.Format("BoardId").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_UpdateCardPriorityInColumnCommand_ShouldFailValidation_WhenEmptyColumnId()
        {
            // Arrange
            var command = new UpdateCardPriorityInColumnCommand(Guid.NewGuid(), Guid.Empty, Guid.NewGuid(), 2);

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.RequiredField.Format("ColumnId").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_UpdateCardPriorityInColumnCommand_ShouldFailValidation_WhenEmptyCardId()
        {
            // Arrange
            var command = new UpdateCardPriorityInColumnCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.Empty, 2);

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.RequiredField.Format("CardId").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_UpdateCardPriorityInColumnCommand_ShouldFailValidation_WhenNewPriorityInColumnLessThanOne()
        {
            // Arrange
            var command = new UpdateCardPriorityInColumnCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 0);

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.MustBeGreatherThan.Format("NewCardPriorityInColumn", 0).Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_UpdateCardPriorityInColumnCommand_ShouldFailValidation_WhenBoardNotFound()
        {
            // Arrange
            var board = new Board("Personal Board");

            var command = new UpdateCardPriorityInColumnCommand(board.Id, Guid.NewGuid(), Guid.NewGuid(), 2);

            _autoMocker.GetMock<IBoardRepository>()
                .Setup(x => x.GetById(command.BoardId)).ReturnsAsync((Board)null);

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            _autoMocker.GetMock<IBoardRepository>()
                .Verify(x => x.GetById(It.Is<Guid>(x => x.Equals(command.BoardId))), Times.Once);

            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.NotFound.Format("Board").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_UpdateCardPriorityInColumnCommand_ShouldFailValidation_WhenColumnNotFoundInBoard()
        {
            // Arrange
            var board = new Board("Personal Board");

            var command = new UpdateCardPriorityInColumnCommand(board.Id, Guid.NewGuid(), Guid.NewGuid(), 2);

            _autoMocker.GetMock<IBoardRepository>()
                .Setup(x => x.GetById(command.BoardId)).ReturnsAsync(board);

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            _autoMocker.GetMock<IBoardRepository>()
                .Verify(x => x.GetById(It.Is<Guid>(x => x.Equals(command.BoardId))), Times.Once);

            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.NotFound.Format("Column").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_UpdateCardPriorityInColumnCommand_ShouldFailValidation_WhenCardNotFoundInColumn()
        {
            // Arrange
            var board = new Board("Personal Board");
            var column = new Column("To do");
            board.AddColumn(column);

            var command = new UpdateCardPriorityInColumnCommand(board.Id, column.Id, Guid.NewGuid(), 2);

            _autoMocker.GetMock<IBoardRepository>()
                .Setup(x => x.GetById(command.BoardId)).ReturnsAsync(board);

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            _autoMocker.GetMock<IBoardRepository>()
                .Verify(x => x.GetById(It.Is<Guid>(x => x.Equals(command.BoardId))), Times.Once);

            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.NotFound.Format("Card").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_UpdateCardPriorityInColumnCommand_ShouldUpdateAndCommit_WhenValid()
        {
            // Arrange
            var board = new Board("Personal Board");
            var column = new Column("To do");
            board.AddColumn(column);

            var sleepCard = new Card("Sleep");
            var workCard = new Card("Work");

            board.AddCardToColumn(column.Id, sleepCard);
            board.AddCardToColumn(column.Id, workCard);

            var command = new UpdateCardPriorityInColumnCommand(board.Id, column.Id, sleepCard.Id, 2);

            _autoMocker.GetMock<IBoardRepository>()
                .Setup(x => x.GetById(command.BoardId)).ReturnsAsync(board);

            _autoMocker.GetMock<IBoardRepository>()
                .Setup(x => x.UnitOfWork).Returns(_autoMocker.GetMock<IUnitOfWork>().Object);

            _autoMocker.GetMock<IUnitOfWork>()
                .Setup(x => x.Commit()).Returns(Task.FromResult(true));

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            _autoMocker.GetMock<IBoardRepository>()
                .Verify(x => x.GetById(It.Is<Guid>(x => x.Equals(command.BoardId))), Times.Once);

            _autoMocker.GetMock<IUnitOfWork>()
                .Verify(x => x.Commit(), Times.Once);

            Assert.True(validationResult.IsValid);
        }
        #endregion

        #region MoveCardBetweenColumnsCommand
        [Fact]
        public async Task Handle_MoveCardBetweenColumnsCommand_ShouldFailValidation_WhenEmptyBoardId()
        {
            // Arrange
            var command = new MoveCardBetweenColumnsCommand(Guid.Empty, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1);

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.RequiredField.Format("BoardId").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_MoveCardBetweenColumnsCommand_ShouldFailValidation_WhenEmptyFromColumnId()
        {
            // Arrange
            var command = new MoveCardBetweenColumnsCommand(Guid.NewGuid(), Guid.Empty, Guid.NewGuid(), Guid.NewGuid(), 1);

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.RequiredField.Format("FromColumnId").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_MoveCardBetweenColumnsCommand_ShouldFailValidation_WhenEmptyToColumnId()
        {
            // Arrange
            var command = new MoveCardBetweenColumnsCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.Empty, Guid.NewGuid(), 1);

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.RequiredField.Format("ToColumnId").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_MoveCardBetweenColumnsCommand_ShouldFailValidation_WhenEmptyCardId()
        {
            // Arrange
            var command = new MoveCardBetweenColumnsCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.Empty, 1);

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.RequiredField.Format("CardId").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_MoveCardBetweenColumnsCommand_ShouldFailValidation_WhenPriorityInColumnLessThanOne()
        {
            // Arrange
            var command = new MoveCardBetweenColumnsCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 0);

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.MustBeGreatherThan.Format("CardPriorityInColumn", 0).Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_MoveCardBetweenColumnsCommand_ShouldFailValidation_WhenBoardNotFound()
        {
            // Arrange
            var command = new MoveCardBetweenColumnsCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1);

            _autoMocker.GetMock<IBoardRepository>()
                .Setup(x => x.GetById(command.BoardId)).ReturnsAsync((Board)null);

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            _autoMocker.GetMock<IBoardRepository>()
                .Verify(x => x.GetById(It.Is<Guid>(x => x.Equals(command.BoardId))), Times.Once);

            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.NotFound.Format("Board").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_MoveCardBetweenColumnsCommand_ShouldFailValidation_WhenFromColumnNotFoundInBoard()
        {
            // Arrange
            var board = new Board("Personal Board");

            var command = new MoveCardBetweenColumnsCommand(board.Id, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1);

            _autoMocker.GetMock<IBoardRepository>()
                .Setup(x => x.GetById(command.BoardId)).ReturnsAsync(board);

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            _autoMocker.GetMock<IBoardRepository>()
                .Verify(x => x.GetById(It.Is<Guid>(x => x.Equals(command.BoardId))), Times.Once);

            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.NotFound.Format("FromColumn").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_MoveCardBetweenColumnsCommand_ShouldFailValidation_WhenToColumnNotFoundInBoard()
        {
            // Arrange
            var board = new Board("Personal Board");
            var fromColumn = new Column("To do");
            board.AddColumn(fromColumn);

            var command = new MoveCardBetweenColumnsCommand(board.Id, fromColumn.Id, Guid.NewGuid(), Guid.NewGuid(), 1);

            _autoMocker.GetMock<IBoardRepository>()
                .Setup(x => x.GetById(command.BoardId)).ReturnsAsync(board);

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            _autoMocker.GetMock<IBoardRepository>()
                .Verify(x => x.GetById(It.Is<Guid>(x => x.Equals(command.BoardId))), Times.Once);

            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.NotFound.Format("ToColumn").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_MoveCardBetweenColumnsCommand_ShouldFailValidation_WhenCardNotFoundInFromColumn()
        {
            // Arrange
            var board = new Board("Personal Board");
            var fromColumn = new Column("To do");
            board.AddColumn(fromColumn);

            var toColumn = new Column("Doing");
            board.AddColumn(toColumn);

            var command = new MoveCardBetweenColumnsCommand(board.Id, fromColumn.Id, toColumn.Id, Guid.NewGuid(), 2);

            _autoMocker.GetMock<IBoardRepository>()
                .Setup(x => x.GetById(command.BoardId)).ReturnsAsync(board);

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            _autoMocker.GetMock<IBoardRepository>()
                .Verify(x => x.GetById(It.Is<Guid>(x => x.Equals(command.BoardId))), Times.Once);

            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.Equal(UserMessages.NotFound.Format("Card").Message, validationResult.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task Handle_MoveCardBetweenColumnsCommand_ShouldUpdateAndCommit_WhenValid()
        {
            // Arrange
            var board = new Board("Personal Board");
            var fromColumn = new Column("To do");
            board.AddColumn(fromColumn);

            var card = new Card("Sleep");
            board.AddCardToColumn(fromColumn.Id, card);

            var toColumn = new Column("Doing");
            board.AddColumn(toColumn);

            var command = new MoveCardBetweenColumnsCommand(board.Id, fromColumn.Id, toColumn.Id, card.Id, 2);

            _autoMocker.GetMock<IBoardRepository>()
                .Setup(x => x.GetById(command.BoardId)).ReturnsAsync(board);

            _autoMocker.GetMock<IBoardRepository>()
                .Setup(x => x.UnitOfWork).Returns(_autoMocker.GetMock<IUnitOfWork>().Object);

            _autoMocker.GetMock<IUnitOfWork>()
                .Setup(x => x.Commit()).Returns(Task.FromResult(true));

            // Act
            var validationResult = await _boardCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            _autoMocker.GetMock<IBoardRepository>()
                .Verify(x => x.GetById(It.Is<Guid>(x => x.Equals(command.BoardId))), Times.Once);

            _autoMocker.GetMock<IUnitOfWork>()
                .Verify(x => x.Commit(), Times.Once);

            Assert.True(validationResult.IsValid);
        }
        #endregion
    }
}
