using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MudDesigner.MudEngine.Tests.Fixture;

namespace MudDesigner.MudEngine.Tests.UnitTests
{
    [TestClass]
    public class AttributeCacheTests
    {
        [TestInitialize]
        public void Setup()
        {
            ReflectedCache.ClearAllReflectedCache();
        }

        #region GetAttribute tests
        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attributes_for_type()
        {
            // Arrange
            var type = typeof(TypePoolFixture);

            // Act
            IEnumerable<Attribute> attributes = AttributeCache.GetAttributes(type);

            // Assert
            Assert.IsTrue(attributes.Count() == 3);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attributes_for_with_property_info()
        {
            // Arrange
            var type = typeof(TypePoolFixture);
            var fixture = new TypePoolFixture();
            var property = PropertyCache.GetProperty(type, p => p.Name == nameof(fixture.PropertyWithAttribute));

            // Act
            IEnumerable<Attribute> attributes = AttributeCache.GetAttributes(type, property);

            // Assert
            Assert.IsTrue(attributes.Count() == 3);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attributes_from_generic_type()
        {
            // Act
            IEnumerable<Attribute> attributes =
                AttributeCache.GetAttributes<AttributeFixture, TypePoolFixture>();

            // Assert
            Assert.IsTrue(attributes.Count() == 2);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attributes_from_generic_type_from_property_info()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            PropertyInfo property = typeof(TypePoolFixture).GetProperty(nameof(fixture.PropertyWithAttribute));

            // Act
            IEnumerable<Attribute> attributes =
                AttributeCache.GetAttributes<AttributeFixture, TypePoolFixture>(property);

            // Assert
            Assert.IsTrue(attributes.Count() == 2);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attributes_from_generic_type_from_property_info_lacking_attribute_returns_null()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            PropertyInfo property = typeof(TypePoolFixture).GetProperty(nameof(fixture.LongFixture));

            // Act
            IEnumerable<Attribute> attributes =
                AttributeCache.GetAttributes<AttributeFixture, TypePoolFixture>(property);

            // Assert
            Assert.IsTrue(attributes.Count() == 0);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attributes_from_generic_type_from_property_info_and_predicate()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            PropertyInfo property = typeof(TypePoolFixture).GetProperty(nameof(fixture.PropertyWithAttribute));

            // Act
            IEnumerable<Attribute> attributes =
                AttributeCache.GetAttributes<AttributeFixture, TypePoolFixture>
                    (property, attribute => attribute.GetType() == typeof(AttributeFixture));

            // Assert
            Assert.IsTrue(attributes.Count() == 2);
            Assert.AreEqual(typeof(AttributeFixture), attributes.FirstOrDefault().GetType());
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attributes_from_generic_type_with_predicate()
        {
            // Act
            IEnumerable<Attribute> attributes =
                AttributeCache.GetAttributes<AttributeFixture, TypePoolFixture>(
                    null, attribute => attribute.GetType() == typeof(AttributeFixture));

            // Assert
            Assert.IsTrue(attributes.Count() == 2);
            Assert.AreEqual(typeof(AttributeFixture), attributes.FirstOrDefault().GetType());
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attributes_of_T_from_type()
        {
            // Act
            IEnumerable<Attribute> attributes = AttributeCache.GetAttributes<AttributeFixture>(typeof(TypePoolFixture));

            // Assert
            Assert.IsTrue(attributes.Count() == 2);
            Assert.AreEqual(typeof(AttributeFixture), attributes.FirstOrDefault().GetType());
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attributes_of_T_from_property_on_type()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            PropertyInfo property = typeof(TypePoolFixture).GetProperty(fixture.GetPropertyName(p => p.PropertyWithAttribute));

            // Act
            IEnumerable<Attribute> attributes = AttributeCache.GetAttributes<AttributeFixture>(typeof(TypePoolFixture), property);

            // Assert
            Assert.IsTrue(attributes.Count() == 2);
            Assert.AreEqual(typeof(AttributeFixture), attributes.FirstOrDefault().GetType());
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_attributes_from_instance()
        {
            // Arrange
            var fixture = new TypePoolFixture();

            // Act
            IEnumerable<Attribute> attributes =
                AttributeCache.GetAttributes<AttributeFixture, TypePoolFixture>(fixture);

            // Assert
            Assert.IsTrue(attributes.Count() == 2);
            Assert.AreEqual(typeof(AttributeFixture), attributes.FirstOrDefault().GetType());
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_attributes_from_instance_property()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            var property = typeof(TypePoolFixture).GetProperty(fixture.GetPropertyName(p => p.PropertyWithAttribute));

            // Act
            IEnumerable<Attribute> attributes =
                AttributeCache.GetAttributes<AttributeFixture, TypePoolFixture>(fixture, property);

            // Assert
            Assert.IsTrue(attributes.Count() == 2);
            Assert.AreEqual(typeof(AttributeFixture), attributes.FirstOrDefault().GetType());
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_attributes_from_instance_property_with_predicate()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            var property = typeof(TypePoolFixture).GetProperty(fixture.GetPropertyName(p => p.PropertyWithAttribute));

            // Act
            IEnumerable<Attribute> attributes =
                AttributeCache.GetAttributes<AttributeFixture, TypePoolFixture>(
                    fixture, property, attribute => attribute is AttributeFixture);

            // Assert
            Assert.IsTrue(attributes.Count() == 2);
            Assert.AreEqual(typeof(AttributeFixture), attributes.FirstOrDefault().GetType());
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_attributes_from_instance_with_predicate()
        {
            // Arrange
            var fixture = new TypePoolFixture();

            // Act
            IEnumerable<Attribute> attributes =
                AttributeCache.GetAttributes<AttributeFixture, TypePoolFixture>(
                    fixture, null, attribute => attribute is AttributeFixture);

            // Assert
            Assert.IsTrue(attributes.Count() == 2);
            Assert.AreEqual(typeof(AttributeFixture), attributes.FirstOrDefault().GetType());
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_attributes_from_null_instance()
        {
            // Arrange
            var fixture = new TypePoolFixture();

            // Act
            IEnumerable<Attribute> attributes =
                AttributeCache.GetAttributes<AttributeFixture, TypePoolFixture>(null, null, null);

            // Assert
            Assert.IsTrue(attributes.Count() == 2);
            Assert.AreEqual(typeof(AttributeFixture), attributes.FirstOrDefault().GetType());
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attribute_from_type()
        {
            // Act
            Attribute attribute =
                AttributeCache.GetAttribute(typeof(TypePoolFixture));

            // Assert
            Assert.IsNotNull(attribute);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attribute_from_property_on_type()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            var property = typeof(TypePoolFixture).GetProperty(fixture.GetPropertyName(p => p.PropertyWithAttribute));

            // Act
            Attribute attribute =
                AttributeCache.GetAttribute(typeof(TypePoolFixture), property);

            // Assert
            Assert.IsNotNull(attribute);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attribute_from_property_on_type_with_predicate()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            var property = typeof(TypePoolFixture).GetProperty(fixture.GetPropertyName(p => p.PropertyWithAttribute));

            // Act
            Attribute attribute =
                AttributeCache.GetAttribute(typeof(TypePoolFixture), property, a => a is AttributeFixture);

            // Assert
            Assert.IsNotNull(attribute);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attribute_on_type_with_predicate()
        {
            // Act
            Attribute attribute =
                AttributeCache.GetAttribute(typeof(TypePoolFixture), null, a => a is AttributeFixture);

            // Assert
            Assert.IsNotNull(attribute);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Get_attribute_on_null_type()
        {
            // Act
            Attribute attribute = AttributeCache.GetAttribute(null, null, null);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attributes_of_T_from_property_on_type_with_predicate()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            PropertyInfo property = typeof(TypePoolFixture).GetProperty(fixture.GetPropertyName(p => p.PropertyWithAttribute));

            // Act
            IEnumerable<Attribute> attributes =
                AttributeCache.GetAttributes<AttributeFixture>(
                    typeof(TypePoolFixture), property, attribute => attribute is AttributeFixture);

            // Assert
            Assert.IsTrue(attributes.Count() == 2);
            Assert.AreEqual(typeof(AttributeFixture), attributes.FirstOrDefault().GetType());
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_attribute_from_type()
        {
            // Act
            AttributeFixture attribute =
                AttributeCache.GetAttribute<AttributeFixture>(typeof(TypePoolFixture));

            // Assert
            Assert.IsNotNull(attribute);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_attribute_from_property_on_type()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            var property = typeof(TypePoolFixture).GetProperty(fixture.GetPropertyName(p => p.PropertyWithAttribute));

            // Act
            AttributeFixture attribute =
                AttributeCache.GetAttribute<AttributeFixture>(typeof(TypePoolFixture), property);

            // Assert
            Assert.IsNotNull(attribute);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_attribute_from_property_on_type_with_predicate()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            var property = typeof(TypePoolFixture).GetProperty(fixture.GetPropertyName(p => p.PropertyWithAttribute));

            // Act
            AttributeFixture attribute =
                AttributeCache.GetAttribute<AttributeFixture>(typeof(TypePoolFixture), property, a => a.IsEnabled);

            // Assert
            Assert.IsNotNull(attribute);
            Assert.IsTrue(attribute.IsEnabled);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_attribute_on_type_with_predicate()
        {
            // Act
            AttributeFixture attribute =
                AttributeCache.GetAttribute<AttributeFixture>(typeof(TypePoolFixture), null, a => a.IsEnabled);

            // Assert
            Assert.IsNotNull(attribute);
            Assert.IsTrue(attribute.IsEnabled);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Get_generic_attribute_on_null_type()
        {
            // Act
            AttributeFixture attribute = AttributeCache.GetAttribute<AttributeFixture>(null, null, null);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_attribute_from_instance()
        {
            // Arrange
            var fixture = new TypePoolFixture();

            // Act
            AttributeFixture attribute =
                AttributeCache.GetAttribute<AttributeFixture, TypePoolFixture>();

            // Assert
            Assert.IsNotNull(attribute);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_attribute_from_instance_property()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            var property = typeof(TypePoolFixture).GetProperty(fixture.GetPropertyName(p => p.PropertyWithAttribute));

            // Act
            AttributeFixture attribute =
                AttributeCache.GetAttribute<AttributeFixture, TypePoolFixture>(property);

            // Assert
            Assert.IsNotNull(attribute);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_single_attribute_from_instance_property()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            var property = typeof(TypePoolFixture).GetProperty(fixture.GetPropertyName(p => p.PropertyWithAttribute));

            // Act
            AttributeFixture attribute =
                AttributeCache.GetAttribute<AttributeFixture, TypePoolFixture>(property, fixture);

            // Assert
            Assert.IsNotNull(attribute);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_single_attribute_from_instance_property_with_predicate()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            var property = typeof(TypePoolFixture).GetProperty(fixture.GetPropertyName(p => p.PropertyWithAttribute));

            // Act
            AttributeFixture attribute =
                AttributeCache.GetAttribute<AttributeFixture, TypePoolFixture>(
                    property,
                    fixture,
                    a => a.IsEnabled);

            // Assert
            Assert.IsNotNull(attribute);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_attribute_from_null_instance_with_predicate()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            var property = typeof(TypePoolFixture).GetProperty(fixture.GetPropertyName(p => p.PropertyWithAttribute));

            // Act
            AttributeFixture attribute =
                AttributeCache.GetAttribute<AttributeFixture, TypePoolFixture>(
                    property, null, a => a.IsEnabled);

            // Assert
            Assert.IsNotNull(attribute);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_attribute_from_null_property_and_null_instance_with_predicate()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            var property = typeof(TypePoolFixture).GetProperty(fixture.GetPropertyName(p => p.PropertyWithAttribute));

            // Act
            AttributeFixture attribute =
                AttributeCache.GetAttribute<AttributeFixture, TypePoolFixture>(
                    null, null, a => a.IsEnabled);

            // Assert
            Assert.IsNotNull(attribute);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_generic_attribute_from_null_instance()
        {
            // Arrange
            var fixture = new TypePoolFixture();

            // Act
            IEnumerable<Attribute> attributes =
                AttributeCache.GetAttributes<AttributeFixture, TypePoolFixture>(null, null, null);

            // Assert
            Assert.IsTrue(attributes.Count() == 2);
            Assert.AreEqual(typeof(AttributeFixture), attributes.FirstOrDefault().GetType());
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attributes_of_T_from_type_with_predicate()
        {
            // Act
            IEnumerable<Attribute> attributes =
                AttributeCache.GetAttributes<AttributeFixture>(
                    typeof(TypePoolFixture), null, attribute => attribute is AttributeFixture);

            // Assert
            Assert.IsTrue(attributes.Count() == 2);
            Assert.AreEqual(typeof(AttributeFixture), attributes.FirstOrDefault().GetType());
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attributes_with_property_info_using_predicate()
        {
            // Arrange
            var type = typeof(TypePoolFixture);
            var fixture = new TypePoolFixture();
            var property = PropertyCache.GetProperty(type, p => p.Name == nameof(fixture.PropertyWithAttribute));

            // Act
            IEnumerable<Attribute> attributes = AttributeCache.GetAttributes(
                type,
                property,
                attribute => attribute.GetType() == typeof(AttributeFixture));

            // Assert
            Assert.IsTrue(attributes.Count() == 2);
        }



        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_attribute_with_cache()
        {
            // Arrange
            var type = typeof(TypePoolFixture);
            var fixture = new TypePoolFixture();
            var property = PropertyCache.GetProperty(type, p => p.Name == nameof(fixture.PropertyWithAttribute));
            IEnumerable<Attribute> attributes = AttributeCache.GetAttributes(type, property);

            // Act

            // Assert
            Assert.IsTrue(attributes.Count() == 3);
        }
        #endregion
    }
}
