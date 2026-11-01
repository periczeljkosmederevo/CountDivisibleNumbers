using System.Linq;
using System.Text;

namespace CountDivisibleNumbers.Algorithms
{
    /// <summary>
    /// Provides a brute-force (linear scan) implementation for counting and finding numbers 
    /// divisible by any of the given test numbers within a range [m, n]. 
    /// Useful as a baseline reference for correctness verification and small-scale testing.
    /// <para>Original algorithm concept and contribution by Dharmraj Thakur:</para>
    /// <para>https://www.c-sharpcorner.com/members/dharmraj-thakur</para>
    /// </summary>
    public static class StringBuilderDivisibilityCounterDharmrajThakur
    {
        /// <summary>
        /// Calculates the count of numbers in the range [m, n] that are divisible 
        /// by any of the numbers in the test array p using a direct linear scan.
        /// </summary>
        /// <param name="m">The lower bound of the range (inclusive).</param>
        /// <param name="n">The upper bound of the range (inclusive).</param>
        /// <param name="p">An array of test numbers for the divisibility check (pre-filtered).</param>
        /// <returns>The count of divisible numbers in the specified range [m, n].</returns>
        public static long CalculateRange(long m, long n, long[] p)
        {
            if (n <= 0 || m > n || p == null || p.Length == 0)
                return 0;

            long count = 0;

            for (long i = m; i <= n; i++)
            {
                // .Any() short-circuits on the first matching divisor, improving performance
                if (p.Any(divisor => divisor > 0 && i % divisor == 0))
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Finds and returns a comma-separated string of all numbers up to n 
        /// that are divisible by any of the given test numbers (based on the original snippet).
        /// </summary>
        /// <param name="n">The upper limit (exclusive, matching original snippet logic).</param>
        /// <param name="p">An array of test numbers.</param>
        /// <returns>A comma-separated string of divisible numbers.</returns>
        public static string FindDivisibleNumbersString(long n, long[] p)
        {
            StringBuilder result = new StringBuilder();

            for (long i = 1; i < n; i++)
            {
                if (p.Any(m => m > 0 && i % m == 0))
                {
                    result.Append(i).Append(",");
                }
            }

            return result.ToString();
        }
    }
}