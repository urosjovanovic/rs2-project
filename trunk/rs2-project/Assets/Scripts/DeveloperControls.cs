using UnityEngine;
using System.Collections;

public class DeveloperControls : MonoBehaviour
{
	
		public Transform BirdviewCamera;
		public Transform DirectLight;
		public Transform Player1Camera;
		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
				//Birdview camera toggle (not needed really... use Scene overview)
				/*if (Input.GetKeyDown (KeyCode.C)) {
						BirdviewCamera.gameObject.camera.enabled = !BirdviewCamera.gameObject.camera.enabled;
				}*/
		
				//Main light toggle
				if (Input.GetKeyDown (KeyCode.L)) {
						DirectLight.gameObject.light.enabled = !DirectLight.gameObject.light.enabled;
				}
		}
}
