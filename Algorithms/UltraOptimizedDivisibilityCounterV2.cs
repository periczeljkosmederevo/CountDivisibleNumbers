namespace CountDivisibleNumbers.Algorithms
{
    /// <summary>
    /// Provides an optimized recursive implementation of the divisibility counting algorithm
    /// using the Inclusion-Exclusion Principle, without memoization or dictionary lookups.
    /// It directly calculates the number of divisible integers within the specified range
    /// by recursively evaluating products of selected divisors.
    /// </summary>
    public static class UltraOptimizedDivisibilityCounterV2
    {
        /// <summary>
        /// Calculates the count of integers in the range [m, n] that are divisible
        /// by at least one divisor from the specified array p.
        /// Uses the Inclusion-Exclusion Principle to calculate the result directly
        /// for the requested range, avoiding separate calculations for n and m - 1.
        /// </summary>
        /// <param name="m">The lower bound of the range (inclusive).</param>
        /// <param name="n">The upper bound of the range (inclusive).</param>
        /// <param name="p">An array of test divisors, sorted in ascending order.</param>
        /// <returns>
        /// The number of integers in the specified range [m, n] divisible
        /// by at least one divisor from the array p.
        /// </returns>
        /// <remarks>
        /// IMPORTANT:
        /// The divisors must be positive, distinct, and pairwise coprime.
        /// No duplicates or divisors sharing common factors are allowed.
        /// The input array p must be filtered beforehand to satisfy these conditions.
        ///
        /// The array should be sorted in ascending order to enable early termination
        /// when a divisor product exceeds the upper bound n.
        ///
        /// Intermediate results are accumulated using Int128 to reduce the risk
        /// of arithmetic overflow during inclusion-exclusion calculations.
        /// The final result is checked when converted to long.
        /// </remarks>
        public static long CalculateRange(long m, long n, long[] p)
        {
            if (m > n || n < 1 || p.Length == 0)
                return 0;

            long lower = m <= 1 ? 0 : m - 1;

            Int128 result = 0;

            Accumulate(n, lower, p, 0, 1, true, ref result);

            return checked((long)result);
        }

        /// <summary>
        /// Calculates the count of positive integers in the range [1, n]
        /// that are divisible by at least one divisor from the specified array p.
        /// </summary>
        /// <param name="n">The upper bound of the range (inclusive).</param>
        /// <param name="p">An array of test divisors, sorted in ascending order.</param>
        /// <returns>
        /// The number of integers in the range [1, n] divisible
        /// by at least one divisor from p.
        /// </returns>
        public static long Calculate(long n, long[] p)
        {
            return CalculateRange(1, n, p);
        }

        /// <summary>
        /// Recursively evaluates the Inclusion-Exclusion Principle by generating
        /// products of selected divisors and accumulating their contribution
        /// to the final count.
        ///
        /// Each recursive level selects the next divisor, calculates the number
        /// of multiples of the resulting product within [lower + 1, n],
        /// and alternates between addition and subtraction according to
        /// the Inclusion-Exclusion Principle.
        ///
        /// Products that exceed the upper bound are skipped, allowing early
        /// termination of the current iteration when divisors are sorted
        /// in ascending order.
        /// </summary>
        /// <param name="n">The upper bound of the counting range.</param>
        /// <param name="lower">The value immediately preceding the lower bound.</param>
        /// <param name="p">The sorted array of test divisors.</param>
        /// <param name="index">The starting index for selecting the next divisor.</param>
        /// <param name="product">The product of previously selected divisors.</param>
        /// <param name="add">
        /// Indicates whether the current contribution must be added or subtracted.
        /// </param>
        /// <param name="result">
        /// A reference to the Int128 accumulator storing the intermediate result.
        /// </param>
        private static void Accumulate(
            long n,
            long lower,
            long[] p,
            int index,
            long product,
            bool add,
            ref Int128 result)
        {
            for (int i = index; i < p.Length; i++)
            {
                long divisor = p[i];

                if (divisor > n / product)
                    break;

                long nextProduct = product * divisor;

                long count = n / nextProduct
                           - lower / nextProduct;

                if (add)
                    result += count;
                else
                    result -= count;

                Accumulate(
                    n,
                    lower,
                    p,
                    i + 1,
                    nextProduct,
                    !add,
                    ref result);
            }
        }
    }
}
