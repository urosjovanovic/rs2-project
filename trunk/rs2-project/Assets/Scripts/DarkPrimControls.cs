using UnityEngine;
using System.Collections;

public class DarkPrimControls : MonoBehaviour
{
		public Texture opacityMap;
		private bool nightmareVision = false;
		private GameObject[] walls;

		// Use this for initialization
		void Start ()
		{
				walls = GameObject.FindGameObjectsWithTag ("Wall");
		}
	
		// Update is called once per frame
		void Update ()
		{
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
}
