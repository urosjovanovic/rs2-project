using UnityEngine;
using System.Collections;

public class CollisionControl : MonoBehaviour {

    public PhotonView view;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        GameObject prim = GameObject.FindGameObjectWithTag("Prim");
        GameObject darkPrim = GameObject.FindGameObjectWithTag("DarkPrim");

        if (prim && darkPrim)
        {
            view.RPC("DeactivatePlayerColliders", PhotonTargets.All, null);
            this.enabled = false;
        }
	}

    [RPC]
    private void DeactivatePlayerColliders()
    {
        GameObject prim = GameObject.FindGameObjectWithTag("Prim");
        GameObject darkPrim = GameObject.FindGameObjectWithTag("DarkPrim");

        if (prim && darkPrim)
        {
            Debug.Log("Disabling colliders.");
            //Ignorisemo sudare izmedju likova
            Physics.IgnoreCollision(darkPrim.GetComponentInChildren<CharacterController>().collider, prim.GetComponentInChildren<CharacterController>().collider, true);
            //Sprecavamo trigerovanje EndGameSkripte
            darkPrim.GetComponentInChildren<CapsuleCollider>().enabled = false;
        }
    }
}
