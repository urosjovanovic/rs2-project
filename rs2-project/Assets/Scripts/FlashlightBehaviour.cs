using UnityEngine;
using System.Collections;
using System;

public class FlashlightBehaviour : MonoBehaviour
{
		public Transform parent;
		public float lag = 5.0f;

		private Transform camera;

        public float blinkCooldown = 0.5f;
        private float currentCooldown;
        public float blinkDuration = 0.005f;

        private double blinkProbability = 0.005;

        private float cutoffDistance = 5;

        public int probabilityMagicNumber = 3;

        public static bool FlashlightHardDisabled = false;
        System.Random rand = new System.Random();

        Light flashlight;
		// Use this for initialization
		void Start ()
        {
            camera = parent.FindChild ("MainCamera");
				if (!camera) 
						Debug.Log ("FlashlightBehaviour: No camera object found on parent.");

            currentCooldown = blinkCooldown;
            flashlight = this.GetComponent<Light>();

            if (!flashlight)
            {
                throw new Exception("Flashlight is null!");
            }
        }
	
		// Update is called once per frame
		void Update ()
		{		
				//follow parent
				this.transform.position = parent.transform.position;

				//sway with camera
				this.transform.rotation = Quaternion.Slerp (this.transform.rotation, camera.transform.rotation, Time.deltaTime * lag);

                if (currentCooldown >= 0)
                {
                    currentCooldown -= Time.deltaTime;
                }
                else
                {
					if (flashlight.enabled && flashlight.gameObject.GetComponent<FlashlightRecharge>().flashLightEnabled)
                    {
                        var darkPrim = GameObject.FindGameObjectWithTag("DarkPrim");

                        if (darkPrim)
                        {
                            float currentDistance = Vector3.Distance(this.transform.position, darkPrim.transform.position);
                            

                            blinkProbability = Math.Round(cutoffDistance / (currentDistance*probabilityMagicNumber), 2);

                            double r = ((double)rand.Next(100)) / 100;

                            if (r <= blinkProbability)
                            {
                                StartCoroutine(FlashlightBlink());
                            }

                            currentCooldown = blinkCooldown;
                        }
                    }
                }
		}

        public IEnumerator FlashlightBlink()
        {
            yield return StartCoroutine(FlashlightOff());

            FlashlightOn();
        }

        IEnumerator FlashlightOff()
        {
            flashlight.enabled = false;
            yield return new WaitForSeconds(blinkDuration);  
        }

        void FlashlightOn()
        {
            flashlight.enabled = true;
        }
}
