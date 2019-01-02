``` ini

BenchmarkDotNet=v0.11.3, OS=macOS Mojave 10.14.2 (18C54) [Darwin 18.2.0]
Intel Core i7-7700HQ CPU 2.80GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.2.100
  [Host]     : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT DEBUG
  DefaultJob : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT


```
|             Method |     Mean |     Error |    StdDev |   Median | Gen 0/1k Op | Gen 1/1k Op | Gen 2/1k Op | Allocated Memory/Op |
|------------------- |---------:|----------:|----------:|---------:|------------:|------------:|------------:|--------------------:|
|        JaroCurrent | 3.117 ms | 0.1603 ms | 0.4546 ms | 3.093 ms |    445.3125 |           - |           - |          1379.81 KB |
|            JaroNew | 2.407 ms | 0.0902 ms | 0.2514 ms | 2.326 ms |    378.9063 |           - |           - |          1173.09 KB |
| JaroNewStructTuple | 2.043 ms | 0.0392 ms | 0.0366 ms | 2.043 ms |     23.4375 |           - |           - |            79.78 KB |
