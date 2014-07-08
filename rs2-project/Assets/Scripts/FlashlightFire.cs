using UnityEngine;
using System.Collections;
using System;

public class FlashlightFire : MonoBehaviour {

    public float fireRate = 0.1f;
    float coolDown = 0;
    private PhotonView view;
    private bool freezeActive = false;
    private float freezeTime = 10;
    private GameObject darkPrimSprite = null;

    FlashlightRecharge flashlightRecharge;

	// Use this for initialization
	void Start () {
        view = this.gameObject.GetComponent<PhotonView>();

        flashlightRecharge = this.GetComponent<FlashlightRecharge>();

        if (!flashlightRecharge) throw new Exception("FlashlightRecharge is null!");
	}
	
	// Update is called once per frame
	void Update () {

        //flashlight rays
		if (this.gameObject.light.enabled && this.gameObject.GetComponent<FlashlightRecharge>().flashLightEnabled)
        {
            coolDown -= Time.deltaTime;
            Fire();
        }

        if (!freezeActive && darkPrimSprite != null)
            GameObject.Destroy(darkPrimSprite);
	
	}

    void Fire()
    {
        if (freezeActive)
            return;

        if (coolDown > 0)
            return;

        var fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, fwd, out hit, 5))
        {
            if (hit.rigidbody != null && hit.rigidbody.gameObject.tag == "DarkPrim")
            {
                Debug.Log("There is something in front of the object!");
                this.audio.PlayOneShot(SoundPool.Scream);
                darkPrimSprite = Instantiate(Resources.Load("DarkPrim2DSprite")) as GameObject;
                darkPrimSprite.transform.position = hit.rigidbody.gameObject.transform.position;
                view.RPC("FreezeDarkPrim", PhotonTargets.All, null);
                freezeActive = true;
                flashlightRecharge.flashLightTimeRemaining = 0;

            }
        }

        coolDown = fireRate;
    }

    [RPC]
    void FreezeDarkPrim()
    {
       GameObject darkprim =  GameObject.FindGameObjectWithTag("DarkPrim");
       GameObject prim = GameObject.FindGameObjectWithTag("Prim");
       if (darkprim && prim)
       {
           Debug.Log("Freeze motherfucker!");
           darkprim.GetComponent<CharacterMotor>().canControl = false;

           var darkPrimCtrls = darkprim.GetComponent<DarkPrimControls>();

           if (darkPrimCtrls.enabled && darkPrimCtrls.nightmareVision)
           {
               darkPrimCtrls.DisableVision();
               darkPrimCtrls.IsFrozen = true;
           }

           //Ignorisemo sudare izmedju likova
           Physics.IgnoreCollision(darkprim.GetComponentInChildren<CharacterController>().collider, prim.GetComponentInChildren<CharacterController>().collider, true);
           //Sprecavamo trigerovanje EndGameSkripte
           darkprim.GetComponentInChildren<CapsuleCollider>().enabled = false;

           StartCoroutine(WaitAndUnfreeze(freezeTime,prim,darkprim));
       }
    }

    IEnumerator WaitAndUnfreeze(float time,GameObject prim, GameObject darkprim)
    {
        yield return new WaitForSeconds(time);
        //obj.GetComponentInChildren<CapsuleCollider>().enabled = true;
        darkprim.GetComponent<CharacterMotor>().canControl = true;
        Physics.IgnoreCollision(darkprim.GetComponentInChildren<CharacterController>().collider, prim.GetComponentInChildren<CharacterController>().collider, false);
        darkprim.GetComponentInChildren<CapsuleCollider>().enabled = true;
        var darkPrimCtrls = darkprim.GetComponent<DarkPrimControls>();
        if(darkPrimCtrls.enabled)
        {
            darkPrimCtrls.IsFrozen = false;
        }
        freezeActive = false;
    }
}
