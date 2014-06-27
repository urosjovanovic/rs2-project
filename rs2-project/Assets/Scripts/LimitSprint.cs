using UnityEngine;
using System.Collections;

public class LimitSprint : MonoBehaviour
{
    public float sprintLifeTime;
    public float sprintReChargeTime;
    public float sprintTimeRemaining;
    public bool sprintEnabled = true;

    // Use this for initialization
    void Start()
    {
        sprintTimeRemaining = sprintLifeTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GetComponent<Sprint>().isInSprint)
        {
            sprintTimeRemaining -= Time.deltaTime;

            if (sprintTimeRemaining <= 0)
            {
                RechargeVision();
            }
        }
        else
        {
            if (sprintTimeRemaining < sprintLifeTime)
            {
                sprintTimeRemaining += (Time.deltaTime * sprintLifeTime) / sprintReChargeTime;
            }
            else if (sprintTimeRemaining > sprintLifeTime)
            {
                sprintTimeRemaining = sprintLifeTime;
            }
        }
    }

    private void RechargeVision()
    {
        sprintEnabled = false;
        StartCoroutine(WaitAndUnfreeze());
    }

    IEnumerator WaitAndUnfreeze()
    {
        yield return new WaitForSeconds(sprintReChargeTime);
        sprintTimeRemaining = sprintLifeTime;
        sprintEnabled = true;
    }
}
