using BenchmarkDotNet.Attributes;
using CountDivisibleNumbers.Algorithms;
using CountDivisibleNumbers.Helpers;

namespace CountDivisibleNumbers.Benchmark
{
    [MemoryDiagnoser] // Tracks memory allocation and Garbage Collector activity
    [RPlotExporter]   // Generates graphical reports (optional)
    public class DivisibilityBenchmark
    {
        // We can define different ranges for n using [Params]
        [Params(1_000L, 1_000_000L)]//, 1_000_000_000_000_000L)]
        public long N { get; set; }

        private long _m;
        private long[] _p;

        [GlobalSetup]
        public void Setup()
        {
            _m = 1;
            // Test array of divisors (sanitized using the project helper)
            _p = new long[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 31 };
            _p = DivisibilityHelper.FilterDuplicatesAndDivisible(_p);
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
        public long Ultra()
        {
            return UltraOptimizedDivisibilityCounter.CalculateRange(_m, N, _p);
        }

        [Benchmark]
        public long UltraV2()
        {
            return UltraOptimizedDivisibilityCounterV2.CalculateRange(_m, N, _p);
        }

        [Benchmark]
        public long BitMaskPotti()
        {
            return BitMaskDivisibilityCounterArunPotti.CalculateRange(_m, N, _p);
        }

        [Benchmark]
        public long Linq()
        {
            return LINQDivisibilityCounter.CalculateRange(_m, N, _p);
        }

        [Benchmark]
        public long NaiveArticle()
        {
            // Brute-force is too slow for massive numbers, skip for N > 1,000,000
            if (N > 1_000_000L)
                return -1;
            return OriginalNaiveDivisibilityCounter.CalculateRange(_m, N, _p);
        }

        [Benchmark]
        public long StringBuilder()
        {
            // Brute-force is too slow for massive numbers, skip for N > 1,000,000
            if (N > 1_000_000L)
                return -1;
            return StringBuilderDivisibilityCounterDharmrajThakur.CalculateRange(_m, N, _p);
        }
    }
}