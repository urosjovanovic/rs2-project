using UnityEngine;
using System.Collections;

public class DeveloperControls : MonoBehaviour {
	
	public Transform BirdviewCamera;
	public Transform DirectLight;
	public Transform Player1Camera;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Birdview camera toggle
		if(Input.GetKeyDown(KeyCode.C))
		{
				Player1Camera.gameObject.camera.enabled = !Player1Camera.gameObject.camera.enabled;
				BirdviewCamera.gameObject.camera.enabled = !Player1Camera.gameObject.camera.enabled;
		}
		
		//Main light toggle
		if(Input.GetKeyDown(KeyCode.L))
		{
			DirectLight.gameObject.light.enabled = !DirectLight.gameObject.light.enabled;
		}
	}
}
