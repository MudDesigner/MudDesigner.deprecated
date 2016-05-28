using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MudDesigner.MudEngine.Tests.Fixture;

namespace MudDesigner.MudEngine.Tests.UnitTests.ExtensionsTests
{
    [TestClass]
    public class ObjectExtensionsTests
    {
        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Extension_throws_exception_if_expression_is_null()
        {
            // Arrange
            var fixture = new ComponentFixture();

            // Act
            fixture.GetPropertyName(null);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Extension_returns_correct_property_name()
        {
            var fixture = new ComponentFixture();

            // Act
            string propertyName = fixture.GetPropertyName(p => p.CreationDate);

            // Assert
            Assert.AreEqual("CreationDate", propertyName);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        [ExpectedException(typeof(NotSupportedException))]
        public void Extension_throws_exception_when_used_with_invalid_expression()
        {
            var fixture = new TypePoolFixture();

            // Act
            string propertyName = fixture.GetPropertyName(p => p.IsEnabled && p.IsEnabled);

            // Assert
            Assert.AreEqual("CreationDate", propertyName);
        }
    }
}
