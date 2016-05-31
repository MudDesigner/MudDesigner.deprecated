using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MudDesigner.MudEngine.Tests.Fixture;
using MudDesigner.MudEngine;

namespace MudDesigner.MudEngine.Tests.UnitTests.ExtensionsTests
{
    [TestClass]
    public class ICollectionExtensionsTests
    {
        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Extension_returns_random_value_from_a_collection()
        {
            // Arrange
            var collection = new List<TypePoolFixture>();
            collection.Add(new TypePoolFixture { DoubleNumber = 5 });
            collection.Add(new TypePoolFixture { DoubleNumber = 15 });
            collection.Add(new TypePoolFixture { DoubleNumber = 50 });
            collection.Add(new TypePoolFixture { DoubleNumber = 100 });

            // Act
            var result = collection.AnyOrDefaultFromWeight(fixture => fixture.DoubleNumber);

            // Assert
            Assert.AreNotEqual(0, result.DoubleNumber);
            Assert.IsTrue(collection.Any(item => Math.Round(item.DoubleNumber,2).Equals(Math.Round(result.DoubleNumber, 2))));
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Extension_returns_default_value_if_collection_is_empty()
        {
            // Arrange
            var collection = new List<TypePoolFixture>();

            // Act
            var result = collection.AnyOrDefaultFromWeight(fixture => fixture.DoubleNumber);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Extension_returns_only_element_if_only_one_item_exists()
        {
            // Arrange
            var collection = new List<TypePoolFixture>();
            collection.Add(new TypePoolFixture { DoubleNumber = 5 });

            // Act
            var result = collection.AnyOrDefaultFromWeight(fixture => fixture.DoubleNumber);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.DoubleNumber);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Extension_throws_exception_if_it_cant_generate_a_random_element()
        {
            // Arrange
            var collection = new List<TypePoolFixture>();
            collection.Add(new TypePoolFixture { DoubleNumber = 5 });
            collection.Add(new TypePoolFixture { DoubleNumber = 5 });
            collection.Add(new TypePoolFixture { DoubleNumber = 5 });

            // Act
            var result = collection.AnyOrDefaultFromWeight(null);
        }
    }
}
