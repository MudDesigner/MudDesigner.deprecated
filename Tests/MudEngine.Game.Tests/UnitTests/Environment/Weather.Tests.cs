using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MudDesigner.MudEngine.Environment;

namespace MudDesigner.MudEngine.Tests.UnitTests.Environment
{
    [TestClass]
    public class WeatherTests
    {
        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void ClearWeather_instance_has_correct_values()
        {
            // Act
            var weather = new ClearWeather();

            // Assert
            Assert.AreEqual("Clear", weather.Name, "The weather name was not the correct name.");
            Assert.AreEqual(80, weather.OccurrenceProbability, "The probability of this weather occurring is incorrect.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void RainyWeather_instance_has_correct_values()
        {
            // Act
            var weather = new RainyWeather();

            // Assert
            Assert.AreEqual("Rainy", weather.Name, "The weather name was not the correct name.");
            Assert.AreEqual(30, weather.OccurrenceProbability, "The probability of this weather occurring is incorrect.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [TestCategory("Engine Core Environment")]
        [Owner("Johnathon Sullinger")]
        public void ThunderstormWeather_instance_has_correct_values()
        {
            // Act
            var weather = new ThunderstormWeather();

            // Assert
            Assert.AreEqual("Thunderstorm", weather.Name, "The weather name was not the correct name.");
            Assert.AreEqual(15, weather.OccurrenceProbability, "The probability of this weather occurring is incorrect.");
        }
    }
}
