using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MudDesigner.MudEngine.Environment;

namespace MudEngine.Game.Tests.UnitTests.Environment
{
    [TestClass]
    public class MudZoneFactoryTests
    {
        /// <summary>
        /// Creates a zone with a name and assigns it.
        /// </summary>
        /// <remarks>
        /// Created to test the solution for <see href="https://github.com/MudDesigner/MudEngine/issues/1"/> issue #1
        /// </remarks>
        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Environment")]
        [Owner("Johnathon Sullinger")]
        public async Task Create_zone_with_name_assigns_name()
        {
            // Arrange
            const string _zoneName = "Test Zone";
            IZoneFactory zoneFactory = new MudZoneFactory(Mock.Of<IRoomFactory>());

            // Act
            IZone zone = await zoneFactory.CreateZone(_zoneName, Mock.Of<IRealm>());

            // Assert
            Assert.AreEqual(_zoneName, zone.Name);
        }

        /// <summary>
        /// Creates a zone with a blank name and tests for an exception
        /// </summary>
        /// <remarks>
        /// Created to test the solution for <see href="https://github.com/MudDesigner/MudEngine/issues/1"/> issue #1
        /// </remarks>
        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Environment")]
        [Owner("Johnathon Sullinger")]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task Exception_thrown_on_creation_of_zone_with_blank_name()
        {
            // Arrange
            IZoneFactory zoneFactory = new MudZoneFactory(Mock.Of<IRoomFactory>());

            // Act
            IZone zone = await zoneFactory.CreateZone(null, Mock.Of<IRealm>());
        }
    }
}
