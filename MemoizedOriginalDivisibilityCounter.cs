using System;
using System.Collections.Generic;

namespace CountDivisibleNumbers
{
    /// <summary>
    /// Provides an optimized memoized version of the original recursive divisibility algorithm,
    /// using the Inclusion-Exclusion Principle with caching to eliminate redundant calculations and memory allocations.
    /// </summary>
    public static class MemoizedOriginalDivisibilityCounter
    {
        private static readonly Dictionary<(long n, int index), long> _memo = new();

        /// <summary>
        /// Calculates the count of numbers in the range [m, n] that are divisible 
        /// by any of the numbers in the test array p, utilizing memoization.
        /// </summary>
        /// <param name="m">The lower bound of the range (inclusive).</param>
        /// <param name="n">The upper bound of the range (inclusive).</param>
        /// <param name="p">An array of test numbers for the divisibility check.</param>
        /// <returns>The count of divisible numbers in the specified range [m, n].</returns>
        public static long CalculateRange(long m, long n, long[] p)
        {
            _memo.Clear();
            long countN = Compute(n, p, 0);

            _memo.Clear();
            long countM1 = Compute(m - 1, p, 0);

            return countN - countM1;
        }

        /// <summary>
        /// Core recursive function with memoization implementing the Inclusion-Exclusion Principle 
        /// to count how many numbers from 1 to n are divisible by any number in array p.
        /// </summary>
        /// <param name="n">The upper limit of the number range [1, n].</param>
        /// <param name="p">The array of test divisors.</param>
        /// <param name="index">The current index in the test array.</param>
        /// <returns>The count of divisible numbers up to n starting from the given index.</returns>
        private static long Compute(long n, long[] p, int index)
        {
            if (index >= p.Length)
                return 0;

            var key = (n, index);
            if (_memo.TryGetValue(key, out long cachedResult))
            {
                return cachedResult;
            }

            long countDivisible = n / p[index];
            long term1 = Compute(n / p[index], p, index + 1);
            long term2 = Compute(n, p, index + 1);

            long r = countDivisible - term1 + term2;

            _memo[key] = r;
            return r;
        }

        /// <summary>
        /// Filters out redundant test divisors to optimize the inclusion-exclusion calculation.
        /// </summary>
        /// <remarks>
        /// Since each additional divisor doubles the number of recursive branches (2^k complexity), 
        /// having redundant numbers (e.g., both 2 and 4) creates unnecessary overhead. 
        /// This method detects and removes multiples (subordinate divisors), drastically 
        /// speeding up all algorithms and preventing redundant computations.
        /// </remarks>
        /// <param name="p">The raw array of test numbers.</param>
        /// <returns>A filtered array containing only independent base divisors.</returns>
        static long[] FilterDivisible(long[] p)
        {
            List<long> q = new List<long>();
            int k = 0;
            long[] tempP = (long[])p.Clone();

            foreach (long e in tempP)
            {
                for (int i = k + 1; i < tempP.Length; i++)
                {
                    if (tempP[k] != 0 && tempP[i] != 0 && tempP[k] % tempP[i] == 0)
                    {
                        tempP[k] = 0;
                        break;
                    }
                }
                if (tempP[k] != 0)
                    q.Add(e);
                k++;
            }

            return q.ToArray();
        }
    }
}