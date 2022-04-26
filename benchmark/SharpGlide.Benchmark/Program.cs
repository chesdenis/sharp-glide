using System;
using BenchmarkDotNet.Running;
using SharpGlide.Benchmark.Tunnels;

namespace SharpGlide.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            //BenchmarkRunner.Run<ReaderBenchmarkTests>();
            
            // //new ReaderBenchmarkTests().ReaderDirectBenchmark();
            // new ReaderBenchmarkTests().ReaderViaCallbackBenchmark();
        }
    }
}