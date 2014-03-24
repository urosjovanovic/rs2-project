using UnityEngine;
using System.Collections;

public class PrimsControls : MonoBehaviour {

	public Transform flashlight;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKeyDown(KeyCode.F))
		{
			flashlight.gameObject.light.enabled = !flashlight.gameObject.light.enabled;
		}
	}
}
