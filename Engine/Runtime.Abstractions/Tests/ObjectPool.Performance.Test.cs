using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Fakes;
using MudDesigner.Runtime;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class ObjectPoolPerformanceTests
    {
        const int _iterations = 10000000;

        [TestMethod]
        public void BasicRent()
        {
            // Arrange
            var pool = new ObjectPool<MessageFake>(100);
            var watch = new Stopwatch();
            var times = new List<double>();
            double currentTime;
            double timeStamp;

            // Act
            watch.Start();
            for(int count = 0; count < _iterations; count++)
            {
                currentTime = watch.Elapsed.Ticks;
                MessageFake instance = pool.Rent();
                timeStamp = watch.Elapsed.Ticks;

                times.Add(timeStamp - currentTime);
                pool.Return(instance);
            }

            watch.Stop();
            Debug.WriteLine($"Average ticks: {times.Average()}");
        }

        [TestMethod]
        public void RentWithFactory()
        {
            // Arrange
            var pool = new ObjectPool<MessageFake>(100, () => new MessageFake { Content = "Foo Bar" });
            var watch = new Stopwatch();
            var times = new List<double>();
            double currentTime;
            double timeStamp;

            // Act
            watch.Start();
            for (int count = 0; count < _iterations; count++)
            {
                currentTime = watch.Elapsed.Ticks;
                MessageFake instance = pool.Rent();
                timeStamp = watch.Elapsed.Ticks;

                times.Add(timeStamp - currentTime);
                pool.Return(instance);
            }

            watch.Stop();
            Debug.WriteLine($"Average ticks: {times.Average()}");
        }

        [TestMethod]
        public void RentWithResettingViaCallback()
        {
            // Arrange
            var pool = new ObjectPool<MessageFake>(100, (instance) => instance.Content = "Foo Bar");
            var watch = new Stopwatch();
            var times = new List<double>();
            double currentTime;
            double timeStamp;

            // Act
            watch.Start();
            for (int count = 0; count < _iterations; count++)
            {
                currentTime = watch.Elapsed.Ticks;
                MessageFake instance = pool.Rent();
                timeStamp = watch.Elapsed.Ticks;

                times.Add(timeStamp - currentTime);
                pool.Return(instance);
            }

            watch.Stop();
            Debug.WriteLine($"Average ticks: {times.Average()}");
        }

        [TestMethod]
        public void RentWithResettingViaInterface()
        {
            // Arrange
            var pool = new ObjectPool<ResettableMessageFake>(100);
            var watch = new Stopwatch();
            var times = new List<double>();
            double currentTime;
            double timeStamp;

            // Act
            watch.Start();
            for (int count = 0; count < _iterations; count++)
            {
                currentTime = watch.Elapsed.Ticks;
                ResettableMessageFake instance = pool.Rent();
                timeStamp = watch.Elapsed.Ticks;

                times.Add(timeStamp - currentTime);
                pool.Return(instance);
            }

            watch.Stop();
            Debug.WriteLine($"Average ticks: {times.Average()}");
        }
    }
}
