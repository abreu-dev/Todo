using Todo.Application.DTOs.AccountDTO;
using Todo.Application.DTOs.BoardDTOs;
using Todo.Application.DTOs.UserDTOs;
using Todo.Domain.Validators;
using Todo.IntegrationTests.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions.Ordering;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
[assembly: TestCaseOrderer("Xunit.Extensions.Ordering.TestCaseOrderer", "Xunit.Extensions.Ordering")]
[assembly: TestCollectionOrderer("Xunit.Extensions.Ordering.CollectionOrderer", "Xunit.Extensions.Ordering")]
[assembly: TestFramework("Xunit.Extensions.Ordering.TestFramework", "Xunit.Extensions.Ordering")]

namespace Todo.IntegrationTests
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection)), Order(1)]
    public class AccountsIntegrationTests : IClassFixture<CustomWebAppFactory>
    {
        private readonly IntegrationTestsFixture _testsFixture;

        public AccountsIntegrationTests(IntegrationTestsFixture testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Theory, Order(1)]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("todo.com")]
        public async Task ShouldFailToRegisterTodoUser_WhenInvalidEmail(string email)
        {
            // Arrange
            var registerUserDTO = new RegisterUserDTO()
            {
                Email = email,
                Username = "TodoUser",
                Password = "Todo%123"
            };

            // Act
            var response = await _testsFixture.ClientWithoutAuthorizationHeader().PostAsync("/api/accounts/register", registerUserDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal($"Email '{email}' is invalid.", validationResult.Errors.Single());
        }

        [Theory, Order(2)]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Todo User")]
        public async Task ShouldFailToRegisterTodoUser_WhenInvalidUsername(string username)
        {
            // Arrange
            var registerUserDTO = new RegisterUserDTO()
            {
                Email = "todo@todo.com",
                Username = username,
                Password = "Todo%123"
            };

            // Act
            var response = await _testsFixture.ClientWithoutAuthorizationHeader().PostAsync("/api/accounts/register", registerUserDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal($"Username '{username}' is invalid, can only contain letters or digits.", validationResult.Errors.Single());
        }

        [Fact, Order(3)]
        public async Task ShouldFailToRegisterTodoUser_WhenInvalidPassword()
        {
            // Arrange
            var registerUserDTO = new RegisterUserDTO()
            {
                Email = "todo@todo.com",
                Username = "TodoUser",
                Password = ""
            };

            // Act
            var response = await _testsFixture.ClientWithoutAuthorizationHeader().PostAsync("/api/accounts/register", registerUserDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(6, validationResult.Errors.Count());

            Assert.NotNull(validationResult.Errors.Single(x => x == "Passwords must be at least 6 characters."));
            Assert.NotNull(validationResult.Errors.Single(x => x == "Passwords must have at least one non alphanumeric character."));
            Assert.NotNull(validationResult.Errors.Single(x => x == "Passwords must have at least one digit ('0'-'9')."));
            Assert.NotNull(validationResult.Errors.Single(x => x == "Passwords must have at least one lowercase ('a'-'z')."));
            Assert.NotNull(validationResult.Errors.Single(x => x == "Passwords must have at least one uppercase ('A'-'Z')."));
            Assert.NotNull(validationResult.Errors.Single(x => x == "Passwords must use at least 1 different characters."));
        }

        [Fact, Order(4)]
        public async Task ShouldRegisterTodoUser()
        {
            // Arrange
            var registerUserDTO = new RegisterUserDTO()
            {
                Email = "todo@todo.com",
                Username = "TodoUser",
                Password = "Todo%123"
            };

            // Act
            var response = await _testsFixture.ClientWithoutAuthorizationHeader().PostAsync("/api/accounts/register", registerUserDTO.ToStringContent());

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact, Order(5)]
        public async Task ShouldFailToRegisterTesterUser_WhenEmailIsAlreadyTaken()
        {
            // Arrange
            var registerUserDTO = new RegisterUserDTO()
            {
                Email = "todo@todo.com",
                Username = "TesterUser",
                Password = "Tester%123"
            };

            // Act
            var response = await _testsFixture.ClientWithoutAuthorizationHeader().PostAsync("/api/accounts/register", registerUserDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal($"Email '{registerUserDTO.Email}' is already taken.", validationResult.Errors.Single());
        }

        [Fact, Order(6)]
        public async Task ShouldFailToRegisterTesterUser_WhenUsernameIsAlreadyTaken()
        {
            // Arrange
            var registerUserDTO = new RegisterUserDTO()
            {
                Email = "tester@tester.com",
                Username = "TodoUser",
                Password = "Tester%123"
            };

            // Act
            var response = await _testsFixture.ClientWithoutAuthorizationHeader().PostAsync("/api/accounts/register", registerUserDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal($"Username '{registerUserDTO.Username}' is already taken.", validationResult.Errors.Single());
        }

        [Fact, Order(7)]
        public async Task ShouldRegisterTesterUser()
        {
            // Arrange
            var registerUserDTO = new RegisterUserDTO()
            {
                Email = "tester@tester.com",
                Username = "TesterUser",
                Password = "Tester%123"
            };

            // Act
            var response = await _testsFixture.ClientWithoutAuthorizationHeader().PostAsync("/api/accounts/register", registerUserDTO.ToStringContent());

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact, Order(8)]
        public async Task ShouldFailToLoginTodoUser_WhenIncorrectUsername()
        {
            // Arrange
            var loginUserDTO = new LoginUserDTO()
            {
                Username = "TodoUser1",
                Password = "Todo%123"
            };

            // Act
            var response = await _testsFixture.ClientWithoutAuthorizationHeader().PostAsync("/api/accounts/login", loginUserDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal("Incorrect username or password.", validationResult.Errors.Single());
        }

        [Fact, Order(9)]
        public async Task ShouldFailToLoginTodoUser_WhenIncorrectPassword()
        {
            // Arrange
            var loginUserDTO = new LoginUserDTO()
            {
                Username = "TodoUser",
                Password = "Todo%1231"
            };

            // Act
            var response = await _testsFixture.ClientWithoutAuthorizationHeader().PostAsync("/api/accounts/login", loginUserDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal("Incorrect username or password.", validationResult.Errors.Single());
        }

        [Fact, Order(10)]
        public async Task ShouldLoginTodoUser()
        {
            // Arrange
            var loginUserDTO = new LoginUserDTO()
            {
                Username = "TodoUser",
                Password = "Todo%123"
            };

            // Act
            var response = await _testsFixture.ClientWithoutAuthorizationHeader().PostAsync("/api/accounts/login", loginUserDTO.ToStringContent());

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var userToken = await response.Content.ToObject<LoginResponseDTO>();
            Assert.NotNull(userToken);
            Assert.NotEmpty(userToken.AccessToken);

            _testsFixture.StoreTodoUserAccessToken(userToken.AccessToken);
        }

        [Fact, Order(11)]
        public async Task ShouldLoginTesterUser()
        {
            // Arrange
            var loginUserDTO = new LoginUserDTO()
            {
                Username = "TesterUser",
                Password = "Tester%123"
            };

            // Act
            var response = await _testsFixture.ClientWithoutAuthorizationHeader().PostAsync("/api/accounts/login", loginUserDTO.ToStringContent());

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var userToken = await response.Content.ToObject<LoginResponseDTO>();
            Assert.NotNull(userToken);
            Assert.NotEmpty(userToken.AccessToken);

            _testsFixture.StoreTesterUserAccessToken(userToken.AccessToken);
        }
    }

    [Collection(nameof(IntegrationApiTestsFixtureCollection)), Order(2)]
    public class BoardIntegrationTests : IClassFixture<CustomWebAppFactory>
    {
        private readonly IntegrationTestsFixture _testsFixture;

        public static BoardDTO Board { get; set; }

        public BoardIntegrationTests(IntegrationTestsFixture testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact, Order(1)]
        public async Task ShouldNotAuthorizedToObtainListOfBoards()
        {
            // Arrange & Act & Assert
            var response = await _testsFixture.ClientWithoutAuthorizationHeader().GetAsync($"/api/boards");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact, Order(2)]
        public async Task ShouldObtainAEmptyListOfBoards()
        {
            // Arrange & Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().GetAsync($"/api/boards");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var boards = await response.Content.ToObject<IEnumerable<BoardDTO>>();

            Assert.NotNull(boards);
            Assert.Empty(boards);
        }

        [Fact, Order(3)]
        public async Task ShouldNotAuthorizedToObtainABoard()
        {
            // Arrange & Act & Assert
            var response = await _testsFixture.ClientWithoutAuthorizationHeader().GetAsync($"/api/boards/{Guid.NewGuid()}");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact, Order(4)]
        public async Task ShouldNotObtainAnyBoard()
        {
            // Arrange & Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().GetAsync($"/api/boards/{Guid.NewGuid()}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            var board = await response.Content.ToObject<BoardDTO>();
            Assert.Null(board);
        }

        [Fact, Order(5)]
        public async Task ShouldNotAuthorizedToCreatePersonalBoard()
        {
            // Arrange
            var addBoardDTO = new AddBoardDTO()
            {
                BoardTitle = "Personal Board"
            };

            // Act
            var response = await _testsFixture.ClientWithoutAuthorizationHeader().PostAsync("/api/boards", addBoardDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact, Order(6)]
        public async Task ShouldFailToCreatePersonalBoard_WhenEmptyBoardTitle()
        {
            // Arrange
            var addBoardDTO = new AddBoardDTO()
            {
                BoardTitle = ""
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PostAsync("/api/boards", addBoardDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.RequiredField.Format("BoardTitle").Message, validationResult.Errors.Single());
        }

        [Fact, Order(7)]
        public async Task ShouldCreatePersonalBoard()
        {
            // Arrange
            var addBoardDTO = new AddBoardDTO()
            {
                BoardTitle = "Personal Board"
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PostAsync("/api/boards", addBoardDTO.ToStringContent());

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            var board = (await (await _testsFixture.ClientWithTodoUserAuthorizationHeader().GetAsync("/api/boards")).Content.ToObject<IEnumerable<BoardDTO>>()).SingleOrDefault();
            Assert.NotNull(board);

            Board = await (await _testsFixture.ClientWithTodoUserAuthorizationHeader().GetAsync($"/api/boards/{board.Id}")).Content.ToObject<BoardDTO>();
            Assert.NotNull(Board);
            Assert.Equal("Personal Board", Board.Title);
        }

        [Fact, Order(8)]
        public async Task ShouldNotAuthorizedToUpdateBoardTitle()
        {
            // Arrange
            var updateBoardTitleDTO = new UpdateBoardTitleDTO()
            {
                BoardId = Board.Id,
                NewBoardTitle = "Work Board"
            };

            // Act
            var response = await _testsFixture.ClientWithoutAuthorizationHeader().PutAsync("/api/boards/title", updateBoardTitleDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact, Order(9)]
        public async Task ShouldFailToUpdateBoardTitleFromPersonalBoardToWorkBoard_WhenEmptyBoardId()
        {
            // Arrange
            var updateBoardTitleDTO = new UpdateBoardTitleDTO()
            {
                BoardId = Guid.Empty,
                NewBoardTitle = "Work Board"
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/title", updateBoardTitleDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.RequiredField.Format("BoardId").Message, validationResult.Errors.Single());
        }

        [Fact, Order(10)]
        public async Task ShouldFailToUpdateBoardTitleFromPersonalBoardToWorkBoard_WhenEmptyNewBoardTitle()
        {
            // Arrange
            var updateBoardTitleDTO = new UpdateBoardTitleDTO()
            {
                BoardId = Board.Id,
                NewBoardTitle = ""
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/title", updateBoardTitleDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.RequiredField.Format("NewBoardTitle").Message, validationResult.Errors.Single());
        }

        [Fact, Order(11)]
        public async Task ShouldFailToUpdateBoardTitleFromPersonalBoardToWorkBoard_WhenBoardNotFound()
        {
            // Arrange
            var updateBoardTitleDTO = new UpdateBoardTitleDTO()
            {
                BoardId = Guid.NewGuid(),
                NewBoardTitle = "Work Board"
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/title", updateBoardTitleDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.NotFound.Format("Board").Message, validationResult.Errors.Single());
        }

        [Fact, Order(12)]
        public async Task ShouldUpdateBoardTitleFromPersonalBoardToWorkBoard()
        {
            // Arrange
            var updateBoardTitleDTO = new UpdateBoardTitleDTO()
            {
                BoardId = Board.Id,
                NewBoardTitle = "Work Board"
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/title", updateBoardTitleDTO.ToStringContent());

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            Board = await (await _testsFixture.ClientWithTodoUserAuthorizationHeader().GetAsync($"/api/boards/{Board.Id}")).Content.ToObject<BoardDTO>();

            Assert.NotNull(Board);
            Assert.Equal("Work Board", Board.Title);
        }

        [Fact, Order(13)]
        public async Task ShouldNotAuthorizedToAddColumnToBoard()
        {
            // Arrange
            var addColumnToBoardDTO = new AddColumnToBoardDTO()
            {
                BoardId = Board.Id,
                ColumnTitle = "Doing"
            };

            // Act
            var response = await _testsFixture.ClientWithoutAuthorizationHeader().PostAsync("/api/boards/columns", addColumnToBoardDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact, Order(14)]
        public async Task ShouldFailToAddDoingColumnToBoard_WhenEmptyBoardId()
        {
            // Arrange
            var addColumnToBoardDTO = new AddColumnToBoardDTO()
            {
                BoardId = Guid.Empty,
                ColumnTitle = "Doing"
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PostAsync("/api/boards/columns", addColumnToBoardDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.RequiredField.Format("BoardId").Message, validationResult.Errors.Single());
        }

        [Fact, Order(15)]
        public async Task ShouldFailToAddDoingColumnToBoard_WhenEmptyColumnTitle()
        {
            // Arrange
            var addColumnToBoardDTO = new AddColumnToBoardDTO()
            {
                BoardId = Board.Id,
                ColumnTitle = ""
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PostAsync("/api/boards/columns", addColumnToBoardDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.RequiredField.Format("ColumnTitle").Message, validationResult.Errors.Single());
        }

        [Fact, Order(16)]
        public async Task ShouldFailToAddDoingColumnToBoard_WhenBoardNotFound()
        {
            // Arrange
            var addColumnToBoardDTO = new AddColumnToBoardDTO()
            {
                BoardId = Guid.NewGuid(),
                ColumnTitle = "Doing"
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PostAsync("/api/boards/columns", addColumnToBoardDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.NotFound.Format("Board").Message, validationResult.Errors.Single());
        }

        [Fact, Order(17)]
        public async Task ShouldAddDoingColumnToBoard()
        {
            // Arrange
            var addColumnToBoardDTO = new AddColumnToBoardDTO()
            {
                BoardId = Board.Id,
                ColumnTitle = "Doing"
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PostAsync("/api/boards/columns", addColumnToBoardDTO.ToStringContent());

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            Board = await (await _testsFixture.ClientWithTodoUserAuthorizationHeader().GetAsync($"/api/boards/{Board.Id}")).Content.ToObject<BoardDTO>();

            Assert.NotNull(Board);
            Assert.Single(Board.Columns);
            Assert.Equal("Doing", Board.Columns.ElementAt(0).Title);
            Assert.Equal(1, Board.Columns.ElementAt(0).PositionInBoard);
        }

        [Fact, Order(18)]
        public async Task ShouldAddTodoColumnToBoard()
        {
            // Arrange
            var addColumnToBoardDTO = new AddColumnToBoardDTO()
            {
                BoardId = Board.Id,
                ColumnTitle = "To do"
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PostAsync("/api/boards/columns", addColumnToBoardDTO.ToStringContent());

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            Board = await (await _testsFixture.ClientWithTodoUserAuthorizationHeader().GetAsync($"/api/boards/{Board.Id}")).Content.ToObject<BoardDTO>();

            Assert.NotNull(Board);
            Assert.Equal(2, Board.Columns.Count());
            Assert.Equal("To do", Board.Columns.ElementAt(1).Title);
            Assert.Equal(2, Board.Columns.ElementAt(1).PositionInBoard);
        }

        [Fact, Order(19)]
        public async Task ShouldAddCompletedColumnToBoard()
        {
            // Arrange
            var addColumnToBoardDTO = new AddColumnToBoardDTO()
            {
                BoardId = Board.Id,
                ColumnTitle = "Completed"
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PostAsync("/api/boards/columns", addColumnToBoardDTO.ToStringContent());

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            Board = await (await _testsFixture.ClientWithTodoUserAuthorizationHeader().GetAsync($"/api/boards/{Board.Id}")).Content.ToObject<BoardDTO>();

            Assert.NotNull(Board);
            Assert.Equal(3, Board.Columns.Count());
            Assert.Equal("Completed", Board.Columns.ElementAt(2).Title);
            Assert.Equal(3, Board.Columns.ElementAt(2).PositionInBoard);
        }

        [Fact, Order(20)]
        public async Task ShouldNotAuthorizedToUpdateColumnTitle()
        {
            // Arrange
            var updateColumnTitleDTO = new UpdateColumnTitleDTO()
            {
                BoardId = Board.Id,
                ColumnId = Board.Columns.ElementAt(2).Id,
                NewColumnTitle = "Done"
            };

            // Act
            var response = await _testsFixture.ClientWithoutAuthorizationHeader().PutAsync("/api/boards/columns/title", updateColumnTitleDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact, Order(21)]
        public async Task ShouldFailToUpdateColumnTitleFromCompletedToDone_WhenEmptyBoardId()
        {
            // Arrange
            var updateColumnTitleDTO = new UpdateColumnTitleDTO()
            {
                BoardId = Guid.Empty,
                ColumnId = Board.Columns.ElementAt(2).Id,
                NewColumnTitle = "Done"
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/columns/title", updateColumnTitleDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.RequiredField.Format("BoardId").Message, validationResult.Errors.Single());
        }

        [Fact, Order(22)]
        public async Task ShouldFailToUpdateColumnTitleFromCompletedToDone_WhenEmptyColumnId()
        {
            // Arrange
            var updateColumnTitleDTO = new UpdateColumnTitleDTO()
            {
                BoardId = Board.Id,
                ColumnId = Guid.Empty,
                NewColumnTitle = "Done"
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/columns/title", updateColumnTitleDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.RequiredField.Format("ColumnId").Message, validationResult.Errors.Single());
        }

        [Fact, Order(23)]
        public async Task ShouldFailToUpdateColumnTitleFromCompletedToDone_WhenEmptyNewColumnTitle()
        {
            // Arrange
            var updateColumnTitleDTO = new UpdateColumnTitleDTO()
            {
                BoardId = Board.Id,
                ColumnId = Board.Columns.ElementAt(2).Id,
                NewColumnTitle = ""
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/columns/title", updateColumnTitleDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.RequiredField.Format("NewColumnTitle").Message, validationResult.Errors.Single());
        }

        [Fact, Order(24)]
        public async Task ShouldFailToUpdateColumnTitleFromCompletedToDone_WhenBoardNotFound()
        {
            // Arrange
            var updateColumnTitleDTO = new UpdateColumnTitleDTO()
            {
                BoardId = Guid.NewGuid(),
                ColumnId = Board.Columns.ElementAt(2).Id,
                NewColumnTitle = "Done"
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/columns/title", updateColumnTitleDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.NotFound.Format("Board").Message, validationResult.Errors.Single());
        }

        [Fact, Order(25)]
        public async Task ShouldFailToUpdateColumnTitleFromCompletedToDone_WhenColumnNotFoundInBoard()
        {
            // Arrange
            var updateColumnTitleDTO = new UpdateColumnTitleDTO()
            {
                BoardId = Board.Id,
                ColumnId = Guid.NewGuid(),
                NewColumnTitle = "Done"
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/columns/title", updateColumnTitleDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.NotFound.Format("Column").Message, validationResult.Errors.Single());
        }

        [Fact, Order(26)]
        public async Task ShouldUpdateColumnTitleFromCompletedToDone()
        {
            // Arrange
            var updateColumnTitleDTO = new UpdateColumnTitleDTO()
            {
                BoardId = Board.Id,
                ColumnId = Board.Columns.ElementAt(2).Id,
                NewColumnTitle = "Done"
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/columns/title", updateColumnTitleDTO.ToStringContent());

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            Board = await (await _testsFixture.ClientWithTodoUserAuthorizationHeader().GetAsync($"/api/boards/{Board.Id}")).Content.ToObject<BoardDTO>();

            Assert.NotNull(Board);
            Assert.Equal(3, Board.Columns.Count());
            Assert.Equal("Done", Board.Columns.ElementAt(2).Title);
            Assert.Equal(3, Board.Columns.ElementAt(2).PositionInBoard);
        }

        [Fact, Order(27)]
        public async Task ShouldNotAuthorizedToUpdateColumnPositionInBoard()
        {
            // Arrange
            var updateColumnPositionInBoardDTO = new UpdateColumnPositionInBoardDTO()
            {
                BoardId = Board.Id,
                ColumnId = Board.Columns.ElementAt(1).Id,
                NewColumnPositionInBoard = 1
            };

            // Act
            var response = await _testsFixture.ClientWithoutAuthorizationHeader().PutAsync("/api/boards/columns/position-in-board", updateColumnPositionInBoardDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact, Order(28)]
        public async Task ShoudFailToUpdateColumnPositionInBoardOfTodoColumnFromTwoToOne_WhenEmptyBoardId()
        {
            // Arrange
            var updateColumnPositionInBoardDTO = new UpdateColumnPositionInBoardDTO()
            {
                BoardId = Guid.Empty,
                ColumnId = Board.Columns.ElementAt(1).Id,
                NewColumnPositionInBoard = 1
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/columns/position-in-board", updateColumnPositionInBoardDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.RequiredField.Format("BoardId").Message, validationResult.Errors.Single());
        }

        [Fact, Order(29)]
        public async Task ShoudFailToUpdateColumnPositionInBoardOfTodoColumnFromTwoToOne_WhenEmptyColumnId()
        {
            // Arrange
            var updateColumnPositionInBoardDTO = new UpdateColumnPositionInBoardDTO()
            {
                BoardId = Board.Id,
                ColumnId = Guid.Empty,
                NewColumnPositionInBoard = 1
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/columns/position-in-board", updateColumnPositionInBoardDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.RequiredField.Format("ColumnId").Message, validationResult.Errors.Single());
        }

        [Fact, Order(30)]
        public async Task ShoudFailToUpdateColumnPositionInBoardOfTodoColumnFromTwoToOne_WhenNewColumnPositionInBoardLessThanOne()
        {
            // Arrange
            var updateColumnPositionInBoardDTO = new UpdateColumnPositionInBoardDTO()
            {
                BoardId = Board.Id,
                ColumnId = Board.Columns.ElementAt(1).Id,
                NewColumnPositionInBoard = 0
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/columns/position-in-board", updateColumnPositionInBoardDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.MustBeGreatherThan.Format("NewColumnPositionInBoard", 0).Message, validationResult.Errors.Single());
        }

        [Fact, Order(31)]
        public async Task ShoudFailToUpdateColumnPositionInBoardOfTodoColumnFromTwoToOne_WhenBoardNotFound()
        {
            // Arrange
            var updateColumnPositionInBoardDTO = new UpdateColumnPositionInBoardDTO()
            {
                BoardId = Guid.NewGuid(),
                ColumnId = Board.Columns.ElementAt(1).Id,
                NewColumnPositionInBoard = 1
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/columns/position-in-board", updateColumnPositionInBoardDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.NotFound.Format("Board").Message, validationResult.Errors.Single());
        }

        [Fact, Order(32)]
        public async Task ShoudFailToUpdateColumnPositionInBoardOfTodoColumnFromTwoToOne_WhenColumnNotFoundInBoard()
        {
            // Arrange
            var updateColumnPositionInBoardDTO = new UpdateColumnPositionInBoardDTO()
            {
                BoardId = Board.Id,
                ColumnId = Guid.NewGuid(),
                NewColumnPositionInBoard = 1
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/columns/position-in-board", updateColumnPositionInBoardDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.NotFound.Format("Column").Message, validationResult.Errors.Single());
        }

        [Fact, Order(33)]
        public async Task ShouldUpdateColumnPositionInBoardOfTodoColumnFromTwoToOne()
        {
            // Arrange
            var updateColumnPositionInBoardDTO = new UpdateColumnPositionInBoardDTO()
            {
                BoardId = Board.Id,
                ColumnId = Board.Columns.ElementAt(1).Id,
                NewColumnPositionInBoard = 1
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/columns/position-in-board", updateColumnPositionInBoardDTO.ToStringContent());

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            Board = await (await _testsFixture.ClientWithTodoUserAuthorizationHeader().GetAsync($"/api/boards/{Board.Id}")).Content.ToObject<BoardDTO>();

            Assert.NotNull(Board);
            Assert.Equal(3, Board.Columns.Count());

            Assert.Equal("To do", Board.Columns.ElementAt(0).Title);
            Assert.Equal(1, Board.Columns.ElementAt(0).PositionInBoard);

            Assert.Equal("Doing", Board.Columns.ElementAt(1).Title);
            Assert.Equal(2, Board.Columns.ElementAt(1).PositionInBoard);

            Assert.Equal("Done", Board.Columns.ElementAt(2).Title);
            Assert.Equal(3, Board.Columns.ElementAt(2).PositionInBoard);
        }

        [Fact, Order(34)]
        public async Task ShouldObtainAListOfBoardsWithWorkBoardAndDontBringTheColumns()
        {
            // Arrange & Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().GetAsync($"/api/boards");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var boards = await response.Content.ToObject<IEnumerable<BoardDTO>>();

            Assert.NotNull(boards);
            Assert.Single(boards);

            Assert.Equal(Board.Id, boards.Single().Id);
            Assert.Equal("Work Board", boards.Single().Title);
            Assert.Empty(boards.Single().Columns);
        }

        [Fact, Order(35)]
        public async Task ShouldObtainWorkBoardWithItsThreeColumns()
        {
            // Arrange & Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().GetAsync($"/api/boards/{Board.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var board = await response.Content.ToObject<BoardDTO>();

            Assert.NotNull(board);

            Assert.Equal(Board.Id, board.Id);
            Assert.Equal("Work Board", board.Title);
            Assert.Equal(3, board.Columns.Count());

            Assert.Equal("To do", board.Columns.ElementAt(0).Title);
            Assert.Equal(1, board.Columns.ElementAt(0).PositionInBoard);

            Assert.Equal("Doing", board.Columns.ElementAt(1).Title);
            Assert.Equal(2, board.Columns.ElementAt(1).PositionInBoard);

            Assert.Equal("Done", board.Columns.ElementAt(2).Title);
            Assert.Equal(3, board.Columns.ElementAt(2).PositionInBoard);
        }

        [Fact, Order(36)]
        public async Task ShouldNotAuthorizedToAddCardToColumn()
        {
            // Arrange
            var addCardToColumnDTO = new AddCardToColumnDTO()
            {
                BoardId = Board.Id,
                ColumnId = Board.Columns.ElementAt(0).Id,
                CardTitle = "Sleep"
            };

            // Act
            var response = await _testsFixture.ClientWithoutAuthorizationHeader().PostAsync("/api/boards/cards", addCardToColumnDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact, Order(37)]
        public async Task ShouldFailToAddSleepCardToTodoColumn_WhenEmptyBoardId()
        {
            // Arrange
            var addCardToColumnDTO = new AddCardToColumnDTO()
            {
                BoardId = Guid.Empty,
                ColumnId = Board.Columns.ElementAt(0).Id,
                CardTitle = "Sleep"
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PostAsync("/api/boards/cards", addCardToColumnDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.RequiredField.Format("BoardId").Message, validationResult.Errors.Single());
        }

        [Fact, Order(38)]
        public async Task ShouldFailToAddSleepCardToTodoColumn_WhenEmptyColumnId()
        {
            // Arrange
            var addCardToColumnDTO = new AddCardToColumnDTO()
            {
                BoardId = Board.Id,
                ColumnId = Guid.Empty,
                CardTitle = "Sleep"
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PostAsync("/api/boards/cards", addCardToColumnDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.RequiredField.Format("ColumnId").Message, validationResult.Errors.Single());
        }

        [Fact, Order(39)]
        public async Task ShouldFailToAddSleepCardToTodoColumn_WhenEmptyCardTitle()
        {
            // Arrange
            var addCardToColumnDTO = new AddCardToColumnDTO()
            {
                BoardId = Board.Id,
                ColumnId = Board.Columns.ElementAt(0).Id,
                CardTitle = ""
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PostAsync("/api/boards/cards", addCardToColumnDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.RequiredField.Format("CardTitle").Message, validationResult.Errors.Single());
        }

        [Fact, Order(40)]
        public async Task ShouldFailToAddSleepCardToTodoColumn_WhenBoardNotFound()
        {
            // Arrange
            var addCardToColumnDTO = new AddCardToColumnDTO()
            {
                BoardId = Guid.NewGuid(),
                ColumnId = Board.Columns.ElementAt(0).Id,
                CardTitle = "Sleep"
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PostAsync("/api/boards/cards", addCardToColumnDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.NotFound.Format("Board").Message, validationResult.Errors.Single());
        }

        [Fact, Order(41)]
        public async Task ShouldFailToAddSleepCardToTodoColumn_WhenColumnNotFoundInBoard()
        {
            // Arrange
            var addCardToColumnDTO = new AddCardToColumnDTO()
            {
                BoardId = Board.Id,
                ColumnId = Guid.NewGuid(),
                CardTitle = "Sleep"
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PostAsync("/api/boards/cards", addCardToColumnDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.NotFound.Format("Column").Message, validationResult.Errors.Single());
        }

        [Fact, Order(42)]
        public async Task ShouldAddSleepCardToTodoColumn()
        {
            // Arrange
            var addCardToColumnDTO = new AddCardToColumnDTO()
            {
                BoardId = Board.Id,
                ColumnId = Board.Columns.ElementAt(0).Id,
                CardTitle = "Sleep"
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PostAsync("/api/boards/cards", addCardToColumnDTO.ToStringContent());

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            Board = await (await _testsFixture.ClientWithTodoUserAuthorizationHeader().GetAsync($"/api/boards/{Board.Id}")).Content.ToObject<BoardDTO>();

            Assert.NotNull(Board);
            Assert.Single(Board.Columns.ElementAt(0).Cards);
            Assert.Equal("Sleep", Board.Columns.ElementAt(0).Cards.ElementAt(0).Title);
            Assert.Equal(1, Board.Columns.ElementAt(0).Cards.ElementAt(0).Priority);
        }

        [Fact, Order(43)]
        public async Task ShouldAddWorkCardToTodoColumn()
        {
            // Arrange
            var addCardToColumnDTO = new AddCardToColumnDTO()
            {
                BoardId = Board.Id,
                ColumnId = Board.Columns.ElementAt(0).Id,
                CardTitle = "Work"
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PostAsync("/api/boards/cards", addCardToColumnDTO.ToStringContent());

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            Board = await (await _testsFixture.ClientWithTodoUserAuthorizationHeader().GetAsync($"/api/boards/{Board.Id}")).Content.ToObject<BoardDTO>();

            Assert.NotNull(Board);
            Assert.Equal(2, Board.Columns.ElementAt(0).Cards.Count());
            Assert.Equal("Work", Board.Columns.ElementAt(0).Cards.ElementAt(1).Title);
            Assert.Equal(2, Board.Columns.ElementAt(0).Cards.ElementAt(1).Priority);
        }

        [Fact, Order(44)]
        public async Task ShouldAddDevelopCardToTodoColumn()
        {
            // Arrange
            var addCardToColumnDTO = new AddCardToColumnDTO()
            {
                BoardId = Board.Id,
                ColumnId = Board.Columns.ElementAt(0).Id,
                CardTitle = "Develop"
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PostAsync("/api/boards/cards", addCardToColumnDTO.ToStringContent());

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            Board = await (await _testsFixture.ClientWithTodoUserAuthorizationHeader().GetAsync($"/api/boards/{Board.Id}")).Content.ToObject<BoardDTO>();

            Assert.NotNull(Board);
            Assert.Equal(3, Board.Columns.ElementAt(0).Cards.Count());
            Assert.Equal("Develop", Board.Columns.ElementAt(0).Cards.ElementAt(2).Title);
            Assert.Equal(3, Board.Columns.ElementAt(0).Cards.ElementAt(2).Priority);
        }

        [Fact, Order(45)]
        public async Task ShouldNotAuthorizedToUpdateCardPriorityInColumn()
        {
            // Arrange
            var updateCardPriorityInColumnDTO = new UpdateCardPriorityInColumnDTO()
            {
                BoardId = Board.Id,
                ColumnId = Board.Columns.ElementAt(0).Id,
                CardId = Board.Columns.ElementAt(0).Cards.ElementAt(1).Id,
                NewCardPriorityInColumn = 1
            };

            // Act
            var response = await _testsFixture.ClientWithoutAuthorizationHeader().PutAsync("/api/boards/cards/priority", updateCardPriorityInColumnDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact, Order(46)]
        public async Task ShoudFailToUpdateWorkCardPriorityInColumnOfTodoColumnFromTwoToOne_WhenEmptyBoardId()
        {
            // Arrange
            var updateCardPriorityInColumnDTO = new UpdateCardPriorityInColumnDTO()
            {
                BoardId = Guid.Empty,
                ColumnId = Board.Columns.ElementAt(0).Id,
                CardId = Board.Columns.ElementAt(0).Cards.ElementAt(1).Id,
                NewCardPriorityInColumn = 1
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/cards/priority", updateCardPriorityInColumnDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.RequiredField.Format("BoardId").Message, validationResult.Errors.Single());
        }

        [Fact, Order(47)]
        public async Task ShoudFailToUpdateWorkCardPriorityInColumnOfTodoColumnFromTwoToOne_WhenEmptyColumnId()
        {
            // Arrange
            var updateCardPriorityInColumnDTO = new UpdateCardPriorityInColumnDTO()
            {
                BoardId = Board.Id,
                ColumnId = Guid.Empty,
                CardId = Board.Columns.ElementAt(0).Cards.ElementAt(1).Id,
                NewCardPriorityInColumn = 1
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/cards/priority", updateCardPriorityInColumnDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.RequiredField.Format("ColumnId").Message, validationResult.Errors.Single());
        }

        [Fact, Order(48)]
        public async Task ShoudFailToUpdateWorkCardPriorityInColumnOfTodoColumnFromTwoToOne_WhenEmptyCardId()
        {
            // Arrange
            var updateCardPriorityInColumnDTO = new UpdateCardPriorityInColumnDTO()
            {
                BoardId = Board.Id,
                ColumnId = Board.Columns.ElementAt(0).Id,
                CardId = Guid.Empty,
                NewCardPriorityInColumn = 1
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/cards/priority", updateCardPriorityInColumnDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.RequiredField.Format("CardId").Message, validationResult.Errors.Single());
        }

        [Fact, Order(49)]
        public async Task ShoudFailToUpdateWorkCardPriorityInColumnOfTodoColumnFromTwoToOne_WhenNewCardPriorityInColumnLessThanOne()
        {
            // Arrange
            var updateCardPriorityInColumnDTO = new UpdateCardPriorityInColumnDTO()
            {
                BoardId = Board.Id,
                ColumnId = Board.Columns.ElementAt(0).Id,
                CardId = Board.Columns.ElementAt(0).Cards.ElementAt(1).Id,
                NewCardPriorityInColumn = 0
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/cards/priority", updateCardPriorityInColumnDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.MustBeGreatherThan.Format("NewCardPriorityInColumn", 0).Message, validationResult.Errors.Single());
        }

        [Fact, Order(50)]
        public async Task ShoudFailToUpdateWorkCardPriorityInColumnOfTodoColumnFromTwoToOne_WhenBoardNotFound()
        {
            // Arrange
            var updateCardPriorityInColumnDTO = new UpdateCardPriorityInColumnDTO()
            {
                BoardId = Guid.NewGuid(),
                ColumnId = Board.Columns.ElementAt(0).Id,
                CardId = Board.Columns.ElementAt(0).Cards.ElementAt(1).Id,
                NewCardPriorityInColumn = 1
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/cards/priority", updateCardPriorityInColumnDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.NotFound.Format("Board").Message, validationResult.Errors.Single());
        }

        [Fact, Order(51)]
        public async Task ShoudFailToUpdateWorkCardPriorityInColumnOfTodoColumnFromTwoToOne_WhenColumnNotFoundInBoard()
        {
            // Arrange
            var updateCardPriorityInColumnDTO = new UpdateCardPriorityInColumnDTO()
            {
                BoardId = Board.Id,
                ColumnId = Guid.NewGuid(),
                CardId = Board.Columns.ElementAt(0).Cards.ElementAt(1).Id,
                NewCardPriorityInColumn = 1
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/cards/priority", updateCardPriorityInColumnDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.NotFound.Format("Column").Message, validationResult.Errors.Single());
        }

        [Fact, Order(52)]
        public async Task ShoudFailToUpdateWorkCardPriorityInColumnOfTodoColumnFromTwoToOne_WhenCardNotFoundInColumn()
        {
            // Arrange
            var updateCardPriorityInColumnDTO = new UpdateCardPriorityInColumnDTO()
            {
                BoardId = Board.Id,
                ColumnId = Board.Columns.ElementAt(0).Id,
                CardId = Guid.NewGuid(),
                NewCardPriorityInColumn = 1
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/cards/priority", updateCardPriorityInColumnDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.NotFound.Format("Card").Message, validationResult.Errors.Single());
        }

        [Fact, Order(53)]
        public async Task ShouldUpdateWorkCardPriorityInColumnOfTodoColumnFromTwoToOne()
        {
            // Arrange
            var updateCardPriorityInColumnDTO = new UpdateCardPriorityInColumnDTO()
            {
                BoardId = Board.Id,
                ColumnId = Board.Columns.ElementAt(0).Id,
                CardId = Board.Columns.ElementAt(0).Cards.ElementAt(1).Id,
                NewCardPriorityInColumn = 1
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/cards/priority", updateCardPriorityInColumnDTO.ToStringContent());

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            Board = await (await _testsFixture.ClientWithTodoUserAuthorizationHeader().GetAsync($"/api/boards/{Board.Id}")).Content.ToObject<BoardDTO>();

            Assert.NotNull(Board);
            Assert.Equal(3, Board.Columns.Count());

            Assert.Equal("To do", Board.Columns.ElementAt(0).Title);
            Assert.Equal(1, Board.Columns.ElementAt(0).PositionInBoard);
            Assert.Equal(3, Board.Columns.ElementAt(0).Cards.Count());

            Assert.Equal("Work", Board.Columns.ElementAt(0).Cards.ElementAt(0).Title);
            Assert.Equal("Sleep", Board.Columns.ElementAt(0).Cards.ElementAt(1).Title);
            Assert.Equal("Develop", Board.Columns.ElementAt(0).Cards.ElementAt(2).Title);

            Assert.Equal("Doing", Board.Columns.ElementAt(1).Title);
            Assert.Equal(2, Board.Columns.ElementAt(1).PositionInBoard);
            Assert.Empty(Board.Columns.ElementAt(1).Cards);

            Assert.Equal("Done", Board.Columns.ElementAt(2).Title);
            Assert.Equal(3, Board.Columns.ElementAt(2).PositionInBoard);
            Assert.Empty(Board.Columns.ElementAt(2).Cards);
        }

        [Fact, Order(54)]
        public async Task ShouldUpdateDevelopCardPriorityInColumnOfTodoColumnFromThreeToTwo()
        {
            // Arrange
            var updateCardPriorityInColumnDTO = new UpdateCardPriorityInColumnDTO()
            {
                BoardId = Board.Id,
                ColumnId = Board.Columns.ElementAt(0).Id,
                CardId = Board.Columns.ElementAt(0).Cards.ElementAt(2).Id,
                NewCardPriorityInColumn = 2
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/cards/priority", updateCardPriorityInColumnDTO.ToStringContent());

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            Board = await (await _testsFixture.ClientWithTodoUserAuthorizationHeader().GetAsync($"/api/boards/{Board.Id}")).Content.ToObject<BoardDTO>();

            Assert.NotNull(Board);
            Assert.Equal(3, Board.Columns.Count());

            Assert.Equal("To do", Board.Columns.ElementAt(0).Title);
            Assert.Equal(1, Board.Columns.ElementAt(0).PositionInBoard);
            Assert.Equal(3, Board.Columns.ElementAt(0).Cards.Count());

            Assert.Equal("Work", Board.Columns.ElementAt(0).Cards.ElementAt(0).Title);
            Assert.Equal("Develop", Board.Columns.ElementAt(0).Cards.ElementAt(1).Title);
            Assert.Equal("Sleep", Board.Columns.ElementAt(0).Cards.ElementAt(2).Title);

            Assert.Equal("Doing", Board.Columns.ElementAt(1).Title);
            Assert.Equal(2, Board.Columns.ElementAt(1).PositionInBoard);
            Assert.Empty(Board.Columns.ElementAt(1).Cards);

            Assert.Equal("Done", Board.Columns.ElementAt(2).Title);
            Assert.Equal(3, Board.Columns.ElementAt(2).PositionInBoard);
            Assert.Empty(Board.Columns.ElementAt(2).Cards);
        }

        [Fact, Order(55)]
        public async Task ShouldNotAuthorizedToMoveCardBetweenColumns()
        {
            // Arrange
            var cardBetweenColumnsDTO = new MoveCardBetweenColumnsDTO()
            {
                BoardId = Board.Id,
                FromColumnId = Board.Columns.ElementAt(0).Id,
                ToColumnId = Board.Columns.ElementAt(1).Id,
                CardId = Board.Columns.ElementAt(0).Cards.ElementAt(0).Id,
                CardPriorityInColumn = 1
            };

            // Act
            var response = await _testsFixture.ClientWithoutAuthorizationHeader().PutAsync("/api/boards/cards/move-to-column", cardBetweenColumnsDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact, Order(56)]
        public async Task ShoudFailToMoveWorkCardFromTodoColumnToDoingColumn_WhenEmptyBoardId()
        {
            // Arrange
            var cardBetweenColumnsDTO = new MoveCardBetweenColumnsDTO()
            {
                BoardId = Guid.Empty,
                FromColumnId = Board.Columns.ElementAt(0).Id,
                ToColumnId = Board.Columns.ElementAt(1).Id,
                CardId = Board.Columns.ElementAt(0).Cards.ElementAt(0).Id,
                CardPriorityInColumn = 1
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/cards/move-to-column", cardBetweenColumnsDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.RequiredField.Format("BoardId").Message, validationResult.Errors.Single());
        }

        [Fact, Order(57)]
        public async Task ShoudFailToMoveWorkCardFromTodoColumnToDoingColumn_WhenEmptyFromColumnId()
        {
            // Arrange
            var cardBetweenColumnsDTO = new MoveCardBetweenColumnsDTO()
            {
                BoardId = Board.Id,
                FromColumnId = Guid.Empty,
                ToColumnId = Board.Columns.ElementAt(1).Id,
                CardId = Board.Columns.ElementAt(0).Cards.ElementAt(0).Id,
                CardPriorityInColumn = 1
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/cards/move-to-column", cardBetweenColumnsDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.RequiredField.Format("FromColumnId").Message, validationResult.Errors.Single());
        }

        [Fact, Order(58)]
        public async Task ShoudFailToMoveWorkCardFromTodoColumnToDoingColumn_WhenEmptyToColumnId()
        {
            // Arrange
            var cardBetweenColumnsDTO = new MoveCardBetweenColumnsDTO()
            {
                BoardId = Board.Id,
                FromColumnId = Board.Columns.ElementAt(0).Id,
                ToColumnId = Guid.Empty,
                CardId = Board.Columns.ElementAt(0).Cards.ElementAt(0).Id,
                CardPriorityInColumn = 1
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/cards/move-to-column", cardBetweenColumnsDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.RequiredField.Format("ToColumnId").Message, validationResult.Errors.Single());
        }

        [Fact, Order(59)]
        public async Task ShoudFailToMoveWorkCardFromTodoColumnToDoingColumn_WhenEmptyCardId()
        {
            // Arrange
            var cardBetweenColumnsDTO = new MoveCardBetweenColumnsDTO()
            {
                BoardId = Board.Id,
                FromColumnId = Board.Columns.ElementAt(0).Id,
                ToColumnId = Board.Columns.ElementAt(1).Id,
                CardId = Guid.Empty,
                CardPriorityInColumn = 1
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/cards/move-to-column", cardBetweenColumnsDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.RequiredField.Format("CardId").Message, validationResult.Errors.Single());
        }

        [Fact, Order(60)]
        public async Task ShoudFailToMoveWorkCardFromTodoColumnToDoingColumn_WhenCardPriorityInColumnLessThanOne()
        {
            // Arrange
            var cardBetweenColumnsDTO = new MoveCardBetweenColumnsDTO()
            {
                BoardId = Board.Id,
                FromColumnId = Board.Columns.ElementAt(0).Id,
                ToColumnId = Board.Columns.ElementAt(1).Id,
                CardId = Board.Columns.ElementAt(0).Cards.ElementAt(0).Id,
                CardPriorityInColumn = 0
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/cards/move-to-column", cardBetweenColumnsDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.MustBeGreatherThan.Format("CardPriorityInColumn", 0).Message, validationResult.Errors.Single());
        }

        [Fact, Order(61)]
        public async Task ShoudFailToMoveWorkCardFromTodoColumnToDoingColumn_WhenBoardNotFound()
        {
            // Arrange
            var cardBetweenColumnsDTO = new MoveCardBetweenColumnsDTO()
            {
                BoardId = Guid.NewGuid(),
                FromColumnId = Board.Columns.ElementAt(0).Id,
                ToColumnId = Board.Columns.ElementAt(1).Id,
                CardId = Board.Columns.ElementAt(0).Cards.ElementAt(0).Id,
                CardPriorityInColumn = 1
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/cards/move-to-column", cardBetweenColumnsDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.NotFound.Format("Board").Message, validationResult.Errors.Single());
        }

        [Fact, Order(62)]
        public async Task ShoudFailToMoveWorkCardFromTodoColumnToDoingColumn_WhenFromColumnNotFound()
        {
            // Arrange
            var cardBetweenColumnsDTO = new MoveCardBetweenColumnsDTO()
            {
                BoardId = Board.Id,
                FromColumnId = Guid.NewGuid(),
                ToColumnId = Board.Columns.ElementAt(1).Id,
                CardId = Board.Columns.ElementAt(0).Cards.ElementAt(0).Id,
                CardPriorityInColumn = 1
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/cards/move-to-column", cardBetweenColumnsDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.NotFound.Format("FromColumn").Message, validationResult.Errors.Single());
        }

        [Fact, Order(63)]
        public async Task ShoudFailToMoveWorkCardFromTodoColumnToDoingColumn_WhenToColumnNotFound()
        {
            // Arrange
            var cardBetweenColumnsDTO = new MoveCardBetweenColumnsDTO()
            {
                BoardId = Board.Id,
                FromColumnId = Board.Columns.ElementAt(0).Id,
                ToColumnId = Guid.NewGuid(),
                CardId = Board.Columns.ElementAt(0).Cards.ElementAt(0).Id,
                CardPriorityInColumn = 1
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/cards/move-to-column", cardBetweenColumnsDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.NotFound.Format("ToColumn").Message, validationResult.Errors.Single());
        }

        [Fact, Order(64)]
        public async Task ShoudFailToMoveWorkCardFromTodoColumnToDoingColumn_WhenCardNotFound()
        {
            // Arrange
            var cardBetweenColumnsDTO = new MoveCardBetweenColumnsDTO()
            {
                BoardId = Board.Id,
                FromColumnId = Board.Columns.ElementAt(0).Id,
                ToColumnId = Board.Columns.ElementAt(1).Id,
                CardId = Guid.NewGuid(),
                CardPriorityInColumn = 1
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/cards/move-to-column", cardBetweenColumnsDTO.ToStringContent());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var validationResult = await response.Content.ToObject<ValidationResult>();
            Assert.Equal(UserMessages.NotFound.Format("Card").Message, validationResult.Errors.Single());
        }

        [Fact, Order(65)]
        public async Task ShouldMoveWorkCardFromTodoColumnToDoingColumn()
        {
            // Arrange
            var cardBetweenColumnsDTO = new MoveCardBetweenColumnsDTO()
            {
                BoardId = Board.Id,
                FromColumnId = Board.Columns.ElementAt(0).Id,
                ToColumnId = Board.Columns.ElementAt(1).Id,
                CardId = Board.Columns.ElementAt(0).Cards.ElementAt(0).Id,
                CardPriorityInColumn = 1
            };

            // Act
            var response = await _testsFixture.ClientWithTodoUserAuthorizationHeader().PutAsync("/api/boards/cards/move-to-column", cardBetweenColumnsDTO.ToStringContent());

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            Board = await (await _testsFixture.ClientWithTodoUserAuthorizationHeader().GetAsync($"/api/boards/{Board.Id}")).Content.ToObject<BoardDTO>();

            Assert.NotNull(Board);
            Assert.Equal(3, Board.Columns.Count());

            Assert.Equal("To do", Board.Columns.ElementAt(0).Title);
            Assert.Equal(1, Board.Columns.ElementAt(0).PositionInBoard);
            Assert.Equal(2, Board.Columns.ElementAt(0).Cards.Count());

            Assert.Equal("Develop", Board.Columns.ElementAt(0).Cards.ElementAt(0).Title);
            Assert.Equal("Sleep", Board.Columns.ElementAt(0).Cards.ElementAt(1).Title);

            Assert.Equal("Doing", Board.Columns.ElementAt(1).Title);
            Assert.Equal(2, Board.Columns.ElementAt(1).PositionInBoard);
            Assert.Single(Board.Columns.ElementAt(1).Cards);

            Assert.Equal("Work", Board.Columns.ElementAt(1).Cards.ElementAt(0).Title);

            Assert.Equal("Done", Board.Columns.ElementAt(2).Title);
            Assert.Equal(3, Board.Columns.ElementAt(2).PositionInBoard);
            Assert.Empty(Board.Columns.ElementAt(2).Cards);
        }

        [Fact, Order(66)]
        public async Task ShouldObtainAEmptyListOfBoards_WhenUsingAnotherAuthorizationHeader()
        {
            // Arrange & Act
            var response = await _testsFixture.ClientWithTesterUserAuthorizationHeader().GetAsync($"/api/boards");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var boards = await response.Content.ToObject<IEnumerable<BoardDTO>>();

            Assert.NotNull(boards);
            Assert.Empty(boards);
        }

        [Fact, Order(67)]
        public async Task ShouldNotObtainBoard_WhenUsingAnotherAuthorizationHeader()
        {
            // Arrange & Act
            var response = await _testsFixture.ClientWithTesterUserAuthorizationHeader().GetAsync($"/api/boards/{Board.Id}");

            // Assert            
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            var board = await response.Content.ToObject<BoardDTO>();
            Assert.Null(board);
        }
    }
}
