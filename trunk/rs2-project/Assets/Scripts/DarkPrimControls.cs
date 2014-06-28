using UnityEngine;
using System.Collections;

public class DarkPrimControls : MonoBehaviour
{
		public Texture opacityMap;
		public bool nightmareVision = false;
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
		
				if (Input.GetKeyDown (KeyCode.E) && GetComponent<LimitVision>().visionEnabled) 
                {
                    if (!nightmareVision)
                        EnableVision();
                    else
                        DisableVision();
				}
                
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    GameObject.Find("_SCRIPTS").GetComponent<EndGameScript>().enabled = true;
                }
		}

		void OnTriggerEnter (Collider other)
		{
				if (other.transform.parent.gameObject.tag == "Prim") 
                {
                        GameObject.Find("_SCRIPTS").GetComponent<EndGameScript>().enabled = true;
				}				
		}

        public void EnableVision()
        {
            foreach (var wall in walls)
            {
                wall.renderer.material.shader = Shader.Find("Transparent/Diffuse");
                wall.renderer.material.color = new Color(1, 0, 0, 0.2f);
            }


            GameObject prim = GameObject.FindGameObjectWithTag("Prim");

            if(prim!=null)
            {
                prim = prim.transform.FindChild("Graphics").gameObject;
                (prim.GetComponent("Halo") as Behaviour).enabled = true;
            }        

            nightmareVision = true;
        }

        public void DisableVision()
        {
            foreach (var wall in walls)
            {
                wall.renderer.material.shader = Shader.Find("Diffuse");
                wall.renderer.material.color = Color.gray;

                GameObject prim = GameObject.FindGameObjectWithTag("Prim");

                if (prim != null)
                {
                    prim = prim.transform.FindChild("Graphics").gameObject;
                    (prim.GetComponent("Halo") as Behaviour).enabled = false;
                }  

            }

            nightmareVision = false;
        }

}
