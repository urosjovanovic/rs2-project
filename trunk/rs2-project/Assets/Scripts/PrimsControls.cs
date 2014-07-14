using UnityEngine;
using System.Collections;
using System;

public class PrimsControls : MonoBehaviour
{

    #region Class fields
    public Transform flashlight;
    private FlashlightBehaviour fb;
    UnityEngine.Object pauseObject;

    private bool isPause = false;
    public bool IsPause
    {
        get
        {
            return isPause;
        }

        set
        {
            isPause = value;
        }
    }

    private int markerCount = 3;

    bool flashlightOn = false;

    public int MarkerCount
    {
        get { return markerCount;  }
        set { markerCount = value; }
    }

    #endregion

    void Start()
    {
        if(!(fb = flashlight.GetComponent<FlashlightBehaviour>()))  throw new NullReferenceException("FlashlightBehaviour component in PrimsControls script is null!");

        pauseObject = Resources.Load("PauseCamera");
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
                    marker.transform.position = new Vector3(marker.transform.position.x, -0.495f, marker.transform.position.z);
                    marker.transform.Rotate(new Vector3(90, this.transform.localEulerAngles.y, 0));
                }
            }
            if(!ConfigManager.infiniteMarkers)
            {
                markerCount--;
                markerCount = Mathf.Clamp(markerCount, 0, 6);
            }
        }

        //activate PauseMenu
        if (!IsPause && Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = true;

            GameObject[] cameras = GameObject.FindGameObjectsWithTag("MainCamera");
            GameObject myCamera = null;

            foreach (var camera in cameras)
            {
                if (camera.transform.parent.gameObject.tag == "Prim")
                    myCamera = camera;

                camera.camera.enabled = false;
                camera.GetComponent<GUILayer>().enabled = false;
            }

            this.gameObject.GetComponent<CharacterMotor>().canControl = false;
            this.gameObject.GetComponent<PrimsControls>().enabled = false;
            this.gameObject.GetComponentInChildren<UIPrim>().enabled = false;

            GameObject endgGameCamera = GameObject.Find("EndGameCamera");
            endgGameCamera.camera.enabled = false;

            GameObject pauseCamera = (GameObject)Instantiate(pauseObject);
            pauseCamera.GetComponent<PauseScript>().calledByPrim = true;
            /*if (myCamera != null)
            {
                pauseCamera.transform.position = myCamera.transform.position;
                pauseCamera.transform.rotation = myCamera.transform.rotation;
            }*/
        }
	}
}
