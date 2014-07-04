﻿using UnityEngine;
using System.Collections;

public class DarkPrimControls : MonoBehaviour
{
		public Texture opacityMap;
		public bool nightmareVision = false;
		private GameObject[] walls;
		private GameObject[] footsteps;
        private CharacterMotor motor;
        private float[] speed;

		// Use this for initialization
		void Start ()
		{
				walls = GameObject.FindGameObjectsWithTag ("Wall");
                motor = this.GetComponent<CharacterMotor>();
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

            SlowDown();

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

            RestoreSpeed();

            nightmareVision = false;
        }

        private void SlowDown()
        {
            speed = new float[]{motor.movement.maxForwardSpeed, motor.movement.maxSidewaysSpeed, motor.movement.maxBackwardsSpeed};
            motor.movement.maxForwardSpeed = 0.5f;
            motor.movement.maxSidewaysSpeed = 0.5f;
            motor.movement.maxBackwardsSpeed = 0.5f;
        }

        private void RestoreSpeed()
        {
            motor.movement.maxForwardSpeed = speed[0];
            motor.movement.maxSidewaysSpeed = speed[1];
            motor.movement.maxBackwardsSpeed = speed[2];
        }

}
