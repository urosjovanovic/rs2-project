using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class InitializeWorld : MonoBehaviour
{

    #region Private class fields and properties

        // a generated maze matrix
		private int[,] mazeMatrix;
		private int matrixSize = 0;

        // photon view for RPC methods
		private PhotonView thisScriptView;
        
        // generated spawn points
		private Dictionary<string, GridNode> spawnNodes;
        
        // MarkerCollectible prefab
        private UnityEngine.Object markerCollectible = Resources.Load("MarkerCollectible");
        private UnityEngine.Object exit = Resources.Load("Exit");
        private UnityEngine.Object easterEgg = Resources.Load("EasterEgg");

        // WhisperObject
        private UnityEngine.Object whisper = Resources.Load("WhisperObject");

    #endregion


    #region Public class fields and properties
        
        /// <summary>
        /// Always spawn as a given character
        /// </summary>
        public bool AlwaysSpawnAs = false;

        /// <summary>
        /// Possible values for this field are "Prim" and "DarkPrim"
        /// </summary>
        public string SpawnAs = "Prim";

        /// <summary>
        /// Prefabs to be instantiated. Must be set from Scene Inspector.
        /// </summary>
        public Transform WallPrefab;
        public Transform FloorPrefab; //TODO

    #endregion


    #region Start ( Maze and spawn point generation, Prim stuff ) and Update ( sending data over the network )


        /// <summary>
		/// Generate and render the world for the master client
		/// </summary>
		void Start ()
		{

                // get the neccessary components
				thisScriptView = this.GetComponent<PhotonView> ();
                if (thisScriptView == null) throw new Exception("Component thisScriptView is NULL.");

                markerCollectible = Resources.Load("MarkerCollectible");
                if (markerCollectible == null) throw new Exception("Object markerCollectible is null. Resource load failed.");


                // Master client is always Prim, so we need to generate the maze,
                // instantiate Prim, collectible markers and the exit.
				if (PhotonNetwork.isMasterClient) {

						Debug.Log ("-- I AM THE MASTER CLIENT --");


                        // Generate the world
                        Debug.Log("Generating the world.");
                        var maze = GenerateWorld.GenerateMaze(32);
                        mazeMatrix = maze.matrix;
                        matrixSize = mazeMatrix.GetLength(0);

                        // Generate all spawn nodes
                        spawnNodes = maze.GetSpawnNodes();
                        var primSpawn = spawnNodes["Prim"];
                        var exitSpawn = spawnNodes["Exit"];
                        var darkPrimSpawn = spawnNodes["DarkPrim"];
                        var markerCollectibleNodes = maze.GetMarkerCollectibleSpawnNodes();
                        var whisperNodes = maze.GetWhisperSourceSpawnNodes(SoundPool.Whispers.Length);
                        var easterEggNode = maze.GetEasterEggNode();

                        // MAZE RENDERING 
                        for (int i = 0; i < mazeMatrix.GetLength(0); i++)
                            RenderOneMazeRow(i, getRow(i, mazeMatrix));

                        // Spawn the character
                        // If AlwaysSpawnAs is not set, default is Prim
                        if (AlwaysSpawnAs && SpawnAs.Equals("DarkPrim"))
                        {
                            Vector3 darkPrimSpawnPosition = GetVectorFromNode(darkPrimSpawn, 0.1f);

                            var rotation = Quaternion.LookRotation((GetVectorFromNode(darkPrimSpawn.Edges[0], 0.1f) - darkPrimSpawnPosition).normalized);

                            spawnPlayer("DarkPrim", darkPrimSpawnPosition, rotation);
                            Debug.Log("DarkPrim spawned.");
                        }
                        else
                        {
                            // Spawn Prim
                            Vector3 primSpawnPosition = GetVectorFromNode(primSpawn, 0.1f);

                            var rotation = Quaternion.LookRotation((GetVectorFromNode(primSpawn.Edges[0], 0.1f) - primSpawnPosition).normalized);


                            spawnPlayer("Prim", primSpawnPosition, rotation);

                            Debug.Log("PRIM spawned.");

                            var exitNode = spawnNodes["Exit"];

							var exitNeighbour = exitNode.Edges[0];
							var exitNeighbourPosition = GetVectorFromNode(exitNeighbour,-1);
							var exitPosition = GetVectorFromNode(exitNode,-1);
							
							// place the exit between the center of the exit node and the center of the neighbour node
							var newExitPosition = (exitPosition + exitNeighbourPosition)/2;

                            // Instantiate exit
                            GameObject exitObj = (GameObject)Instantiate(exit, newExitPosition, Quaternion.LookRotation((GetVectorFromNode(exitNode.Edges[0], 0) - GetVectorFromNode(exitNode, 0)).normalized)); // look toward a neighbour node
                            
                            // Instantiate the whispers
                            InstantiateWhisperObjects(whisperNodes);

                            // Instantiate the marker collectibles
                            InstantiateMarkerPrefabs(markerCollectibleNodes);

                            var easterEggObject = (GameObject)Instantiate(easterEgg, GetVectorFromNode(easterEggNode, -0.497f), Quaternion.identity);
                            easterEggObject.transform.Rotate(new Vector3(90, 90, 0));
                            easterEggObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

                        }

				} else {
						Debug.Log ("-- SLAVE CLIENT -- \n Waiting for data to be recieved over the network.");
				}
        
		}

        

        private void InstantiateWhisperObjects(List<GridNode> whisperNodes)
        {
            foreach (var node in whisperNodes)
            {
                Instantiate(whisper, GetVectorFromNode(node, 0), Quaternion.identity);
            }
        }

        /// <summary>
        /// Calculates the vector position of a graph node in the world
        /// </summary>
        /// <param name="node"> A node in the graph </param>
        /// <param name="VerticalOffset"> Vertical position </param>
        /// <returns> A Vector3 representing the position of the node </returns>
        private Vector3 GetVectorFromNode(GridNode node, float VerticalOffset)
        {
            return new Vector3(node.j * 2, VerticalOffset, matrixSize - node.i * 2);
        }

        /// <summary>
        /// Instantiate the MarkerCollectible prefab given list of nodes 
        /// </summary>
        /// <param name="markerCollectibleNodes"> List of graph nodes that should contain the marker collectibles </param>
        private void InstantiateMarkerPrefabs(List<GridNode> markerCollectibleNodes)
        {
            foreach (var node in markerCollectibleNodes)
            {
                int XPos = node.j * 2;
                int ZPos = matrixSize - node.i * 2;

                var ins = (GameObject)Instantiate(markerCollectible, new Vector3(XPos, 0.1f, ZPos), Quaternion.identity);

                ins.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                ins.transform.Translate(new Vector3(0, -0.6f, 0));
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

						// when the world is generated, instantiate the other player ( DarkPrim )
						if (rowsSent >= matrixSize) {

                            if (AlwaysSpawnAs && SpawnAs.Equals("DarkPrim"))
                            {
                                var primSpawn = spawnNodes["Prim"];
                                Vector3 primSpawnPosition = GetVectorFromNode(primSpawn, 0.1f);

                                var rotation = Quaternion.LookRotation((GetVectorFromNode(primSpawn.Edges[0], 0.1f) - primSpawnPosition).normalized);


                                thisScriptView.RPC("spawnPlayer", PhotonTargets.Others, new object[] {
										"Prim",
										primSpawnPosition,
                                        rotation
								});
                            }
                            else
                            {
                                var darkPrimSpawn = spawnNodes["DarkPrim"];
                                Vector3 darkPrimSpawnPosition = GetVectorFromNode(darkPrimSpawn, 0.1f);

                                var rotation = Quaternion.LookRotation((GetVectorFromNode(darkPrimSpawn.Edges[0], 0.1f) - darkPrimSpawnPosition).normalized);

                                thisScriptView.RPC("spawnPlayer", PhotonTargets.Others, new object[] {
										"DarkPrim",
										darkPrimSpawnPosition,
                                        rotation
								});

                                Debug.Log("DarkPrim joined the room.");
                            }
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
				var result = new int[matrix.GetLength(0)];

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

                        GameObject kocka = null;

						int c = row [i];

						Vector3 pos;
						if (c == 0) {
                                kocka = (GameObject)GameObject.Instantiate((GameObject)Resources.Load("FloorPrefab"));
								pos = new Vector3 (i, -1, width - rowIndex);
								kocka.gameObject.name = pos.ToString ();
								kocka.transform.position = pos;
								//kocka.gameObject.tag = "Floor";
								kocka.gameObject.AddComponent<FloorBehaviour> ();
								kocka.transform.parent = Floor.transform;
						} else {
                                kocka = (GameObject)GameObject.Instantiate((GameObject)Resources.Load("WallPrefab"));
								pos = new Vector3 (i, 0, width - rowIndex);
								kocka.gameObject.name = pos.ToString ();
								kocka.transform.position = pos;
								//kocka.gameObject.tag = "Wall";
								kocka.transform.parent = Walls.transform;
						}

                        kocka.isStatic = true;
				}


		}


		/// <summary>
		/// Spawns the chosen player prefab at the given position
		/// </summary>
		/// <param name="who"> Prim or DarkPrim </param>
		/// <param name="where"> Spawn point coordinates </param>
		[RPC]
		public void spawnPlayer (string who, Vector3 where, Quaternion rotation)
		{
				GameObject player = null;

				player = (GameObject)PhotonNetwork.Instantiate (who, where, rotation, 0);

                PhotonNetwork.player.name = player.tag;

                //global
    
                ((MonoBehaviour)player.GetComponent("FPSInputController")).enabled = true;
                //((MonoBehaviour)player.GetComponent ("MouseLook")).enabled = true;
                player.transform.FindChild("MainCamera").GetComponent<AudioSource>().enabled = true;
                player.transform.FindChild("MainCamera").gameObject.camera.enabled = true;
                player.transform.FindChild("MainCamera").GetComponent<AudioListener>().enabled = true;

                //Prim only
				if (player.gameObject.tag == "Prim") 
                {
						//spawn the goddamn flashlight...
						GameObject flashlight = (GameObject)PhotonNetwork.Instantiate ("Flashlight", new Vector3 (0, 2, 0), Quaternion.identity, 0);
						flashlight.GetComponent<FlashlightBehaviour> ().enabled = true;
						flashlight.GetComponent<FlashlightBehaviour> ().parent = player.transform;
						//set the controls for the goddamn flashlight...
						player.GetComponent<PrimsControls> ().flashlight = flashlight.transform;
                        player.transform.FindChild("Point light").light.enabled = true;
                        ((MonoBehaviour)player.GetComponent("Sprint")).enabled = true;
                        ((MonoBehaviour)player.GetComponent("PrimsControls")).enabled = true;
                        ((MonoBehaviour)player.GetComponent("GenerateFootsteps")).enabled = true;
                        ((MonoBehaviour)player.GetComponent("EndGamePrim")).enabled = true;
                        player.transform.FindChild("MainCamera").GetComponent<UIPrim>().enabled = true;
				}
                    //DarkPrim only
                else if (player.gameObject.tag == "DarkPrim") 
                {
						((MonoBehaviour)player.GetComponent ("DarkPrimControls")).enabled = true;

                        //Lampa nije bitna vise za DarkPrima...
                        /*GameObject flashlight = GameObject.FindGameObjectWithTag ("Flashlight");
						if (flashlight) {
								//Ako lampa vec postoji, znaci da je Prim vec spawnovan
								flashlight.GetComponent<FlashlightBehaviour> ().parent = GameObject.FindGameObjectWithTag ("Prim").transform;
						}*/

                        ((MonoBehaviour)player.GetComponent("EndGameDarkPrim")).enabled = true;
                        player.transform.FindChild("MainCamera").GetComponent<UIDarkPrim>().enabled = true;

                        GameObject.Find("Skylight").light.enabled = true;
				}

                GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
                foreach (var wall in walls)
                {
                    wall.renderer.material.color = Color.gray;
                }
                GameObject[] floors = GameObject.FindGameObjectsWithTag("Floor");
                foreach (var floor in floors)
                {
                    floor.renderer.material.color = Color.white;
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
