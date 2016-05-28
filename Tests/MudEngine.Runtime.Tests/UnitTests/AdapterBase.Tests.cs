using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MudDesigner.MudEngine.MessageBrokering;
using MudDesigner.MudEngine.Tests.Fixtures;

namespace MudDesigner.MudEngine.Tests.UnitTests
{
    [TestClass]
    public class AdapterBaseTests
    {
        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Adapter_can_set_and_publish_a_broker()
        {
            // Arrange
            var brokerMock = new Mock<IMessageBroker>(MockBehavior.Strict);
            brokerMock.Setup(mock => mock.Publish(It.IsAny<IMessage>())).Verifiable();

            var fixture = Mock.Of<AdapterBase>();
            string content = "Unit Test";

            // Act
            fixture.SetNotificationManager(brokerMock.Object);
            fixture.PublishMessage(new MessageFixture<string>(content));

            // Assert
            brokerMock.Verify(mock => mock.Publish(It.IsAny<IMessage>()));
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Publishing_throws_exception_when_no_broker_has_been_set()
        {
            // Arrange
            var fixture = Mock.Of<AdapterBase>();
            string content = "Unit Test";

            // Act
            fixture.PublishMessage(new MessageFixture<string>(content));

            // Assert
            Assert.AreEqual(fixture.MessageBroker, MessageBrokerFactory.Instance, "Fixture was not assigned the default broker.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void subscribing_throws_exception_when_broker_is_null()
        {
            // Arrange
            var fixture = Mock.Of<AdapterBase>();

            // Act
            fixture.SubscribeToMessage<MessageFixture<string>>((message, subscription) => subscription.Unsubscribe());
            
            // Assert
            Assert.AreEqual(fixture.MessageBroker, MessageBrokerFactory.Instance, "Fixture was not assigned the default broker.");
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Subscribing_can_receive_messages()
        {
            // Arrange
            var brokerMock = new Mock<IMessageBroker>(MockBehavior.Strict);
            brokerMock
                .Setup(mock => mock.Subscribe(It.IsAny<Action<MessageFixture<string>, ISubscription>>(),
                    It.IsAny<Func<IMessage, bool>>()))
                .Returns(Mock.Of<ISubscription>());

            var fixture = Mock.Of<AdapterBase>();

            // Act
            fixture.SetNotificationManager(brokerMock.Object);
            fixture.SubscribeToMessage<MessageFixture<string>>((message, subscription) => { });

            // Assert
            brokerMock.Verify(mock => mock.Subscribe(It.IsAny<Action<MessageFixture<string>, ISubscription>>(), It.IsAny<Func<IMessage, bool>>()));
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Subscribing_twice_does_not_duplicate_the_subscription()
        {
            // Arrange
            int count = 0;
            var brokerMock = new Mock<IMessageBroker>(MockBehavior.Strict);

            // Create a mock that mocks the subscription, returning an ISubscription mock. 
            brokerMock
                .Setup(mock => mock.Subscribe(It.IsAny<Action<MessageFixture<string>, ISubscription>>(),
                    It.IsAny<Func<IMessage, bool>>()))
                .Returns(Mock.Of<ISubscription>());

            // Setup a callback on publication to count how many times our publish method actually publishes
            brokerMock.Setup(mock => mock.Publish(It.IsAny<MessageFixture<string>>())).Callback(() => count += 1);

            var fixture = Mock.Of<AdapterBase>();

            // Act - Test that we do not get hit with two publications, despite having tried to subscribe twice.
            fixture.SetNotificationManager(brokerMock.Object);
            fixture.SubscribeToMessage<MessageFixture<string>>((message, subscription) => { });
            fixture.SubscribeToMessage<MessageFixture<string>>((message, subscription) => { });

            fixture.PublishMessage(new MessageFixture<string>("Unit Test"));

            // Assert
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Unsubscribing_has_subscriber_stop_receiving_publications()
        {
            // Arrange
            int count = 0;
            var brokerMock = new Mock<IMessageBroker>(MockBehavior.Strict);
            Action<MessageFixture<string>, ISubscription> publication = null;
            var subscriptionMock = new Mock<ISubscription>();
            subscriptionMock.Setup(mock => mock.Unsubscribe()).Callback(() => publication = (msg, sub) => { return; });

            // Create a mock that mocks the subscription, returning an ISubscription mock. 
            brokerMock
              .Setup(mock => mock.Subscribe(It.IsAny<Action<MessageFixture<string>, ISubscription>>(),
                  It.IsAny<Func<IMessage, bool>>()))
              .Returns(subscriptionMock.Object)
              .Callback((Action<MessageFixture<string>, ISubscription> callback, Func<IMessage, bool> predicate) => { publication = callback; });

            // Setup a callback on publication to count how many times our publish method actually publishes
            brokerMock.Setup(mock => mock.Publish(It.IsAny<MessageFixture<string>>())).Callback(() => publication(null, subscriptionMock.Object));

            var fixture = Mock.Of<AdapterBase>();
            fixture.SetNotificationManager(brokerMock.Object);

            // Act - Test that we do not get hit with two publications once we unsubscribe
            fixture.SubscribeToMessage<MessageFixture<string>>((message, subscription) => 
            {
                count += 1;
                subscription.Unsubscribe();
            });

            fixture.PublishMessage(new MessageFixture<string>("Unit Test"));
            fixture.PublishMessage(new MessageFixture<string>("Unit Test"));

            // Assert
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Disposing_prevents_publications()
        {
            // Arrange
            int count = 0;
            var brokerMock = new Mock<IMessageBroker>(MockBehavior.Strict);
            Action<MessageFixture<string>, ISubscription> publication = null;
            var subscriptionMock = new Mock<ISubscription>();
            subscriptionMock.Setup(mock => mock.Unsubscribe()).Callback(() => publication = (msg, sub) => { return; });

            // Create a mock that mocks the subscription, returning an ISubscription mock. 
            brokerMock
              .Setup(mock => mock.Subscribe(It.IsAny<Action<MessageFixture<string>, ISubscription>>(),
                  It.IsAny<Func<IMessage, bool>>()))
              .Returns(subscriptionMock.Object)
              .Callback((Action<MessageFixture<string>, ISubscription> callback, Func<IMessage, bool> predicate) => { publication = callback; });

            // Setup a callback on publication to count how many times our publish method actually publishes
            brokerMock.Setup(mock => mock.Publish(It.IsAny<MessageFixture<string>>())).Callback(() => publication(null, subscriptionMock.Object));

            var fixture = new AdapterFixture();
            fixture.SetNotificationManager(brokerMock.Object);

            // Act - Test that we do not get hit with two publications once we unsubscribe
            fixture.SubscribeToMessage<MessageFixture<string>>((message, subscription) =>
            {
                count += 1;
            });

            fixture.Dispose();
            fixture.PublishMessage(new MessageFixture<string>("Unit Test"));
            fixture.PublishMessage(new MessageFixture<string>("Unit Test"));

            // Assert
            Assert.AreEqual(0, count);
        }

        [TestMethod]
        [TestCategory("MudDesigner")]
        [TestCategory("Engine")]
        [TestCategory("Engine Core")]
        [Owner("Johnathon Sullinger")]
        public void Unsubscribing_does_not_throw_exception_when_never_originally_subscribed()
        {
            // Arrange
            var fixture = Mock.Of<AdapterBase>();

            // Act
            fixture.UnsubscribeFromMessage<MessageFixture<string>>();
        }
    }
}
