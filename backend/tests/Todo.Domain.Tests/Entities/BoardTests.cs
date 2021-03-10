using Todo.Domain.Entities;
using Todo.Domain.Validators;
using System;
using Xunit;

namespace Todo.Domain.Tests.Entities
{
    public class BoardTests
    {
        #region Constructor
        [Fact]
        public void Constructor_ShouldThrowException_WhenEmptyTitle()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<Exception>(() => new Board(""));
            Assert.Equal(UserMessages.RequiredField.Format("Title").Message, ex.Message);
        }

        [Fact]
        public void Constructor_ShouldInstantiate_WhenValid()
        {
            // Arrange & Act
            var board = new Board("Personal Board");

            // Assert
            Assert.Equal("Personal Board", board.Title);
        }
        #endregion

        #region AddColumn
        [Fact]
        public void AddColumn_ShouldThrowException_WhenEmptyColumn()
        {
            // Arrange
            var board = new Board("Personal Board");

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => board.AddColumn(null));
            Assert.Equal(UserMessages.RequiredField.Format("Column").Message, ex.Message);
        }

        [Fact]
        public void AddColumn_ShouldThrowException_WhenTryingToAddAColumnThatAlreadyInList()
        {
            // Arrange
            var board = new Board("Personal Board");
            var column = new Column("To do");
            board.AddColumn(column);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => board.AddColumn(column));
            Assert.Equal("That Column already is in the Board.", ex.Message);
        }

        [Fact]
        public void AddColumn_ShouldDefineColumnBoardIdEqualsToBoardId()
        {
            // Arrange
            var board = new Board("Personal Board");
            var column = new Column("To Do");

            // Act
            board.AddColumn(column);

            // Assert
            Assert.Equal(board.Id, column.BoardId);
        }

        [Fact]
        public void AddColumn_ShouldAlwaysDefineColumnPositionInBoardForTheLast()
        {
            // Arrange
            var board = new Board("Personal Board");

            var todoColumn = new Column("To Do");
            var doingColumn = new Column("Doing");
            var doneColumn = new Column("Done");

            // Act
            board.AddColumn(todoColumn);
            board.AddColumn(doingColumn);
            board.AddColumn(doneColumn);

            // Assert
            Assert.Equal(1, todoColumn.PositionInBoard);
            Assert.Equal(2, doingColumn.PositionInBoard);
            Assert.Equal(3, doneColumn.PositionInBoard);
        }
        #endregion

        #region ColumnExists
        [Fact]
        public void ColumnExists_ShouldThrowException_WhenEmptyColumnId()
        {
            // Arrange
            var board = new Board("Personal Board");

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => board.ColumnExists(Guid.Empty));
            Assert.Equal(UserMessages.RequiredField.Format("ColumnId").Message, ex.Message);
        }

        [Fact]
        public void ColumnExists_ShouldReturnTrue_WhenColumnInList()
        {
            // Arrange
            var board = new Board("Personal Board");
            var todoColumn = new Column("To Do");
            board.AddColumn(todoColumn);

            // Act
            var columnExists = board.ColumnExists(todoColumn.Id);

            // Assert
            Assert.True(columnExists);
        }

        [Fact]
        public void ColumnExists_ShouldReturnFalse_WhenColumnNotInList()
        {
            // Arrange
            var board = new Board("Personal Board");
            var todoColumn = new Column("To Do");

            // Act
            var columnExists = board.ColumnExists(todoColumn.Id);

            // Assert
            Assert.False(columnExists);
        }
        #endregion

        #region UpdateColumnPositionInBoard
        [Fact]
        public void UpdateColumnPositionInBoard_ShouldThrowException_WhenPositionLessThanOne()
        {
            // Arrange
            var board = new Board("Personal Board");

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => board.UpdateColumnPositionInBoard(Guid.NewGuid(), 0));
            Assert.Equal(UserMessages.MustBeGreatherThan.Format("PositionInBoard", 0).Message, ex.Message);
        }

        [Fact]
        public void UpdateColumnPositionInBoard_ShouldThrowException_WhenEmptyColumnId()
        {
            // Arrange
            var board = new Board("Personal Board");

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => board.UpdateColumnPositionInBoard(Guid.Empty, 5));
            Assert.Equal(UserMessages.RequiredField.Format("ColumnId").Message, ex.Message);
        }

        [Fact]
        public void UpdateColumnPositionInBoard_ShouldThrowException_WhenColumnNotFoundInList()
        {
            // Arrange
            var board = new Board("Personal Board");

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => board.UpdateColumnPositionInBoard(Guid.NewGuid(), 1));
            Assert.Equal(UserMessages.NotFound.Format("Column").Message, ex.Message);
        }

        [Fact]
        public void UpdateColumnPositionInBoard_ShouldDoNothing_WhenColumnIsAlreadyOnThatPosition()
        {
            // Arrange
            var board = new Board("Personal Board");

            var todoColumn = new Column("To Do");
            var doingColumn = new Column("Doing");
            var doneColumn = new Column("Done");

            board.AddColumn(todoColumn);
            board.AddColumn(doingColumn);
            board.AddColumn(doneColumn);

            // Act
            board.UpdateColumnPositionInBoard(doneColumn.Id, 3);

            // Assert
            Assert.Equal(1, todoColumn.PositionInBoard);
            Assert.Equal(2, doingColumn.PositionInBoard);
            Assert.Equal(3, doneColumn.PositionInBoard);
        }

        [Fact]
        public void UpdateColumnPositionInBoard_ShouldInvertTodoColumnWithDoingColumnAndKeepDoneColumnInSamePosition()
        {
            // Arrange
            var board = new Board("Personal Board");

            var todoColumn = new Column("To Do");
            var doingColumn = new Column("Doing");
            var doneColumn = new Column("Done");

            board.AddColumn(todoColumn);
            board.AddColumn(doingColumn);
            board.AddColumn(doneColumn);

            // Act
            board.UpdateColumnPositionInBoard(todoColumn.Id, 2);

            // Assert
            Assert.Equal(1, doingColumn.PositionInBoard);
            Assert.Equal(2, todoColumn.PositionInBoard);
            Assert.Equal(3, doneColumn.PositionInBoard);
        }

        [Fact]
        public void UpdateColumnPositionInBoard_ShouldDefineTodoColumnPositionInBoardToThreeAndChangeOthersBySubtractingOneToCurrentPosition()
        {
            // Arrange
            var board = new Board("Personal Board");

            var todoColumn = new Column("To Do");
            var doingColumn = new Column("Doing");
            var doneColumn = new Column("Done");

            board.AddColumn(todoColumn);
            board.AddColumn(doingColumn);
            board.AddColumn(doneColumn);

            // Act
            board.UpdateColumnPositionInBoard(todoColumn.Id, 3);

            // Assert
            Assert.Equal(1, doingColumn.PositionInBoard);
            Assert.Equal(2, doneColumn.PositionInBoard);
            Assert.Equal(3, todoColumn.PositionInBoard);
        }

        [Fact]
        public void UpdateColumnPositionInBoard_ShouldInvertDoingColumnWithTodoColumnAndKeepDoneColumnInSamePosition()
        {
            // Arrange
            var board = new Board("Personal Board");

            var todoColumn = new Column("To Do");
            var doingColumn = new Column("Doing");
            var doneColumn = new Column("Done");

            board.AddColumn(todoColumn);
            board.AddColumn(doingColumn);
            board.AddColumn(doneColumn);

            // Act
            board.UpdateColumnPositionInBoard(doingColumn.Id, 1);

            // Assert
            Assert.Equal(1, doingColumn.PositionInBoard);
            Assert.Equal(2, todoColumn.PositionInBoard);
            Assert.Equal(3, doneColumn.PositionInBoard);
        }

        [Fact]
        public void UpdateColumnPositionInBoard_ShouldInvertDoingColumnWithDoneColumnAndKeepTodoColumnInSamePosition()
        {
            // Arrange
            var board = new Board("Personal Board");

            var todoColumn = new Column("To Do");
            var doingColumn = new Column("Doing");
            var doneColumn = new Column("Done");

            board.AddColumn(todoColumn);
            board.AddColumn(doingColumn);
            board.AddColumn(doneColumn);

            // Act
            board.UpdateColumnPositionInBoard(doingColumn.Id, 3);

            // Assert
            Assert.Equal(1, todoColumn.PositionInBoard);
            Assert.Equal(2, doneColumn.PositionInBoard);
            Assert.Equal(3, doingColumn.PositionInBoard);
        }

        [Fact]
        public void UpdateColumnPositionInBoard_ShouldDefineDoneColumnPositionInBoardToOneAndChangeOthersByAddingOneToCurrentPosition()
        {
            // Arrange
            var board = new Board("Personal Board");

            var todoColumn = new Column("To Do");
            var doingColumn = new Column("Doing");
            var doneColumn = new Column("Done");

            board.AddColumn(todoColumn);
            board.AddColumn(doingColumn);
            board.AddColumn(doneColumn);

            // Act
            board.UpdateColumnPositionInBoard(doneColumn.Id, 1);

            // Assert
            Assert.Equal(1, doneColumn.PositionInBoard);
            Assert.Equal(2, todoColumn.PositionInBoard);
            Assert.Equal(3, doingColumn.PositionInBoard);
        }

        [Fact]
        public void UpdateColumnPositionInBoard_ShouldInvertDoneColumnWithDoingColumnAndKeepTodoColumnInSamePosition()
        {
            // Arrange
            var board = new Board("Personal Board");

            var todoColumn = new Column("To Do");
            var doingColumn = new Column("Doing");
            var doneColumn = new Column("Done");

            board.AddColumn(todoColumn);
            board.AddColumn(doingColumn);
            board.AddColumn(doneColumn);

            // Act
            board.UpdateColumnPositionInBoard(doneColumn.Id, 2);

            // Assert
            Assert.Equal(1, todoColumn.PositionInBoard);
            Assert.Equal(2, doneColumn.PositionInBoard);
            Assert.Equal(3, doingColumn.PositionInBoard);
        }

        [Fact]
        public void UpdateColumnPositionInBoard_ShouldDefinePositionInBoardEqualsToLengthIfInformedHigherThanTheLength()
        {
            // Arrange
            var board = new Board("Personal Board");

            var todoColumn = new Column("To Do");
            var doingColumn = new Column("Doing");
            var doneColumn = new Column("Done");

            board.AddColumn(todoColumn);
            board.AddColumn(doingColumn);
            board.AddColumn(doneColumn);

            // Act
            board.UpdateColumnPositionInBoard(doneColumn.Id, 4);

            // Assert
            Assert.Equal(1, todoColumn.PositionInBoard);
            Assert.Equal(2, doingColumn.PositionInBoard);
            Assert.Equal(3, doneColumn.PositionInBoard);
        }
        #endregion

        #region UpdateTitle
        [Fact]
        public void UpdateTitle_ShouldThrowException_WhenEmptyNewTitle()
        {
            // Arrange
            var board = new Board("Personal Board");

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => board.UpdateTitle(""));
            Assert.Equal(UserMessages.RequiredField.Format("NewTitle").Message, ex.Message);
        }

        [Fact]
        public void UpdateTitle_ShouldChangeActualTitleForNewTitle()
        {
            // Arrange
            var board = new Board("Personal Board");

            // Act
            board.UpdateTitle("Work Board");

            // Assert
            Assert.Equal("Work Board", board.Title);
        }
        #endregion

        #region UpdateColumnTitle
        [Fact]
        public void UpdateColumnTitle_ShouldThrowException_WhenEmptyColumnNewTitle()
        {
            // Arrange
            var board = new Board("Personal Board");
            var column = new Column("To do");
            board.AddColumn(column);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => board.UpdateColumnTitle(column.Id, ""));
            Assert.Equal(UserMessages.RequiredField.Format("ColumnNewTitle").Message, ex.Message);
        }

        [Fact]
        public void UpdateColumnTitle_ShouldThrowException_WhenEmptyColumnId()
        {
            // Arrange
            var board = new Board("Personal Board");

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => board.UpdateColumnTitle(Guid.Empty, "Doing"));
            Assert.Equal(UserMessages.RequiredField.Format("ColumnId").Message, ex.Message);
        }

        [Fact]
        public void UpdateColumnTitle_ShouldThrowException_WhenColumnNotFound()
        {
            // Arrange
            var board = new Board("Personal Board");

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => board.UpdateColumnTitle(Guid.NewGuid(), "Doing"));
            Assert.Equal(UserMessages.NotFound.Format("Column").Message, ex.Message);
        }

        [Fact]
        public void UpdateColumnTitle_ShouldChangeActualColumnTitleForColumnNewTitle()
        {
            // Arrange
            var board = new Board("Personal Board");
            var column = new Column("To do");
            board.AddColumn(column);

            // Act
            board.UpdateColumnTitle(column.Id, "Doing");

            // Assert
            Assert.Equal("Doing", column.Title);
        }
        #endregion

        #region AddCardToColumn
        [Fact]
        public void AddCardToColumn_ShouldThrowException_WhenEmptyColumnId()
        {
            // Arrange
            var board = new Board("Personal Board");
            var card = new Card("Sleep");

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => board.AddCardToColumn(Guid.Empty, card));
            Assert.Equal(UserMessages.RequiredField.Format("ColumnId").Message, ex.Message);
        }

        [Fact]
        public void AddCardToColumn_ShouldThrowException_WhenColumnNotFoundInList()
        {
            // Arrange
            var board = new Board("Personal Board");
            var card = new Card("Sleep");

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => board.AddCardToColumn(Guid.NewGuid(), card));
            Assert.Equal(UserMessages.NotFound.Format("Column").Message, ex.Message);
        }

        [Fact]
        public void AddCardToColumn_ShouldThrowException_WhenEmptyCard()
        {
            // Arrange
            var board = new Board("Personal Board");
            var column = new Column("To do");
            board.AddColumn(column);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => board.AddCardToColumn(column.Id, null));
            Assert.Equal(UserMessages.RequiredField.Format("Card").Message, ex.Message);
        }

        [Fact]
        public void AddCardToColumn_ShouldThrowException_WhenTryingToAddACardThatAlreadyInColumn()
        {
            // Arrange
            var board = new Board("Personal Board");
            var column = new Column("To do");
            board.AddColumn(column);
            var card = new Card("Sleep");
            board.AddCardToColumn(column.Id, card);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => board.AddCardToColumn(column.Id, card));
            Assert.Equal("That Card already is in the Column.", ex.Message);
        }

        [Fact]
        public void AddCardToColumn_ShouldDefineCardColumnIdEqualsToColumnId()
        {
            // Arrange
            var board = new Board("Personal Board");
            var column = new Column("To Do");
            board.AddColumn(column);

            var card = new Card("Sleep");

            // Act
            board.AddCardToColumn(column.Id, card);

            // Assert
            Assert.Equal(column.Id, card.ColumnId);
        }

        [Fact]
        public void AddCardToColumn_ShouldAlwaysDefineCardPriorityForTheLast()
        {
            // Arrange
            var board = new Board("Personal Board");
            var column = new Column("To Do");
            board.AddColumn(column);

            var sleepCard = new Card("Sleep");
            var workCard = new Card("Work");
            var developCard = new Card("Develop");

            // Act
            board.AddCardToColumn(column.Id, sleepCard);
            board.AddCardToColumn(column.Id, workCard);
            board.AddCardToColumn(column.Id, developCard);

            // Assert
            Assert.Equal(1, sleepCard.Priority);
            Assert.Equal(2, workCard.Priority);
            Assert.Equal(3, developCard.Priority);
        }
        #endregion

        #region UpdateCardPriorityInColumn
        [Fact]
        public void UpdateCardPriorityInColumn_ShouldThrowException_WhenPriorityLessThanOne()
        {
            // Arrange
            var board = new Board("Personal Board");
            var column = new Column("To do");
            board.AddColumn(column);
            var card = new Card("Sleep");

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => board.UpdateCardPriorityInColumn(column.Id, card.Id, 0));
            Assert.Equal(UserMessages.MustBeGreatherThan.Format("Priority", 0).Message, ex.Message);
        }

        [Fact]
        public void UpdateCardPriorityInColumn_ShouldThrowException_WhenEmptyColumnId()
        {
            // Arrange
            var board = new Board("Personal Board");
            var column = new Column("To do");
            board.AddColumn(column);
            var card = new Card("Sleep");

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => board.UpdateCardPriorityInColumn(Guid.Empty, card.Id, 1));
            Assert.Equal(UserMessages.RequiredField.Format("ColumnId").Message, ex.Message);
        }

        [Fact]
        public void UpdateCardPriorityInColumn_ShouldThrowException_WhenColumnNotFoundInBoard()
        {
            // Arrange
            var board = new Board("Personal Board");
            var column = new Column("To do");
            board.AddColumn(column);
            var card = new Card("Sleep");

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => board.UpdateCardPriorityInColumn(Guid.NewGuid(), card.Id, 1));
            Assert.Equal(UserMessages.NotFound.Format("Column").Message, ex.Message);
        }

        [Fact]
        public void UpdateCardPriorityInColumn_ShouldThrowException_WhenEmptyCardId()
        {
            // Arrange
            var board = new Board("Personal Board");
            var column = new Column("To do");
            board.AddColumn(column);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => board.UpdateCardPriorityInColumn(column.Id, Guid.Empty, 1));
            Assert.Equal(UserMessages.RequiredField.Format("CardId").Message, ex.Message);
        }

        [Fact]
        public void UpdateCardPriorityInColumn_ShouldThrowException_WhenCardNotFoundInColumn()
        {
            // Arrange
            var board = new Board("Personal Board");
            var column = new Column("To do");
            board.AddColumn(column);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => board.UpdateCardPriorityInColumn(column.Id, Guid.NewGuid(), 1));
            Assert.Equal(UserMessages.NotFound.Format("Card").Message, ex.Message);
        }

        [Fact]
        public void UpdateCardPriorityInColumn_ShouldDoNothing_WhenCardIsAlreadyOnThatPriority()
        {
            // Arrange
            var board = new Board("Personal Board");
            var column = new Column("To do");
            board.AddColumn(column);

            var sleepCard = new Card("Sleep");
            var workCard = new Card("Work");
            var developCard = new Card("Develop");

            board.AddCardToColumn(column.Id, sleepCard);
            board.AddCardToColumn(column.Id, workCard);
            board.AddCardToColumn(column.Id, developCard);

            // Act
            board.UpdateCardPriorityInColumn(column.Id, sleepCard.Id, 1);

            // Assert
            Assert.Equal(1, sleepCard.Priority);
            Assert.Equal(2, workCard.Priority);
            Assert.Equal(3, developCard.Priority);
        }

        [Fact]
        public void UpdateCardPriorityInColumn_ShouldInvertSleepCardWithWorkCardAndKeepDevelopCardInSamePriority()
        {
            // Arrange
            var board = new Board("Personal Board");
            var column = new Column("To do");
            board.AddColumn(column);

            var sleepCard = new Card("Sleep");
            var workCard = new Card("Work");
            var developCard = new Card("Develop");

            board.AddCardToColumn(column.Id, sleepCard);
            board.AddCardToColumn(column.Id, workCard);
            board.AddCardToColumn(column.Id, developCard);

            // Act
            board.UpdateCardPriorityInColumn(column.Id, sleepCard.Id, 2);

            // Assert
            Assert.Equal(1, workCard.Priority);
            Assert.Equal(2, sleepCard.Priority);
            Assert.Equal(3, developCard.Priority);
        }

        [Fact]
        public void UpdateCardPriorityInColumn_ShouldDefineSleepCardPriorityToThreeAndChangeOthersBySubtractingOneToCurrentPriority()
        {
            // Arrange
            var board = new Board("Personal Board");
            var column = new Column("To do");
            board.AddColumn(column);

            var sleepCard = new Card("Sleep");
            var workCard = new Card("Work");
            var developCard = new Card("Develop");

            board.AddCardToColumn(column.Id, sleepCard);
            board.AddCardToColumn(column.Id, workCard);
            board.AddCardToColumn(column.Id, developCard);

            // Act
            board.UpdateCardPriorityInColumn(column.Id, sleepCard.Id, 3);

            // Assert
            Assert.Equal(1, workCard.Priority);
            Assert.Equal(2, developCard.Priority);
            Assert.Equal(3, sleepCard.Priority);
        }

        [Fact]
        public void UpdateCardPriorityInColumn_ShouldInvertWorkCardWithSleepCardAndKeepDevelopCardInSamePriority()
        {
            // Arrange
            var board = new Board("Personal Board");
            var column = new Column("To do");
            board.AddColumn(column);

            var sleepCard = new Card("Sleep");
            var workCard = new Card("Work");
            var developCard = new Card("Develop");

            board.AddCardToColumn(column.Id, sleepCard);
            board.AddCardToColumn(column.Id, workCard);
            board.AddCardToColumn(column.Id, developCard);

            // Act
            board.UpdateCardPriorityInColumn(column.Id, workCard.Id, 1);

            // Assert
            Assert.Equal(1, workCard.Priority);
            Assert.Equal(2, sleepCard.Priority);
            Assert.Equal(3, developCard.Priority);
        }

        [Fact]
        public void UpdateCardPriorityInColumn_ShouldInvertWorkCardWithDevelopCardAndKeepSleepCardInSamePriority()
        {
            // Arrange
            var board = new Board("Personal Board");
            var column = new Column("To do");
            board.AddColumn(column);

            var sleepCard = new Card("Sleep");
            var workCard = new Card("Work");
            var developCard = new Card("Develop");

            board.AddCardToColumn(column.Id, sleepCard);
            board.AddCardToColumn(column.Id, workCard);
            board.AddCardToColumn(column.Id, developCard);

            // Act
            board.UpdateCardPriorityInColumn(column.Id, workCard.Id, 3);

            // Assert
            Assert.Equal(1, sleepCard.Priority);
            Assert.Equal(2, developCard.Priority);
            Assert.Equal(3, workCard.Priority);
        }

        [Fact]
        public void UpdateCardPriorityInColumn_ShouldDefineDevelopCardPriorityToOneAndChangeOthersByAddingOneToCurrentPriority()
        {
            // Arrange
            var board = new Board("Personal Board");
            var column = new Column("To do");
            board.AddColumn(column);

            var sleepCard = new Card("Sleep");
            var workCard = new Card("Work");
            var developCard = new Card("Develop");

            board.AddCardToColumn(column.Id, sleepCard);
            board.AddCardToColumn(column.Id, workCard);
            board.AddCardToColumn(column.Id, developCard);

            // Act
            board.UpdateCardPriorityInColumn(column.Id, developCard.Id, 1);

            // Assert
            Assert.Equal(1, developCard.Priority);
            Assert.Equal(2, sleepCard.Priority);
            Assert.Equal(3, workCard.Priority);
        }

        [Fact]
        public void UpdateCardPriorityInColumn_ShouldInvertDevelopCardWithWorkCardAndKeepSleepCardInSamePriority()
        {
            // Arrange
            var board = new Board("Personal Board");
            var column = new Column("To do");
            board.AddColumn(column);

            var sleepCard = new Card("Sleep");
            var workCard = new Card("Work");
            var developCard = new Card("Develop");

            board.AddCardToColumn(column.Id, sleepCard);
            board.AddCardToColumn(column.Id, workCard);
            board.AddCardToColumn(column.Id, developCard);

            // Act
            board.UpdateCardPriorityInColumn(column.Id, developCard.Id, 2);

            // Assert
            Assert.Equal(1, sleepCard.Priority);
            Assert.Equal(2, developCard.Priority);
            Assert.Equal(3, workCard.Priority);
        }

        [Fact]
        public void UpdateCardPriorityInColumn_ShouldDefinePriorityEqualsToLengthIfInformedHigherThanTheLength()
        {
            // Arrange
            var board = new Board("Personal Board");
            var column = new Column("To do");
            board.AddColumn(column);

            var sleepCard = new Card("Sleep");
            var workCard = new Card("Work");
            var developCard = new Card("Develop");

            board.AddCardToColumn(column.Id, sleepCard);
            board.AddCardToColumn(column.Id, workCard);
            board.AddCardToColumn(column.Id, developCard);

            // Act
            board.UpdateCardPriorityInColumn(column.Id, developCard.Id, 4);

            // Assert
            Assert.Equal(1, sleepCard.Priority);
            Assert.Equal(2, workCard.Priority);
            Assert.Equal(3, developCard.Priority);
        }
        #endregion

        #region CardExistsInColumn
        [Fact]
        public void CardExistsInColumn_ShouldThrowException_WhenColumnNotFound()
        {
            // Arrange
            var board = new Board("Personal Board");

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => board.CardExistsInColumn(Guid.NewGuid(), Guid.NewGuid()));
            Assert.Equal(UserMessages.NotFound.Format("Column").Message, ex.Message);
        }

        [Fact]
        public void CardExistsInColumn_ShouldReturnTrue_WhenCardInColumn()
        {
            // Arrange
            var board = new Board("Personal Board");
            var column = new Column("To Do");
            board.AddColumn(column);

            var card = new Card("Sleep");
            board.AddCardToColumn(column.Id, card);

            // Act
            var cardExists = board.CardExistsInColumn(column.Id, card.Id);

            // Assert
            Assert.True(cardExists);
        }

        [Fact]
        public void CardExistsInColumn_ShouldReturnFalse_WhenCardNotInColumn()
        {
            // Arrange
            var board = new Board("Personal Board");
            var column = new Column("To Do");
            board.AddColumn(column);

            var card = new Card("Sleep");

            // Act
            var cardExists = board.CardExistsInColumn(column.Id, card.Id);

            // Assert
            Assert.False(cardExists);
        }
        #endregion

        #region MoveCardBetweenColumns
        [Fact]
        public void MoveCardBetweenColumns_ShouldThrowException_WhenPriorityLessThanOne()
        {
            // Arrange
            var board = new Board("Personal Board");
            var fromColumn = new Column("To do");
            board.AddColumn(fromColumn);

            var toColumn = new Column("Doing");
            board.AddColumn(toColumn);

            var card = new Card("Sleep");
            board.AddCardToColumn(fromColumn.Id, card);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => board.MoveCardBetweenColumns(card.Id, fromColumn.Id, toColumn.Id, 0));
            Assert.Equal(UserMessages.MustBeGreatherThan.Format("Priority", 0).Message, ex.Message);
        }

        [Fact]
        public void MoveCardBetweenColumns_ShouldThrowException_WhenEmptyFromColumnId()
        {
            // Arrange
            var board = new Board("Personal Board");
            var fromColumn = new Column("To do");
            board.AddColumn(fromColumn);

            var toColumn = new Column("Doing");
            board.AddColumn(toColumn);

            var card = new Card("Sleep");
            board.AddCardToColumn(fromColumn.Id, card);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => board.MoveCardBetweenColumns(card.Id, Guid.Empty, toColumn.Id, 1));
            Assert.Equal(UserMessages.RequiredField.Format("ColumnId").Message, ex.Message);
        }

        [Fact]
        public void MoveCardBetweenColumns_ShouldThrowException_WhenEmptyToColumnId()
        {
            // Arrange
            var board = new Board("Personal Board");
            var fromColumn = new Column("To do");
            board.AddColumn(fromColumn);

            var toColumn = new Column("Doing");
            board.AddColumn(toColumn);

            var card = new Card("Sleep");
            board.AddCardToColumn(fromColumn.Id, card);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => board.MoveCardBetweenColumns(card.Id, fromColumn.Id, Guid.Empty, 1));
            Assert.Equal(UserMessages.RequiredField.Format("ColumnId").Message, ex.Message);
        }

        [Fact]
        public void MoveCardBetweenColumns_ShouldThrowException_WhenToColumnNotFoundInBoard()
        {
            // Arrange
            var board = new Board("Personal Board");
            var fromColumn = new Column("To do");
            board.AddColumn(fromColumn);

            var toColumn = new Column("Doing");

            var card = new Card("Sleep");
            board.AddCardToColumn(fromColumn.Id, card);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => board.MoveCardBetweenColumns(card.Id, fromColumn.Id, toColumn.Id, 1));
            Assert.Equal(UserMessages.NotFound.Format("ToColumn").Message, ex.Message);
        }

        [Fact]
        public void MoveCardBetweenColumns_ShouldThrowException_WhenEmptyCardId()
        {
            // Arrange
            var board = new Board("Personal Board");
            var fromColumn = new Column("To do");
            board.AddColumn(fromColumn);

            var toColumn = new Column("Doing");
            board.AddColumn(toColumn);

            var card = new Card("Sleep");
            board.AddCardToColumn(fromColumn.Id, card);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => board.MoveCardBetweenColumns(Guid.Empty, fromColumn.Id, toColumn.Id, 1));
            Assert.Equal(UserMessages.RequiredField.Format("CardId").Message, ex.Message);
        }

        [Fact]
        public void MoveCardBetweenColumns_ShouldRemoveCardFromColumnAndPlaceInToColumn()
        {
            // Arrange
            var board = new Board("Personal Board");
            var fromColumn = new Column("To do");
            board.AddColumn(fromColumn);

            var toColumn = new Column("Doing");
            board.AddColumn(toColumn);

            var card = new Card("Sleep");
            board.AddCardToColumn(fromColumn.Id, card);

            // Act
            board.MoveCardBetweenColumns(card.Id, fromColumn.Id, toColumn.Id, 1);

            // Assert
            Assert.Empty(fromColumn.Cards);
            Assert.Single(toColumn.Cards);
            Assert.Equal(toColumn.Id, card.ColumnId);
            Assert.Equal(1, card.Priority);
        }

        [Fact]
        public void MoveCardBetweenColumns_ShouldRemoveSleepCardFromColumnAndAdjustDevelopCardPriorityAndKeepWorkCardPriorityInFromColumn()
        {
            // Arrange
            var board = new Board("Personal Board");
            var fromColumn = new Column("To do");
            board.AddColumn(fromColumn);

            var toColumn = new Column("Doing");
            board.AddColumn(toColumn);

            var sleepCard = new Card("Sleep");
            var workCard = new Card("Work");
            var developCard = new Card("Develop");

            board.AddCardToColumn(fromColumn.Id, workCard);
            board.AddCardToColumn(fromColumn.Id, sleepCard);
            board.AddCardToColumn(fromColumn.Id, developCard);

            // Act
            board.MoveCardBetweenColumns(sleepCard.Id, fromColumn.Id, toColumn.Id, 1);

            // Assert
            Assert.Equal(2, fromColumn.Cards.Count);
            Assert.Single(toColumn.Cards);

            Assert.Equal(toColumn.Id, sleepCard.ColumnId);
            Assert.Equal(1, sleepCard.Priority);

            Assert.Equal(1, workCard.Priority);
            Assert.Equal(2, developCard.Priority);
        }

        [Fact]
        public void MoveCardBetweenColumns_ShouldRemoveWorkCardFromColumnAndAdjustDevelopAndSleepCardPriorityInFromColumn()
        {
            // Arrange
            var board = new Board("Personal Board");
            var fromColumn = new Column("To do");
            board.AddColumn(fromColumn);

            var toColumn = new Column("Doing");
            board.AddColumn(toColumn);

            var sleepCard = new Card("Sleep");
            var workCard = new Card("Work");
            var developCard = new Card("Develop");

            board.AddCardToColumn(fromColumn.Id, workCard);
            board.AddCardToColumn(fromColumn.Id, sleepCard);
            board.AddCardToColumn(fromColumn.Id, developCard);

            // Act
            board.MoveCardBetweenColumns(workCard.Id, fromColumn.Id, toColumn.Id, 1);

            // Assert
            Assert.Equal(2, fromColumn.Cards.Count);
            Assert.Single(toColumn.Cards);

            Assert.Equal(toColumn.Id, workCard.ColumnId);
            Assert.Equal(1, workCard.Priority);

            Assert.Equal(1, sleepCard.Priority);
            Assert.Equal(2, developCard.Priority);
        }

        [Fact]
        public void MoveCardBetweenColumns_ShouldRemoveDevelopCardFromColumnAndKeepWorkAndSleepCardPriorityInFromColumn()
        {
            // Arrange
            var board = new Board("Personal Board");
            var fromColumn = new Column("To do");
            board.AddColumn(fromColumn);

            var toColumn = new Column("Doing");
            board.AddColumn(toColumn);

            var sleepCard = new Card("Sleep");
            var workCard = new Card("Work");
            var developCard = new Card("Develop");

            board.AddCardToColumn(fromColumn.Id, workCard);
            board.AddCardToColumn(fromColumn.Id, sleepCard);
            board.AddCardToColumn(fromColumn.Id, developCard);

            // Act
            board.MoveCardBetweenColumns(developCard.Id, fromColumn.Id, toColumn.Id, 1);

            // Assert
            Assert.Equal(2, fromColumn.Cards.Count);
            Assert.Single(toColumn.Cards);

            Assert.Equal(toColumn.Id, developCard.ColumnId);
            Assert.Equal(1, developCard.Priority);

            Assert.Equal(1, workCard.Priority);
            Assert.Equal(2, sleepCard.Priority);
        }

        [Fact]
        public void MoveCardBetweenColumns_ShouldRemoveDevelopCardFromColumnAndKeepWorkAndSleepCardPriorityInToColumn()
        {
            // Arrange
            var board = new Board("Personal Board");

            var fromColumn = new Column("To do");
            board.AddColumn(fromColumn);

            var toColumn = new Column("Doing");
            board.AddColumn(toColumn);

            var sleepCard = new Card("Sleep");
            var workCard = new Card("Work");
            var developCard = new Card("Develop");

            board.AddCardToColumn(fromColumn.Id, developCard);

            board.AddCardToColumn(toColumn.Id, sleepCard);
            board.AddCardToColumn(toColumn.Id, workCard);

            // Act
            board.MoveCardBetweenColumns(developCard.Id, fromColumn.Id, toColumn.Id, 3);

            // Assert
            Assert.Empty(fromColumn.Cards);
            Assert.Equal(3, toColumn.Cards.Count);

            Assert.Equal(toColumn.Id, developCard.ColumnId);
            Assert.Equal(3, developCard.Priority);

            Assert.Equal(1, sleepCard.Priority);
            Assert.Equal(2, workCard.Priority);
        }

        [Fact]
        public void MoveCardBetweenColumns_ShouldRemoveWorkCardFromColumnAndAdjustDevelopPriorityAndKeepSleepCardPriorityInToColumn()
        {
            // Arrange
            var board = new Board("Personal Board");

            var fromColumn = new Column("To do");
            board.AddColumn(fromColumn);

            var toColumn = new Column("Doing");
            board.AddColumn(toColumn);

            var sleepCard = new Card("Sleep");
            var workCard = new Card("Work");
            var developCard = new Card("Develop");

            board.AddCardToColumn(fromColumn.Id, workCard);

            board.AddCardToColumn(toColumn.Id, sleepCard);
            board.AddCardToColumn(toColumn.Id, developCard);

            // Act
            board.MoveCardBetweenColumns(workCard.Id, fromColumn.Id, toColumn.Id, 2);

            // Assert
            Assert.Empty(fromColumn.Cards);
            Assert.Equal(3, toColumn.Cards.Count);

            Assert.Equal(toColumn.Id, workCard.ColumnId);
            Assert.Equal(2, workCard.Priority);

            Assert.Equal(1, sleepCard.Priority);
            Assert.Equal(3, developCard.Priority);
        }

        [Fact]
        public void MoveCardBetweenColumns_ShouldRemoveSleepCardFromColumnAndAdjustDevelopAndWorkCardPriorityInToColumn()
        {
            // Arrange
            var board = new Board("Personal Board");

            var fromColumn = new Column("To do");
            board.AddColumn(fromColumn);

            var toColumn = new Column("Doing");
            board.AddColumn(toColumn);

            var sleepCard = new Card("Sleep");
            var workCard = new Card("Work");
            var developCard = new Card("Develop");

            board.AddCardToColumn(fromColumn.Id, sleepCard);

            board.AddCardToColumn(toColumn.Id, workCard);
            board.AddCardToColumn(toColumn.Id, developCard);

            // Act
            board.MoveCardBetweenColumns(sleepCard.Id, fromColumn.Id, toColumn.Id, 1);

            // Assert
            Assert.Empty(fromColumn.Cards);
            Assert.Equal(3, toColumn.Cards.Count);

            Assert.Equal(toColumn.Id, sleepCard.ColumnId);
            Assert.Equal(1, sleepCard.Priority);

            Assert.Equal(2, workCard.Priority);
            Assert.Equal(3, developCard.Priority);
        }
        #endregion
    }
}
