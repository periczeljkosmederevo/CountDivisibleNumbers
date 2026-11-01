using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CountDivisibleNumbers.Algorithms
{
    /// <summary>
    /// Provides an ultra-optimized memoized version of the recursive divisibility algorithm,
    /// using the Inclusion-Exclusion Principle with single-lookup caching (via CollectionsMarshal) 
    /// to eliminate redundant calculations and memory allocations.
    /// </summary>
    public static class UltraOptimizedDivisibilityCounter
    {
        // Dictionary for memoization storing precalculated results by key (n, index)
        private static readonly Dictionary<(long n, int index), long> _memo = new();

        /// <summary>
        /// Calculates the count of numbers in the range [m, n] that are divisible 
        /// by any of the numbers in the test array p, utilizing memoization.
        /// </summary>
        /// <param name="m">The lower bound of the range (inclusive).</param>
        /// <param name="n">The upper bound of the range (inclusive).</param>
        /// <param name="p">An array of test numbers for the divisibility check.</param>
        /// <returns>The count of divisible numbers in the specified range [m, n].</returns>
        /// <remarks>
        /// IMPORTANT: No duplicates are allowed, and no mutually divisible numbers are allowed 
        /// (e.g., 4 and 2, or 35 and 5). The input array p must be filtered beforehand 
        /// to ensure correct mathematical results and optimal recursive performance.
        /// </remarks>
        public static long CalculateRange(long m, long n, long[] p)
        {
            _memo.Clear();
            long countN = Compute(n, p, 0);
            _memo.Clear();
            long countM1 = Compute(m - 1, p, 0);
            return countN - countM1;
        }

        /// <summary>
        /// Initiates the divisibility calculation for the range [1, n] after clearing the cache.
        /// </summary>
        /// <param name="n">The upper limit of the number range [1, n].</param>
        /// <param name="p">An array of test divisors.</param>
        /// <returns>The count of divisible numbers in the range from 1 to n.</returns>
        public static long Calculate(long n, long[] p)
        {
            _memo.Clear();
            return Compute(n, p, 0);
        }

        /// <summary>
        /// Core recursive function with memoization implementing the Inclusion-Exclusion Principle 
        /// to count how many numbers from 1 to n are divisible by any number in array p, 
        /// optimized with single-lookup caching via CollectionsMarshal.
        /// </summary>
        /// <param name="n">The upper limit of the number range [1, n].</param>
        /// <param name="p">The array of test divisors.</param>
        /// <param name="index">The current index in the test array.</param>
        /// <returns>The count of divisible numbers up to n starting from the given index.</returns>
        /// <remarks>
        /// IMPORTANT: No duplicates are allowed, and no mutually divisible numbers are allowed 
        /// (e.g., 4 and 2, or 35 and 5). The input array p must be filtered beforehand 
        /// to ensure correct mathematical results and optimal recursive performance.
        /// </remarks>
        private static long Compute(long n, long[] p, int index)
        {
            if (index >= p.Length)
                return 0;

            var key = (n, index);

            // SINGLE-LOOKUP SEARCH AND INSERT: Returns a reference to the value in the dictionary.
            // If the key exists, 'exists' is true and we immediately return the cached result.
            // If it does not exist, space is allocated, 'exists' is false, and 'r' becomes a reference 
            // where we directly write the computed value without performing another lookup!
            ref long r = ref CollectionsMarshal.GetValueRefOrAddDefault(_memo, key, out bool exists);
            if (exists)
            {
                return r;
            }

            long countDivisible = n / p[index];
            long term1 = Compute(n / p[index], p, index + 1);
            long term2 = Compute(n, p, index + 1);

            r = countDivisible - term1 + term2;
            return r;
        }
    }
}