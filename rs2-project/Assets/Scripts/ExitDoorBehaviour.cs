using UnityEngine;
using System.Collections;

public class ExitDoorBehaviour : MonoBehaviour {

    public float animationTime = 3.0f;
    private float angle = 90;
    private Transform doorPanel;
    private bool opening = false;
    private bool closing = false;
    private enum DoorState { OPENED, CLOSED, ANIM };
    private DoorState state = DoorState.CLOSED;

	// Use this for initialization
	void Start () {
        doorPanel = this.transform.parent.transform.FindChild("panel").transform;
	}
	
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Prim")
        {
            if (state == DoorState.CLOSED)
            {
                opening = true;
                state = DoorState.ANIM;
                StartCoroutine(OpenDoorAnimated());
            }
        }
    }

    void OnTriggerExit()
    {
        if(state == DoorState.OPENED)
        {
            closing = true;
            state = DoorState.ANIM;
            StartCoroutine(CloseDoorAnimated());
        }
    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    IEnumerator OpenDoorAnimated()
    {
        float step = angle / animationTime;
        step /= 100.0f;
        float rotated = 0;

        while(rotated + 0.1 < angle)
        {
            yield return new WaitForSeconds(1 / 100.0f);
            doorPanel.Rotate(Vector3.forward, step);
            rotated += step;

        }

        opening = false;
        state = DoorState.OPENED;
    }

    IEnumerator CloseDoorAnimated()
    {
        StartCoroutine(Wait(animationTime));
        float step = angle / animationTime;
        step /= 100.0f;
        float rotated = 0;

        while (rotated + 0.1 < angle)
        {
            yield return new WaitForSeconds(1 / 100.0f);
            doorPanel.Rotate(Vector3.forward, -step);
            rotated += step;

        }

        closing = false;
        state = DoorState.CLOSED;
    }
}
