using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MudDesigner.MudEngine.Actors;
using MudDesigner.MudEngine.Environment;

namespace MudDesigner.MudEngine.Tests.UnitTests.Environment
{
    [TestClass]
    public class OccupancyChangedEventArgsTests
    {
        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void Event_arg_is_instanced_with_valid_properties()
        {
            // Arrange
            var departure = Mock.Of<IRoom>();
            var arrival = Mock.Of<IRoom>();
            var character = Mock.Of<ICharacter>();
            var travelDirection = Mock.Of<ITravelDirection>();

            // Act
            var eventArgs = new RoomOccupancyChangedEventArgs(
                character,
                travelDirection,
                departure,
                arrival);

            // Assert
            Assert.IsNotNull(eventArgs.Occupant, "Occupant was null.");
            Assert.IsNotNull(eventArgs.TravelDirection, "TravelDirection was null.");
            Assert.IsNotNull(eventArgs.ArrivalRoom, "ArrivalRoom was null.");
            Assert.IsNotNull(eventArgs.DepartureRoom, "DepartureRoom was null.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Args_throw_exception_with_null_character()
        {
            // Arrange
            var departure = Mock.Of<IRoom>();
            var arrival = Mock.Of<IRoom>();
            var travelDirection = Mock.Of<ITravelDirection>();

            // Act
            var eventArgs = new RoomOccupancyChangedEventArgs(
                null,
                travelDirection,
                departure,
                arrival);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Args_throw_exception_with_null_travel_direction()
        {
            // Arrange
            var departure = Mock.Of<IRoom>();
            var arrival = Mock.Of<IRoom>();
            var character = Mock.Of<ICharacter>();

            // Act
            var eventArgs = new RoomOccupancyChangedEventArgs(
                character,
                null,
                departure,
                arrival);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Args_throw_exception_with_null_departure_room()
        {
            // Arrange
            var arrival = Mock.Of<IRoom>();
            var character = Mock.Of<ICharacter>();
            var travelDirection = Mock.Of<ITravelDirection>();

            // Act
            var eventArgs = new RoomOccupancyChangedEventArgs(
                character,
                travelDirection,
                null,
                arrival);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Args_throw_exception_with_null_arrival_room()
        {
            // Arrange
            var departure = Mock.Of<IRoom>();
            var character = Mock.Of<ICharacter>();
            var travelDirection = Mock.Of<ITravelDirection>();

            // Act
            var eventArgs = new RoomOccupancyChangedEventArgs(
                character,
                travelDirection,
                departure,
                null);
        }
    }
}
