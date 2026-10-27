namespace CountDivisibleNumbers
{
    /// <summary>
    /// Provides a LINQ and bitmask-based implementation for counting numbers 
    /// divisible by any of the given test numbers within a range [m, n], 
    /// utilizing subset iteration and Least Common Multiple (LCM) calculations.
    /// </summary>
    public static class LINQDivisibilityCounter
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
            return CountUpTo(n, p) - CountUpTo(m - 1, p);
        }

        /// <summary>
        /// Counts how many numbers from 1 to n are divisible by any number in array p 
        /// using a LINQ query and bitmask subset generation.
        /// </summary>
        /// <param name="n">The upper limit of the number range [1, n].</param>
        /// <param name="p">The array of test divisors.</param>
        /// <returns>The count of divisible numbers up to n.</returns>
        private static long CountUpTo(long n, long[] p)
        {
            if (n <= 0 || p == null || p.Length == 0)
                return 0;

            int k = p.Length;
            int totalSubsets = 1 << k; // 2^k combinations

            // LINQ query to generate and sum all subsets using bitmasks
            return Enumerable.Range(1, totalSubsets - 1)
                .Sum(mask =>
                {
                    long lcm = 1;
                    int bits = 0;

                    for (int i = 0; i < k; i++)
                    {
                        if ((mask & (1 << i)) != 0)
                        {
                            bits++;
                            lcm = SafeLcm(lcm, p[i]);

                            // If LCM exceeds n, n / LCM will be 0, so we can break early
                            if (lcm > n)
                            {
                                lcm = n + 1;
                                break;
                            }
                        }
                    }

                    long term = n / lcm;
                    // Odd number of elements in the subset adds the term, even subtracts it
                    return (bits % 2 == 1) ? term : -term;
                });
        }

        /// <summary>
        /// Calculates the Greatest Common Divisor (GCD) of two numbers using the Euclidean algorithm.
        /// </summary>
        private static long Gcd(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        /// <summary>
        /// Calculates the Least Common Multiple (LCM) safely, preventing overflow for large long values.
        /// </summary>
        private static long SafeLcm(long a, long b)
        {
            if (a == 0 || b == 0)
                return 0;
            long gcd = Gcd(a, b);
            if (a / gcd > long.MaxValue / b)
                return long.MaxValue;
            return (a / gcd) * b;
        }
    }
}