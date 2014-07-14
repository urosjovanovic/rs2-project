using UnityEngine;
using System.Collections;
using System;

public class EndGameDarkPrim : MonoBehaviour
{
    #region Class fields
    private PhotonView view;

    private bool canExitGame = false;
    public bool CanExitGame
    {
        get
        {
            return canExitGame;
        }

        set
        {
            canExitGame = value;
        }
    }
    #endregion

    void Start()
    {
        if (!(view = this.gameObject.GetComponent<PhotonView>())) throw new NullReferenceException("PhotonView component in the script is null.");

        
    }

    // Update is called once per frame
    void Update()
    {
        if (CanExitGame)
        {
            view.RPC("LoseThisGame", PhotonTargets.Others, null);
            Lose();
            this.enabled = false;
        }

        if (this.gameObject.transform.position.y < -15.0)
        {
            view.RPC("LoseThisGame", PhotonTargets.Others, null);
            Lose();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("PRE TRIGGER");
        //Prim salje preko mreze DarkPrim-u
        if (other.transform.parent.gameObject.tag == "Prim")
        {
            Debug.Log("TRIGGER");
            //view.RPC("WinThisGame", PhotonTargets.Others, null);
            //Win();
        }
    }

    #region Lose

    [RPC]
    void LoseThisGame()
    {
        GameObject prim = GameObject.FindGameObjectWithTag("Prim");
        EndGameScript otherEndScript = prim.GetComponent<EndGameScript>();

        if (otherEndScript == null)
        {
            prim.AddComponent("EndGameScript");
            otherEndScript = prim.GetComponent<EndGameScript>();
        }

        otherEndScript.PrimWin = true;
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

        myEndScript.PrimWin = true;
        myEndScript.enabled = true;
    }

    #endregion

    #region Win
    [RPC]
    void WinThisGame()
    {
        GameObject prim = GameObject.FindGameObjectWithTag("Prim");
        EndGameScript otherEndScript = prim.GetComponent<EndGameScript>();

        if (otherEndScript == null)
        {
            prim.AddComponent("EndGameScript");
            otherEndScript = prim.GetComponent<EndGameScript>();
        }

        otherEndScript.PrimWin = false;
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

        myEndScript.PrimWin = false;
        myEndScript.enabled = true;
    }

    #endregion
}
