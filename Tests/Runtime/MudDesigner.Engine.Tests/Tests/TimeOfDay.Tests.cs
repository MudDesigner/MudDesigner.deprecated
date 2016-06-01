using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MudDesigner.Engine.Game.Tests
{
    [TestClass]
    public class TimeOfDayTests
    {
        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [Owner("Johnathon Sullinger")]
        public void Decrement_by_minute_removes_minutes_from_property()
        {
            // Arrange
            var timeOfDay = new TimeOfDay(5, 30);

            // Act
            timeOfDay.DecrementByMinute(15);

            // Assert
            timeOfDay.Minute.Should().Be(15, "the minute was decremented by 15, from 30 minutes.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [Owner("Johnathon Sullinger")]
        public void Decrement_by_minute_removes_hour_if_minute_reaches_zero()
        {
            // Arrange
            var timeOfDay = new TimeOfDay(5, 30);

            // Act
            timeOfDay.DecrementByMinute(31);

            // Assert
            timeOfDay.Should().Be(new TimeOfDay(4, 59), "the time of day was not equal to 4:59, after decrementing 31 minutes.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [Owner("Johnathon Sullinger")]
        public void Decrement_by_hour_removes_hour_from_property()
        {
            // Arrange
            var timeOfDay = new TimeOfDay(5, 30);

            // Act
            timeOfDay.DecrementByHour(2);

            // Assert
            timeOfDay.ShouldBeEquivalentTo(new TimeOfDay(3, 30), "the time of day was not equal to 3:30.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [Owner("Johnathon Sullinger")]
        public void Decrement_by_hours_removes_changes_to_max_value_if_hour_reaches_zero()
        {
            // Arrange
            var timeOfDay = new TimeOfDay(5, 30, 24);

            // Act
            timeOfDay.DecrementByHour(6);

            // Assert
            timeOfDay.ShouldBeEquivalentTo(new TimeOfDay(23, 30, 24), "the time of day did not roll backwards into the previous day.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [Owner("Johnathon Sullinger")]
        public void Increment_by_minute_adds_minutes_to_property()
        {
            // Arrange
            var timeOfDay = new TimeOfDay();

            // Act
            timeOfDay.IncrementByMinute(30);

            // Assert
            timeOfDay.ShouldBeEquivalentTo(new TimeOfDay(0, 30), "the time was not 00:30 as expected");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [Owner("Johnathon Sullinger")]
        public void Item_can_be_cloned()
        {
            // Arrange
            var timeOfDay = new TimeOfDay(5, 30, 22);

            // Act
            var clone = timeOfDay.Clone();

            // Assert
            clone.ShouldBeEquivalentTo(timeOfDay, "the original time of day was not cloned correctly.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [Owner("Johnathon Sullinger")]
        public void Items_to_string_value_formats_single_digits()
        {
            // Arrange
            var timeOfDay = new TimeOfDay(5, 5, 24);

            // Act
            string time = timeOfDay.ToString();

            // Assert
            time.Should().Be($"0{timeOfDay.Hour}:0{timeOfDay.Minute}");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [Owner("Johnathon Sullinger")]
        public void Items_to_string_value_formats_single_digit_hour()
        {
            // Arrange
            var timeOfDay = new TimeOfDay(4, 15, 24);

            // Act
            string time = timeOfDay.ToString();

            // Assert
            time.Should().Be($"0{timeOfDay.Hour}:{timeOfDay.Minute}");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [Owner("Johnathon Sullinger")]
        public void Items_to_string_value_formats_single_digit_minute()
        {
            // Arrange
            var timeOfDay = new TimeOfDay(12, 5, 24);

            // Act
            string time = timeOfDay.ToString();

            // Assert
            time.Should().Be($"{timeOfDay.Hour}:0{timeOfDay.Minute}");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [Owner("Johnathon Sullinger")]
        public void Items_to_string_value_formats_multi_digits()
        {
            // Arrange
            var timeOfDay = new TimeOfDay(15, 15, 24);

            // Act
            string time = timeOfDay.ToString();

            // Assert
            time.Should().Be($"{timeOfDay.Hour}:{timeOfDay.Minute}");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [Owner("Johnathon Sullinger")]
        public void Increment_minute_over_60_increases_hour()
        {
            // Arrange
            var timeOfDay = new TimeOfDay(5, 30);

            // Act
            timeOfDay.IncrementByMinute(30);

            // Assert
            timeOfDay.ShouldBeEquivalentTo(new TimeOfDay(6, 0));
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [Owner("Johnathon Sullinger")]
        public void Increment_minute_over_a_hour_by_one_minute()
        {
            // Arrange
            var timeOfDay = new TimeOfDay(5, 59);

            // Act
            timeOfDay.IncrementByMinute(1);

            // Assert
            timeOfDay.ShouldBeEquivalentTo(new TimeOfDay(6, 0));
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [Owner("Johnathon Sullinger")]
        public void Increment_minute_over_60_increases_hour_and_adjusts_minute()
        {
            // Arrange
            var timeOfDay = new TimeOfDay(5, 30);

            // Act
            timeOfDay.IncrementByMinute(31);

            // Assert
            timeOfDay.ShouldBeEquivalentTo(new TimeOfDay(6, 1));
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [Owner("Johnathon Sullinger")]
        public void Increment_hour_adds_hours()
        {
            // Arrange
            var timeOfDay = new TimeOfDay(5, 30);

            // Act
            timeOfDay.IncrementByHour(5);

            // Assert
            timeOfDay.ShouldBeEquivalentTo(new TimeOfDay(10, 30));
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [Owner("Johnathon Sullinger")]
        public void Increment_hour_beyond_hours_per_day_resets_hours()
        {
            // Arrange
            var timeOfDay = new TimeOfDay(20, 30, 24);

            // Act
            timeOfDay.IncrementByHour(4);

            // Assert
            timeOfDay.ShouldBeEquivalentTo(new TimeOfDay(0, 30, 24));
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [Owner("Johnathon Sullinger")]
        public void Increment_hour_without_minutes_beyond_hours_per_day_resets_hours()
        {
            // Arrange
            var timeOfDay = new TimeOfDay(20, 0, 24);

            // Act
            timeOfDay.IncrementByHour(4);

            // Assert
            timeOfDay.ShouldBeEquivalentTo(new TimeOfDay(0, 0));
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [Owner("Johnathon Sullinger")]
        public void Increment_hour_with_minutes_within_hours_per_day_does_not_reset_hours()
        {
            // Arrange
            var timeOfDay = new TimeOfDay(20, 59, 24);

            // Act
            timeOfDay.IncrementByHour(3);

            // Assert
            timeOfDay.ShouldBeEquivalentTo(new TimeOfDay(23, 59, 24));
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [Owner("Johnathon Sullinger")]
        public void Instancing_with_hours_per_day_assigns_value_to_property()
        {
            // Act
            var timeOfDay = new TimeOfDay(5, 30, 20);

            // Assert
            timeOfDay.HoursPerDay.Should().Be(20);
        }
    }
}
