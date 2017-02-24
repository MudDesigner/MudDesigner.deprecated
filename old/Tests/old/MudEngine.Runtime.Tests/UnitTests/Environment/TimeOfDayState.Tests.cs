using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MudDesigner.MudEngine.Environment;

namespace MudDesigner.MudEngine.Tests.UnitTests.Environment
{
    [TestClass]
    public class TimeOfDayStateTests
    {
        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void Ctor_sets_id_and_creation_date()
        {
            // Act
            var state = new TimePeriod();

            // Act
            Assert.IsNotNull(state.Id, "No ID was generated.");
            Assert.IsFalse(state.CreationDate == default(DateTime));
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void Can_set_and_retrieve_name()
        {
            // Arrange
            string name = "Test";

            // Act
            var state = new TimePeriod { Name = name };

            // Act
            Assert.AreEqual(name, state.Name);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void Can_set_and_retrieve_start_time()
        {
            // Arrange
            var startTime = Mock.Of<ITimeOfDay>();

            // Act
            var state = new TimePeriod { StateStartTime = startTime };

            // Act
            Assert.AreEqual(startTime, state.StateStartTime);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public async Task Can_get_the_time_state_has_been_alive()
        {
            // Arrange
            var state = new TimePeriod();

            // Act
            await Task.Delay(TimeSpan.FromSeconds(1));
            var aliveTime = (int)state.TimeAlive;

            // Act
            Assert.IsTrue(aliveTime >= 1);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [ExpectedException(typeof(ArgumentNullException))]
        [Owner("Johnathon Sullinger")]
        public void Initialize_throws_exception_with_null_start_time()
        {
            // Arrange
            var state = new TimePeriod();

            // Act
            state.Start(null, 2);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [ExpectedException(typeof(InvalidTimeOfDayException))]
        [Owner("Johnathon Sullinger")]
        public void Initialize_throws_exception_with_time_of_day_containing_no_hoursPerDay()
        {
            // Arrange
            var day = Mock.Of<ITimeOfDay>();
            var state = new TimePeriod();

            // Act
            state.Start(day, 2);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void Initialize_assigns_time_of_day()
        {
            // Arrange
            var day = Mock.Of<ITimeOfDay>(m => m.Hour == 5 && m.HoursPerDay == 24);

            // Mock out the cloning of the ITimeOfDay instance.
            // The state initialization performs two clones at the moment.
            Mock.Get(day).Setup(mock => mock.Clone()).Returns(day);
            Mock.Get(day.Clone()).Setup(mock => mock.Clone()).Returns(day);

            var state = new TimePeriod();

            // Act
            state.Start(day, 0.05);

            // Assert
            Assert.IsNotNull(state.StateStartTime);
            Assert.AreEqual(5, state.StateStartTime.Hour);
            Assert.AreEqual(0, state.StateStartTime.Minute);
            Assert.AreEqual(24, state.StateStartTime.HoursPerDay);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public async Task State_clock_increments_by_the_hour()
        {
            // Arrange
            var state = new TimePeriod();
            var continueIncrementingHour = true;

            var day = Mock.Of<ITimeOfDay>(m => m.Hour == 5 && m.HoursPerDay == 24);
            Mock.Get(day)
                .Setup(m => m.IncrementByHour(It.IsAny<int>()))
                .Callback((int time) =>
                {
                    // This prevents the timer from incrementing the hours past what our test is expecting.
                    // We aren't testing the number of times that the timer increments the hours, 
                    // we are just testing that the timer actually increments the hours.
                    if (!continueIncrementingHour)
                    {

                        return;
                    }

                    day = Mock.Of<ITimeOfDay>(m => m.Hour == day.Hour + time && m.HoursPerDay == day.HoursPerDay);
                    state.StateStartTime = day;

                    continueIncrementingHour = false;
                });
             
            // Mock out the cloning of the ITimeOfDay instance.
            // The state initialization performs two clones at the moment.
            Mock.Get(day).Setup(mock => mock.Clone()).Returns(day);
            Mock.Get(day.Clone()).Setup(mock => mock.Clone()).Returns(day);

            // Act
            state.Start(day, 0.005);
            await Task.Delay(TimeSpan.FromSeconds(1));

            // Assert
            Assert.AreEqual(6, state.StateStartTime.Hour);
        }
    }
}
