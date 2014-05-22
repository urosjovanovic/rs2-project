using UnityEngine;
using System.Collections;

public class DarkPrimControls : MonoBehaviour
{
		public Texture opacityMap;
		private bool nightmareVision = false;
		private GameObject[] walls;
		private GameObject[] footsteps;

		// Use this for initialization
		void Start ()
		{
				walls = GameObject.FindGameObjectsWithTag ("Wall");
		}
	
		// Update is called once per frame
		void Update ()
		{
				footsteps = GameObject.FindGameObjectsWithTag ("Footstep");
				if (nightmareVision) {
						foreach (var footstep in footsteps) {
								footstep.GetComponent<MeshRenderer> ().enabled = true;
						}
				} else if (!ConfigManager.alwaysShowFootsteps) {
						foreach (var footstep in footsteps) {
								footstep.GetComponent<MeshRenderer> ().enabled = false;
						}
				}
		
				if (Input.GetKeyDown (KeyCode.E)) {
						if (!nightmareVision) {
								foreach (var wall in walls) {
										wall.renderer.material.shader = Shader.Find ("Transparent/Diffuse");
										wall.renderer.material.color = new Color (1, 0, 0, 0.2f);
								}
								nightmareVision = true;
						} else {
								foreach (var wall in walls) {
										wall.renderer.material.shader = Shader.Find ("Diffuse");
										wall.renderer.material.color = new Color (1, 1, 1, 1);
								}
								nightmareVision = false;
						}
				}
		}

		void OnTriggerEnter (Collider other)
		{
				if (other.transform.parent.gameObject.tag == "Prim") {
						var distance = Vector3.Distance (this.transform.position, other.transform.parent.gameObject.transform.position);
						Debug.Log ("DarkPrim: End Game " + distance + " " + System.DateTime.Now);
                        Application.LoadLevel("GameOver");
				}				
		}

		void OnTriggerStay (Collider other)
		{
			
		}

		void OnTriggerExit (Collider other)
		{
			
		}

}
