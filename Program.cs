/************************************************************************
* Program Licence :                                                    *
*                                                                      *
* Copyright © 13.1.2018 , Perić Željko                                 *
* (periczeljkosmederevo@yahoo.com)                                     *
*                                                                      *
* According to its main purpose, this program is licensed              *
* under the terms of 'Free Software' licence agreement.                *
*                                                                      *
* If You do not know what those terms apply to                         *
* please read explanation at the following link :                      *
* (http://www.gnu.org/philosophy/free-sw.html.en)                      *
*                                                                      *
* Since it is Free Software this program has no warranty of any kind.  *
 ************************************************************************
* Ethical Notice :                                                     *
*                                                                      *
* It is not ethical to change program code signed by its author        *
* and then to redistribute it under the same author name,              *
* especially if it is incorrect.                                       *
*                                                                      *
* It is recommended that if You make improvement in program code,      *
* to make remarks of it and then to sign it with Your own name,        *
* for further redistribution as new major version of program.          *
*                                                                      *
* Author name and references of old program code version should be     *
* kept, for tracking history of program development.                   *
*                                                                      *
* For any further information please contact code author at his email. *
 ************************************************************************/
/***************************************
* List Of Revisions                    *
 ***************************************
* 13.01.2018 : Version 1.0             *
*              Initial release         *
*              of recursive counter    *
*              by Perić Željko.        *
*              (Free Software)         *
*                                      *
* 27.10.2026   Major revision          *
*              Upgraded for .NET,      *
*              extended support to     *
*              long data types, and    *
*              added Memoized, LINQ,   *
*              and BitMask classes     *
*              for benchmarking.       *
*              New version 2.0         *
 ***************************************/
/************************************************-------------------------
* Program name  : Count divisible numbers                                *
* Program ver.  : 2.0                                                    *
* Created by    : SharpDevelop                                           *
* Updated by    : Visual Studio 2026                                     *
* Code author   : Perić Željko                                           *
* Code language : C#                                                     *
* Date created  : 13.1.2018                                              *
* Time created  : 9:27                                                   *
* Last updated  : 27.10.2026                                             *
* Time updated  : 19:15                                                  *
*                                                                        *
* Description   : Program counts how many numbers in a selected          *
*                 range [1-n] are divisible by any of                    *
*                 given test numbers                                     *
*                                        All the best,                   *
*                                        Author                          *
*                                                                        *
 *************************************************----------------*********/

using BenchmarkDotNet.Running;
using CountDivisibleNumbers.Benchmark;
using CountDivisibleNumbers.Helpers;
using CountDivisibleNumbers.Tests;
using System;
using System.Collections.Generic;

namespace CountDivisibleNumbers
{
    class Program
    {
        public static void Main()
        {
            #region Console Initialization & Styling
            Console.Title = "Count divisible numbers by any of given numbers (Multi-Algorithm Benchmark)";

            try
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Green;
            }
            catch { }
            #endregion

            #region Automated Correctness Verification
            // Run correctness verification tests for all algorithms before starting user input
            try
            {
                DivisibilityVerificationTests.RunAllTests();
                Console.WriteLine();
                Console.WriteLine("Press any key to proceed to interactive execution...");
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine($"[ERROR] Correctness test failed: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey(true);
                return;
            }
            #endregion

            ConsoleKeyInfo answer = new ConsoleKeyInfo('Y', ConsoleKey.Y, true, true, true);

            #region Interactive Execution Loop
            while (answer.Key == ConsoleKey.Y)
            {
                Console.Clear();
                Console.WriteLine("-------------------------------------------------");
                Console.WriteLine(" Program for counting numbers in a given range   ");
                Console.WriteLine();
                Console.WriteLine("         [ 1 <= m <= n <= long.MaxValue ]        ");
                Console.WriteLine();
                Console.WriteLine("    divisible by any of given p(k) test numbers  ");
                Console.WriteLine("-------------------------------------------------");
                Console.WriteLine();

                Console.WriteLine(" Enter range of numbers [m, n] to be tested : ");
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
                    s.Add(p[q]);
                }

                Console.WriteLine();
                Console.Write(" Enter number of test iterations (e.g., 10) : ");
                int iterations = int.Parse(Console.ReadLine());

                Console.WriteLine();
                Console.WriteLine("-------------------------------------------------");

                p = s.ToArray();
                p = DivisibilityHelper.FilterDuplicatesAndDivisible(p);

                // Run performance tester with trimmed mean calculation
                DivisibilityPerformanceTester.RunPerformanceTests(m, n, p, iterations);

                #region BenchmarkDotNet Execution
                Console.WriteLine();
                Console.Write(" Run full benchmark (BenchmarkDotNet) [Y/N] ? ");
                ConsoleKeyInfo benchAnswer = Console.ReadKey(true);
                Console.WriteLine(benchAnswer.KeyChar);

                if (benchAnswer.Key == ConsoleKey.Y)
                {
                    Console.WriteLine();
                    Console.WriteLine("Running BenchmarkDotNet... Please wait.");
                    var summary = BenchmarkRunner.Run<DivisibilityBenchmark>();
                }
                #endregion

                Console.WriteLine();
                Console.Write(" Another Test [Y/N] ");
                answer = Console.ReadKey(true);
            }
            #endregion
        }
    }
}