using UnityEngine;
using System.Collections;

public class FlashlightBehaviour : MonoBehaviour
{
		public Transform parent;
		public float lag = 5.0f;
		
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
		}
}
