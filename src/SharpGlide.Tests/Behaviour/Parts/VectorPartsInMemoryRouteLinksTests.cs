using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using SharpGlide.Behaviours;
using SharpGlide.Extensions;
using SharpGlide.Flows;
using SharpGlide.Tests.Model.VectorPart;
using SharpGlide.Tunnels.Routes;
using Xunit;

namespace SharpGlide.Tests.Behaviour.Parts
{
    public class VectorPartsInMemoryRouteLinksTests
    {
        // [Fact]
        // public async Task VectorPartShouldFlowDataToAnotherVectorPart()
        // {
        //     // Arrange
        //     var partA = new SimplePublisherWithTwoRoutingWay();
        //     var partB = new SimpleReceiverOfAFlow();
        //     var partC = new SimpleReceiverOfBFlow();
        //
        //     partA.ConfigureStartAs<StartInBackground>();
        //     partB.ConfigureStartAs<StartInBackground>();
        //     partC.ConfigureStartAs<StartInBackground>();
        //
        //     var consumeRoute = ConsumeRoute.Default;
        //     partA.FlowFromSelf(consumeRoute);
        //
        //     partA.FlowTo(partB, new AFlow(), new AbXFlow());
        //     partA.FlowTo(partC, new AFlow(), new AbYFlow());
        //
        //     // Act
        //     partA.TakeAndConsume(10);
        //     await partA.StartAsync();
        //     await partB.StartAsync();
        //     await partC.StartAsync();
        //
        //     // Assert
        //     partA.WasPublished("20").Should().BeTrue();
        //     partA.WasPublished("30").Should().BeTrue();
        //
        //     partB.WasConsumed("20").Should().BeTrue();
        //     partB.WasConsumed("30").Should().BeFalse();
        //
        //     partC.WasConsumed("30").Should().BeTrue();
        //     partC.WasConsumed("20").Should().BeFalse();
        // }
        //
        // public class AFlow : PublishRoute
        // {
        //     public AFlow()
        //     {
        //         this.Topic = "A->";
        //     }
        // }
        //
        // public class AbXFlow : ConsumeRoute
        // {
        //     public AbXFlow()
        //     {
        //         this.Topic = "A->";
        //         this.Queue = "BXFlow";
        //         this.RoutingKey = "BXFlowRk";
        //     }
        // } 
        //
        // public class AbYFlow : ConsumeRoute
        // {
        //     public AbYFlow()
        //     {
        //         this.Topic = "A->";
        //         this.Queue = "BYFlow";
        //         this.RoutingKey = "BYFlowRk";
        //     }
        // }
        //
        // public class SimplePublisherWithTwoRoutingWay : TestVectorPartAssertable<int, string>
        // {
        //     public override async Task ProcessAsync(int data, CancellationToken cancellationToken)
        //     {
        //         ConsumedData.Add(data);
        //
        //         var valueA = data * 2;
        //         var valueB = data * 3;
        //
        //         Publish(Map(valueA), new AFlow().CreateChild<AFlow>("Rk.AFlow"));
        //         PublishedData.Add(Map(valueA));
        //         Publish(Map(valueB), new AFlow().CreateChild<AFlow>("Rk.BFlow"));
        //         PublishedData.Add(Map(valueB));
        //
        //         await StopAsync();
        //     }
        //
        //     public override string Map(int data) => data.ToString();
        // }
        //
        // public class SimpleReceiverOfAFlow : TestVectorPartAssertable<string, byte[]>
        // {
        //     public override async Task ProcessAsync(string data, CancellationToken cancellationToken)
        //     {
        //         ConsumedData.Add(data);
        //
        //         await StopAsync();
        //     }
        //
        //     public override byte[] Map(string data) => Encoding.UTF8.GetBytes(data);
        // }
        //
        // public class SimpleReceiverOfBFlow : TestVectorPartAssertable<string, byte[]>
        // {
        //     public override async Task ProcessAsync(string data, CancellationToken cancellationToken)
        //     {
        //         ConsumedData.Add(data);
        //         await StopAsync();
        //     }
        //
        //     public override byte[] Map(string data) => Encoding.UTF8.GetBytes(data);
        // }
    }
}