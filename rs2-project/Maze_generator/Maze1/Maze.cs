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

        int rows;
        int cols;
        #endregion 

        #region Class constructors

        /// <summary>
        /// A public constructor for the maze class 
        /// </summary>
        /// <param name="rows"> Number of rows in a maze matrix </param>
        /// <param name="cols"> Number of columns in a maze matrix </param>
        public Maze(int rows, int cols)
        {
            if (rows % 2 != 0 || cols % 2 != 0)
                throw new ArgumentException("Public Maze constructor argument exception: Rows and columns cannot be odd numbers.");

            GridGraph graph = new GridGraph(rows / 2, cols / 2);
            GridGraph maze = graph.PrimsAlgorithm();

            this.rows = rows - 1;
            this.cols = cols - 1;

            matrix = maze.createWallMatrix();
        }
        #endregion

        #region Methods: printToFile
        /// <summary> Writes a maze matrix to a file with a given path  </summary>
        /// <param name="filePath"> A path to a file.</param>
        public void printToFile(string filePath){

            string output = "";

            // make an output string
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    output += matrix[i, j];
                }
                output += System.Environment.NewLine;
            }

            System.IO.File.WriteAllText(filePath, output);

        }
        #endregion


    }
}
