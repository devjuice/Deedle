﻿#if INTERACTIVE
#load "../../bin/Deedle.fsx"
#r "../../packages/NUnit.2.6.3/lib/nunit.framework.dll"
#r "../../packages/FsCheck.0.9.1.0/lib/net40-Client/FsCheck.dll"
#r "../../packages/MathNet.Numerics.3.0.0-alpha8/lib/net40/MathNet.Numerics.dll"
#load "../Common/FsUnit.fs"
#else
module Deedle.Tests.Deque
#endif

open System
open System.Linq
open System.Collections.Generic
open System.Globalization
open FsUnit
open FsCheck
open NUnit.Framework

open Deedle

[<Test>]
let ``empty deque is empty`` () =
  let d = Deque()

  d.IsEmpty |> shouldEqual true
  
[<Test>]
let ``can add single element`` () =
  let d = Deque()

  d.Add(1)
  
  d.IsEmpty |> shouldEqual false 
  d.Count |> shouldEqual 1  
  d.Last |> shouldEqual 1
  d.First |> shouldEqual 1

[<Test>]
let ``can add two elements`` () =
  let d = Deque()

  d.Add(1)
  d.Add(2)
  
  d.IsEmpty |> shouldEqual false 
  d.Count |> shouldEqual 2
  d.Last |> shouldEqual 2
  d.First |> shouldEqual 1

[<Test>]
let ``can add elements`` () =
  let d = Deque()

  d.Add(1)
  d.Add(1)
  d.Add(2)
  d.Add(3)
  d.Add(4)
  
  d.IsEmpty |> shouldEqual false 
  d.Count |> shouldEqual 5
  d.Last |> shouldEqual 4
  d.First |> shouldEqual 1

[<Test>]
let ``can double the capacity`` () =
  let d = Deque()

  for x in 1..17 do
      d.Add(x)
 
  d |> Seq.toList |> shouldEqual [1..17]

[<Test>]
let ``can add elements and remove from front`` () =
  let d = Deque()

  for x in 1..2049 do
      d.Add(x.ToString())
 
  d.IsEmpty |> shouldEqual false 
  d.Count |> shouldEqual 2049

  for x in 1..2049 do
      d.RemoveFirst() |> shouldEqual (x.ToString())

  d.IsEmpty |> shouldEqual true
  d.Count |> shouldEqual 0


[<Test>]
let ``can add elements and remove from back`` () =
  let d = Deque()

  for x in 1..2049 do
      d.Add(x)
 
  d.IsEmpty |> shouldEqual false 
  d.Count |> shouldEqual 2049

  for x in 1..2049 do
      d.RemoveLast() |> shouldEqual (2049-x+1)

  d.IsEmpty |> shouldEqual true
  d.Count |> shouldEqual 0


[<Test>]
let ``can add elements and interate`` () =
  let d = Deque()

  for x in 1..2049 do
      d.Add(x)
 
  let l = ref 1
  for x in d do
    x |> shouldEqual !l
    l := x + 1

[<Test>]
let ``can add elements and convert to list`` () =
  let d = Deque()

  for x in 1..2049 do
      d.Add(x)
 
  d |> Seq.toList |> shouldEqual [1..2049]


[<Test>]
let ``can add and remove elements and convert to list`` () =
  let d = Deque()
  let max = 17

  for x in 1..max do
      d.Add(x)
 
  for x in 1..max do
      d.RemoveFirst()

  d.IsEmpty |> shouldEqual true

  for x in 1..max do
      d.Add(x)

  d.Last |> shouldEqual max

  let l = d |> Seq.toList
  l |> Seq.length |> shouldEqual max
  l |> Seq.last |> shouldEqual max
  l |> shouldEqual [1..max]

[<Test>]
let ``moving max regression is fixed`` () =
  let d = Deque()

  d.Add(4, 0)
  d.Add(5, -1)
  d.RemoveFirst()
  d.RemoveLast()
  d.Add(7, 3)
  d.RemoveFirst()
  d.Add(8, -5)
  d.RemoveLast()
  d.Add(9, 4)
  d.RemoveFirst()
  d.IsEmpty |> shouldEqual true

  d.Add(10, 8)
  d.First |> shouldEqual (10,8)
  d.Last |> shouldEqual (10,8)


[<Test>]
let ``moving max regression at front`` () =
  let d = Deque()

  d.Add(4, 0)
  d.Add(5, -1)
  d.RemoveFirst() |> shouldEqual (4,0)
  d.RemoveFirst() |> shouldEqual (5,-1)  
  d.IsEmpty |> shouldEqual true
  d.Add(7, 3) 
  d.RemoveFirst() |> shouldEqual (7,3)
  d.Add(8, -5) 
  d.RemoveFirst() |> shouldEqual (8,-5)
  d.Add(9, 4) 
  d.RemoveFirst() |> shouldEqual (9,4)
  d.IsEmpty |> shouldEqual true

  d.Add(10, 8)
  d.First |> shouldEqual (10,8)
  d.Last |> shouldEqual (10,8)


[<Test>]
let ``moving max regression at back`` () =
  let d = Deque()

  d.Add(4, 0)
  d.Add(5, -1)
  d.RemoveLast()
  d.RemoveLast()
  d.Add(7, 3)
  d.RemoveLast()
  d.Add(8, -5)
  d.RemoveLast()
  d.Add(9, 4)
  d.RemoveLast()
  d.IsEmpty |> shouldEqual true

  d.Add(10, 8)
  d.First |> shouldEqual (10,8)
  d.Last |> shouldEqual (10,8)
