using UnityEngine;
using System.Collections;

public class DeveloperControls : MonoBehaviour
{
	
		public Transform BirdviewCamera;
		public Transform DirectLight;
		public Transform Player1Camera;

		private bool pathColoring = false;

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

				//Colorize path
				if (Input.GetKeyDown (KeyCode.P)) {
						GameObject[] floors = GameObject.FindGameObjectsWithTag ("Floor");

						if (!pathColoring) {
								foreach (var floor in floors) {
										floor.GetComponent<PathColor> ().Colorize ();
										floor.GetComponent<PathColor> ().autoRefresh = true;
								}
								pathColoring = true;
						} else {
								foreach (var floor in floors) {
										floor.GetComponent<PathColor> ().ResetColor ();
										floor.GetComponent<PathColor> ().autoRefresh = false;
								}
								pathColoring = false;
						}
				}
		}
}
