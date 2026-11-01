using System;
using System.Diagnostics;
using System.Linq;
using CountDivisibleNumbers.Algorithms;
using CountDivisibleNumbers.Helpers;

namespace CountDivisibleNumbers.Tests
{
    /// <summary>
    /// Provides performance testing for all divisibility counting algorithms by executing 
    /// multiple iterations, discarding the fastest and slowest outlier runs, and calculating 
    /// the trimmed average execution time for the remaining runs.
    /// </summary>
    public static class DivisibilityPerformanceTester
    {
        public class BenchmarkResult
        {
            public string AlgorithmName { get; set; }
            public long DivisibleCount { get; set; }
            public long NotDivisibleCount { get; set; }
            public TimeSpan TrimmedAverageTime { get; set; }
        }

        /// <summary>
        /// Runs performance tests across all algorithms for a specified number of iterations,
        /// applies trimmed mean filtering (removes 1 fastest and 1 slowest run if iterations >= 3),
        /// and prints/returns the results.
        /// </summary>
        public static void RunPerformanceTests(long m, long n, long[] p, int iterations)
        {
            if (iterations < 3)
            {
                Console.WriteLine("[Warning] Iterations count is less than 3. For reliable trimmed mean, at least 3 iterations are recommended.");
                iterations = Math.Max(1, iterations);
            }

            long totalCount = n - m + 1;

            // Define algorithm wrappers to test
            var algorithms = new[]
            {
                new { Name = "1. Original Recursive", Func = new Func<long>(() => OriginalDivisibilityCounter.CalculateRange(m, n, p)) },
                new { Name = "2. Memoized Recursive", Func = new Func<long>(() => MemoizedOriginalDivisibilityCounter.CalculateRange(m, n, p)) },
                new { Name = "3. LINQ Bitmask", Func = new Func<long>(() => LINQDivisibilityCounter.CalculateRange(m, n, p)) },
                new { Name = "4. Original Naive (Article)", Func = new Func<long>(() => OriginalNaiveDivisibilityCounter.CalculateRange(m, n, p)) },
                new { Name = "5. StringBuilder (Thakur)", Func = new Func<long>(() => StringBuilderDivisibilityCounterDharmrajThakur.CalculateRange(m, n, p)) },
                new { Name = "6. Ultra Optimized", Func = new Func<long>(() => UltraOptimizedDivisibilityCounter.CalculateRange(m, n, p)) },
                new { Name = "7. Ultra Optimized V2", Func = new Func<long>(() => UltraOptimizedDivisibilityCounterV2.CalculateRange(m, n, p)) },
                new { Name = "8. BitMask (Arun Potti)", Func = new Func<long>(() => BitMaskDivisibilityCounterArunPotti.CalculateRange(m, n, p)) }
            };

            Console.WriteLine();
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine($" Running Performance Test ({iterations} iterations)...");
            Console.WriteLine("-------------------------------------------------");

            foreach (var algo in algorithms)
            {
                long lastDivisibleCount = 0;
                var elapsedTimes = new TimeSpan[iterations];

                // Warm-up run (JIT compilation trigger)
                try
                { lastDivisibleCount = algo.Func(); }
                catch { }

                // Execution loop
                for (int i = 0; i < iterations; i++)
                {
                    var sw = Stopwatch.StartNew();
                    try
                    {
                        lastDivisibleCount = algo.Func();
                    }
                    catch (Exception)
                    {
                        lastDivisibleCount = -1; // Error marker
                    }
                    sw.Stop();
                    elapsedTimes[i] = sw.Elapsed;
                }

                // Calculate Trimmed Mean if iterations >= 3
                TimeSpan finalTime;
                if (iterations >= 3)
                {
                    var sortedTimes = elapsedTimes.OrderBy(t => t.Ticks).ToArray();
                    // Discard 1 fastest (index 0) and 1 slowest (index last)
                    var trimmed = sortedTimes.Skip(1).Take(sortedTimes.Length - 2).ToArray();
                    long avgTicks = (long)trimmed.Average(t => t.Ticks);
                    finalTime = TimeSpan.FromTicks(avgTicks);
                }
                else
                {
                    long avgTicks = (long)elapsedTimes.Average(t => t.Ticks);
                    finalTime = TimeSpan.FromTicks(avgTicks);
                }

                long notDivisibleCount = totalCount - lastDivisibleCount;

                // Display individual algorithm summary
                Console.WriteLine();
                Console.WriteLine($"=== {algo.Name} ===");
                Console.WriteLine($" Divisible count     : {lastDivisibleCount}");
                Console.WriteLine($" Not divisible count : {notDivisibleCount}");
                Console.WriteLine($" Trimmed Avg Time    : {finalTime.TotalMilliseconds:F4} ms ({finalTime})");
            }

            Console.WriteLine("-------------------------------------------------");
        }
    }
}