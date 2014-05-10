using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateFootsteps : MonoBehaviour
{
		public Material footLeft;
		public Material footRight;
		public float foostepLifeTime = 2.0f; //u sekundama

		private bool lastWasLeft = false;

		// Use this for initialization
		void Start ()
		{
				StartCoroutine (generateFootsteps ());			
		}
	
		// Update is called once per frame
		void Update ()
		{
		}

		IEnumerator generateFootsteps ()
		{
				CharacterController controller = GetComponent<CharacterController> ();
				while (true) {
						yield return new WaitForSeconds (0.25f);
						if (controller.velocity.magnitude > 0.02) {
								GameObject footstep = generateFootstep ();
						}
				}
		}

		private GameObject generateFootstep ()
		{
				GameObject quad = GameObject.CreatePrimitive (PrimitiveType.Quad);
				quad.GetComponent<MeshCollider> ().enabled = false;
				quad.transform.position = this.transform.position;
				quad.transform.Rotate (new Vector3 (90, this.transform.rotation.eulerAngles.y, 0));
				//TODO: Namestiti malo bolje lociranje po Y osi
				quad.transform.position = new Vector3 (quad.transform.position.x, -0.499f, quad.transform.position.z);
				quad.transform.localScale = new Vector3 (0.25f, 0.25f, 0.25f);
				if (!lastWasLeft) {
						quad.renderer.material = footLeft;
						lastWasLeft = true;
				} else {
						quad.renderer.material = footRight;
						lastWasLeft = false;
				}

				quad.AddComponent ("FootstepBehaviour");
				quad.GetComponent<FootstepBehaviour> ().lifetime = foostepLifeTime;

				return quad;
		}
}
