using UnityEngine;
using System.Collections;

public class HeadBob : MonoBehaviour {

    private float timer = 0.0f;
    public float bobbingSpeed = 0.22f;
    public float bobbingAmount = 0.05f;
    public float midpoint = 1.0f;


    void Start()
    {
        this.transform.localPosition = new Vector3(0f, midpoint, 0f);
        Screen.showCursor = false;
    }

    // Update is called once per frame
    void Update()
    {

        float waveslice = 0.0f;
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var run = Input.GetKey("left shift") ;
        var w = Input.GetKey(KeyCode.W);

        if (run && w && this.transform.parent.gameObject.GetComponent<LimitSprint>().sprintEnabled)
        {
            if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
            {
                timer = 0.0f;
            }
            else
            {
                waveslice = Mathf.Sin(timer);
                timer = timer + bobbingSpeed;
                if (timer > Mathf.PI * 2)
                {
                    timer = timer - (Mathf.PI * 2);
                }
            }

            if (waveslice != 0)
            {
                var translateChange = waveslice * bobbingAmount;
                var totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
                totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
                translateChange = totalAxes * translateChange;
                this.transform.localPosition = new Vector3(0f, midpoint + translateChange, 0f);
            }
            else
            {
                this.transform.localPosition = new Vector3(0f, midpoint, 0f);
            }
        }
    }
}
