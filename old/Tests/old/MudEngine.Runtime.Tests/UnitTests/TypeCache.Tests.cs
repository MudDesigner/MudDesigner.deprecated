using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MudDesigner.MudEngine.Tests.Fixture;

namespace MudDesigner.MudEngine.Tests.UnitTests
{
    [TestClass]
    public class TypeCacheTests
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
        public void Remove_type_from_pool_without_cache()
        {
            // Act
            TypeCache.ClearTypeFromPool<ComponentFixture>();

            // Assert
            Assert.IsFalse(TypeCache.HasTypeInCache<ComponentFixture>());
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Remove_type_from_pool_with_existing_cache()
        {
            // Arrange
            PropertyCache.GetPropertiesForType(typeof(ComponentFixture));

            // Act
            TypeCache.ClearTypeFromPool<ComponentFixture>();

            // Assert
            Assert.IsFalse(TypeCache.HasTypeInCache<ComponentFixture>());
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Add_type_adds_a_type_to_cache()
        {
            // Arrange
            Type fixtureType = typeof(ComponentFixture);

            // Act
            TypeCache.AddType(fixtureType);
            Type cachedType = TypeCache.GetType<ComponentFixture>();

            // Assert
            Assert.IsTrue(object.ReferenceEquals(fixtureType, cachedType));
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [ExpectedException(typeof(ArgumentNullException))]
        [Owner("Johnathon Sullinger")]
        public void Clear_type_from_pool_with_null_instance_throws_exception()
        {
            // Act
            TypeCache.ClearTypeFromPool(null);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Clear_type_from_pool_clears_cache()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            TypeCache.AddType(typeof(TypePoolFixture));

            // Act
            TypeCache.ClearTypeFromPool(fixture);

            // Assert
            Assert.IsFalse(TypeCache.HasTypeInCache<TypePoolFixture>());
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Has_type_in_cache_returns_true_when_type_is_cached()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            TypeCache.AddType(typeof(TypePoolFixture));

            // Act
            bool exists = TypeCache.HasTypeInCache(fixture);

            // Assert
            Assert.IsTrue(exists);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Has_type_in_cache_returns_false_when_type_is_never_cached()
        {
            // Arrange
            var fixture = new TypePoolFixture();

            // Act
            bool exists = TypeCache.HasTypeInCache(fixture);

            // Assert
            Assert.IsFalse(exists);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Add_type_twice_does_not_return_duplicates()
        {
            // Arrange
            Type fixtureType = typeof(ComponentFixture);
            Type fixtureType2 = typeof(ComponentFixture);

            // Act
            TypeCache.AddType(fixtureType);
            TypeCache.AddType(fixtureType2);

            IEnumerable<Type> types = TypeCache.GetTypes(type => type == typeof(ComponentFixture));

            // Assert
            Assert.AreEqual(1, types.Count());
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_type_by_name_returns_type()
        {
            // Arrange
            Type componentFixture = typeof(ComponentFixture);
            Type typePoolFixture = typeof(TypePoolFixture);
            TypeCache.AddType(componentFixture);
            TypeCache.AddType(typePoolFixture);

            // Act
            Type returnedType = TypeCache.GetTypeByName(typeof(TypePoolFixture).Name);

            // Assert
            Assert.IsNotNull(returnedType);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_type_from_pool_from_generic()
        {
            // Act
            Type fixtureType = TypeCache.GetType<ComponentFixture>();

            // Assert
            Assert.IsTrue(fixtureType == typeof(ComponentFixture));
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null_type_throws_exception_when_manually_added()
        {
            // Act
            TypeCache.AddType(null);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_type_from_pool_from_parameterized_generic()
        {
            // Arrange
            var fixture = new ComponentFixture();

            // Act
            Type fixtureType = TypeCache.GetType(fixture);

            // Assert
            Assert.IsTrue(fixtureType == typeof(ComponentFixture));
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_types_using_predicate()
        {
            // Arrange
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());
            types.AsParallel().ForAll(type => PropertyCache.GetPropertiesForType(type));

            // Act
            IEnumerable<Type> typeCollection = TypeCache.GetTypes(t => t.IsSubclassOf(typeof(ComponentFixture)));

            // Assert
            Assert.IsTrue(typeCollection.Any());
            Assert.IsTrue(typeCollection.All(t => t.IsSubclassOf(typeof(ComponentFixture))));
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_type_returns_cached_type()
        {
            // Arrange
            Type originalType = TypeCache.GetType<TypePoolFixture>();

            // Act
            Type cachedType = TypeCache.GetType<TypePoolFixture>();

            // Assert
            Assert.AreEqual(typeof(TypePoolFixture), cachedType);
            Assert.IsTrue(object.ReferenceEquals(originalType, cachedType), "The cached type was not returned.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_type_from_instance_returns_a_type()
        {
            // Arrange
            var fixture = new TypePoolFixture();

            // Act
            Type cachedType = TypeCache.GetType(fixture);

            // Assert
            Assert.AreEqual(typeof(TypePoolFixture), cachedType);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Get_type_from_instance_returns_cached_type()
        {
            // Arrange
            var fixture = new TypePoolFixture();
            Type originalType = TypeCache.GetType(fixture);

            // Act
            Type cachedType = TypeCache.GetType(fixture);

            // Assert
            Assert.AreEqual(typeof(TypePoolFixture), cachedType);
            Assert.IsTrue(object.ReferenceEquals(originalType, cachedType), "The cached type was not returned.");
        }
    }
}
