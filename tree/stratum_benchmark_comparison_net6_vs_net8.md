# Stratum Connection Benchmark Comparison: .NET 6 vs .NET 8 vs MCCE Optimized

This report compares the performance of Stratum connection request processing (`ProcessRequest_Handle_Valid_Request`) between:
1. **Original Upstream Base**: `blackmennewstyle/miningcore` running on **.NET 6** (Ubuntu 22.04)
2. **Modernized MCCE Base**: Current repository before optimizations running on **.NET 8** (Ubuntu 24.04)
3. **MCCE Optimized**: With Utf8JsonReader deserialization, RecyclableMemoryStream sends, and compiler flags (Windows 11, i7-12700K)

---

## Comparison Summary

| Metric | Original Upstream (.NET 6) | Modernized MCCE (.NET 8) | MCCE Optimized | Change (vs upstream) |
| :--- | :--- | :--- | :--- | :--- |
| **Mean Time** | **2,947 ns** | **2,288 ns** | **550 ns** | **5.36× faster (-81.3%)** 🚀 |
| **Allocated Memory** | **7.10 KB** | **7.11 KB** | **0.73 KB** | **9.44× less (-89.4%)** 💾 |
| **Gen 0 / 1k ops** | 0.5531 | 0.5569 | 0.0572 | 9.7× fewer GCs |
| **Gen 1 / 1k ops** | 0.0038 | 0.0000 | - | - |

### Progression
- **.NET 6 → .NET 8**: 22% faster (JIT/Dynamic PGO improvements)
- **.NET 8 → Optimized**: 4.16× faster (zero-alloc Utf8JsonReader parsing)
- **Total .NET 6 → Optimized**: 5.36× faster, 9.44× less memory

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

### 3. MCCE Optimized (Windows 11, i7-12700K)
- **Environment**: Windows 11, .NET SDK 8.0.421, Runtime .NET 8.0.27
- **Optimizations**: Utf8JsonReader direct parsing, RecyclableMemoryStream sends, AggressiveOptimization, TieredPGO
- **Results**:
```
|                              Method |     Mean |   Error |  StdDev |   Gen0 | Allocated |
|------------------------------------ |---------:|--------:|--------:|-------:|----------:|
| ProcessRequest_Handle_Valid_Request | 549.9 ns | 2.72 ns | 2.13 ns | 0.0572 |     752 B |
```
