namespace CountDivisibleNumbers
{
    /// <summary>
    /// Provides a linear scan (brute-force) implementation for counting numbers 
    /// divisible by any of the given test numbers within a range [m, n], 
    /// utilizing LINQ queries for concise divisibility checks.
    /// </summary>
    public static class LinearScanDivisibility
    {
        /// <summary>
        /// Calculates the count of numbers in the range [m, n] that are divisible 
        /// by any of the numbers in the test array p using a linear scan.
        /// </summary>
        /// <param name="m">The lower bound of the range (inclusive).</param>
        /// <param name="n">The upper bound of the range (inclusive).</param>
        /// <param name="p">An array of test numbers for the divisibility check.</param>
        /// <returns>The count of divisible numbers in the specified range [m, n].</returns>
        public static long CalculateRange(long m, long n, long[] p)
        {
            if (n <= 0 || m > n || p == null || p.Length == 0)
                return 0;

            long count = 0;

            for (long i = m; i <= n; i++)
            {
                // .Any() instantly short-circuits on the first matching divisor, 
                // avoiding unnecessary iterations and overhead.
                if (p.Any(divisor => divisor > 0 && i % divisor == 0))
                {
                    count++;
                }
            }

            return count;
        }
    }
}