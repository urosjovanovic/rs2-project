using UnityEngine;
using System.Collections;

public class FlashlightBehaviour : MonoBehaviour
{
		public Transform parent;
		public float lag = 5.0f;
		public float fireRate = 0.5f;	
		float coolDown = 0;

		private Transform camera;

		// Use this for initialization
		void Start ()
		{
				camera = parent.FindChild ("MainCamera");
				if (!camera) 
						Debug.Log ("FlashlightBehaviour: No camera object found on parent.");
				
		}
	
		// Update is called once per frame
		void Update ()
		{		
				//follow parent
				this.transform.position = parent.transform.position;

				//sway with camera
				this.transform.rotation = Quaternion.Slerp (this.transform.rotation, camera.transform.rotation, Time.deltaTime * lag);

				
				//flashlight rays
				if (this.gameObject.light.enabled) 
				{
					coolDown -= Time.deltaTime;
					Fire ();
				}
		}

		void Fire()
		{
				if (coolDown > 0)
						return;

				var fwd = transform.TransformDirection (Vector3.forward);
				RaycastHit hit;
			
				if (Physics.Raycast (transform.position, fwd, out hit, 3)) 
				{
						if (hit.rigidbody!=null && hit.rigidbody.gameObject.tag == "DarkPrim")
						{
							Debug.Log ("There is something in front of the object!");
							//hit.rigidbody.gameObject.GetComponent<CharacterController>().enabled=false;
						}
				}
				
				coolDown = fireRate;
		}
}
