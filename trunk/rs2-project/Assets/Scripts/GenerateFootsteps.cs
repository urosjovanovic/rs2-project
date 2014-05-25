using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateFootsteps : MonoBehaviour
{
		public Material footLeft;
		public Material footRight;

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
								generateFootstep ();
						}
				}
		}

		private void generateFootstep ()
        {
            #region 2D footsteps
            string nextFoot;
				if (!lastWasLeft) {
						nextFoot = "FootstepLeft";
						lastWasLeft = true;
				} else {
						nextFoot = "FootstepRight";
						lastWasLeft = false;
				}

				GameObject footstep = PhotonNetwork.Instantiate (nextFoot, this.transform.position, Quaternion.identity, 0);
				footstep.transform.Rotate (new Vector3 (90, this.transform.rotation.eulerAngles.y, 0));
				//TODO: Namestiti malo bolje lociranje po Y osi
				footstep.transform.position = new Vector3 (footstep.transform.position.x, -0.498f, footstep.transform.position.z);
				footstep.transform.localScale = new Vector3 (0.22f, 0.22f, 0.22f);
            #endregion

                #region 3D footsteps(not finished)
                /*GameObject footstep = PhotonNetwork.Instantiate("FootstepBeam", this.transform.position, Quaternion.identity, 0);
                footstep.transform.Rotate(new Vector3(-90, this.transform.localEulerAngles.y, 0));
                //TODO: Namestiti malo bolje lociranje po Y osi
                if (lastWasLeft)
                {
                    footstep.transform.localPosition = new Vector3(footstep.transform.localPosition.x + 0.1f, -0.499f, footstep.transform.localPosition.z);
                    footstep.transform.localScale = new Vector3(-0.1f, -0.1f, 0.1f);
                    lastWasLeft = false;
                }
                else
                {
                    footstep.transform.localPosition = new Vector3(footstep.transform.localPosition.x - 0.1f, -0.499f, footstep.transform.localPosition.z);
                    footstep.transform.localScale = new Vector3(0.1f, -0.1f, 0.1f);
                    lastWasLeft = true;
                }*/
                #endregion

                if (ConfigManager.alwaysShowFootsteps)
						footstep.GetComponent<MeshRenderer> ().enabled = true;

				footstep.GetComponent<FootstepBehaviour> ().isOwner = true;
		}
}
