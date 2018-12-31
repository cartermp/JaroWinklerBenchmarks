open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running

[<MemoryDiagnoser>]
type Benchmark() =
    let ident = "phillip"
    let suggestions = ResizeArray<string>(Names.names)

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
