using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MudDesigner.MudEngine.Environment;

namespace MudDesigner.MudEngine.Tests.UnitTests.Environment
{
    [TestClass]
    public class TravelDirectionTests
    {
        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void EastDirection_has_correct_name()
        {
            // Act
            var direction = new EastDirection();

            // Assert
            Assert.AreEqual("East", direction.Direction, "The travel direction was not correctly assigned.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void EastDirection_has_correct_reverse_direction()
        {
            // Arrange
            var direction = new EastDirection();

            // Act
            ITravelDirection reverseDirection = direction.GetOppositeDirection();

            // Assert
            Assert.AreEqual("West", reverseDirection.Direction, "The opposite travel direction was not correctly returned.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void WestDirection_has_correct_name()
        {
            // Act
            var direction = new WestDirection();

            // Assert
            Assert.AreEqual("West", direction.Direction, "The travel direction was not correctly assigned.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void WestDirection_has_correct_reverse_direction()
        {
            // Arrange
            var direction = new WestDirection();

            // Act
            ITravelDirection reverseDirection = direction.GetOppositeDirection();

            // Assert
            Assert.AreEqual("East", reverseDirection.Direction, "The opposite travel direction was not correctly returned.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void NorthDirection_has_correct_name()
        {
            // Act
            var direction = new NorthDirection();

            // Assert
            Assert.AreEqual("North", direction.Direction, "The travel direction was not correctly assigned.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void NorthDirection_has_correct_reverse_direction()
        {
            // Arrange
            var direction = new NorthDirection();

            // Act
            ITravelDirection reverseDirection = direction.GetOppositeDirection();

            // Assert
            Assert.AreEqual("South", reverseDirection.Direction, "The opposite travel direction was not correctly returned.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void SouthDirection_has_correct_name()
        {
            // Act
            var direction = new SouthDirection();

            // Assert
            Assert.AreEqual("South", direction.Direction, "The travel direction was not correctly assigned.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void SouthDirection_has_correct_reverse_direction()
        {
            // Arrange
            var direction = new SouthDirection();

            // Act
            ITravelDirection reverseDirection = direction.GetOppositeDirection();

            // Assert
            Assert.AreEqual("North", reverseDirection.Direction, "The opposite travel direction was not correctly returned.");
        }
    }
}
