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

			    InitializeWorld ();
                
		}

		public void InitializeWorld ()
		{
				GameObject god = (GameObject)PhotonNetwork.Instantiate ("TheCreator", Vector3.zero, Quaternion.identity, 0);

				var initScript = ((MonoBehaviour)god.GetComponent ("InitializeWorld"));

				if (initScript != null) {
						initScript.enabled = true;
				} else {
						Debug.LogError ("InitializeWorld is null! FREAK OUT!");
				}

               
		}

       

}
