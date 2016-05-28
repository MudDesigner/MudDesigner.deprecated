using Microsoft.VisualStudio.TestTools.UnitTesting;
using MudDesigner.MudEngine.Environment;

namespace MudDesigner.MudEngine.Tests.UnitTests.Environment
{
    [TestClass]
    public class TimeOfDayTests
    {
        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void Decrement_by_minute_removes_minutes_from_property()
        {
            // Arrange
            var timeOfDay = new TimeOfDay(5, 30);

            // Act
            timeOfDay.DecrementByMinute(15);

            // Assert
            Assert.AreEqual(15, timeOfDay.Minute, "The minute value was not set.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void Decrement_by_minute_removes_hour_if_minute_reaches_zero()
        {
            // Arrange
            var timeOfDay = new TimeOfDay(5, 30);

            // Act
            timeOfDay.DecrementByMinute(31);

            // Assert
            Assert.AreEqual(59, timeOfDay.Minute, "The minute value was not set.");
            Assert.AreEqual(4, timeOfDay.Hour, "The hour value was not set.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void Decrement_by_hour_removes_hour_from_property()
        {
            // Arrange
            var timeOfDay = new TimeOfDay(5, 30);

            // Act
            timeOfDay.DecrementByHour(2);

            // Assert
            Assert.AreEqual(3, timeOfDay.Hour, "The hour value was not set.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void Decrement_by_hours_removes_changes_to_max_value_if_hour_reaches_zero()
        {
            // Arrange
            var timeOfDay = new TimeOfDay(5, 30, 24);

            // Act
            timeOfDay.DecrementByHour(6);

            // Assert
            Assert.AreEqual(23, timeOfDay.Hour, "The hour value was not set.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void Increment_by_minute_adds_minutes_to_property()
        {
            // Arrange
            var timeOfDay = new TimeOfDay();

            // Act
            timeOfDay.IncrementByMinute(30);

            // Assert
            Assert.AreEqual(30, timeOfDay.Minute, "The minute value was not set.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void Item_can_be_cloned()
        {
            // Arrange
            var timeOfDay = new TimeOfDay(5, 30, 24);

            // Act
            var clone = timeOfDay.Clone();

            // Assert
            Assert.AreNotEqual(timeOfDay, clone);
            Assert.AreEqual(timeOfDay.Hour, clone.Hour);
            Assert.AreEqual(timeOfDay.Minute, clone.Minute);
            Assert.AreEqual(timeOfDay.HoursPerDay, clone.HoursPerDay);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void Items_to_string_value_formats_single_digits()
        {
            // Arrange
            var timeOfDay = new TimeOfDay(5, 5, 24);

            // Act
            string time = timeOfDay.ToString();

            // Assert
            Assert.AreEqual("05:05", time);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void Items_to_string_value_formats_single_digit_hour()
        {
            // Arrange
            var timeOfDay = new TimeOfDay(15, 15, 24);

            // Act
            string time = timeOfDay.ToString();

            // Assert
            Assert.AreEqual("15:15", time);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void Increment_minute_over_60_increases_hour()
        {
            // Arrange
            var timeOfDay = new TimeOfDay(5, 30);

            // Act
            timeOfDay.IncrementByMinute(30);

            // Assert
            Assert.AreEqual(0, timeOfDay.Minute, "The minute value was not set.");
            Assert.AreEqual(6, timeOfDay.Hour, "The hour was not incremeneted when the minute reached 60.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void Increment_minute_over_a_hour_by_one_minute()
        {
            // Arrange
            var timeOfDay = new TimeOfDay(5, 59);

            // Act
            timeOfDay.IncrementByMinute(5);

            // Assert
            Assert.AreEqual(0, timeOfDay.Minute, "The minute value was not set.");
            Assert.AreEqual(6, timeOfDay.Hour, "The hour was not incremeneted when the minute reached 60.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void Increment_minute_over_60_increases_hour_and_adjusts_minute()
        {
            // Arrange
            var timeOfDay = new TimeOfDay(5, 30);

            // Act
            timeOfDay.IncrementByMinute(31);

            // Assert
            Assert.AreEqual(1, timeOfDay.Minute, "The minute value was not set.");
            Assert.AreEqual(6, timeOfDay.Hour, "The hour was not incremeneted when the minute reached 60.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void Increment_hour_adds_hours()
        {
            // Arrange
            var timeOfDay = new TimeOfDay(5, 30);

            // Act
            timeOfDay.IncrementByHour(5);

            // Assert
            Assert.AreEqual(10, timeOfDay.Hour, "Hour value was not set.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void Increment_hour_beyond_hours_per_day_resets_hours()
        {
            // Arrange
            var timeOfDay = new TimeOfDay(20, 30, 24);

            // Act
            timeOfDay.IncrementByHour(4);

            // Assert
            Assert.AreEqual(0, timeOfDay.Hour, "Hour value was not set.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void Increment_hour_without_minutes_beyond_hours_per_day_resets_hours()
        {
            // Arrange
            var timeOfDay = new TimeOfDay(20, 0, 24);

            // Act
            timeOfDay.IncrementByHour(4);

            // Assert
            Assert.AreEqual(0, timeOfDay.Hour, "Hour value was not set.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void Increment_hour_with_minutes_within_hours_per_day_does_not_reset_hours()
        {
            // Arrange
            var timeOfDay = new TimeOfDay(20, 59, 24);

            // Act
            timeOfDay.IncrementByHour(3);

            // Assert
            Assert.AreEqual(23, timeOfDay.Hour, "Hour value was not set.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void Instancing_with_hour_and_minute_sets_properties()
        {
            // Act
            var timeOfDay = new TimeOfDay(5, 30);

            // Assert
            Assert.AreEqual(30, timeOfDay.Minute, "The minute value was not set.");
            Assert.AreEqual(5, timeOfDay.Hour, "The hour value was not set.");
            Assert.IsTrue(timeOfDay.HoursPerDay > 0, "The hours per day was not assigned a default value.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void Instancing_with_hour_and_minute_and_hoursPerDay_sets_properties()
        {
            // Act
            var timeOfDay = new TimeOfDay(5, 30, 20);

            // Assert
            Assert.AreEqual(30, timeOfDay.Minute, "The minute value was not set.");
            Assert.AreEqual(5, timeOfDay.Hour, "The hour value was not set.");
            Assert.AreEqual(20, timeOfDay.HoursPerDay, "The hours per day was not set.");
        }
    }
}
