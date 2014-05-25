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

		public bool AlwaysSpawnAs = false;
		public string SpawnAs = "Prim";

		Dictionary<string, GridNode> spawnNodes;

    #endregion


    #region Start and Update


		/// <summary>
		/// Generate and render the world for the master client
		/// </summary>
		void Start ()
		{

				thisScriptView = this.GetComponent<PhotonView> ();

				if (thisScriptView == null) {
						Debug.LogError ("thisScriptView is null! FREAK OUT!!");
				}

				if (PhotonNetwork.isMasterClient) {
						Debug.Log ("I am the master client. Generating the maze.");

						// prims spawn coordinates
						int x_sp = 0;
						int z_sp = 0;

						// If staticMaze variable is set generate a static maze from a given file
						// procedurally generate a maze otherwise
						if (ConfigManager.staticMaze) {
								mazeMatrix = GenerateWorld.GenerateMazeStatic (@"maze32.txt");
								matrixSize = mazeMatrix.GetLength (0);
						} else {

								var maze = GenerateWorld.GenerateMaze ();

								mazeMatrix = maze.matrix;
								matrixSize = mazeMatrix.GetLength (0);

								spawnNodes = maze.GetSpawnNodes ();

								var primSpawn = spawnNodes ["Prim"];

								x_sp = primSpawn.i;
								z_sp = primSpawn.j;

						}

						for (int i = 0; i < mazeMatrix.GetLength(0); i++)
								RenderOneMazeRow (i, getRow (i, mazeMatrix));

						// Spawn Prim
						int XPosition = x_sp * 2;
						int ZPosition = z_sp * 2 + 1;
						spawnPlayer ("Prim", new Vector3 (XPosition, 0.1f, ZPosition));


						// Instantiate EXIT
						var exitNode = spawnNodes ["Exit"];

						int exitX = exitNode.i * 2;
						int exitZ = exitNode.j * 2 + 1;

						GameObject exit = GameObject.CreatePrimitive (PrimitiveType.Sphere);
						exit.transform.position = new Vector3 (exitX, 0.3f, exitZ);
						exit.transform.localScale = new Vector3 (0.2f, 0.2f, 0.2f);
				} else {
						Debug.Log ("I am not the master client, waiting for data to be recieved over the network.");
				}
        

		}
    
		/// <summary>
		/// The master client waits until the other client connects then the RPC method for sending the maze data
		/// over the network is called.
		/// </summary>
		void Update ()
		{
				if (PhotonNetwork.isMasterClient && rowsSent < matrixSize && PhotonNetwork.playerList.Length == 2) {
						thisScriptView.RPC ("RenderOneMazeRow", PhotonTargets.Others, new object[] {
								rowsSent,
								getRow (rowsSent, mazeMatrix)
						});
						rowsSent++;

						// when the world is generated, instantiate the other player
						if (rowsSent >= matrixSize) {
								var darkPrimSpawn = spawnNodes ["DarkPrim"];

								int XPosition = darkPrimSpawn.i * 2;
								int ZPosition = darkPrimSpawn.j * 2 + 1;

								thisScriptView.RPC ("spawnPlayer", PhotonTargets.Others, new object[] {
										"DarkPrim",
										new Vector3 (XPosition, 1f, ZPosition)
								});
						}

				}

				// If the other player disconnects, reset the process
				if (PhotonNetwork.playerList.Length == 1 && rowsSent > 0) {
						rowsSent = 0;
				}
		}

    #endregion


    #region Variables necessary for data transfer over the network

		/// <summary>
		/// Keeps track of the current row sent to the other client
		/// </summary>
		int rowsSent = 0;

    #endregion


    #region Support methods for network transfer

		/// <summary>
		/// Fetch one row from an integer matrix
		/// </summary>
		/// <param name="index"> Row index</param>
		/// <param name="matrix"> Target matrix </param>
		/// <returns> An array of integers representing a row </returns>
		private int[] getRow (int index, int[,] matrix)
		{
				var result = new int[31];

				for (int j = 0; j < matrix.GetLength(1); j++) {
						result [j] = matrix [index, j];
				}

				return result;
		}

    #endregion


    #region Render and SpawnPlayer methods

		/// <summary>
		/// We render the world row by row, because the F*****G Photon RPC can't use
		/// and data structure more complex then a F*****G array.
		/// </summary>
		/// <param name="rowIndex"> Index of a row in a maze matrix</param>
		/// <param name="row"> Integer row </param>
		[RPC]
		void RenderOneMazeRow (int rowIndex, int[] row)
		{

				int width = row.Length;

				GameObject Walls = new GameObject ();
				Walls.gameObject.name = "Walls";
				GameObject Floor = new GameObject ();
				Floor.gameObject.name = "Floor";

				Vector3 center = new Vector3 (width / 2, 0, width / 2);

				Walls.transform.position = center;
				Floor.transform.position = center;


				for (int i = 0; i < width; i++) {

						GameObject kocka = GameObject.CreatePrimitive (PrimitiveType.Cube);
						kocka.isStatic = true;

						int c = row [i];

						Vector3 pos;
						if (c == 0) {
								pos = new Vector3 (i, -1, width - rowIndex);
								kocka.gameObject.name = pos.ToString ();
								kocka.transform.position = pos;
								kocka.gameObject.tag = "Floor";
								kocka.gameObject.AddComponent<PathColor> ();
								kocka.transform.parent = Floor.transform;
						} else {
								pos = new Vector3 (i, 0, width - rowIndex);
								kocka.gameObject.name = pos.ToString ();
								kocka.transform.position = pos;
								kocka.gameObject.tag = "Wall";
								kocka.transform.parent = Walls.transform;
						}
				}


		}


		/// <summary>
		/// Spawns the chosen player prefab at the given position
		/// </summary>
		/// <param name="who"> Prim or DarkPrim </param>
		/// <param name="where"> Spawn point coordinates </param>
		[RPC]
		public void spawnPlayer (string who, Vector3 where)
		{

				GameObject player = null;

				if (AlwaysSpawnAs) {
						player = (GameObject)PhotonNetwork.Instantiate (SpawnAs, where, Quaternion.identity, 0);
				} else {

						player = (GameObject)PhotonNetwork.Instantiate (who, where, Quaternion.identity, 0);

				}

				if (player.gameObject.tag == "Prim") {
						//spawn the goddamn flashlight...
						GameObject flashlight = (GameObject)PhotonNetwork.Instantiate ("Flashlight", new Vector3 (0, 2, 0), Quaternion.identity, 0);
						flashlight.GetComponent<FlashlightBehaviour> ().enabled = true;
						flashlight.GetComponent<FlashlightBehaviour> ().parent = player.transform;
						//set the controls for the goddamn flashlight...
						player.GetComponent<PrimsControls> ().flashlight = flashlight.transform;
                        player.transform.FindChild("Point light").light.enabled = true;
				}

				((MonoBehaviour)player.GetComponent ("FPSInputController")).enabled = true;
				//((MonoBehaviour)player.GetComponent ("MouseLook")).enabled = true;
				((MonoBehaviour)player.GetComponent ("Sprint")).enabled = true;
				player.transform.FindChild ("MainCamera").gameObject.camera.enabled = true;

				if (player.gameObject.tag == "Prim") {
						((MonoBehaviour)player.GetComponent ("PrimsControls")).enabled = true;
						((MonoBehaviour)player.GetComponent ("GenerateFootsteps")).enabled = true;
				} else if (player.gameObject.tag == "DarkPrim") {
						((MonoBehaviour)player.GetComponent ("DarkPrimControls")).enabled = true;
						
                        //Lampa nije bitna vise za DarkPrima...
                        /*GameObject flashlight = GameObject.FindGameObjectWithTag ("Flashlight");
						if (flashlight) {
								//Ako lampa vec postoji, znaci da je Prim vec spawnovan
								flashlight.GetComponent<FlashlightBehaviour> ().parent = GameObject.FindGameObjectWithTag ("Prim").transform;
						}*/

                        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
                        foreach(var wall in walls)
                        {
                            wall.renderer.material.color = Color.gray;
                        }
                        GameObject[] floors = GameObject.FindGameObjectsWithTag("Floor");
                        foreach(var floor in floors)
                        {
                            floor.renderer.material.color = Color.gray;
                        }
                        GameObject.Find("Skylight").light.enabled = true;
				}


				// movement setup
				var motor = player.GetComponent<CharacterMotor> ();

				if (motor != null) {
						motor.jumping.enabled = ConfigManager.canJump;
				} else {
						Debug.Log ("Failed to get component CharacterMotor.");
				}
		}

    #endregion


    #region Serialize view
		// We don't use this for network transfer any more, but Unity complains if we erase this method.
		public void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
		{


		}
    #endregion
}
