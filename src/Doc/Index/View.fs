module Doc.Index.View

open Fable.Core
open Fable.Import
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Global
open Types

let tileDocs tile =
  match tile with
  | Tile info ->
      div
        [ ClassName "tile is-parent is-vertical" ]
        [ article
            [ ClassName "tile is-child box" ]
            [ p
                [ ClassName "title" ]
                [ a
                    [ Href info.url ]
                    [ str info.title ] ]
              p
                [ ClassName "subtitle" ]
                [ div
                    [ ClassName "content"
                      DangerouslySetInnerHTML { __html = (Marked.Globals.marked.parse info.description) } ]
                    [ ] ] ] ]
  // Render an empty tile
  | Placeholder ->
      div
        [ ClassName "tile is-parent is-vertical" ]
        [ article
            [ ClassName "tile is-child" ]
            [  ] ]

let tileVertical tileList =
  div
    [ ClassName "tile is-vertical is-6" ]
    (tileList |> List.map tileDocs)

let docsTiles tileList =
  let rec divideTiles tiles index sectionInfo =
    match tiles with
    | tile::trail ->
        let sectionInfo' =
          match index % 2 with
          | 0 ->
              { sectionInfo with
                  left = sectionInfo.left @ [ Tile tile] }
          | 1 ->
              { sectionInfo with
                      right = sectionInfo.right @ [ Tile tile] }
          | _ -> failwith "Should not happened"
        divideTiles trail (index + 1) sectionInfo'
    | [] ->
        // Ensure we have the same number of tiles in both columns
        // This prevent to have taller tiles
        if (index % 2) <> 0 then
          { sectionInfo with
              right = sectionInfo.right @ [ Placeholder ] }
        else
          sectionInfo

  let sections = divideTiles tileList 0 SectionInfo.Empty
  div
    [ ClassName "tile is-ancestor" ]
    [ tileVertical sections.left
      tileVertical sections.right ]

let root =
  div
    [ ClassName "section" ]
    [ docsTiles
        [ { title = "Elmish"
            description =
              """
Official documentation about elmish.

Elmish is the part responsible of handling your **state** during the **life cicle** of your application.
              """
            url = "https://fable-elmish.github.io/elmish/"
          }
          { title = "Elmish-Browser"
            description =
              """
Official documentation about elmish-browser.

Elmish-browser implements **routing** and **navigation** for elmish apps targeting browser (React) renderers.
              """
            url = "https://fable-elmish.github.io/browser/"
          } ] ]
