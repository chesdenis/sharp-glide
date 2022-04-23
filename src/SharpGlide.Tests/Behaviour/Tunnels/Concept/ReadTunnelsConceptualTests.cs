using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using SharpGlide.Tests.Behaviour.Tunnels.Concept.Examples;
using SharpGlide.Tunnels.Readers;
using Xunit;

namespace SharpGlide.Tests.Behaviour.Tunnels.Concept
{
    // public class ReadTunnelsConceptualTests
    // {
    //     [Fact]
    //     public async Task ShouldReadSingleMessage()
    //     {
    //         // Arrange
    //         var sut = new DirectReaderExample();
    //         var func = sut.ReadExpr.Compile();
    //
    //         // Act
    //         sut.Stack.Push(123);
    //         var data = func();
    //
    //         // Assert
    //         data.Should().Be(123);
    //     }
    //
    //     [Fact]
    //     public async Task ShouldReadMultipleData()
    //     {
    //         // Arrange
    //         var sut = new DirectReaderExample();
    //         var func = sut.ReadRangeExpr.Compile();
    //
    //         // Act
    //         sut.Stack.Push(1);
    //         sut.Stack.Push(2);
    //         sut.Stack.Push(3);
    //         var data = func().ToList();
    //
    //         // Assert
    //         data[0].Should().Be(3);
    //         data[1].Should().Be(2);
    //         data[2].Should().Be(1);
    //     }
    //
    //     [Fact]
    //     public async Task ShouldReadMultipleDataOnDemand()
    //     {
    //         // Arrange
    //         var sut = new DirectReaderExample();
    //         var func = sut.ReadRangeExpr.Compile();
    //
    //         // Act
    //         sut.Stack.Push(1);
    //         sut.Stack.Push(2);
    //         sut.Stack.Push(3);
    //         var data = func().Take(2).ToList();
    //
    //         // Assert
    //         data.Count.Should().Be(2);
    //         data[0].Should().Be(3);
    //         data[1].Should().Be(2);
    //     }
    //
    //     [Fact]
    //     public async Task ShouldReadDataUsingCallback()
    //     {
    //         // Arrange
    //         var sut = new ReaderViaCallbackExample();
    //         var func = sut.ReadViaCallbackExpr.Compile();
    //         var collectedData = new List<decimal>();
    //
    //         // Act
    //         sut.Stack.Push(1);
    //         sut.Stack.Push(2);
    //         sut.Stack.Push(3);
    //         func(data => collectedData.Add(data));
    //
    //         // Assert
    //         collectedData.Count.Should().Be(3);
    //         collectedData[0].Should().Be(3);
    //         collectedData[1].Should().Be(2);
    //         collectedData[2].Should().Be(1);
    //     }
    //
    //     [Fact]
    //     public async Task ShouldReadDataUsingCallbackRange()
    //     {
    //         // Arrange
    //         var sut = new ReaderViaCallbackExample();
    //         var func = sut.ReadRangeViaCallbackExpr.Compile();
    //         var collectedData = new List<decimal>();
    //
    //         // Act
    //         sut.Stack.Push(1);
    //         sut.Stack.Push(2);
    //         sut.Stack.Push(3);
    //         func((data) => { collectedData.AddRange(data); });
    //
    //         // Assert
    //         collectedData.Count.Should().Be(3);
    //         collectedData[0].Should().Be(3);
    //         collectedData[1].Should().Be(2);
    //         collectedData[2].Should().Be(1);
    //     }
    //
    //     [Fact]
    //     public async Task ShouldReadDataUsingEvent()
    //     {
    //         // Arrange
    //         var sut = new ReaderViaEventExample();
    //         var func = sut.ReadViaEventExpr.Compile();
    //         var collectedData = new List<decimal>();
    //
    //         sut.OnRead += (sender, e) => collectedData.Add(e);
    //
    //         // Act
    //         sut.Stack.Push(1);
    //         sut.Stack.Push(2);
    //         sut.Stack.Push(3);
    //         func();
    //
    //         // Assert
    //         collectedData.Count.Should().Be(3);
    //         collectedData[0].Should().Be(3);
    //         collectedData[1].Should().Be(2);
    //         collectedData[2].Should().Be(1);
    //     }
    //     
    //     
    //     [Fact]
    //     public async Task ShouldReadDataRangeUsingEvent()
    //     {
    //         // Arrange
    //         var sut = new ReaderViaEventExample();
    //         var func = sut.ReadRangeViaEventCallbackExpr.Compile();
    //         var collectedData = new List<decimal>();
    //
    //         sut.OnReadRange += (sender, e) => collectedData.AddRange(e);
    //
    //         // Act
    //         sut.Stack.Push(1);
    //         sut.Stack.Push(2);
    //         sut.Stack.Push(3);
    //         func();
    //
    //         // Assert
    //         collectedData.Count.Should().Be(3);
    //         collectedData[0].Should().Be(3);
    //         collectedData[1].Should().Be(2);
    //         collectedData[2].Should().Be(1);
    //     }
    //
    //     [Fact]
    //     public async Task ShouldReadPagedData()
    //     {
    //         // Arrange
    //         var sut = new PagedReaderExample();
    //         var func = sut.ReadPageExpr.Compile();
    //         sut.LargeList.Add(10);
    //         sut.LargeList.Add(20);
    //         sut.LargeList.Add(30);
    //         sut.LargeList.Add(40);
    //         sut.LargeList.Add(50);
    //         sut.LargeList.Add(60);
    //
    //         // Act & Assert
    //         func(((pageData, info) =>
    //         {
    //             var pageDataAsArray = pageData as decimal[] ?? pageData.ToArray();
    //             pageDataAsArray.Should().HaveCount(2);
    //             pageDataAsArray.ElementAt(0).Should().Be(10);
    //             pageDataAsArray.ElementAt(1).Should().Be(20);
    //
    //             info.PageIndex.Should().Be(0);
    //
    //         }), new PageInfo
    //         {
    //             PageIndex = 0,
    //             PageSize = 2
    //         });
    //         
    //         func((pageData, info) =>
    //         {
    //             var pageDataAsArray = pageData as decimal[] ?? pageData.ToArray();
    //             pageDataAsArray.Should().HaveCount(2);
    //             pageDataAsArray.ElementAt(0).Should().Be(30);
    //             pageDataAsArray.ElementAt(1).Should().Be(40);
    //             
    //             info.PageIndex.Should().Be(1);
    //         }, new PageInfo
    //         {
    //             PageIndex = 1,
    //             PageSize = 2
    //         });
    //         
    //         func((pageData, info) =>
    //         {
    //             var pageDataAsArray = pageData as decimal[] ?? pageData.ToArray();
    //             pageDataAsArray.Should().HaveCount(6);
    //
    //             info.PageIndex.Should().Be(0);
    //         }, new PageInfo
    //         {
    //             PageIndex = 0,
    //             PageSize = 1000
    //         });
    //     }
    //     
    //      
    //     [Fact]
    //     public async Task ShouldReadDataUsingCustomExpression()
    //     {
    //         // Arrange
    //         var sut = new DirectReaderWithCustomExpression(new DirectReaderExample());
    //         var func = sut.ReadExpr.Compile();
    //         var collectedData = new List<decimal>();
    //
    //         // Act
    //         var data = func();
    //
    //         // Assert
    //         data.Should().Be(10);
    //     }
    // }
}