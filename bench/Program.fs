open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running

[<MemoryDiagnoser>]
type Benchmark() =
    let ident = "phillip"
    let mutable suggestions = ResizeArray<string>()

    [<GlobalSetup>]
    member __.Setup() =
        // 8400 names, created in a fairly lulzworthy way
        let nms = Names.names @ Names.names @ Names.names @ Names.names @ Names.names @ Names.names @ Names.names        
        suggestions <- ResizeArray<string>(nms @ nms @ nms @ nms @ nms @ nms)

    [<Benchmark>]
    member __.JaroCurrent() = EditDistance.FilterPredictions ident suggestions JaroCurrent.JaroWinklerDistance |> ignore

    [<Benchmark>]
    member __.JaroNew() = EditDistance.FilterPredictions ident suggestions JaroNew.JaroWinklerDistance |> ignore

    [<Benchmark>]
    member __.JaroNewStructTuple() = EditDistance.FilterPredictions ident suggestions JaroNewStructTuple.JaroWinklerDistance |> ignore

[<EntryPoint>]
let main _ =
    let summary = BenchmarkRunner.Run<Benchmark>()
    printfn "%A" summary
    0 // return an integer exit code
