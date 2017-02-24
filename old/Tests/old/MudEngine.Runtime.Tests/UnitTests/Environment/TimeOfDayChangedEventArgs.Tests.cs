using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MudDesigner.MudEngine.Environment;

namespace MudDesigner.MudEngine.Tests.UnitTests.Environment
{
    [TestClass]
    public class TimeOfDayChangedEventArgsTests
    {
        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void New_instance_assigns_properties_from_ctor()
        {
            // Arrange
            var transitionTo = Mock.Of<ITimePeriod>();
            var transitionFrom = Mock.Of<ITimePeriod>();

            // Act
            var args = new TimeOfDayChangedEventArgs(transitionFrom, transitionTo);

            // Assert
            Assert.AreEqual(transitionTo, args.TransitioningTo);
            Assert.AreEqual(transitionFrom, args.TransitioningFrom);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Exception_thrown_when_transitionTo_is_null()
        {
            // Arrange
            var transitionFrom = Mock.Of<ITimePeriod>();

            // Act
            var args = new TimeOfDayChangedEventArgs(null, transitionFrom);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Exception_thrown_when_transitionFrom_is_null()
        {
            // Arrange
            var transitionTo = Mock.Of<ITimePeriod>();

            // Act
            var args = new TimeOfDayChangedEventArgs(transitionTo, null);
        }
    }
}
