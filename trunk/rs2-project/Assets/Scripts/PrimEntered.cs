using UnityEngine;
using System.Collections;

public class PrimEntered : MonoBehaviour 
{

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.gameObject.tag == "Prim")
        {
            
            EndGamePrim script = other.transform.parent.gameObject.GetComponent<EndGamePrim>();

            if (script != null)
                script.WinWhenExit();
        }
    }
}
