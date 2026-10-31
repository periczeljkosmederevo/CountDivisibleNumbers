# CountDivisibleNumbers

**CountDivisibleNumbers** is a high-performance, algorithmic .NET 10 solution designed to solve the classic number theory problem: *counting how many integers in a given range $[1, n]$ (or $[m, n]$) are divisible by any number in a set of prime/test divisors*. This project modernizes and optimizes a solution originally proposed on C# Corner, leveraging advanced mathematical principles and cutting-edge .NET performance features.

---

### 🚀 Key Features

- **Inclusion-Exclusion Principle Architecture:** Bypasses slow, naive linear scans by mathematically computing set unions and intersections recursively.
- **Advanced Memoization & Zero-Allocation Caching:** Utilizes `CollectionsMarshal.GetValueRefOrAddDefault` to perform single-lookup dictionary caching, eliminating redundant recursive branches and managed memory allocations ($\text{0 B}$).
- **High-Precision Int128 Accumulation (`UltraOptimizedDivisibilityCounterV2`):** Avoids dictionary overhead entirely through direct recursive product accumulation with `Int128` types and early-exit optimizations for extreme ranges up to `long.MaxValue`.
- **Comprehensive Benchmarking:** Fully integrated with `BenchmarkDotNet` to measure execution times, memory footprints, and Garbage Collection metrics across varying data scales.

---

### 🛠 Solution Components

The solution contains several architectural iterations and benchmark utilities within the Visual Studio environment:

#### 📂 Core Components

- `OriginalDivisibilityCounter.cs` & `OriginalNaiveDivisibilityCounter.cs`: Basic loop and preliminary recursive implementations.
- `MemoizedOriginalDivisibilityCounter.cs`: First-generation recursive algorithm equipped with a dictionary cache for memoization.
- `UltraOptimizedDivisibilityCounter.cs`: Highly optimized memoized implementation using `CollectionsMarshal` for single-lookup insertion/retrieval.
- `UltraOptimizedDivisibilityCounterV2.cs`: Advanced zero-allocation direct recursion using `Int128` and sorted-array early terminations.

#### 🤝 Community & Alternative Implementations

- `StringBuilderDivisibilityCounterDharmrajThakur.cs`: Modernized recreation and testing of the string-building/LINQ approach suggested by community member Dharmraj Thakur.
- `LINQDivisibilityCounter.cs`: Functional LINQ-based evaluation for baseline comparisons.

#### 📊 Benchmarking & Entry Point

- `DivisibilityBenchmark.cs`: Comprehensive BenchmarkDotNet harness evaluating performance against small ($N = 1,000$), medium ($N = 1,000,000$), and colossal ($N = 1,000,000,000,000,000$) datasets.
- `Program.cs`: Console entry point for executing solutions and benchmarks.

---

### Getting Started

#### Prerequisites

- **.NET 10.0 SDK** (or compatible modern .NET target supporting `Int128` and `CollectionsMarshal`).
- Visual Studio 2022 or a compatible C# IDE/CLI environment.

#### Installation & Setup

1. Clone or download the repository workspace.
2. Open the `CountDivisibleNumbers.sln` solution file in Visual Studio.
3. Restore NuGet packages (specifically `BenchmarkDotNet`).
4. Build the solution via **Build $\rightarrow$ Rebuild Solution** in Release mode for accurate benchmarking.

---

### Usage

1. **Run Benchmarks:** Set the configuration to `Release` and run `Program.cs` to execute the `BenchmarkDotNet` suite and view performance metrics.
2. **Execute Algorithms Directly:** Call `UltraOptimizedDivisibilityCounter.CalculateRange(m, n, p)` or `UltraOptimizedDivisibilityCounterV2.CalculateRange(m, n, p)` with a pre-filtered, sorted array of test divisors.

---

### License

- This project is licensed under the [CC0-1.0 license (Public Domain)](https://creativecommons.org/publicdomain/zero/1.0/).
- To the extent possible under law, the author(s) have dedicated all copyright and related rights to this software to the public domain worldwide.
- Feel free to use, modify, and distribute the code without any restrictions.

---

### Support

For technical inquiries or follow-ups, please contact:

- **Name:** Željko Perić
- **Email:** periczeljkosmederevo@yahoo.com