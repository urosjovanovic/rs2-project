using UnityEngine;
using System.Collections;

public class PrimsControls : MonoBehaviour {

	public Transform flashlight;
    private int markerCount = 3;


	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKeyDown(KeyCode.F))
		{
			flashlight.gameObject.light.enabled = !flashlight.gameObject.light.enabled;
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

    void OnGUI()
    {
        GUI.TextField(new Rect(Screen.width-150, Screen.height-25, 300, 50), markerCount.ToString());
    }

	void OnTriggerEnter (Collider other)
	{
		if (other.transform.parent.gameObject.tag == "DarkPrim") 
		{
			var distance = Vector3.Distance(this.transform.position, other.transform.parent.gameObject.transform.position);
			Debug.Log ("Prim: End Game " + distance + " " + System.DateTime.Now);
		}				
	}

	void OnTriggerStay (Collider other)
	{

	}

	void OnTriggerExit (Collider other)
	{

	}
}
