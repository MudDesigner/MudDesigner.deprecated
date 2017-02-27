using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MudDesigner.Runtime;
using MudDesigner.Runtime.Game;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MudDesigner.Engine.Runtime.Tests
{
    [TestClass]
    public class MudUniverseClockTests
    {
        private const int _expectedHours = 8;

        [TestMethod]
        public void HoursPerDayIsSet()
        {
            // Arrange
            IUniverseClock clock = new MudUniverseClock(_expectedHours, Mock.Of<IStopwatch>(), Mock.Of<ITimeOfDayFactory>(), Mock.Of<IMessageBrokerFactory>());

            // Act
            int givenHours = clock.HoursPerDay;

            // Assert
            Assert.AreEqual(_expectedHours, givenHours);
        }

        [TestMethod]
        public async Task InitializeStartsUniverseStopwatch()
        {
            // Arrange
            var stopwatchMock = new Mock<IStopwatch>();
            IUniverseClock clock = new MudUniverseClock(_expectedHours, stopwatchMock.Object, Mock.Of<ITimeOfDayFactory>(), Mock.Of<IMessageBrokerFactory>());

            // Act
            await clock.Initialize();

            // Assert
            stopwatchMock.Verify(mock => mock.Start(), Times.Exactly(1));
        }

        [TestMethod]
        public async Task DeleteStopsUniverseStopwatch()
        {
            // Arrange
            var stopwatchMock = new Mock<IStopwatch>();
            IUniverseClock clock = new MudUniverseClock(_expectedHours, stopwatchMock.Object, Mock.Of<ITimeOfDayFactory>(), Mock.Of<IMessageBrokerFactory>());

            // Act
            await clock.Delete();

            // Assert
            stopwatchMock.Verify(mock => mock.Stop(), Times.Exactly(1));
        }

        [TestMethod]
        public async Task GetCurrentUniverseTimeReturnsCorrectWhenOverHoursPerDay()
        {
            // Arrange
            const int _hoursPerDayForTest = 4;
            const int _hoursOffset = 30;

            // Setup the stopwatch to say it has ran for _expectedHours - _hoursOffset in length.
            var stopwatchMock = new Mock<IStopwatch>();
            stopwatchMock.Setup(mock => mock.GetHours())
                .Returns(_hoursPerDayForTest + _hoursOffset);

            // Setup a time of day factory to return a new time of day to return any hour/minute/second/millisecond combination
            // it is given. The universe clock will give it the stopwatch watch we mocked above.
            var timeOfDayFactoryMock = new Mock<ITimeOfDayFactory>();
            timeOfDayFactoryMock.Setup(mock => mock.Create(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns<int, int, int, int>((hour, minute, second, millisecond) =>
                {
                    var timeOfDayMock = new Mock<ITimeOfDay>();
                    timeOfDayMock.SetupGet(mock => mock.Hour).Returns(hour);
                    timeOfDayMock.SetupGet(mock => mock.Minute).Returns(minute);
                    timeOfDayMock.SetupGet(mock => mock.Second).Returns(second);
                    timeOfDayMock.SetupGet(mock => mock.Millisecond).Returns(millisecond);
                    return timeOfDayMock.Object;
                });

            IUniverseClock clock = new MudUniverseClock(_hoursPerDayForTest, stopwatchMock.Object, timeOfDayFactoryMock.Object, Mock.Of<IMessageBrokerFactory>());
            await clock.Initialize();

            // Act
            ITimeOfDay currentTime = clock.GetCurrentUniverseTime();

            // Assert
            Assert.AreEqual(2, currentTime.Hour, $"There are {_hoursPerDayForTest} hours in a day. The stopwatch has ran for {_hoursOffset}, meaning 7 days (28 hours) and is 2 hours into the 8th days.");
        }
    }
}
