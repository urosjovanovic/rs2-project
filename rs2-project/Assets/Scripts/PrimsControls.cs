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
