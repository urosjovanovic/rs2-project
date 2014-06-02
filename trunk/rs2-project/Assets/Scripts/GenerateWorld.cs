using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GenerateWorld : MonoBehaviour {
	

    /// <summary>
    /// Procedurally generates a new maze
    /// </summary>
    /// <returns> An integer maze matrix </returns>
    public static Maze GenerateMaze(int size)
    {
        return new Maze(size,size);
    }

    
}


#region Class Maze

/// <summary>
/// A class representing a maze
/// </summary>
public class Maze
{

    #region Class fields and properties
    public int[,] matrix
    {
        get;
        private set;
    }

    public GridGraph Graph
    {
        get;
        private set;
    }

    int matrixRows;
    int matrixCols;

    int graphCols;
    int graphRows;

    #endregion

    #region Class constructors

    /// <summary>
    /// A public constructor for the maze class 
    /// </summary>
    /// <param name="matrixRows"> Number of rows in a maze matrix </param>
    /// <param name="matrixCols"> Number of columns in a maze matrix </param>
    public Maze(int matrixRows, int matrixCols)
    {
        if (matrixRows % 2 != 0 || matrixCols % 2 != 0)
            throw new ArgumentException("Public Maze constructor argument exception: Rows and columns cannot be odd numbers.");

        graphRows = matrixRows / 2;
        graphCols = matrixCols / 2;

        GridGraph graph = new GridGraph(matrixRows / 2, matrixCols / 2);
        GridGraph mazeInverted = graph.PrimsAlgorithm();
        Graph = mazeInverted.ComplementaryGraph();

        this.matrixRows = matrixRows - 1;
        this.matrixCols = matrixCols - 1;

        matrix = mazeInverted.CreateWallMatrix();
    }
    #endregion


    #region Methods: PrintToFile, GetSpawnNodes, GetMarkerCollectibleSpawnNodes

    /// <summary> Writes a maze matrix to a file with a given path  </summary>
    /// <param name="filePath"> A path to a file.</param>
    public void PrintToFile(string filePath)
    {

        string output = "";

        // make an output string
        for (int i = 0; i < matrixRows; i++)
        {
            for (int j = 0; j < matrixCols; j++)
            {
                output += matrix[i, j];
            }
            output += System.Environment.NewLine;
        }

        System.IO.File.WriteAllText(filePath, output);

    }

    /// <summary>
    /// Calculates the optimal spawn nodes for Prim, Dark Prim and the exit
    /// </summary>
    /// <returns> Dictionary: "Prim" -> GridNode, "DarkPrim" -> GridNode, "Exit" -> GridNode</returns>
    public Dictionary<string, GridNode> GetSpawnNodes()
    {
        var spawnNodes = new Dictionary<string, GridNode>();

        System.Random rand = new System.Random();

        int side = rand.Next(1, 5);

        int limit = graphCols - 1;


        // Prim gets a random edge node for his spawn point
        GridNode primSpawnPoint = new GridNode(0, 0);

        // up
        if (side == 1)
        {
            int i = 0;
            int j = rand.Next(0, limit);

            primSpawnPoint = Graph[i, j];
        }
        // right
        if (side == 2)
        {
            int j = limit;
            int i = rand.Next(0, limit);

            primSpawnPoint = Graph[i, j];
        }
        //down
        if (side == 3)
        {
            int i = limit;
            int j = rand.Next(0, limit);

            primSpawnPoint = Graph[i, j];
        }
        //left
        if (side == 4)
        {
            int j = 0;
            int i = rand.Next(0, limit);

            primSpawnPoint = Graph[i, j];
        }

        spawnNodes.Add("Prim", primSpawnPoint);

        
        var distances = Graph.DistancesFromNode(primSpawnPoint);

        // Exit is the node that is farthest from Prim
        var farthestFromPrim = (from d in distances
                                where d.Value == distances.Max(x => x.Value)
                                select d.Key).First();


        spawnNodes.Add("Exit", farthestFromPrim);

        // Dark Prim gets spawned at a node that is farther then the average distance from Prim
        var fartherThenAverage = from d in distances
                                 where d.Value >= distances.Average(x => x.Value)
                                 select d.Key;

        int r = rand.Next(0, fartherThenAverage.Count());

        spawnNodes.Add("DarkPrim", fartherThenAverage.ElementAt(r));


        return spawnNodes;
    }

