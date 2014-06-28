using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FlashlightBehaviour : MonoBehaviour
{

        public float flashlightLag = 5.0f;
      
        #region Components

        private Transform camera;
        public Transform parent;
        Light flashlight;
        FlashlightRecharge flashlightRecharge;

        #endregion

        #region Variables for blinking

        private double blinkProbability;
        public float blinkCutoffDistance;
        public int probabilityMagicNumber;
        
        public float blinkCooldown;
        private float currentBlinkCooldown;
        public float blinkDuration;

        System.Random rand = new System.Random();

        private bool isBlinking;
        #endregion

        #region Cookie stuff
        public enum FlashlightCookieMode { FC_NORMAL = 0, FC_RUN, FC_WAKEUP, FC_HEISCOMING };

        public Texture2D[] cookies;

        #endregion

        #region Flashlight controls
        public bool FlashlightIsOn;

        public void TurnFlashlightOff()
        {
            FlashlightIsOn = false;
        }

        public void TurnFlashlightOn()
        {
            FlashlightIsOn = true;
        }
        #endregion

		void Start ()
        {
            if (!(camera = parent.FindChild ("MainCamera"))) throw new NullReferenceException("MainCamera in FlashlightBehaviour script is null!");
            if (!(flashlight = this.GetComponent<Light>())) throw new NullReferenceException("flashlight in FlashliightBehaviour script is null!");
            if (!(flashlightRecharge = flashlight.gameObject.GetComponent<FlashlightRecharge>())) throw new NullReferenceException("flaslightRecharge component in FlashlightBehaviour script is null!");

            currentBlinkCooldown = blinkCooldown;
            FlashlightIsOn = false;
            isBlinking = false;
        }
	
		// Update is called once per frame
		void Update ()
		{
                // update the flashlight state
                if(!isBlinking) flashlight.enabled = FlashlightIsOn;

				//follow parent
				this.transform.position = parent.transform.position;

				//sway with camera
				this.transform.rotation = Quaternion.Slerp (this.transform.rotation, camera.transform.rotation, Time.deltaTime * flashlightLag);

                if (currentBlinkCooldown >= 0)
                {
                    currentBlinkCooldown -= Time.deltaTime;
                }
                else
                {
					if (FlashlightIsOn)
                    {
                        var darkPrim = GameObject.FindGameObjectWithTag("DarkPrim");

                        if (darkPrim)
                        {
                            float currentDistance = Vector3.Distance(this.transform.position, darkPrim.transform.position);

                            if (currentDistance < blinkCutoffDistance)
                            {
                                blinkProbability = 1;
                            }
                            else
                            {
                                blinkProbability = Math.Round(blinkCutoffDistance / (currentDistance * probabilityMagicNumber), 3);
                            }

                            double r = ((double)rand.Next(1000)) / 1000;

                            if (r <= blinkProbability && FlashlightIsOn)
                            {
                                StartCoroutine(FlashlightBlink());
                                currentBlinkCooldown = blinkCooldown;
                            }

                        }
                    }
                }
		}

        public IEnumerator FlashlightBlink()
        {
            isBlinking = true;
            SwitchCookie(FlashlightCookieMode.FC_RUN);

            this.audio.PlayOneShot(SoundPool.FlashlightBuzz);

            for (int i = 0; i < (int)(blinkCooldown/blinkDuration); i++)
            {
                yield return StartCoroutine(FlashlightOff());
            }

            if (FlashlightIsOn)
                FlashlightOn();

            SwitchCookie(FlashlightCookieMode.FC_RUN);
            isBlinking = false;
        }

        IEnumerator FlashlightOff()
        {
            if (FlashlightIsOn)
            {
                flashlight.enabled = !flashlight.enabled;
                yield return new WaitForSeconds(blinkDuration);
            }
        }

        void FlashlightOn()
        {
            flashlight.enabled = true;
        }

        public void SwitchCookie(FlashlightCookieMode mode)
        {
            this.light.cookie = cookies[(int)mode];
        }
}
