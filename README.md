# Graphs
My C# graph library

## First steps
### New Graph objectum
Create a new Graph objectum:
```C#
Graph graph = new Graph();
```
## Functions
### Random graph
Create a random graph with 5-15 vertices and 5-15 edges with the following function:
```C#
graph.createRandomGraph();
```
Or specify the number of vertices and edges:
```C#
graph.createRandomGraph([int] vertices, [int] edges);
```
### Create vertices and edges:
Every vertice has a unique ID, and every edge contains 2 vertice IDs
To create a new vertice with a random ID:
```C#
graph.createNewPoint();
```
Or create a new vertice with a specific ID:
```C#
graph.createNewPoint([string] ID);
```

To create an edge you need 2 vertice IDs or vertice objects (GraphPoint):
```C#
graph.createNewConnection([string] ID1, [string] ID2);
graph.createNewConnection([GraphPoint] vertice1, [GraphPoint] vertice2);
```
### Get a list of all vertices and edges:
```C#
List<GraphPoint> vertices = graph.getPoints();
List<GraphConnection> vertices = graph.getConnections();
```
### Degrees
To calculate the degree of a vertice you need to call the following function:
```C#
int degree = graph.getPointDegree([GraphPoint] vertice);
```
Or you can get an array of every degree in ascending order:
```C#
int[] degrees = graph.getDegree();
```
### Count components
You can simply count the number of components by the following function:
```C#
int components = graph.countComponents();
```

## Graph convertions
### Graph to Matrix
You can convert you graph into a edge matrix by the following function:
```C#
bool[,] arr = graph.convertToBoolArray();
```
### Graph to string
Or you can convert your graph into a matrix string:
```C#
string matrix = graph.boolString();
```
The result will look like this:
```
7
 0 0 0 0 1 1 1
 0 0 0 0 1 1 1
 0 0 0 0 1 1 1
 0 0 0 0 1 1 1
 1 1 1 1 0 0 0
 1 1 1 1 0 0 0
 1 1 1 1 0 0 0
```
### String to graph
To get a graph from a matrix string:
```C#
graph.importFromStringData([string] data);
```
### Degrees to graph
**Not fully functional**
You can generate a graph from a list of degrees:
```C#
graph.generateGraphFromDegree([int[]] degreeArray);
```
## Operators
### Is matching
To detect if a graph matches with another graph use the following operators:
```C#
if (graph1 == graph2) {

}
```
*Note, that this perator does not check for isomorphism*
### Complementer
To get a graphs complementer use the following operator:
```C#
Graph comp = ~ graph;
```
### Isomorphism
To check is two graphs are isomorph, use the following operator:
```C#
if (graph1 ^ graph2) {

}
```
## Output image
You can simply generate an image of the graph with this function:
```C#
Image img = graph.generateImage();
```
