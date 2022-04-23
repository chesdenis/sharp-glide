using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using SharpGlide.Routing;
using SharpGlide.Tests.Behaviour.Parts.Concept.Examples;
using SharpGlide.Tests.Behaviour.Tunnels.Concept.Examples;
using Xunit;

namespace SharpGlide.Tests.Behaviour.Parts.Concept
{
    public class TwoInTwoOutPartTests
    {
        [Fact]
        public async Task ShouldReadDataFromTwoSourcesAndPushDataToTwoSources()
        {
            // // Arrange
            // var readerA = new DirectReaderExample();
            // var readerB = new DirectReaderExample();
            //
            // var writerA = new DirectWriterExample();
            // var writerB = new DirectWriterExample();
            //
            // var sut = new TwoInTwoOutPart(
            //     readerA.ReadExpr.Compile(), 
            //     readerB.ReadRangeExpr.Compile(),
            //     writerA.WriteExpr.Compile(), 
            //     writerB.WriteRangeExpr.Compile());
            //
            // // Act
            // readerA.Stack.Push(10);
            // readerA.Stack.Push(20);
            // readerA.Stack.Push(30);
            //
            // readerB.Stack.Push(100);
            // readerB.Stack.Push(200);
            // readerB.Stack.Push(300);
            //
            // await sut.ProcessAsync(CancellationToken.None);
            //     
            // // Assert
            // writerA.Stack.Should().HaveCount(1);
            // writerB.Stack.Should().HaveCount(3);
        } 
        
        [Fact]
        public async Task ShouldReadDataFromTwoSources()
        {
            // Arrange
            // var directReader = new DirectReaderExample();
            //
            // Func<Task<decimal>> sourceA = async () =>
            // {
            //     var compile = directReader.ReadExpr.Compile();
            //     return compile();
            // };
            //
            // Func<Task<IEnumerable<decimal>>> sourceBRange = async () => directReader.ReadRangeExpr.Compile()();
            //
            // Func<decimal, Route, Task> writeActionA = async (arg1, route) =>
            // {
            //     
            // };
            //
            // Func<IEnumerable<decimal>, IRoute, Task> writeRangeActionB;
            // var part = new TwoInTwoOutPart(sourceA, sourceBRange, writeActionA, writeRangeActionB);

            // Act
            //await sut.ProcessAsync(CancellationToken.None);

            // Assert

        }
        //
        // [Fact]
        // public async Task ShouldReadDataFromTwoSourcesAndPushDataToTwoTypedSources()
        // {
        //     // Arrange
        //     var readerA = new DirectReaderExample();
        //     var readerB = new DirectReaderExample();
        //    
        //     var writerA = new DirectWriterExample();
        //     var writerB = new DirectWriterExample();
        //
        //     var sut = new TwoInTwoOutPart(
        //          readerA.ReadExpr.Compile(), 
        //         readerB.ReadRangeExpr.Compile(),
        //         writerA.WriteExpr.Compile(), 
        //         writerB.WriteRangeExpr.Compile());
        //
        //     // Act
        //     readerA.Stack.Push(10);
        //     readerA.Stack.Push(20);
        //     readerA.Stack.Push(30);
        //     
        //     readerB.Stack.Push(100);
        //     readerB.Stack.Push(200);
        //     readerB.Stack.Push(300);
        //
        //     await sut.ProcessAsync(CancellationToken.None);
        //         
        //     // Assert
        //     writerA.Stack.Should().HaveCount(1);
        //     writerB.Stack.Should().HaveCount(3);
        // }

    }
}