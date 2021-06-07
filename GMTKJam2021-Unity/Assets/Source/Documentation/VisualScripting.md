# Getting Started With Visual Scripting
Unity now supports visual scripting which means you can build game logic by using a series of nodes in a graph.
![Example of visual scripting](Images/visualscripting.gif)

## Add a script machine to an object and use the "Edit Graph" button to open the visual graph
The `source` option can be set to either `Graph` or `Embed`.
- **Graph**: Uses a reference to a graph that can be shared between multiple `script machines`. This also keeps all changes saved at runtime.
- **Embed**: Saves all the graph data into the object being edited. This can only be used on a single object and all changes are reverted when play mode is exited.

![Example of visual scripting](Images/addingscriptgraph.gif)