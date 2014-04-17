using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze1
{
    /// <summary>
    /// A class representing a grid graph
    /// </summary>
    class GridGraph
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


        #region Methods: Create an integer matrix from a gridGraph


        /// <summary> Creates an integer matrix from a GridGraph </summary>
        /// <returns> An integer matrix where 1 represents a wall and 0 represents a room or a passage </returns>
        public int[,] createWallMatrix()
        {

            //one row has n + n-1 elements ( n nodes and n-1 edges )
            int rows = 2*m - 1;
            int cols = 2*n - 1;

            // a new integer matrix
            var matrix = new int[rows, cols];

            //initialization ( all fields are walls (1) )
            // rooms are marked with 0
            for(int i=0;i<rows;i++)
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = 1;
                }

            //initialize rooms
            // now our maze looks like:
            // room wall room wall room wall
            // wall room wall ...
            // no rooms are connected
            for (int i = 0; i <= rows / 2; i++ )
                for (int j = 0; j <= cols/2; j++)
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
                    if (i - 1 >= 0 && !grid[i,j].isConnectedTo(new GridNode(i-1,j)))
                    {
                        matrix[2*i - 1, 2*j] = 0;
                    }

                    //room under
                    if (i + 1 < m && !grid[i, j].isConnectedTo(new GridNode(i + 1, j)))
                    {
                        matrix[2*i + 1, 2*j] = 0;
                    }

                    //room to the left
                    if (j - 1 >= 0 && !grid[i, j].isConnectedTo(new GridNode(i, j-1)))
                    {
                        matrix[2*i, 2*j - 1] = 0;
                    }

                    //room to the right
                    if (j + 1 < n && !grid[i, j].isConnectedTo(new GridNode(i, j + 1)))
                    {
                        matrix[2*i, 2*j + 1] = 0;
                    }
                }
            }

            return matrix;

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
            
            Random rand = new Random();

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
}
