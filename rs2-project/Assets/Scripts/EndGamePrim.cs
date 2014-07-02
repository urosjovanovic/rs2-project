using UnityEngine;
using System.Collections;
using System;

public class EndGamePrim : MonoBehaviour 
{
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
            view.RPC("LoseThisGame", PhotonTargets.Others, null);
            Lose();
            //this.enabled = false;
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
        //Prim salje preko mreze DarkPrim-u
        if (other.transform.parent.gameObject.tag == "DarkPrim")
        {
            view.RPC("LoseThisGame", PhotonTargets.Others, null);
            Lose();
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

    void Lose()
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
