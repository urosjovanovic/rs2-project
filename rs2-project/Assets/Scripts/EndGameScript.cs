using UnityEngine;
using System.Collections;

public class EndGameScript : Photon.MonoBehaviour
{
		public bool PrimWin;
		// Use this for initialization
		void Start ()
		{
				GameObject[] cameras = GameObject.FindGameObjectsWithTag ("MainCamera");
                foreach (var camera in cameras)
                {
                    camera.camera.enabled = false;
                    camera.audio.Stop();
                    camera.audio.PlayOneShot(SoundPool.EndGameTheme);
                    camera.GetComponent<GUILayer>().enabled = false;
                }

                DisablePlayers();
				
                GameObject.Find ("EndGameLight").light.enabled = true;
                
                //GameObject[] walls = GameObject.FindGameObjectsWithTag ("Wall");
                //foreach (var wall in walls) {
                //        wall.renderer.material.color = Color.grey;
                //}
        
				//GameObject[] floors = GameObject.FindGameObjectsWithTag("Floor");
				//foreach(var floor in floors)
				//{
				//    floor.renderer.material.color = Color.white;
				//    //floor.GetComponent<PathColor>().enabled = true;
				//    floor.GetComponent<FloorBehaviour>().Colorize();
				//}

				AnimatePaths ();

                GameObject.Find("EndGameCamera").GetComponent<Animator>().enabled = true;

				if (PrimWin) {
						if (this.gameObject.tag == "Prim")
								GameObject.Find ("Win").GetComponent<MeshRenderer> ().enabled = true;
						else if (this.gameObject.tag == "DarkPrim")
								GameObject.Find ("Lose").GetComponent<MeshRenderer> ().enabled = true;
				} else {
						if (this.gameObject.tag == "DarkPrim")
								GameObject.Find ("Win").GetComponent<MeshRenderer> ().enabled = true;
						else if (this.gameObject.tag == "Prim")
								GameObject.Find ("Lose").GetComponent<MeshRenderer> ().enabled = true;
				}
		}
	
		// Update is called once per frame
		void Update ()
		{
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PhotonNetwork.Disconnect();
            }
		}
        
        void OnDisconnectedFromPhoton()
        {
            Application.LoadLevel("MainMenu");
        }

		private void AnimatePaths ()
		{
				StartCoroutine (AnimatePrimPath ());
				StartCoroutine (AnimateDarkPrimPath ());
		}

		private IEnumerator AnimatePrimPath ()
		{
				foreach (var block in PathData.Instance.PrimPath) {
                    if (block != null)
                    {
                        block.GetComponent<FloorBehaviour>().Colorize(Color.red);
                        yield return new WaitForSeconds(0.1f);
                    }
				}
		}

		private IEnumerator AnimateDarkPrimPath ()
		{
				foreach (var block in PathData.Instance.DarkPrimPath) {
                    if (block != null)
                    {
                        block.GetComponent<FloorBehaviour>().Colorize(Color.black);
                        yield return new WaitForSeconds(0.1f);
                    }
				}
		}

        private void DisablePlayers()
        {
            try
            {
                GameObject prim = GameObject.FindGameObjectWithTag("Prim");
                prim.GetComponent<CharacterMotor>().canControl = false;
                prim.GetComponent<PrimsControls>().enabled = false;
                prim.GetComponentInChildren<UIPrim>().enabled = false;
            }
            catch
            {
            }

            try
            {
                GameObject darkPrim = GameObject.FindGameObjectWithTag("DarkPrim");
                darkPrim.GetComponent<CharacterMotor>().canControl = false;
                darkPrim.GetComponent<DarkPrimControls>().DisableVision();
                darkPrim.GetComponent<DarkPrimControls>().enabled = false;
                darkPrim.GetComponentInChildren<UIDarkPrim>().enabled = false;
            }
            catch
            {
            }
        }
}
