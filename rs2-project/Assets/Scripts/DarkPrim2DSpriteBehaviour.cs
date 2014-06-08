using UnityEngine;
using System.Collections;

public class DarkPrim2DSpriteBehaviour : MonoBehaviour {

    private Transform target;
    /// <summary>
    /// The object which should 2D sprite be oriented at.
    /// </summary>
    public Transform Target
    {
        get { return target; }
        set { target = value; }
    }

	// Use this for initialization
	void Start () {
        target = GameObject.FindGameObjectWithTag("Prim").transform;
	}
	
	// Update is called once per frame
	void Update () {
	    if(target!=null)
        {
            this.transform.LookAt(target.position);
        }
	}
}
