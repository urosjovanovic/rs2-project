using UnityEngine;
using System.Collections;

public class MarkerBehaviour : MonoBehaviour 
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Prim")
        {
            other.gameObject.transform.parent.gameObject.GetComponent<PrimsControls>().MarkerCount++;
            //play a sound
            GameObject.Destroy(this.transform.parent.gameObject);
        }
    }
}
