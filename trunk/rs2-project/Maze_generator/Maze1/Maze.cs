using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze1
{
    /// <summary>
    /// A class representing a maze
    /// </summary>
    class Maze
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


        #region Methods: PrintToFile, GetSpawnNodes

        /// <summary> Writes a maze matrix to a file with a given path  </summary>
        /// <param name="filePath"> A path to a file.</param>
        public void PrintToFile(string filePath){

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

            Random rand = new Random();

            int side = rand.Next(1, 5);


            // Prim gets a random edge node for his spawn point
            GridNode primSpawnPoint = new GridNode(0, 0);

            // up
            if (side == 1)
            {
                int i = 0;
                int j = rand.Next(0, 4);

                primSpawnPoint = Graph[i, j];
            }
            // right
            if (side == 2)
            {
                int j = 3;
                int i = rand.Next(0, 4);

                primSpawnPoint = Graph[i, j];
            }
            //down
            if (side == 3)
            {
                int i = 3;
                int j = rand.Next(0, 4);

                primSpawnPoint = Graph[i, j];
            }
            //left
            if (side == 4)
            {
                int j = 0;
                int i = rand.Next(0, 4);

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

        #endregion


    }
}
