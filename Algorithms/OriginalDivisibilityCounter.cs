namespace CountDivisibleNumbers.Algorithms
{
    /// <summary>
    /// Provides the original recursive implementation for counting numbers 
    /// divisible by any of the given test numbers within a range [m, n], 
    /// based on set theory and the Inclusion-Exclusion Principle.
    /// </summary>
    public static class OriginalDivisibilityCounter
    {
        /// <summary>
        /// Calculates the count of numbers in the range [m, n] that are divisible 
        /// by any of the numbers in the test array p.
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
            return F(n, p) - F(m - 1, p);
        }

        /// <summary>
        /// Core recursive function implementing the Inclusion-Exclusion Principle 
        /// to count how many numbers from 1 to n are divisible by any number in array p.
        /// </summary>
        /// <param name="n">The upper limit of the number range [1, n].</param>
        /// <param name="p">The array of test divisors (must contain no duplicates or mutual multiples).</param>
        /// <returns>The count of divisible numbers up to n.</returns>
        /// <remarks>
        /// IMPORTANT: No duplicates are allowed, and no mutually divisible numbers are allowed 
        /// (e.g., 4 and 2, or 35 and 5). The input array p must be filtered beforehand 
        /// to ensure correct mathematical results and optimal recursive performance.
        /// </remarks>
        private static long F(long n, long[] p)
        {
            if (p.Length == 0)
                return 0;

            long[] q = new long[p.Length - 1];
            Array.Copy(p, 1, q, 0, p.Length - 1);

            long r = (n / p[0]) - (F(n / p[0], q)) + (F(n, q));

            return r;
        }
    }
}