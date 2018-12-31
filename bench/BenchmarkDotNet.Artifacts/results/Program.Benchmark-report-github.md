``` ini

BenchmarkDotNet=v0.11.3, OS=macOS Mojave 10.14.2 (18C54) [Darwin 18.2.0]
Intel Core i7-7700HQ CPU 2.80GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.2.100
  [Host]     : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT DEBUG
  DefaultJob : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT


```
|             Method |     Mean |    Error |   StdDev | Gen 0/1k Op | Gen 1/1k Op | Gen 2/1k Op | Allocated Memory/Op |
|------------------- |---------:|---------:|---------:|------------:|------------:|------------:|--------------------:|
|        JaroCurrent | 81.20 us | 2.004 us | 5.750 us |     16.7236 |           - |           - |            51.43 KB |
|            JaroNew | 84.05 us | 1.902 us | 5.550 us |     17.2119 |           - |           - |            52.98 KB |
| JaroNewStructTuple | 73.01 us | 1.891 us | 5.547 us |      6.5918 |           - |           - |            20.62 KB |
