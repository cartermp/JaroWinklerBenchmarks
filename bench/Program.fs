open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running

[<MemoryDiagnoser>]
type Benchmark() =
    let ident = "phillip"
    let mutable suggestions = ResizeArray<string>()

    [<GlobalSetup>]
    member __.Setup() =
        // 8400 names, created in a fairly lulzworthy way
        let ns = Names.names
        let nms = ns @ ns @ ns @ ns @ ns @ ns @ ns     
        suggestions <- ResizeArray<string>(nms @ nms @ nms @ nms @ nms @ nms)

    [<Benchmark(Baseline=true)>]
    member __.JaroCurrent() = EditDistance.FilterPredictions ident suggestions JaroCurrent.JaroWinklerDistance true |> ignore

    [<Benchmark>]
    member __.JaroNew() = EditDistance.FilterPredictions ident suggestions JaroNew.JaroWinklerDistance false |> ignore

    [<Benchmark>]
    member __.JaroNewStructTuple() = EditDistance.FilterPredictions ident suggestions JaroNewStructTuple.JaroWinklerDistance false |> ignore

[<EntryPoint>]
let main _ =
    let summary = BenchmarkRunner.Run<Benchmark>()
    printfn "%A" summary
    0 // return an integer exit code
