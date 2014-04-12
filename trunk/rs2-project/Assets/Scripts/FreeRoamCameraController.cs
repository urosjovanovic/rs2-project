using UnityEngine;
using System.Collections;

public class FreeRoamCameraController : MonoBehaviour
{
		private float speed = 1.5f;

		// Use this for initialization
		void Start ()
		{

	
		}
	
		// Update is called once per frame
		void Update ()
		{
				speed += Input.GetAxis ("Mouse ScrollWheel") * 5;
				if (speed < 0)
						speed = 0;
				float xAxisValue = Input.GetAxis ("Horizontal");
				float zAxisValue = Input.GetAxis ("Vertical");
				transform.Translate (new Vector3 (xAxisValue * speed * Time.deltaTime, 0.0f, zAxisValue * speed * Time.deltaTime));
		}
}
