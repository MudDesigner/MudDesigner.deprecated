using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MudDesigner.MudEngine.Tests.Fixture;

namespace MudDesigner.MudEngine.Tests
{
    [TestClass]
    public class ReflectedCacheTests
    {
        [TestInitialize]
        public void Setup()
        {
            ReflectedCache.ClearAllReflectedCache();
        }

        #region Performance Tests
        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Performance")]
        [Owner("Johnathon Sullinger")]
        public void Performance_test_get_properties_for_type_without_cache()
        {
            // Arrange
            // Pre-load all of the Domain Types so we can test against a Pool containing existing objects.
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());
            types.AsParallel().ForAll(type => PropertyCache.GetPropertiesForType(type));
            var times = new List<double>();
            const int _iterations = 1000;

            // Act
            for (int count = 0; count < _iterations; count++)
            {
                // Remove PayItem so we can test it not existing in the Pool.
                TypeCache.ClearTypeFromPool<TypePoolFixture>();

                // Act
                var timer = new Stopwatch();
                timer.Start();
                IEnumerable<PropertyInfo> results =
                    PropertyCache.GetPropertiesForType<TypePoolFixture>();
                timer.Stop();
                times.Add(timer.Elapsed.TotalMilliseconds);
            }

            Debug.WriteLine($"The average time to fetch an uncached collection of properties over {_iterations} iterations was {times.Sum() / times.Count}ms");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Performance")]
        [Owner("Johnathon Sullinger")]
        public void Performance_test_get_properties_for_type_with_cache()
        {
            // Arrange
            // Pre-load all of the Domain Types so we can test against a Pool containing existing objects.
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());
            types.AsParallel().ForAll(type => PropertyCache.GetPropertiesForType(type));
            const int _iterations = 1000;
            var times = new List<double>();

            // Act
            for (int count = 0; count < _iterations; count++)
            {
                var timer = new Stopwatch();
                timer.Start();
                IEnumerable<PropertyInfo> results =
                    PropertyCache.GetPropertiesForType<TypePoolFixture>();
                timer.Stop();
                times.Add(timer.Elapsed.TotalMilliseconds);
            }

            Debug.WriteLine($"The average time to fetch a cached collection of properties over {_iterations} iterations was {times.Sum() / times.Count}ms");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Performance")]
        [Owner("Johnathon Sullinger")]
        public void Performance_test_get_properties_for_type_without_cache_using_predicate()
        {
            // Arrange
            // Pre-load all of the Types so we can test against a Pool containing existing objects.
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());
            types.AsParallel().ForAll(type => PropertyCache.GetPropertiesForType(type));
            const int _iterations = 1000;
            var times = new List<double>();


            // Act
            for (int count = 0; count < _iterations; count++)
            {
                TypeCache.ClearTypeFromPool<TypePoolFixture>();
                var timer = new Stopwatch();
                timer.Start();
                IEnumerable<PropertyInfo> results =
                    PropertyCache.GetPropertiesForType<TypePoolFixture>(info => Attribute.IsDefined(info, typeof(AttributeFixture)));
                timer.Stop();
                times.Add(timer.Elapsed.TotalMilliseconds);
            }

            Debug.WriteLine($"The average time to fetch an uncached collection of filtered properties over {_iterations} iterations was {times.Sum() / times.Count}ms");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Performance")]
        [Owner("Johnathon Sullinger")]
        public void Performance_test_get_properties_for_type_with_cache_using_predicate()
        {
            // Arrange
            // Pre-load all of the Types so we can test against a Pool containing existing objects.
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());
            types.AsParallel().ForAll(type => PropertyCache.GetPropertiesForType(type));
            const int _iterations = 1000;
            var times = new List<double>();

            AttributeCache.GetAttributes(typeof(TypePoolFixture));

            // Act
            for (int count = 0; count < _iterations; count++)
            {
                var timer = new Stopwatch();
                timer.Start();
                IEnumerable<PropertyInfo> results =
                    PropertyCache.GetPropertiesForType<TypePoolFixture>(info => Attribute.IsDefined(info, typeof(AttributeFixture)));
                timer.Stop();
                times.Add(timer.Elapsed.TotalMilliseconds);
            }

