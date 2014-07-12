using UnityEngine;
using System.Collections;

public class MarkerBehaviour : MonoBehaviour 
{
    void OnTriggerEnter(Collider other)
    {
        if(other.transform.parent.gameObject.tag == "Prim")
        {
            other.gameObject.transform.parent.gameObject.GetComponent<PrimsControls>().MarkerCount++;
            other.gameObject.transform.parent.gameObject.audio.PlayOneShot(SoundPool.pickupSound);

            GameObject.Destroy(this.transform.parent.gameObject);
        }
    }

}
