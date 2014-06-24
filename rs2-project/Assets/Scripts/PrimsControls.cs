using UnityEngine;
using System.Collections;

public class PrimsControls : MonoBehaviour {

	public Transform flashlight;
    private int markerCount = 3;

    public int MarkerCount
    {
        get { return markerCount;  }
        set { markerCount = value; }
    }


	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKeyDown(KeyCode.F) && flashlight.gameObject.GetComponent<FlashlightRecharge>().flashLightEnabled)
		{
			flashlight.gameObject.light.enabled = !flashlight.gameObject.light.enabled;
            flashlight.audio.PlayOneShot(SoundPool.FlashlightClick);
		}

        if(Input.GetKeyDown(KeyCode.T))
        {
            if (markerCount > 0)
            {
                GameObject marker = PhotonNetwork.Instantiate("Marker", this.transform.position, Quaternion.identity, 0);
                if (marker)
                {
                    marker.transform.position = new Vector3(marker.transform.position.x, -0.499f, marker.transform.position.z);
                    marker.transform.Rotate(new Vector3(90, this.transform.localEulerAngles.y, 0));
                }
            }
            if(!ConfigManager.infiniteMarkers)
            {
                markerCount--;
                markerCount = Mathf.Clamp(markerCount, 0, 6);
            }
        }


	}

	void OnTriggerEnter (Collider other)
	{
		if (other.transform.parent.gameObject.tag == "DarkPrim") 
		{
			var distance = Vector3.Distance(this.transform.position, other.transform.parent.gameObject.transform.position);
			Debug.Log ("Prim: End Game " + distance + " " + System.DateTime.Now);
            GameObject.Find("_SCRIPTS").GetComponent<EndGameScript>().enabled = true;
		}				
	}

	void OnTriggerStay (Collider other)
	{

	}

	void OnTriggerExit (Collider other)
	{

	}
}
