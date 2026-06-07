# Stratum Connection Benchmark Comparison: .NET 6 vs .NET 8

This report compares the performance of Stratum connection request processing (`ProcessRequest_Handle_Valid_Request`) between:
1. **Original Upstream Base**: `blackmennewstyle/miningcore` running on **.NET 6** (Ubuntu 22.04)
2. **Modernized MCCE Base**: Current repository before optimizations running on **.NET 8** (Ubuntu 24.04)

---

## Comparison Summary

| Metric | Original Upstream (.NET 6) | Modernized MCCE (.NET 8) | Change | Status |
| :--- | :--- | :--- | :--- | :--- |
| **Execution Time (Mean)** | **2.947 μs** | **2.288 μs** | **-0.659 μs (-22.36%)** | **22% Faster** 🚀 |
| **Allocated Memory** | **7.10 KB** | **7.11 KB** | **+0.01 KB (+0.14%)** | **Equivalently Low** |
| **Gen 0 / 1k ops** | 0.5531 | 0.5569 | +0.0038 (+0.69%) | Negligible Change |
| **Gen 1 / 1k ops** | 0.0038 | 0.0000 | -0.0038 (-100.0%) | Improved |

### Key Observations
- **Execution Speed**: The runtime upgrade to .NET 8 alone results in a **22.36% improvement** in request processing latency (reducing latency from 2.947 μs to 2.288 μs). This is largely due to JIT compiler improvements (such as Dynamic PGO and RyuJIT optimizations in .NET 8).
- **Memory Overhead**: Allocated memory remains stable at ~7.1 KB per connection request processing.

---

## Detailed Benchmark Reports

### 1. Original Upstream (.NET 6)
- **Repo**: `https://github.com/blackmennewstyle/miningcore`
- **Environment**: Ubuntu 22.04, .NET SDK 6.0.428, Runtime .NET 6.0.36
- **Results**:
```
|                              Method |     Mean |     Error |    StdDev |   Gen0 |   Gen1 | Allocated |
|------------------------------------ |---------:|----------:|----------:|-------:|-------:|----------:|
| ProcessRequest_Handle_Valid_Request | 2.947 us | 0.0584 us | 0.0942 us | 0.5531 | 0.0038 |    7.1 KB |
```

### 2. Modernized MCCE (.NET 8)
- **Repo**: Current branch (`dev`)
- **Environment**: Ubuntu 24.04, .NET SDK 8.0.421, Runtime .NET 8.0.27
- **Results**:
```
|                              Method |     Mean |     Error |    StdDev |   Gen0 |   Gen1 | Allocated |
|------------------------------------ |---------:|----------:|----------:|-------:|-------:|----------:|
| ProcessRequest_Handle_Valid_Request | 2.288 us | 0.0453 us | 0.1137 us | 0.5569 |      - |   7.11 KB |
```
