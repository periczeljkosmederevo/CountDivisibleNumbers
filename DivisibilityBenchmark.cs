using BenchmarkDotNet.Attributes;

namespace CountDivisibleNumbers
{
    [MemoryDiagnoser] // Tracks memory allocation and Garbage Collector activity
    [RPlotExporter]   // Generates graphical reports (optional)
    public class DivisibilityBenchmark
    {
        // We can define different ranges for n using [Params]
        [Params(1_000_000L, 1_000_000_000_000L)]
        public long N { get; set; }

        private long _m;
        private long[] _p;

        [GlobalSetup]
        public void Setup()
        {
            _m = 1;
            // Test array of 10 numbers (or more as needed)
            _p = new long[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 27 };
        }

        [Benchmark(Baseline = true)]
        public long Original()
        {
            return OriginalDivisibilityCounter.CalculateRange(_m, N, _p);
        }

        [Benchmark]
        public long Memoized()
        {
            return MemoizedOriginalDivisibilityCounter.CalculateRange(_m, N, _p);
        }

        [Benchmark]
        public long Linq()
        {
            return LINQDivisibilityCounter.CalculateRange(_m, N, _p);
        }
    }
}