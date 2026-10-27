/************************************************************************
 * Program Licence :                                                    *
 *                                                                      *
 * Copyright © 13.1.2018 , Perić Željko                                 *
 * (periczeljkosmederevo@yahoo.com)                                     *
 *                                                                      *
 * According to it's main purpose , this program is licensed            *
 * under the therms of 'Free Software' licence agreement.               *
 *                                                                      *
 * If You do not know what those therms applies to                      *
 * please read explanation at the following link :                      *
 * (http://www.gnu.org/philosophy/free-sw.html.en)                      *
 *                                                                      *
 * Since it is Free Software this program has no warranty of any kind.  *
 ************************************************************************
 * Ethical Notice :                                                     *
 *                                                                      *
 * It is not ethical to change program code signed by it's author       *
 * and then to redistribute it under the same author name ,             *
 * especially if it is incorrect.                                       *
 *                                                                      *
 * It is recommended that if You make improvement in program code ,     *
 * to make remarks of it and then to sign it with Your own name ,       *
 * for further redistribution as new major version of program.          *
 *                                                                      *
 * Author name and references of old program code version should be     *
 * kept , for tracking history of program development.                  *
 *                                                                      *
 * For any further information please contact code author at his email. *
 ************************************************************************/

/***************************************
 * List Of Revisions                   *
 ***************************************
 * 13.01.2018 : Version 1.0            *
 *              Initial release        *
 *              of recursive counter   *
 *              by Perić Željko.       *
 *              (Free Software)        *
 *                                     *
 * 27.10.2026   Major revision         *
 *              Upgraded for .NET,     *
 *              extended support to    *
 *              long data types, and   *
 *              added Memoized and     *
 *              Linq implementations   *
 *              for benchmarking.      *
 *              New version 2.0        *
 ***************************************/

/*************************************************************************
 * Program name  : Count divisible numbers                               *
 * Program ver.  : 2.0                                                   *
 * Created by    : SharpDevelop                                          *
 * Updated by    : Visual Studio 2026                                    *
 * Code author   : Perić Željko                                          *
 * Code language : C#                                                    *
 * Date created  : 13.1.2018                                             *
 * Time created  : 9:27                                                  *
 * Last updated : 27.10.2026                                             *
 * Time updated : 19:15                                                  *
 *                                                                       *
 * Program Description : Program counts how many numbers in a selected   *
 *                       range [1-n] are divisible by any of             *
 *                       given test numbers                              *
 *                                                 All th best,          *
 *                                                 Author                *
 *                                                                       *
 *************************************************************************/

using BenchmarkDotNet.Running;
using System.Diagnostics;

namespace CountDivisibleNumbers
{
    class Program
    {
        public static void Main()
        {
            Console.Title = " Count divisible numbers by any of given numbers (Multi-Algorithm Benchmark)";

            try
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Green;
            }
            catch { }

            ConsoleKeyInfo answer = new ConsoleKeyInfo('Y', ConsoleKey.Y, true, true, true);

            while (answer.Key == ConsoleKey.Y)
            {
                Console.Clear();
                Console.WriteLine("-------------------------------------------------");
                Console.WriteLine(" Program for counting numbers in a given range   ");
                Console.WriteLine();
                Console.WriteLine("        [ 1 <= m <= n <= long.maximum ]          ");
                Console.WriteLine();
                Console.WriteLine("   divisible by any of given p(k) test numbers   ");
                Console.WriteLine("-------------------------------------------------");
                Console.WriteLine();

                Console.WriteLine(" Enter range of numbers [m,n] to be tested : ");
                Console.WriteLine();
                Console.Write(" m = ");
                long m = long.Parse(Console.ReadLine());
                Console.WriteLine();
                Console.Write(" n = ");
                long n = long.Parse(Console.ReadLine());

                Console.WriteLine();
                Console.Write(" Enter count of test numbers : ");
                int k = int.Parse(Console.ReadLine());
                Console.WriteLine();

                long[] p = new long[k];
                List<long> s = new List<long>();

                for (int q = 0; q < k; q++)
                {
                    Console.Write(" Test number " + (q + 1) + " = ");
                    p[q] = long.Parse(Console.ReadLine());

                    if (!s.Contains(p[q]))
                        s.Add(p[q]);
                }

                p = s.ToArray();
                Console.WriteLine();
                Console.WriteLine("-------------------------------------------------");

                Array.Sort(p);
                Array.Reverse(p);
                p = FilterDivisible(p);

                long totalCount = n - m + 1;

                // --- 1. TESTIRANJE STAROG ALGORITMA ---
                var sw1 = Stopwatch.StartNew();
                long x1 = OriginalDivisibility.CalculateRange(m, n, p);
                sw1.Stop();
                long y1 = totalCount - x1;

                // --- 2. TESTIRANJE MEMOIZED ALGORITMA ---
                var sw2 = Stopwatch.StartNew();
                long x2 = MemoizedOriginalDivisibility.CalculateRange(m, n, p);
                sw2.Stop();
                long y2 = totalCount - x2;

                // --- 3. TESTIRANJE LINQ ALGORITMA ---
                var sw3 = Stopwatch.StartNew();
                long x3 = LinqDivisibility.CalculateRange(m, n, p);
                sw3.Stop();
                long y3 = totalCount - x3;

                // --- PRIKAZ REZULTATA ---
                Console.WriteLine();
                Console.WriteLine("=== 1. ORIGINALNI ALGORITAM ===");
                Console.WriteLine(" Divisible count     : " + x1);
                Console.WriteLine(" Not divisible count : " + y1);
                Console.WriteLine(" Vreme izvršavanja   : " + sw1.ElapsedMilliseconds + " ms");

                Console.WriteLine();
                Console.WriteLine("=== 2. MEMOIZED ALGORITAM ===");
                Console.WriteLine(" Divisible count     : " + x2);
                Console.WriteLine(" Not divisible count : " + y2);
                Console.WriteLine(" Vreme izvršavanja   : " + sw2.ElapsedMilliseconds + " ms");

                Console.WriteLine();
                Console.WriteLine("=== 3. LINQ ALGORITAM ===");
                Console.WriteLine(" Divisible count     : " + x3);
                Console.WriteLine(" Not divisible count : " + y3);
                Console.WriteLine(" Vreme izvršavanja   : " + sw3.ElapsedMilliseconds + " ms");
                Console.WriteLine("-------------------------------------------------");

                Console.WriteLine();
                var summary = BenchmarkRunner.Run<DivisibilityBenchmark>();
                Console.WriteLine();
                Console.Write(" Another Test [Y/N] ");
                answer = Console.ReadKey(true);
            }
        }

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