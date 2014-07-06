using UnityEngine;
using System.Collections;

public class FloorBehaviour : MonoBehaviour
{		
        public bool autoRefresh = false;
		private bool walkedOnByPrim = false;
		private bool walkedOnByDarkPrim = false;

		void OnTriggerEnter (Collider other)
		{
				//Debug.Log (other.gameObject.tag);
            if (other.gameObject.tag == "Prim")
            {
                if (!walkedOnByPrim)
                {
                    walkedOnByPrim = true;
                    PathData.Instance.PrimPath.Enqueue(this.transform);
                }
                //Debug.Log(other.gameObject.transform.position.x + " " + other.gameObject.transform.position.z);
            }
            else if (other.gameObject.tag == "DarkPrim")
            {
                if (!walkedOnByDarkPrim)
                {
                    walkedOnByDarkPrim = true;
                    PathData.Instance.DarkPrimPath.Enqueue(this.transform);
                }
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
                Colorize(Color.red);
            if (walkedOnByDarkPrim)
                Colorize(Color.black);
		}

        public void Colorize(Color c)
        {
            this.gameObject.renderer.material.color = c;
        }

		public void ResetColor ()
		{
				this.gameObject.renderer.material.color = Color.white;
		}
}
