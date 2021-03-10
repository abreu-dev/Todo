using Todo.Domain.Entities;
using Todo.Domain.Validators;
using System;
using Xunit;

namespace Todo.Domain.Tests.Entities
{
    public class CardTests
    {
        #region Constructor
        [Fact]
        public void Constructor_ShouldThrowException_WhenEmptyTitle()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<Exception>(() => new Card(""));
            Assert.Equal(UserMessages.RequiredField.Format("Title").Message, ex.Message);
        }

        [Fact]
        public void Constructor_ShouldInstantiate_WhenValid()
        {
            // Arrange & Act
            var card = new Card("Sleep");

            // Assert
            Assert.Equal("Sleep", card.Title);
        }
        #endregion
    }
}
