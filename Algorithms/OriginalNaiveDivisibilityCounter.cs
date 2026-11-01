namespace CountDivisibleNumbers.Algorithms
{
    /// <summary>
    /// Provides the original naive (brute-force) implementation featured in the article's introduction,
    /// using a linear loop and modulo checks to count divisible numbers.
    /// </summary>
    public static class OriginalNaiveDivisibilityCounter
    {
        /// <summary>
        /// Calculates the count of numbers in the range [m, n] that are divisible 
        /// by any of the numbers in the test array p.
        /// </summary>
        /// <param name="m">The lower bound of the range (inclusive).</param>
        /// <param name="n">The upper bound of the range (inclusive).</param>
        /// <param name="p">An array of test numbers for the divisibility check.</param>
        /// <returns>The count of divisible numbers in the specified range [m, n].</returns>
        public static long CalculateRange(long m, long n, long[] p)
        {
            if (n <= 0 || m > n || p == null || p.Length == 0)
                return 0;

            return CountUpTo(n, p) - CountUpTo(m - 1, p);
        }

        /// <summary>
        /// Core naive function from the article counting numbers from 1 to n 
        /// divisible by any test number in array p using a linear scan.
        /// </summary>
        /// <param name="n">The upper limit of the number range [1, n].</param>
        /// <param name="p">The array of test divisors.</param>
        /// <returns>The count of divisible numbers up to n.</returns>
        public static long CountUpTo(long n, long[] p)
        {
            if (n <= 0 || p == null || p.Length == 0)
                return 0;

            long k = 0;
            for (long i = 1; i <= n; i++)
            {
                foreach (long prime in p)
                {
                    if (i % prime == 0)
                    {
                        k++;
                        break;
                    }
                }
            }
            return k;
        }
    }
}