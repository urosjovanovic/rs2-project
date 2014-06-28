using UnityEngine;
using System.Collections;
using System;

public class PrimsControls : MonoBehaviour {

	public Transform flashlight;
    private FlashlightBehaviour fb;

    private int markerCount = 3;

    bool flashlightOn = false;

    public int MarkerCount
    {
        get { return markerCount;  }
        set { markerCount = value; }
    }

    void Start()
    {
        if(!(fb = flashlight.GetComponent<FlashlightBehaviour>()))  throw new NullReferenceException("FlashlightBehaviour component in PrimsControls script is null!");
    }
	
	// Update is called once per frame
	void Update () {
	
        // F toggles the flashlight
		if(Input.GetKeyDown(KeyCode.F) && flashlight.gameObject.GetComponent<FlashlightRecharge>().flashLightEnabled)
		{
            flashlight.audio.PlayOneShot(SoundPool.FlashlightClick);
            
            if (fb.FlashlightIsOn)
            {
                fb.TurnFlashlightOff();
            }
            else
            {
                fb.TurnFlashlightOn();
            }
		}

        // T draws a marker
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

        // Escape ends the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject.Find("_SCRIPTS").GetComponent<EndGameScript>().enabled = true;
        }


	}

	void OnTriggerEnter (Collider other)
	{
		if (other.transform.parent.gameObject.tag == "DarkPrim") 
		{
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
