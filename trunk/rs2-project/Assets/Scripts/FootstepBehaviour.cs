using UnityEngine;
using System.Collections;

public class FootstepBehaviour : MonoBehaviour
{
		public bool isOwner = false; //check if current client is the owner of this object
		public float lifetime = 2.0f;
		public float fadeoutTime = 2.0f;

		// Use this for initialization
		void Start ()
		{
				if (lifetime < fadeoutTime)
						fadeoutTime = lifetime;
				StartCoroutine (lifetimeCounter ());
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		IEnumerator lifetimeCounter ()
		{
				while (true) {
						yield return new WaitForSeconds (lifetime);
						StartCoroutine (fadeOut ());
				}
				
		}

		IEnumerator fadeOut ()
		{
				while (true) {
						float step = fadeoutTime / 100.0f;
						yield return new WaitForSeconds (step);
						Color c = this.gameObject.renderer.material.color;
						this.gameObject.renderer.material.color = new Color (c.r, c.g, c.b, c.a - step);
						if (this.gameObject.renderer.material.color.a <= 0 && isOwner)
								PhotonNetwork.Destroy (this.gameObject); //objekat ce biti unisten na svim klijentima
				}
		}
}
