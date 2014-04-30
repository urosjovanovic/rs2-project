using UnityEngine;
using System.Collections;

public class NetworkManager : Photon.MonoBehaviour
{
		public bool AlwaysSpawnAs = false;
		public string SpawnAs = "Prim";

		// Use this for initialization
		void Start ()
		{
            Screen.showCursor = false;
            Connect ();
		}

		void Connect ()
		{
				PhotonNetwork.ConnectUsingSettings ("v0.1");
		}

		void OnGUI ()
		{
				GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());
		}
	 
		void OnJoinedLobby ()
		{
				PhotonNetwork.JoinRandomRoom ();
		}

		void OnPhotonRandomJoinFailed ()
		{
				Debug.Log ("Failed to join random room, creating new room...");
				PhotonNetwork.CreateRoom ("TestRoom");           
		}

		void OnJoinedRoom ()
		{
				Debug.Log ("Joined Room");

				SpawnPlayer ();
                
		}

		public void SpawnPlayer ()
		{
				GameObject god = (GameObject)PhotonNetwork.Instantiate ("TheCreator", Vector3.zero, Quaternion.identity, 0);

                var initScript = ((MonoBehaviour)god.GetComponent("InitializeWorld"));

                if (initScript != null)
                {
                    initScript.enabled = true;
                }
                else
                {
                    Debug.LogError("InitializeWorld is null! FREAK OUT!");
                }

				GameObject player = null;

				if (AlwaysSpawnAs) {
						player = (GameObject)PhotonNetwork.Instantiate (SpawnAs, new Vector3 (0, 2, 0), Quaternion.identity, 0);
				} else {
						// if room is empty spawn Prim
						if (PhotonNetwork.isMasterClient) {
								player = (GameObject)PhotonNetwork.Instantiate ("Prim", new Vector3 (0, 2, 0), Quaternion.identity, 0);
						} else { //spawn DarkPrim
								player = (GameObject)PhotonNetwork.Instantiate ("DarkPrim", new Vector3 (0, 2, 0), Quaternion.identity, 0);
						}
				}

				((MonoBehaviour)player.GetComponent ("FPSInputController")).enabled = true;
				//((MonoBehaviour)player.GetComponent ("MouseLook")).enabled = true;
				player.transform.FindChild ("MainCamera").gameObject.camera.enabled = true;
				

				// movement setup
				var motor = player.GetComponent<CharacterMotor> ();

				if (motor != null) {
						motor.jumping.enabled = ConfigManager.canJump;  
				} else {
						Debug.Log ("Failed to get component CharacterMotor.");
				}

               
		}

       

}