    /// <summary>
    /// Generates the random spawn nodes for the marker collectibles
    /// </summary>
    /// <param name="howMany"> How many markers to be spawned </param>
    /// <returns> A list of spawn nodes for the marker collectibles </returns>
    public List<GridNode> GetMarkerCollectibleSpawnNodes(int howMany = 3)
    {
        var markers = new List<GridNode>();

        List<GridNode> deadEnds = new List<GridNode>();

        for (int i = 1; i < graphRows -1; i++)
        {
            for (int j = 1; j < graphCols - 1; j++)
            {
                if (Graph[i, j].Edges.Count == 1)
                {
                    deadEnds.Add(Graph[i, j]);
                }
            }
        }

        System.Random rand = new System.Random();

        for (int i = 0; i < howMany; i++)
        {
            int index = rand.Next(deadEnds.Count);

            markers.Add(deadEnds[index]);
            deadEnds.RemoveAt(index);

        }

        return markers;
    }


    #endregion


}

#endregion


#region Class GridNode

/// <summary>
/// A class representing a node inside a grid graph
/// </summary>
public class GridNode
{

    #region Class fields and properties

    private List<GridNode> edges = new List<GridNode>();
    public int i
    {
        get;
        private set;
    }

    public int j
    {
        get;
        private set;
    }

    public List<GridNode> Edges
    {
        get { return edges; }
        set { edges = value; }
    }

    #endregion


    #region Class constructors
    ///<summary> A public constructor for a grid node at index (i,j) </summary>
    public GridNode(int i_pos, int j_pos)
    {
        i = i_pos;
        j = j_pos;
    }
    #endregion


    #region Methods: isConnectedTo and removeEdge

    /// <summary> Check if this node is connected to the specified node </summary>
    public bool isConnectedTo(GridNode node)
    {
        return edges.Contains(node);
    }

    ///<summary> Remove an edge between this node and the specified node </summary>
    public void removeEdge(GridNode node)
    {
        edges.Remove(node);
    }

    #endregion


    #region Operator and method overrides

    //just in case
    public static bool operator ==(GridNode g1, GridNode g2)
    {
        return (g1.i == g2.i) && (g1.j == g2.j);
    }

    public static bool operator !=(GridNode g1, GridNode g2)
    {
        return (g1.i != g2.i) && (g1.j != g2.j);
    }

    /// <summary> Equals method. I wrote this so IList.Remove would work. </summary>
    public override bool Equals(object obj)
    {
        // If parameter cannot be cast to GridNode return false.
        GridNode g2 = obj as GridNode;
        if ((System.Object)g2 == null)
        {
            return false;
        }

        // Return true if the fields match:
        return (i == g2.i) && (j == g2.j);
    }

    // For debugging
    public override string ToString()
    {
        return "(" + i + ", " + j + ")";
    }

    public override int GetHashCode()
    {
        return i ^ j;
    }

    #endregion
}

#endregion


#region Class GridGraph

/// <summary>
/// A class representing a grid graph
/// </summary>
public class GridGraph
{

    #region Class fields and properties

    // a matrix of nodes which represent maze rooms
    private GridNode[,] grid;

    // number of rows
    private int m;

    // number of columns
    private int n;

    //number of nodes in the grid
    public int Length
    {
        get
        {
            return grid.Length;
        }
    }

    #endregion


    #region Class constructors

    ///<summary> A public constructor for a GridGraph</summary>
    ///<param name="m"> Number of rows in a grid </param>
    ///<param name="n"> Number of columns in a grid </param>
    public GridGraph(int m, int n)
    {
        // make a new node matrix
        grid = new GridNode[m, n];
        this.m = m;
        this.n = n;

        // make all the nodes and connect them to their adjacent nodes
        for (int i = 0; i < m; i++)
            for (int j = 0; j < n; j++)
            {
                grid[i, j] = new GridNode(i, j);
                grid[i, j].Edges = this.adjacentToNode(i, j);
            }

    }
    #endregion


    #region Methods: adjacentToNode, getNodeEdges

    ///<summary> Returns nodes adjacent to the node at position i,j </summary>
    /// <returns> A list of adjacent nodes</returns>
    public List<GridNode> adjacentToNode(int i, int j)
    {
        var nodes = new List<GridNode>();

        if (i - 1 >= 0)
        {
            nodes.Add(new GridNode(i - 1, j));
        }

        if (i + 1 < m)
        {
            nodes.Add(new GridNode(i + 1, j));
        }

        if (j - 1 >= 0)
        {
            nodes.Add(new GridNode(i, j - 1));
        }

        if (j + 1 < n)
        {
            nodes.Add(new GridNode(i, j + 1));
        }

        return nodes;
    }

