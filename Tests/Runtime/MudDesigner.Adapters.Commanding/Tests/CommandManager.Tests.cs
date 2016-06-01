using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MudDesigner.Engine;
using MudDesigner.Engine.Game;

namespace MudDesigner.Adapters.Commanding.Tests
{
    [TestClass]
    public class CommandManagerTests
    {
        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [Owner("Johnathon Sullinger")]
        public void Only_allowed_commands_are_returned()
        {
            // Arrange
            var timeOfDay = new TimeOfDay(5, 30);

            // Act
            timeOfDay.DecrementByMinute(15);

            // Assert
            timeOfDay.Minute.Should().Be(15, "the minute was decremented by 15, from 30 minutes.");
        }
    }
}
