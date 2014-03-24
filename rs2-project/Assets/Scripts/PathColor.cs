using UnityEngine;
using System.Collections;

public class PathColor : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		Debug.Log("ENTER");
		if(other.gameObject.tag == "Player")
			this.gameObject.renderer.material.color = Color.red;
	}
}
