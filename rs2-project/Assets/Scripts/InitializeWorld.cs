using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InitializeWorld : MonoBehaviour {

    private bool generated = false;
    private int[,] mazeMatrix;
    private bool sent = false;
    int matrixSize = 0;

	// Generate and render the world
	void Start () {

        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("I am the master client. Generating the maze.");

            // If staticMaze variable is set generate a static maze from a given file
            // procedurally generate a maze otherwise
            if (ConfigManager.staticMaze)
            {
                mazeMatrix = GenerateWorld.GenerateMazeStatic(@"maze32.txt");
                matrixSize = mazeMatrix.GetLength(0);
            }
            else
            {
                mazeMatrix = GenerateWorld.GenerateMaze();
                matrixSize = mazeMatrix.GetLength(0);
            }

            Debug.Log("Rendering the maze.");
            GenerateWorld.RenderMaze(mazeMatrix);
            generated = true;
        }
        else
        {
            Debug.Log("I am not the master client, waiting for data to be recieved over the network.");
        }
        

	}


    #region Variables necessary for data transfer over the network

    int rowsSent = 0;
    int rowsRecieved = 0;
   
    List<int[]> rows= new List<int[]>();
    bool sizeSent = false;

    #endregion


    #region Sending data over the network method 

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
        if (stream.isWriting) {
            //  only the master client sends the matrix data
            if (PhotonNetwork.isMasterClient && rowsSent<matrixSize)
            {
                // the size of the matrix is sent first
                if (!sizeSent)
                {
                    Debug.Log("Sending: Matrix size = " + matrixSize);
                    stream.SendNext(matrixSize);
                    sizeSent = true;
                }
                    //send the rows
                else
                {
                    stream.SendNext(getRow(rowsSent, mazeMatrix));
                    rowsSent++;
                }
            }
               
            }
            else 
            {
                if (!PhotonNetwork.isMasterClient)
                {
                    // read the size of the matrix
                    if (matrixSize == 0)
                    {
                        matrixSize = (int)stream.ReceiveNext();
                        Debug.Log("Matrix size = " + matrixSize);
                    }
                    // read the rows 
                    else
                    {

                        int[] current = (int[])stream.ReceiveNext();

                        rows.Add(current);
                        rowsRecieved++;

                        if (rowsRecieved == matrixSize)
                        {
                            Debug.Log("Recieved data. Rendering the world.");

                            mazeMatrix = combineRows(rows);

                            GenerateWorld.RenderMaze(mazeMatrix);
                        }
                    }

                }
            }   

    }

    #endregion


    #region Support methods for network transfer

    /// <summary>
    /// Combine a list of int arrays into a two dimensional matrix
    /// </summary>
    /// <param name="rows"> List of rows </param>
    /// <returns> A two dimensional integer matrix </returns>
    private int[,] combineRows(List<int[]> rows)
    {
        int r = rows.Count;
        int c = rows[0].Length; 

        var matrix = new int[r, c];

        for (int i = 0; i < r; i++)
        {
            for (int j = 0; j < c; j++)
            {
                matrix[i, j] = rows[i][j];
            }
        }

            return matrix;
    }


    /// <summary>
    /// Fetch one row from an integer matrix
    /// </summary>
    /// <param name="index"> Row index</param>
    /// <param name="matrix"> Target matrix </param>
    /// <returns> An array of integers representing a row </returns>
    private int[] getRow(int index, int[,] matrix)
    {
        var result = new int[31];

        for (int j = 0; j < matrix.GetLength(1); j++)
        {
            result[j] = matrix[index, j];
        }

        return result;
    }

    #endregion

}
