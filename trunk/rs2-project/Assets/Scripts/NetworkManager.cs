using UnityEngine;
using System.Collections;

public class NetworkManager : Photon.MonoBehaviour
{

		// Use this for initialization
		void Start ()
		{
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

		void SpawnPlayer ()
		{
				GameObject player = (GameObject)PhotonNetwork.Instantiate ("PlayerController", new Vector3 (0, 2, 0), Quaternion.identity, 0);
				((MonoBehaviour)player.GetComponent ("FPSInputController")).enabled = true;
				((MonoBehaviour)player.GetComponent ("MouseLook")).enabled = true;
				player.transform.FindChild ("MainCamera").gameObject.camera.enabled = true;
		}
}
