using UnityEngine;
using System.Collections;

public class FlashlightFire : MonoBehaviour {

    public float fireRate = 0.5f;
    float coolDown = 0;
    private PhotonView view;

	// Use this for initialization
	void Start () {
        view = this.gameObject.GetComponent<PhotonView>();
	}
	
	// Update is called once per frame
	void Update () {

        //flashlight rays
        if (this.gameObject.light.enabled)
        {
            coolDown -= Time.deltaTime;
            Fire();
        }
	
	}

    void Fire()
    {
        Debug.Log("Fire!");

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
                this.gameObject.light.enabled = false;
                view.RPC("FreezeDarkPrim", PhotonTargets.All, null);
                //hit.rigidbody.gameObject.GetComponent<CharacterController>().enabled=false;
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
           darkprim.GetComponent<CharacterController>().enabled = false;
           StartCoroutine(WaitAndUnfreeze(10,darkprim));
          //darkprim.GetComponent<CharacterController>().enabled = true;
       }
    }

    IEnumerator WaitAndUnfreeze(float time,GameObject obj)
    {
        yield return new WaitForSeconds(time);
        obj.GetComponent<CharacterController>().enabled = true;
    }
}
