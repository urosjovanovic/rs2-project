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
        this.transform.LookAt(target.position);
        this.GetComponent<MeshRenderer>().enabled = true;
        StartCoroutine(FadeOut());
	}
	
	// Update is called once per frame
	void Update () {
	    if(target!=null)
        {
            this.transform.LookAt(target.position);
        }
	}

    IEnumerator FadeOut()
    {
        float step = 0.01f;
        while(this.renderer.material.color.a > 0)
        {
            yield return new WaitForSeconds(1 / 100.0f);
            Color c = this.renderer.material.color;
            this.renderer.material.color = new Color(c.r, c.g, c.b, c.a - step);
        }
    }
}
