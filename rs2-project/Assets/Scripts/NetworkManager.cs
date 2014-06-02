using UnityEngine;
using System.Collections;
using System;

public class NetworkManager : Photon.MonoBehaviour
{

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
				PhotonNetwork.CreateRoom ("VeryScaryRoom");           
		}

		void OnJoinedRoom ()
		{
				Debug.Log ("Joined Room!");

				InitializeWorld ();
                
		}

		public void InitializeWorld ()
		{
                // AND THAT'S HOW A GOD IS BORN
				GameObject god = (GameObject)PhotonNetwork.Instantiate ("TheCreator", Vector3.zero, Quaternion.identity, 0);

                // AND THIS IS HOW HE CREATED THE WORLD
				var initScript = ((MonoBehaviour)god.GetComponent ("InitializeWorld"));
                if (initScript == null) throw new Exception("Component initScript is null.");
                initScript.enabled = true;
               
		}

       

}
