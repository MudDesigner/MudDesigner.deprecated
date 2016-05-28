using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MudDesigner.MudEngine;

namespace MudDesigner.MudEngine.Tests
{
    [TestClass]
    public class InputArgsTests
    {
        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void New_instance_has_input_args_assigned()
        {
            // Arrange
            string message = "Unit Test Message";

            // Act
            var args = new InputArgs(message);

            // Assert
            Assert.AreEqual(message, args.Message, "The args did not have the message applied.");
        }
    }
}
