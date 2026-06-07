# Stratum Connection Benchmark: MCCE Optimized (Utf8JsonReader + SendMessage + Compiler Flags)

## Comparison Summary

| Metric | Upstream .NET 6 | MCCE .NET 8 (before) | MCCE Optimized (after) | vs Upstream |
| :--- | :--- | :--- | :--- | :--- |
| **Mean Time** | 2,947 ns | 2,288 ns | **550 ns** | **5.36× faster** 🚀 |
| **Allocated Memory** | 7,100 B | 7,110 B | **752 B** | **9.44× less** 💾 |
| **Gen 0 / 1k ops** | 0.5531 | 0.5569 | 0.0572 | **9.7× fewer GCs** |

## What Changed

1. **Utf8JsonReader deserialization** — `ProcessRequestAsync` now parses JSON-RPC directly from `ReadOnlySequence<byte>` using `System.Text.Json.Utf8JsonReader`, completely avoiding:
   - `Encoding.GetString()` string allocation
   - `StringReader` + `JsonTextReader` object allocations
   - Newtonsoft `JToken` tree building for params (params stored as lazy `ReadOnlySequence<byte>` slice)
   
2. **RecyclableMemoryStream for SendMessage** — Eliminated `new StringBuilder()` + `sb.ToString()` + `Encoding.GetBytes()` triple allocation. Uses `RecyclableMemoryStream` + `StreamWriter` + direct `CopyToAsync`.

3. **Compiler optimizations** — Added `TieredPGO`, `OptimizationPreference=Speed`, `InvariantGlobalization=true` to csproj.

4. **AggressiveOptimization/Inlining** — Applied `[MethodImpl]` hints on the 4 hottest methods in the stratum pipeline.

## Test Environment
- **OS**: Windows 11, Intel Core i7-12700K
- **.NET SDK**: 8.0.421, Runtime .NET 8.0.27
- **Configuration**: Release, BenchmarkDotNet v0.13.4

## Raw Results
```
|                              Method |     Mean |   Error |  StdDev |   Gen0 | Allocated |
|------------------------------------ |---------:|--------:|--------:|-------:|----------:|
| ProcessRequest_Handle_Valid_Request | 549.9 ns | 2.72 ns | 2.13 ns | 0.0572 |     752 B |
```
