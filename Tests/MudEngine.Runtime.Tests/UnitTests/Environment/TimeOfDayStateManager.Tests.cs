using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MudDesigner.MudEngine.Environment;

namespace MudDesigner.MudEngine.Tests.UnitTests.Environment
{
    [TestClass]
    public class TimeOfDayStateManagerTests
    {
        [TestInitialize]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Potential Code Quality Issues", "RECS0018:Comparison of floating point numbers with equality operator", Justification = "<Pending>")]
        public void Setup()
        {
            TimePeriodManager.SetDefaultHoursPerDay(24);
            TimePeriodManager.SetFactory(
                (hour, minute, hoursPerDay) => 
                    Mock.Of<ITimeOfDay>(mock => 
                        mock.Hour == hour &&
                        mock.Minute == minute &&
                        mock.HoursPerDay == hoursPerDay));
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_throws_argument_null_exception_when_given_null()
        {
            // Act
            var manager = new TimePeriodManager(null);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void Can_get_time_of_day_state_for_current_real_world_time_lower_end()
        {
            // Arrange
            var manager = new TimePeriodManager(this.CreateStates());
            var time = new DateTime(2015, 8, 2, 6, 0, 0);

            // Act
            ITimePeriod state = manager.GetTimeOfDayState(time);

            // Assert
            Assert.AreEqual(5, state.StateStartTime.Hour);
            Assert.AreEqual(30, state.StateStartTime.Minute);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void Can_get_time_of_day_state_for_current_real_world_time_middle_time()
        {
            // Arrange
            var manager = new TimePeriodManager(this.CreateStates());
            var time = new DateTime(2015, 8, 2, 12, 0, 0);

            // Act
            ITimePeriod state = manager.GetTimeOfDayState(time);

            // Assert
            Assert.AreEqual(12, state.StateStartTime.Hour);
            Assert.AreEqual(0, state.StateStartTime.Minute);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void Can_get_time_of_day_state_for_current_real_world_time_upper_limit()
        {
            // Arrange
            var manager = new TimePeriodManager(this.CreateStates());
            var time = new DateTime(2015, 8, 2, 18, 0, 0);

            // Act
            ITimePeriod state = manager.GetTimeOfDayState(time);

            // Assert
            Assert.AreEqual(18, state.StateStartTime.Hour);
            Assert.AreEqual(0, state.StateStartTime.Minute);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void Can_get_time_of_day_state_when_states_are_out_of_order()
        {
            // Arrange
            var manager = new TimePeriodManager(this.CreateStates());
            var time = new DateTime(2015, 8, 2, 16, 0, 0);

            // Act
            ITimePeriod state = manager.GetTimeOfDayState(time);

            // Assert
            Assert.AreEqual(15, state.StateStartTime.Hour);
            Assert.AreEqual(0, state.StateStartTime.Minute);
        }

        IEnumerable<ITimePeriod> CreateStates()
        {
            var timeOfDayStates = new List<ITimePeriod>();
            var morning = Mock.Of<ITimePeriod>(mock =>
                mock.StateStartTime == Mock.Of<ITimeOfDay>(m => m.Hour == 5 && m.Minute == 30));
            var afternoon = Mock.Of<ITimePeriod>(mock =>
                mock.StateStartTime == Mock.Of<ITimeOfDay>(m => m.Hour == 12));
            var evening = Mock.Of<ITimePeriod>(mock =>
                mock.StateStartTime == Mock.Of<ITimeOfDay>(m => m.Hour == 18));
            var laterAfternoon = Mock.Of<ITimePeriod>(mock =>
                mock.StateStartTime == Mock.Of<ITimeOfDay>(m => m.Hour == 15));

            timeOfDayStates.AddRange(new[] { morning, afternoon, evening, laterAfternoon });

            return timeOfDayStates;
        }
    }
}
