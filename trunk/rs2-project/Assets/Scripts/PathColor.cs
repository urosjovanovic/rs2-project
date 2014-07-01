using UnityEngine;
using System.Collections;

public class PathColor : MonoBehaviour
{
		public bool autoRefresh = false;
		private bool walkedOnByPrim = false;
		private bool walkedOnByDarkPrim = false;

		void OnTriggerEnter (Collider other)
		{
				//Debug.Log (other.gameObject.tag);
            if (other.gameObject.tag == "Prim")
            {
                walkedOnByPrim = true;
                //Debug.Log(other.gameObject.transform.position.x + " " + other.gameObject.transform.position.z);
            }
            else if (other.gameObject.tag == "DarkPrim")
            {
                walkedOnByDarkPrim = true;
                //Debug.Log(other.gameObject.transform.position.x + " " + other.gameObject.transform.position.z);
            }
            else
                Debug.Log("There is someone here... He is not from this world...");

				if (autoRefresh)
						Colorize ();
		}

		public void Colorize ()
		{
				if (walkedOnByPrim)
						this.gameObject.renderer.material.color = Color.red;
				if (walkedOnByDarkPrim)
						this.gameObject.renderer.material.color = Color.black;
		}

		public void ResetColor ()
		{
				this.gameObject.renderer.material.color = Color.white;
		}
}
