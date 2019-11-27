module App

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props

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



// UPDATE

let update (msg:Msg) (state:State) =
  state


// VIEW (rendered with React)

let view (state:State) dispatch =

  div []
      [ str "hello" ]




// App
Program.mkSimple (fun () -> initial) update view
|> Program.withReactSynchronous "elmish-app"
|> Program.withConsoleTrace
|> Program.run
