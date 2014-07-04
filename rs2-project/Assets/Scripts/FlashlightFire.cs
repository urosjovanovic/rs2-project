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
            Debug.DrawLine(transform.position, hit.point, Color.green);
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
       if (darkprim)
       {
           Debug.Log("Freeze motherfucker!");
           darkprim.GetComponent<CharacterMotor>().canControl = false;
           darkprim.GetComponentInChildren<CapsuleCollider>().enabled = false;

           StartCoroutine(WaitAndUnfreeze(freezeTime,darkprim));
          //darkprim.GetComponent<CharacterController>().enabled = true;
       }
    }

    IEnumerator WaitAndUnfreeze(float time,GameObject obj)
    {
        yield return new WaitForSeconds(time);
        obj.GetComponentInChildren<CapsuleCollider>().enabled = true;
        obj.GetComponent<CharacterMotor>().canControl = true;
        freezeActive = false;
    }
}
