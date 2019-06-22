using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LivingDocumentation.Analyzer.Benchmarks
{
    [MemoryDiagnoser]
    [PlainExporter]
    [BenchmarkCategory("Namespace")]
    public class NamespaceTests
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
        public string Namespace(string fullName)
        {
            return fullName.Substring(0, Math.Max(fullName.LastIndexOf('.'), 0)).Trim('.');
        }

        [Benchmark]
        [ArgumentsSource(nameof(FullNames))]
        public string NamespaceSpan(string fullName)
        {
            return fullName.AsSpan().Slice(0, Math.Max(fullName.LastIndexOf('.'), 0)).Trim('.').ToString();
        }

        
        [Benchmark]
        [ArgumentsSource(nameof(FullNames))]
        public string NamespaceSpan2(string fullName)
        {
            var span = fullName.AsSpan();

            return span.Slice(0, Math.Max(span.LastIndexOf('.'), 0)).Trim('.').ToString();
        }
    }
}
