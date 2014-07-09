using UnityEngine;
using System.Collections;

public class LimitVision : MonoBehaviour 
{
    public float visionLifeTime;
    public float visionReChargeTime;
    public float visionTimeRemaining;
    public int visionChargedPercent = 100;
    public bool visionEnabled = true;
    public bool canTurnOffVision = false;

	// Use this for initialization
	void Start () 
    {
        if (ConfigManager.infiniteNightmareVision)
        {
            canTurnOffVision = true;
            this.enabled = false;
        }

        visionTimeRemaining = visionLifeTime;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if(GetComponent<DarkPrimControls>().nightmareVision)
        {
            visionTimeRemaining -= Time.deltaTime;
            visionChargedPercent = (int)(((float)visionTimeRemaining / visionLifeTime) * 100 + 0.5f);

            if (visionTimeRemaining <= 0)
            {
                RechargeVision();
            }
        }
        else
        {
            if (visionTimeRemaining < visionLifeTime)
            {
                visionTimeRemaining += (Time.deltaTime * visionLifeTime) / visionReChargeTime;
                visionChargedPercent = (int)(((float)visionTimeRemaining / visionLifeTime) * 100 + 0.5f);
            }
            else if (visionTimeRemaining > visionLifeTime)
            {
                visionTimeRemaining = visionLifeTime;
                visionChargedPercent = 100;
            }
        }
	}

    private void RechargeVision()
    {
        visionEnabled = false;
        GetComponent<DarkPrimControls>().DisableVision();
        StartCoroutine(WaitAndUnfreeze());
    }

    IEnumerator WaitAndUnfreeze()
    {
        yield return new WaitForSeconds(visionReChargeTime);
        visionTimeRemaining = visionLifeTime;
        visionChargedPercent = 100;
        visionEnabled = true;
    }
}
