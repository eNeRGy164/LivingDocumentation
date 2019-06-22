using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LivingDocumentation.Analyzer.Benchmarks
{
    [MemoryDiagnoser]
    [PlainExporter]
    [BenchmarkCategory("Name")]
    public class NameTests
    {
        public IEnumerable<string> FullNames() 
        {
            yield return string.Empty;
            yield return "Test";
            yield return "Identity.API.Migrations.PersistedGrantDb.PersistedGrantDbContextModelSnapshot";
            yield return Enumerable.Range(0, 25).Select(s => "Test").Aggregate((s, s1) => $"{s}.{s1}");
        }

        [Benchmark(Baseline = true)]
        [ArgumentsSource(nameof(FullNames))]
        public string Name(string fullName)
        {
            return fullName.Substring(Math.Max(0, fullName.LastIndexOf('.'))).Trim('.');
        }

        [Benchmark]
        [ArgumentsSource(nameof(FullNames))]
        public string NameSpan(string fullName)
        {
            return fullName.AsSpan().Slice(Math.Max(0, fullName.LastIndexOf('.'))).Trim('.').ToString();
        }

        [Benchmark]
        [ArgumentsSource(nameof(FullNames))]
        public string NameSpan2(string fullName)
        {
            var span = fullName.AsSpan();

            return span.Slice(Math.Max(0, span.LastIndexOf('.'))).Trim('.').ToString();
        }
    }
}
