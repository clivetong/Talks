
# Download Fable

- Start with https://github.com/fable-compiler/fable2-samples/tree/master/minimal

# Configure VSCode

- VSCode iodine

# Running the example Fable app

- npm initial
- npm start
- Show how it hangs together

# Get a grid working

- Define Position and State 
- Create an initial state
- Render grid in `view` and use `rendercell` to make grid
- Change the css to make it look more grid like

# Make it editable

- Add a rendereditor and rendercell
- Define the StartEdit and UpdateValue events and handle them
- Use onclick to start edit
- Use oninput to trigger the update value

# Parsing

- Explain we'll be using a prewritten parser combinator library
- And add it to the source code

# Unevaluated expressions in cells

- Define a simple tree for expressions Number and Binary
- Define the parser rules for integer
- Define the rule for Binary
- Build the tree
- Print the tree nodes into the grid

# Evaluated 

- Evaluate tree
- And brackets
- And cell reference (try find on the map)

# But errors?

- Railway oriented programming using Option type
- If value is None turn the cell red (in the CSS)
- Handle looping in the references

# And where next?

- Talk about the better way to generate a flow graph
