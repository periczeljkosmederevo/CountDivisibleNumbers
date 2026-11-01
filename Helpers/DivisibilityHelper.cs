using System;
using System.Collections.Generic;
using System.Linq;

namespace CountDivisibleNumbers.Helpers
{
    /// <summary>
    /// Provides helper utilities for processing and preparing test divisors 
    /// used in divisibility and inclusion-exclusion algorithms.
    /// </summary>
    public static class DivisibilityHelper
    {
        /// <summary>
        /// Sanitizes the test divisors array by sorting it descending, removing duplicates, 
        /// and filtering out redundant multiples.
        /// </summary>
        /// <remarks>
        /// Since each additional divisor doubles the number of recursive branches (2^k complexity), 
        /// having redundant numbers (duplicates or multiples like 2 and 4) creates unnecessary overhead. 
        /// This method ensures the array is clean, unique, and contains only independent base divisors, 
        /// drastically speeding up all algorithms and preventing redundant computations.
        /// </remarks>
        /// <param name="p">The raw array of test numbers.</param>
        /// <returns>A sorted, duplicate-free array containing only independent base divisors.</returns>
        public static long[] FilterDuplicatesAndDivisible(long[] p)
        {
            if (p == null || p.Length == 0)
                return p;

            // Remove duplicates and sort descending so larger multiples come first
            long[] sortedDistinct = p.Distinct().OrderByDescending(x => x).ToArray();
            List<long> filtered = new List<long>();

            for (int k = 0; k < sortedDistinct.Length; k++)
            {
                bool isRedundant = false;

                // Check if current number is a multiple of any subsequent (smaller) divisor
                for (int i = k + 1; i < sortedDistinct.Length; i++)
                {
                    if (sortedDistinct[k] % sortedDistinct[i] == 0)
                    {
                        isRedundant = true;
                        break;
                    }
                }

                if (!isRedundant)
                {
                    filtered.Add(sortedDistinct[k]);
                }
            }

            return filtered.ToArray();
        }
    }
}