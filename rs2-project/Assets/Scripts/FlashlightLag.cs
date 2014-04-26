using UnityEngine;
using System.Collections;

public class FlashlightLag : MonoBehaviour
{
		public Transform Camera;
		
		private Quaternion previousRotation;

		// Use this for initialization
		void Start ()
		{
		}
	
		// Update is called once per frame
		void Update ()
		{				
				this.transform.rotation = Quaternion.Slerp (this.transform.rotation, Camera.transform.rotation, Time.deltaTime * 5.0f);
		}
}
