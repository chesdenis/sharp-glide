using System;
using BenchmarkDotNet.Running;
using SharpGlide.Benchmark.Tunnels;

namespace SharpGlide.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Md5VsSha256>();
        }
    }
}