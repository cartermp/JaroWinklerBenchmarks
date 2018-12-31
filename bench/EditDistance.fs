module EditDistance

open System

let maxSuggestions = 5
let minThresholdForSuggestions = 0.7
let highConfidenceThreshold = 0.85
let minStringLengthForThreshold = 3


type String with
    member inline x.StartsWithOrdinal(value) =
        x.StartsWith(value, StringComparison.Ordinal)

    member inline x.EndsWithOrdinal(value) =
        x.EndsWith(value, StringComparison.Ordinal)

/// Computes the restricted Damerau-Levenstein edit distance,
/// also known as the "optimal string alignment" distance.
///  - read more at https://en.wikipedia.org/wiki/Damerau%E2%80%93Levenshtein_distance
///  - Implementation taken from http://www.navision-blog.de/2008/11/01/damerau-levenshtein-distance-in-fsharp-part-ii/
let private calcDamerauLevenshtein (a:string, b:string) =
    let m = b.Length + 1
    let mutable lastLine = Array.init m id
    let mutable lastLastLine = Array.zeroCreate m
    let mutable actLine = Array.zeroCreate m

    for i in 1 .. a.Length do
        actLine.[0] <- i
        for j in 1 .. b.Length do
            let cost = if a.[i-1] = b.[j-1] then 0 else 1
            let deletion = lastLine.[j] + 1
            let insertion = actLine.[j-1] + 1
            let substitution = lastLine.[j-1] + cost
            actLine.[j] <- 
              deletion 
              |> min insertion 
              |> min substitution
  
            if i > 1 && j > 1 then
              if a.[i-1] = b.[j-2] && a.[i-2] = b.[j-1] then
                  let transposition = lastLastLine.[j-2] + cost  
                  actLine.[j] <- min actLine.[j] transposition
      
        // swap lines
        let temp = lastLastLine
        lastLastLine <- lastLine
        lastLine <- actLine
        actLine <- temp
              
    lastLine.[b.Length]

/// Calculates the edit distance between two strings.
/// The edit distance is a metric that allows to measure the amount of difference between two strings 
/// and shows how many edit operations (insert, delete, substitution) are needed to transform one string into the other.
let CalcEditDistance(a:string, b:string) =
    if a.Length > b.Length then
        calcDamerauLevenshtein(a, b)
    else
        calcDamerauLevenshtein(b, a)

/// We report a candidate if its edit distance is <= the threshold.
/// The threshold is set to about a quarter of the number of characters.
let IsInEditDistanceProximity (idText: string) suggestion =
    let editDistance = CalcEditDistance(idText, suggestion)
    let threshold =
        match idText.Length with
        | x when x < 5 -> 1
        | x when x < 7 -> 2
        | x -> x / 4 + 1
    
    editDistance <= threshold

/// Filters predictions based on edit distance to the given unknown identifier.
let FilterPredictions (idText:string) (allSuggestions: ResizeArray<string>) editDistanceFunction =    
    let uppercaseText = idText.ToUpperInvariant()

    let demangle (nm:string) =
        if nm.StartsWithOrdinal("( ") && nm.EndsWithOrdinal(" )") then
            let cleanName = nm.[2..nm.Length - 3]
            cleanName
        else nm

    /// Returns `true` if given string is an operator display name, e.g. ( |>> )
    let IsOperatorName (name: string) =
        if not (name.StartsWithOrdinal("( ") && name.EndsWithOrdinal(" )")) then
            false
        else
            let name = name.[2..name.Length - 3]
            name |> Seq.forall (fun c -> c <> ' ')

    if allSuggestions.Contains idText then [] else // some other parsing error occurred
    allSuggestions
    |> Seq.choose (fun suggestion ->
        // Because beginning a name with _ is used both to indicate an unused
        // value as well as to formally squelch the associated compiler
        // error/warning (FS1182), we remove such names from the suggestions,
        // both to prevent accidental usages as well as to encourage good taste
        if IsOperatorName suggestion || suggestion.StartsWithOrdinal("_") then None else
        let suggestion:string = demangle suggestion
        let suggestedText = suggestion.ToUpperInvariant()
        let similarity = editDistanceFunction uppercaseText suggestedText
        if similarity >= highConfidenceThreshold || suggestion.EndsWithOrdinal("." + idText) then
            Some(similarity, suggestion)
        elif similarity < minThresholdForSuggestions && suggestedText.Length > minStringLengthForThreshold then
            None
        elif IsInEditDistanceProximity uppercaseText suggestedText then
            Some(similarity, suggestion)
        else
            None)
    |> Seq.sortByDescending fst
    |> Seq.mapi (fun i x -> i, x) 
    |> Seq.takeWhile (fun (i, _) -> i < maxSuggestions) 
    |> Seq.map snd 
    |> Seq.toList