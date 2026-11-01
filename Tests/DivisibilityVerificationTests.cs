using System;
using CountDivisibleNumbers.Algorithms;
using CountDivisibleNumbers.Helpers;

namespace CountDivisibleNumbers.Tests
{
    /// <summary>
    /// Provides automated correctness verification for all divisibility counting algorithms
    /// by comparing their outputs against a trusted reference implementation across various ranges and divisor sets.
    /// </summary>
    public static class DivisibilityVerificationTests
    {
        /// <summary>
        /// Runs correctness tests across all implemented algorithms.
        /// Throws an exception if any algorithm produces a mismatched result.
        /// </summary>
        public static void RunAllTests()
        {
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine(" Running Automated Algorithm Correctness Tests... ");
            Console.WriteLine("-------------------------------------------------");

            // Define test scenarios with different ranges [m, n] and divisor arrays
            var testCases = new[]
            {
                new { M = 1L, N = 1000L, Divisors = new long[] { 2, 3, 5 } },
                new { M = 10L, N = 50000L, Divisors = new long[] { 3, 7, 11 } },
                new { M = 1L, N = 1_000_000L, Divisors = new long[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 31 } },
                new { M = 500L, N = 10_000L, Divisors = new long[] { 4, 2, 10, 5, 7 } } // Tests filtering logic as well
            };

            int testIndex = 1;
            foreach (var test in testCases)
            {
                // Pre-filter divisors using the helper to ensure valid test input
                long[] filteredP = DivisibilityHelper.FilterDuplicatesAndDivisible(test.Divisors);

                // Use the naive linear scan as the ground truth reference
                long expectedCount = OriginalNaiveDivisibilityCounter.CalculateRange(test.M, test.N, filteredP);

                Console.WriteLine($"Test #{testIndex}: Range [{test.M}, {test.N}] | Expected: {expectedCount}");

                // Test each algorithm
                VerifyAlgorithm("1. Original Recursive", OriginalDivisibilityCounter.CalculateRange(test.M, test.N, filteredP), expectedCount);
                VerifyAlgorithm("2. Memoized Recursive", MemoizedOriginalDivisibilityCounter.CalculateRange(test.M, test.N, filteredP), expectedCount);
                VerifyAlgorithm("3. LINQ Bitmask", LINQDivisibilityCounter.CalculateRange(test.M, test.N, filteredP), expectedCount);
                VerifyAlgorithm("4. Original Naive", expectedCount, expectedCount); // Self-test
                VerifyAlgorithm("5. StringBuilder (Dharmraj Thakur)", StringBuilderDivisibilityCounterDharmrajThakur.CalculateRange(test.M, test.N, filteredP), expectedCount);
                VerifyAlgorithm("6. Ultra Optimized (CollectionsMarshal)", UltraOptimizedDivisibilityCounter.CalculateRange(test.M, test.N, filteredP), expectedCount);
                VerifyAlgorithm("7. Ultra Optimized V2 (Int128)", UltraOptimizedDivisibilityCounterV2.CalculateRange(test.M, test.N, filteredP), expectedCount);
                VerifyAlgorithm("8. BitMask (Arun Potti)", BitMaskDivisibilityCounterArunPotti.CalculateRange(test.M, test.N, filteredP), expectedCount);

                Console.WriteLine();
                testIndex++;
            }

            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine(" All algorithms passed correctness verification!  ");
            Console.WriteLine("-------------------------------------------------");
        }

        private static void VerifyAlgorithm(string algorithmName, long actual, long expected)
        {
            if (actual != expected)
            {
                throw new InvalidOperationException($"[FAIL] {algorithmName} returned {actual}, but expected {expected}!");
            }
            else
            {
                Console.WriteLine($"  [PASS] {algorithmName}");
            }
        }
    }
}