module App

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props
open Fable.Core.JsInterop

// 0 Define the state

type Position = char * int

type State = {
Cols : char list
Rows : int list 
Active : Position option
Cells: Map<Position, string>

}

// 1 Initial state

let initial = 
 {
  Rows = [1..15]
  Cols = ['A'.. 'K']
  Active = None
  Cells = Map.empty
 }





type Msg = 
  Foo



type Event =
  | StartEdit of Position
  | UpdateValue of Position * string

let update (msg:Event) (state:State) =
  match msg with
  |  StartEdit pos ->
      {state with Active = Some pos }
  | UpdateValue (pos, value) ->
      let newCells = Map.add pos value state.Cells
      {state with Cells = newCells}

let renderEditor dispatch pos value =
  td [  ] [
    input [ DefaultValue value
            AutoFocus true
            Type "text"
            ClassName "selected"
            OnInput (fun ev -> (UpdateValue (pos, !!ev.target?value) |> dispatch ))
        ] 
  ]

let renderView dispatch pos value = 
  td [OnClick (fun _ -> dispatch (StartEdit pos)) ] [ str value]

let renderCell dispatch pos state = 
  let value = Map.tryFind pos state.Cells
  if state.Active = Some pos then
      renderEditor dispatch pos (defaultArg value "")
    else
      renderView dispatch pos (defaultArg value "")

let view (state:State) dispatch =

  table [] [
     yield tr [] [
       yield th [] [] 
       for col in state.Cols -> th [] [str (string col)]
     ]
     for row in state.Rows -> tr [] [
       yield th [] [str (string row)]
       for col in state.Cols -> renderCell dispatch (col,row) state
     ]
  ]



// App
Program.mkSimple (fun () -> initial) update view
|> Program.withReactSynchronous "elmish-app"
|> Program.withConsoleTrace
|> Program.run
