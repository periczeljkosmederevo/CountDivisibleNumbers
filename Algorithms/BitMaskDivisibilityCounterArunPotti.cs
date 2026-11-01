using System.Linq;
using System.Text;

namespace CountDivisibleNumbers.Algorithms
{
    /// <summary>
    /// Provides an optimized bit-masking implementation of the Inclusion-Exclusion Principle 
    /// for counting and finding numbers divisible by any of the given test numbers within a range [m, n]. 
    /// Avoids array allocations and recursion by iterating over all prime combinations using bit masks.
    /// </summary>
    public static class BitMaskDivisibilityCounterArunPotti
    {
        /// <summary>
        /// Calculates the count of numbers in the range [m, n] that are divisible 
        /// by any of the numbers in the test array p using the bit-masking Inclusion-Exclusion Principle.
        /// </summary>
        /// <param name="m">The lower bound of the range (inclusive).</param>
        /// <param name="n">The upper bound of the range (inclusive).</param>
        /// <param name="p">An array of test numbers for the divisibility check (pre-filtered and sorted).</param>
        /// <returns>The count of divisible numbers in the specified range [m, n].</returns>
        public static long CalculateRange(long m, long n, long[] p)
        {
            if (n <= 0 || m > n || p == null || p.Length == 0)
                return 0;

            long countN = Calculate(n, p);
            long lower = m <= 1 ? 0 : m - 1;
            long countLower = Calculate(lower, p);

            return countN - countLower;
        }

        /// <summary>
        /// Calculates the count of positive integers in the range [1, n] 
        /// that are divisible by at least one divisor from the specified array p 
        /// using bit-masking iteration.
        /// </summary>
        /// <param name="n">The upper bound of the range (inclusive).</param>
        /// <param name="p">An array of test divisors.</param>
        /// <returns>The number of integers in the range [1, n] divisible by at least one divisor.</returns>
        public static long Calculate(long n, long[] p)
        {
            if (n <= 0 || p == null || p.Length == 0)
                return 0;

            long result = 0;
            int combinations = 1 << p.Length;

            for (int mask = 1; mask < combinations; mask++)
            {
                long product = 1;
                int count = 0;
                bool valid = true;

                for (int i = 0; i < p.Length; i++)
                {
                    if ((mask & (1 << i)) == 0)
                        continue;

                    // Avoid overflow and combinations that cannot divide any number <= n.
                    if (product > n / p[i])
                    {
                        valid = false;
                        break;
                    }
                    product *= p[i];
                    count++;
                }

                if (!valid)
                    continue;

                long divisible = n / product;
                if ((count & 1) == 1)
                    result += divisible;
                else
                    result -= divisible;
            }

            return result;
        }
    }
}