using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MudDesigner.MudEngine.Environment;

namespace MudDesigner.MudEngine.Tests.UnitTests.Environment
{
    [TestClass]
    public class InvalidTimeOfDayExceptionTests
    {
        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void Exception_data_is_properly_provided_the_time_of_day()
        {
            // Arrange
            var timeOfDay = Mock.Of<ITimeOfDay>(time => time.Hour == 5 && time.Minute == 30);

            // Act
            var exception = new InvalidTimeOfDayException("Tests", timeOfDay);

            // Assert
            Assert.IsTrue(exception.Data.Contains(timeOfDay.GetType()), "Exception data did not contain an ITimeOfDay key.");
            Assert.AreEqual(timeOfDay, exception.Data[timeOfDay.GetType()], "Incorrect type added to the exception data.");
        }
    }
}