    /// <summary> get edges of a node in a graph with given indexes </summary>
    /// <returns> A list of nodes that the given node is connected to </returns>
    public List<GridNode> getNodeEdges(GridNode key)
    {
        var edges = new List<GridNode>();

        for (int i = 0; i < m; i++)
            for (int j = 0; j < n; j++)
            {
                if (grid[i, j] == key)
                {
                    edges = grid[i, j].Edges;
                }
            }

        return edges;
    }
    #endregion


    #region Methods: createWallMatrix, ComplementaryGraph, PrintGridGraph, DistancesFromNode


    /// <summary> Creates an integer matrix from a GridGraph </summary>
    /// <returns> An integer matrix where 1 represents a wall and 0 represents a room or a passage </returns>
    public int[,] CreateWallMatrix()
    {

        //one row has n + n-1 elements ( n nodes and n-1 edges )
        int rows = 2 * m - 1;
        int cols = 2 * n - 1;

        // a new integer matrix
        var matrix = new int[rows, cols];

        //initialization ( all fields are walls (1) )
        // rooms are marked with 0
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
            {
                matrix[i, j] = 1;
            }

        //initialize rooms
        // now our maze looks like:
        // room wall room wall room wall
        // wall room wall ...
        // no rooms are connected
        for (int i = 0; i <= rows / 2; i++)
            for (int j = 0; j <= cols / 2; j++)
            {
                matrix[i * 2, j * 2] = 0;
            }


        //make passages between rooms
        // remember: an edge between two nodes in a graph represents a wall between those nodes
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                // room above
                if (i - 1 >= 0 && !grid[i, j].isConnectedTo(new GridNode(i - 1, j)))
                {
                    matrix[2 * i - 1, 2 * j] = 0;
                }

                //room under
                if (i + 1 < m && !grid[i, j].isConnectedTo(new GridNode(i + 1, j)))
                {
                    matrix[2 * i + 1, 2 * j] = 0;
                }

                //room to the left
                if (j - 1 >= 0 && !grid[i, j].isConnectedTo(new GridNode(i, j - 1)))
                {
                    matrix[2 * i, 2 * j - 1] = 0;
                }

                //room to the right
                if (j + 1 < n && !grid[i, j].isConnectedTo(new GridNode(i, j + 1)))
                {
                    matrix[2 * i, 2 * j + 1] = 0;
                }
            }
        }

        return matrix;

    }


    /// <summary>
    /// Makes a complementary graph 
    /// </summary>
    /// <returns> A complementary grid graph</returns>
    public GridGraph ComplementaryGraph()
    {
        GridGraph complement = new GridGraph(m, n);

        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                foreach (GridNode edge in grid[i, j].Edges)
                {
                    complement[i, j].Edges.Remove(edge);
                }
            }
        }


        return complement;
    }

    /// <summary>
    /// Prints the graph to the console or to a file
    /// </summary>
    /// <param name="path"> Optional: path to an output file </param>
    /// TODO: OUTPUT THE GRAPH TO A FILE
    public void PrintGridGraph(string path = "")
    {
        //one row has n + n-1 elements ( n nodes and n-1 edges )
        int rows = 2 * m - 1;
        int cols = 2 * n - 1;

        // a character matrix
        var matrix = new char[rows, cols];

        //initialization ( all fields are empty )
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
            {
                matrix[i, j] = ' ';
            }

        //initialize graph nodes
        for (int i = 0; i <= rows / 2; i++)
            for (int j = 0; j <= cols / 2; j++)
            {
                matrix[i * 2, j * 2] = 'O';
            }


        // Make edges between nodes
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                // node above
                if (i - 1 >= 0 && grid[i, j].isConnectedTo(new GridNode(i - 1, j)))
                {
                    matrix[2 * i - 1, 2 * j] = '|';
                }

                //node under
                if (i + 1 < m && grid[i, j].isConnectedTo(new GridNode(i + 1, j)))
                {
                    matrix[2 * i + 1, 2 * j] = '|';
                }

                //node to the left
                if (j - 1 >= 0 && grid[i, j].isConnectedTo(new GridNode(i, j - 1)))
                {
                    matrix[2 * i, 2 * j - 1] = '-';
                }

                //node to the right
                if (j + 1 < n && grid[i, j].isConnectedTo(new GridNode(i, j + 1)))
                {
                    matrix[2 * i, 2 * j + 1] = '-';
                }
            }
        }

        StringBuilder sb = new StringBuilder();

        // print the graph
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                sb.Append(matrix[i, j]);
            }
            sb.Append("\n");
        }

        if (!String.IsNullOrEmpty(path))
        {
            File.WriteAllText(path, sb.ToString());
        }
        else
        {

        }

    }


    /// <summary>
    /// Calculates the distances of all the other nodes from a given node
    /// </summary>
    /// <param name="v"> A node</param>
    /// <returns> A dictionary of nodes and their distances from the node v</returns>
    public Dictionary<GridNode, int> DistancesFromNode(GridNode v)
    {

        // distances
        var distances = new Dictionary<GridNode, int>();

        var activeDistances = new Dictionary<GridNode, int>();

        var traversed = new List<GridNode>();

        traversed.Add(v);


        foreach (var neighbour in v.Edges)
        {
            activeDistances[neighbour] = 1;
        }

       

        while (activeDistances.Count > 0)
        {
            // nadji najmanji
            var minDistanceNode = (from node in activeDistances
                                   where node.Value == activeDistances.Min(x => x.Value)
                                   select node).First().Key;

            minDistanceNode = this[minDistanceNode];

            // dodaj ga u distance
            distances[minDistanceNode] = activeDistances[minDistanceNode];
            traversed.Add(minDistanceNode);


            // dodaj njegove susede u aktivne distance
            foreach (var neighbour in minDistanceNode.Edges)
            {
                if (!traversed.Contains(neighbour))
                {
                    if (!distances.ContainsKey(neighbour) || (distances[minDistanceNode] + 1) < distances[neighbour])
                    {
                        activeDistances[neighbour] = distances[minDistanceNode] + 1;
                    }
                }
            }


            // obrisi najmanji
            activeDistances.Remove(minDistanceNode);

        }


        return distances;
    }

    #endregion


    #region Indexers
    ///<summary> Indexer that takes node indexes as arguments </summary>
    /// <returns> A grid node at position (i, j) </returns>
    public GridNode this[int i, int j]
    {
        get
        {
            return grid[i, j];
        }
    }

    ///<summary> Indexer that takes a key node as an argument </summary>
    ///<returns> A node with equal indexes as the key node ( they may have different edges ) </returns>
    public GridNode this[GridNode key]
    {

        get
        {
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                {
                    if (grid[i, j] == key)
                    {
                        return grid[i, j];
                    }
                }

            return null;
        }

        set
        {
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                {
                    if (grid[i, j] == key)
                    {
                        grid[i, j] = value;
                    }
                }

        }

    }

    #endregion


    #region Prims algorithm

    ///<summary> Prims algorithm for maze generation </summary>
    /// <returns> A GridGraph where nodes represent rooms and edges represent walls in a maze </returns>
    public GridGraph PrimsAlgorithm()
    {
        GridGraph maze = this;

        var visitedNodes = new List<GridNode>();
        var wallList = new List<Wall>();

        System.Random rand = new System.Random();

        //pick a random cell first
        GridNode firstCell = maze[rand.Next(m), rand.Next(n)];
        visitedNodes.Add(firstCell);


        //add all it's walls to the wall list
        foreach (var edge in maze.getNodeEdges(firstCell))
        {
            wallList.Add(new Wall(firstCell, edge));
        }
        //while there's walls in the wall list
        while (wallList.Count > 0)
        {
            // pick a random wall
            var randomWall = wallList[rand.Next(wallList.Count)];

            // if the node at the other side of the wall is not visited
            if (!visitedNodes.Contains(randomWall.otherNode))
            {
                // remove an edge between these two nodes from the graph
                maze[randomWall.rootNode].removeEdge(randomWall.otherNode);
                maze[randomWall.otherNode].removeEdge(randomWall.rootNode);

                //add the other node to visited nodes
                visitedNodes.Add(randomWall.otherNode);

                // add other node's walls to the wall list
                foreach (var node in maze[randomWall.otherNode].Edges)
                    if (!wallList.Contains(new Wall(randomWall.otherNode, node)))
                        wallList.Add(new Wall(randomWall.otherNode, node));
            }

            wallList.Remove(randomWall);


        }

        return maze;
    }

    #endregion


    #region private class Wall
    /// <summary> A class that represents a Wall in Prims algorithm </summary>
    private class Wall
    {
        public GridNode rootNode, otherNode;

        public Wall(GridNode root, GridNode other)
        {
            rootNode = root;
            otherNode = other;
        }

        public override bool Equals(object obj)
        {
            // If parameter cannot be cast to GridNode return false.
            Wall g2 = obj as Wall;
            if ((System.Object)g2 == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (rootNode == g2.rootNode) && (otherNode == g2.otherNode);
        }

    }
    #endregion

}

#endregion