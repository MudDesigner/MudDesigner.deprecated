using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MudDesigner.MudEngine.Tests.Fixture;

namespace MudDesigner.MudEngine.Tests.UnitTests
{
    [TestClass]
    public class PropertCacheTests
    {
        [TestInitialize]
        public void Setup()
        {
            ReflectedCache.ClearAllReflectedCache();
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_properties_for_type_without_cache()
        {
            // Act
            var properties = PropertyCache.GetPropertiesForType<TypePoolFixture>();

            // Assert
            Assert.IsTrue(properties.Count() == 11, "The number of properties expected back did not match the number of properties returned for the fixture.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_properties_for_type_with_existing_cache()
        {
            //Arrange
            // Creates an initial cache for us
            var properties = PropertyCache.GetPropertiesForType<TypePoolFixture>();

            // Act
            properties = PropertyCache.GetPropertiesForType<TypePoolFixture>();

            // Assert
            Assert.IsTrue(properties.Count() == 11, "The number of properties expected back did not match the number of properties returned for the fixture.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_properties_for_type_without_cache_using_predicate()
        {
            // Arrange
            var timer = new Stopwatch();
            timer.Start();

            // Act
            var properties =
                PropertyCache.GetPropertiesForType<TypePoolFixture>(
                    info => Attribute.IsDefined(info, typeof(AttributeFixture)));
            timer.Stop();

            Debug.WriteLine(timer.Elapsed.TotalMilliseconds);

            // Assert
            Assert.IsTrue(properties.Count() == 1);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_properties_for_type_with_cache_using_predicate()
        {
            // Arrange
            Get_properties_for_type_without_cache();

            // Act
            PropertyInfo property = PropertyCache.GetProperty<TypePoolFixture>(info => Attribute.IsDefined(info, typeof(AttributeFixture)));

            // Assert
            Assert.IsNotNull(property);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_properties_from_instance()
        {
            // Arrange
            var fixture = new TypePoolFixture();

            // Act
            IEnumerable<PropertyInfo> properties = PropertyCache.GetPropertiesForType(fixture);

            // Assert
            Assert.IsNotNull(properties);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Get_properties_from_null_instance_throws_exception()
        {
            // Arrange
            var fixture = new TypePoolFixture();

            // Act
            IEnumerable<PropertyInfo> properties =
                PropertyCache.GetPropertiesForType<TypePoolFixture>(null, null);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_properties_from_instance_with_predicate()
        {
            // Arrange
            var fixture = new TypePoolFixture();

            // Act
            IEnumerable<PropertyInfo> properties =
                PropertyCache.GetPropertiesForType(
                    fixture,
                    p => p.Name == fixture.GetPropertyName(pr => pr.IsEnabled));

            // Assert
            Assert.IsNotNull(properties);
        }
    }
}
