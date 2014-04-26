using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Maze1
{
    class Program
    {

        #region Test methods
        /// <summary>
        /// Tests the generation speed of a maze with given dimensions
        /// </summary>
        /// <param name="rows"> Number of rows in a maze matrix</param>
        /// <param name="cols"> Number of columns in a maze matrix </param>
        static void testMazeGenerationSpeed(string outputFile, int rows, int cols)
        {
            try
            {
                Maze testMaze = new Maze(rows, cols);

                //start the Stopwatch
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                testMaze.PrintToFile(outputFile);

                stopWatch.Stop();
                // Get the elapsed time as a TimeSpan value.
                TimeSpan ts = stopWatch.Elapsed;

                // Format and display the TimeSpan value. 
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);

                //output results
                Console.WriteLine("-- FINISHED --");
                Console.WriteLine("Maze {0}x{1} generated in {2}.", rows,cols,elapsedTime);

            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        static void Main(string[] args)
        {
            Maze maze = new Maze(8,8);

            maze.Graph.PrintGridGraph();

            Console.WriteLine();

            foreach (var pair in maze.GetSpawnNodes())
            {
                Console.WriteLine(pair.Key + " - " + pair.Value);
            }


        }
    }
}
