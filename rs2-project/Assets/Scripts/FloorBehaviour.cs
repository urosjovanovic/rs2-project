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
            if (other.transform.parent.gameObject.tag == "Prim")
            {
                if (!walkedOnByPrim)
                {
                    walkedOnByPrim = true;
                    PathData.Instance.PrimPath.Enqueue(this.transform);
                }
                //Debug.Log(other.gameObject.transform.position.x + " " + other.gameObject.transform.position.z);
            }
            else if (other.transform.parent.gameObject.tag == "DarkPrim")
            {
                if (!walkedOnByDarkPrim)
                {
                    walkedOnByDarkPrim = true;
                    PathData.Instance.DarkPrimPath.Enqueue(this.transform);
                }
                //Debug.Log(other.gameObject.transform.position.x + " " + other.gameObject.transform.position.z);
            }

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
