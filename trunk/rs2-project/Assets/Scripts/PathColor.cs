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
						walkedOnByPrim = true;
				else if (other.gameObject.tag == "DarkPrim")
						walkedOnByDarkPrim = true;
				else
						Debug.Log ("There is someone here... He is not from this world...");

				if (autoRefresh)
						Colorize ();
		}

		public void Colorize ()
		{
				if (walkedOnByPrim)
						this.gameObject.renderer.material.color = Color.gray;
				if (walkedOnByDarkPrim)
						this.gameObject.renderer.material.color = Color.red;
		}

		public void ResetColor ()
		{
				this.gameObject.renderer.material.color = Color.white;
		}
}
