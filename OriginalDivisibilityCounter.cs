namespace CountDivisibleNumbers
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
        public static long CalculateRange(long m, long n, long[] p)
        {
            return F(n, p) - F(m - 1, p);
        }

        /// <summary>
        /// Core recursive function implementing the Inclusion-Exclusion Principle 
        /// to count how many numbers from 1 to n are divisible by any number in array p.
        /// </summary>
        /// <param name="n">The upper limit of the number range [1, n].</param>
        /// <param name="p">The array of test divisors.</param>
        /// <returns>The count of divisible numbers up to n.</returns>
        private static long F(long n, long[] p)
        {
            if (p.Length == 0)
                return 0;

            long[] q = new long[p.Length - 1];
            Array.Copy(p, 1, q, 0, p.Length - 1);

            long r = (n / p[0]) - (F(n / p[0], q)) + (F(n, q));

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