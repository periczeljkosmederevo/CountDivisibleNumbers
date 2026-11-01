namespace CountDivisibleNumbers.Algorithms
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
        /// Core recursive function with memoization implementing the Inclusion-Exclusion Principle 
        /// to count how many numbers from 1 to n are divisible by any number in array p.
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
    }
}