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
                if(nightmareVision)
                {
                    foreach (var footstep in footsteps)
                    {
                        footstep.GetComponent<MeshRenderer>().enabled = true;
                    }
                }
                else
                {
                    foreach (var footstep in footsteps)
                    {
                       if(!ConfigManager.alwaysShowFootsteps)
                          footstep.GetComponent<MeshRenderer>().enabled = false;
                       else
                           footstep.GetComponent<MeshRenderer>().enabled = true;
                    }
                }
		
				if (Input.GetKeyDown (KeyCode.E)) {
						if (!nightmareVision) {
								foreach (var wall in walls) {
										wall.renderer.material.shader = Shader.Find ("Transparent/Diffuse");
										wall.renderer.material.color = new Color (1, 0, 0, 0.2f);
								}

                                GameObject prim = GameObject.FindGameObjectWithTag("Prim").transform.FindChild("Graphics").gameObject;
                                (prim.GetComponent("Halo") as Behaviour).enabled = true;

								nightmareVision = true;
						} else {
								    foreach (var wall in walls) {
										    wall.renderer.material.shader = Shader.Find ("Diffuse");
                                            wall.renderer.material.color = Color.gray;

                                    GameObject prim = GameObject.FindGameObjectWithTag("Prim").transform.FindChild("Graphics").gameObject;
                                    (prim.GetComponent("Halo") as Behaviour).enabled = false;

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
				}				
		}

		void OnTriggerStay (Collider other)
		{
			
		}

		void OnTriggerExit (Collider other)
		{
			
		}

}