            Debug.WriteLine($"The average time to fetch a cached collection of filtered properties over {_iterations} iterations was {times.Sum() / times.Count}ms");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Performance")]
        [Owner("Johnathon Sullinger")]
        public void Performance_test_get_properties_for_type_without_cache_from_large_cache_pool()
        {
            // Arrange
            // Pre-load all of the Types so we can test against a Pool containing existing objects.
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());
            types.AsParallel().ForAll(type => PropertyCache.GetPropertiesForType(type));
            const int _iterations = 1000;
            var times = new List<double>();

            // Act
            for (int count = 0; count < _iterations; count++)
            {
                // Remove PayItem so we can test it not existing in the Pool.
                TypeCache.ClearTypeFromPool<TypePoolFixture>();

                var timer = new Stopwatch();
                timer.Start();
                var results = PropertyCache.GetPropertiesForType<TypePoolFixture>();
                timer.Stop();
                times.Add(timer.Elapsed.TotalMilliseconds);
            }

            Debug.WriteLine($"The average time to fetch an uncached collection properties from a large pool over {_iterations} iterations was {times.Sum() / times.Count}ms");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Performance")]
        [Owner("Johnathon Sullinger")]
        public void Performance_test_get_properties_for_type_with_cache_from_large_cache_pool()
        {
            // Arrange
            // Pre-load all of the Types so we can test against a Pool containing existing objects.
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());
            types.AsParallel().ForAll(type => PropertyCache.GetPropertiesForType(type));
            const int _iterations = 1000;
            var times = new List<double>();

            // Act
            for (int count = 0; count < _iterations; count++)
            {
                var timer = new Stopwatch();
                timer.Start();
                var results = PropertyCache.GetPropertiesForType<TypePoolFixture>();
                timer.Stop();
                times.Add(timer.Elapsed.TotalMilliseconds);
            }

            Debug.WriteLine($"The average time to fetch a cached collection properties from a large pool over {_iterations} iterations was {times.Sum() / times.Count}ms");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Performance")]
        [Owner("Johnathon Sullinger")]
        public void Performance_test_get_properties_for_type_without_cache_from_large_cache_pool_using_predicate()
        {
            // Arrange
            // Pre-load all of the Types so we can test against a Pool containing existing objects.
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());
            types.AsParallel().ForAll(type => PropertyCache.GetPropertiesForType(type));
            const int _iterations = 1000;
            var times = new List<double>();

            // Act
            for (int count = 0; count < _iterations; count++)
            {
                TypeCache.ClearTypeFromPool<TypePoolFixture>();
                var timer = new Stopwatch();
                timer.Start();
                var results = PropertyCache.GetProperty<TypePoolFixture>(
                        property => Attribute.IsDefined(property, typeof(AttributeFixture)));
                timer.Stop();
                times.Add(timer.Elapsed.TotalMilliseconds);
            }

            Debug.WriteLine($"The average time to fetch an uncached collection of filtered properties from a large pool over {_iterations} iterations was {times.Sum() / times.Count}ms");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Performance")]
        [Owner("Johnathon Sullinger")]
        public void Performance_test_get_properties_for_type_with_cache_from_large_cache_pool_using_predicate()
        {
            // Arrange
            // Pre-load all of the Types so we can test against a Pool containing existing objects.
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());
            types.AsParallel().ForAll(type => PropertyCache.GetPropertiesForType(type));
            const int _iterations = 1000;
            var times = new List<double>();

            // Act
            for (int count = 0; count < _iterations; count++)
            {
                var timer = new Stopwatch();
                timer.Start();
                var results =
                    PropertyCache.GetPropertiesForType<TypePoolFixture>(
                    property => Attribute.IsDefined(property, typeof(AttributeFixture)));
                timer.Stop();
                times.Add(timer.Elapsed.TotalMilliseconds);
            }

            Debug.WriteLine($"The average time to fetch a cached collection of filtered properties from a large pool over {_iterations} iterations was {times.Sum() / times.Count}ms");
        }
        #endregion
    }
}
