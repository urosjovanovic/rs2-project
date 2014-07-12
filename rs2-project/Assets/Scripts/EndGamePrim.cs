using UnityEngine;
using System.Collections;
using System;

public class EndGamePrim : MonoBehaviour 
{
    private bool isDead = false;
    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }

    private PhotonView view;

	void Start ()
    {
        if (!(view = this.gameObject.GetComponent<PhotonView>())) throw new NullReferenceException("PhotonView component in the script is null.");
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //isDead = true;

            view.RPC("LoseThisGame", PhotonTargets.Others, null);
            Lose();
            this.enabled = false;
            
            /*GameObject[] cameras = GameObject.FindGameObjectsWithTag("MainCamera");

            foreach (var camera in cameras)
            {
                camera.camera.enabled = false;
                camera.GetComponent<GUILayer>().enabled = false;
            }

            GameObject endgGameCamera = GameObject.Find("EndGameCamera");
            endgGameCamera.camera.enabled = false;

            GameObject pauseCamera = GameObject.Find("PauseCamera");
            pauseCamera.GetComponent<PauseScript>().enabled = true;
            pauseCamera.GetComponent<Camera>().enabled = true;*/

        }

        if (this.gameObject.transform.position.y < -15.0)
        {
            view.RPC("LoseThisGame", PhotonTargets.Others, null);
            Lose(); 
            //this.enabled = false;
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (!CollisionControl.CollisionAllowed)
            return;

        //Prim salje preko mreze DarkPrim-u
        if (other.transform.parent.gameObject.tag == "DarkPrim")
        {
            isDead = true;
            view.RPC("LoseThisGame", PhotonTargets.Others, null);
            //Lose(); <-- bice pozvano iz UIPrim nakon sto se zavrsi death animacija
        }
    }

    #region Lose

    [RPC]
    void LoseThisGame()
    {
        GameObject darkPrim = GameObject.FindGameObjectWithTag("DarkPrim");
        EndGameScript otherEndScript = darkPrim.GetComponent<EndGameScript>();

        if (otherEndScript == null)
        {
            darkPrim.AddComponent("EndGameScript");
            otherEndScript = darkPrim.GetComponent<EndGameScript>();
        }

        otherEndScript.PrimWin = false;
        otherEndScript.enabled = true;
    }

    public void Lose()
    {
        EndGameScript myEndScript = this.gameObject.GetComponent<EndGameScript>();
        if (myEndScript == null)
        {
            this.gameObject.AddComponent("EndGameScript");
            myEndScript = this.gameObject.GetComponent<EndGameScript>();
        }

        myEndScript.PrimWin = false;
        myEndScript.enabled = true;
    }

    #endregion

    #region Win
    [RPC]
    void WinThisGame()
    {
        GameObject darkPrim = GameObject.FindGameObjectWithTag("DarkPrim");
        EndGameScript otherEndScript = darkPrim.GetComponent<EndGameScript>();

        if (otherEndScript == null)
        {
            darkPrim.AddComponent("EndGameScript");
            otherEndScript = darkPrim.GetComponent<EndGameScript>();
        }

        otherEndScript.PrimWin = true;
        otherEndScript.enabled = true;
    }

    void Win()
    {
        EndGameScript myEndScript = this.gameObject.GetComponent<EndGameScript>();
        if (myEndScript == null)
        {
            this.gameObject.AddComponent("EndGameScript");
            myEndScript = this.gameObject.GetComponent<EndGameScript>();
        }

        myEndScript.PrimWin = true;
        myEndScript.enabled = true;
    }

    public void WinWhenExit()
    {
        view.RPC("WinThisGame", PhotonTargets.Others, null);
        Win();
    }

    #endregion
}
