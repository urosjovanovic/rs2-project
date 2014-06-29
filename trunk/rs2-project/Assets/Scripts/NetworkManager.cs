using UnityEngine;
using System.Collections;
using System;

public class NetworkManager : Photon.MonoBehaviour
{

        GameObject god;
        bool initialized = false;

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
				god = (GameObject)PhotonNetwork.Instantiate ("TheCreator", Vector3.zero, Quaternion.identity, 0);
                initialized = true;
               
		}

        void Update()
        {
            if (initialized && ((!ConfigManager.waitForOtherPlayer) || PhotonNetwork.room.playerCount == 2))
            {
                if (god != null)
                {
                    // AND THIS IS HOW HE CREATED THE WORLD
                    var initScript = ((MonoBehaviour)god.GetComponent("InitializeWorld"));
                    if (initScript == null) throw new Exception("Component initScript is null.");
                    initScript.enabled = true;

                    GameObject loadingGameCamera = GameObject.Find("LoadingGameCamera");
                    loadingGameCamera.GetComponent<GUIText>().enabled = false;
                    loadingGameCamera.camera.enabled = false;

                    this.enabled = false;
                }
            }

        }

       

}
