using BenchmarkDotNet.Running;
using System;

namespace LivingDocumentation.Analyzer.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}
