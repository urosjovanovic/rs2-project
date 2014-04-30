using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InitializeWorld : MonoBehaviour
{

    #region Class fields and properties

    private int[,] mazeMatrix;
    int matrixSize = 0;

    /// <summary>
    /// Photon view necessary for the RPC
    /// </summary>
    PhotonView thisScriptView;

    #endregion


    #region Start and Update

    /// <summary>
	/// Generate and render the world for the master client
	/// </summary>
	void Start () {

        thisScriptView = this.GetComponent<PhotonView>();

        if (thisScriptView == null)
        {
            Debug.LogError("thisScriptView is null! FREAK OUT!!");
        }

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

            for (int i = 0; i < mazeMatrix.GetLength(0); i++ )
                RenderOneMazeRow(i, getRow(i, mazeMatrix));
        }
        else
        {
            Debug.Log("I am not the master client, waiting for data to be recieved over the network.");
        }
        

	}

    #region Variables necessary for data transfer over the network

    /// <summary>
    /// Keeps track of the current row sent to the other client
    /// </summary>
    int rowsSent = 0;

    #endregion

    /// <summary>
    /// The master client waits until the other client connects then the RPC method for sending the maze data
    /// over the network is called.
    /// </summary>
    void Update()
    {
        if (PhotonNetwork.isMasterClient && rowsSent < matrixSize && PhotonNetwork.playerList.Length == 2)
        {
            thisScriptView.RPC("RenderOneMazeRow", PhotonTargets.Others, new object[] { rowsSent, getRow(rowsSent, mazeMatrix) });
            rowsSent++;
        }
    }

    #endregion


    #region Support methods for network transfer

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


    #region Render method

    /// <summary>
    /// We render the world row by row, because the F*****G Photon RPC can't use
    /// and data structure more complex then a F*****G array.
    /// </summary>
    /// <param name="rowIndex"> Index of a row in a maze matrix</param>
    /// <param name="row"> Integer row </param>
    [RPC]
    void RenderOneMazeRow(int rowIndex, int[] row)
    {

        int width = row.Length;

        GameObject Walls = new GameObject();
        Walls.gameObject.name = "Walls";
        GameObject Floor = new GameObject();
        Floor.gameObject.name = "Floor";

        Vector3 center = new Vector3(width / 2, 0, width / 2);

        Walls.transform.position = center;
        Floor.transform.position = center;


        for (int i = 0; i < width; i++)
        {

            GameObject kocka = GameObject.CreatePrimitive(PrimitiveType.Cube);
            kocka.isStatic = true;

            int c = row[i];

            Vector3 pos;
            if (c == 0)
            {
                pos = new Vector3(i, -1, width - rowIndex);
                kocka.gameObject.name = pos.ToString();
                kocka.transform.position = pos;
                kocka.gameObject.tag = "Floor";
                kocka.gameObject.AddComponent<PathColor>();
                kocka.transform.parent = Floor.transform;
            }
            else
            {
                pos = new Vector3(i, 0, width - rowIndex);
                kocka.gameObject.name = pos.ToString();
                kocka.transform.position = pos;
                kocka.gameObject.tag = "Wall";
                kocka.transform.parent = Walls.transform;
            }
        }


    }

    #endregion

    // We don't use this for network transfer any more, but Unity complains if we erase this method.
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {


    }

}
