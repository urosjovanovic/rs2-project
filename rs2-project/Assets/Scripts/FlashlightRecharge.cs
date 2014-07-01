using UnityEngine;
using System.Collections;
using System;

public class FlashlightRecharge : MonoBehaviour
{

    #region Class fields
    public float flashLightLifeTime;
	public float flashLightReChargeTime;
	public float flashLightTimeRemaining;
	public int flashLightChargedPercent = 100;
	public bool flashLightEnabled = true;

	private float treshold;
	private float defaultTreshold = 0.5f;

    #endregion

    #region Components
    private PhotonView view;
    private FlashlightBehaviour fb;
    #endregion

    // Use this for initialization
	void Start () 
	{
        // initialize the components
        if (!(fb = this.gameObject.GetComponent<FlashlightBehaviour>())) throw new NullReferenceException("FlashlightBehaviour component in the FlashlightRecharge script is null.");
		if(!(view = this.gameObject.GetComponent<PhotonView>())) throw new NullReferenceException("PhotonView component in the FlashlightRecharge script is null.");
		
        //initialize the fields
        flashLightTimeRemaining = flashLightLifeTime;
		treshold = defaultTreshold;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (fb.FlashlightIsOn) 
		{
			treshold = defaultTreshold;

			flashLightTimeRemaining -= Time.deltaTime;
			flashLightChargedPercent = (int)(((float)flashLightTimeRemaining/flashLightLifeTime)*100 + 0.5f);
			if (flashLightTimeRemaining <= 0) 
			{
				view.RPC ("ReCharge", PhotonTargets.All, null);
			}

		} 
		else if(flashLightTimeRemaining<flashLightLifeTime)
		{
			treshold -= Time.deltaTime;

			if(treshold<0)
			{
				flashLightTimeRemaining += (Time.deltaTime*flashLightLifeTime)/flashLightReChargeTime;
				flashLightChargedPercent = (int)(((float)flashLightTimeRemaining/flashLightLifeTime)*100 + 0.5f);
			}
		}
		else if(flashLightTimeRemaining>flashLightLifeTime)
		{
			flashLightTimeRemaining = flashLightLifeTime;
			flashLightChargedPercent = 100;
		}
	}
	
	[RPC]
	void ReCharge()
	{
		flashLightEnabled = false;
        fb.TurnFlashlightOff();

		StartCoroutine(WaitAndUnfreeze());
	}
	
	IEnumerator WaitAndUnfreeze()
	{
		yield return new WaitForSeconds(flashLightReChargeTime);
		flashLightTimeRemaining = flashLightLifeTime;
		flashLightChargedPercent = 100;
		treshold = defaultTreshold;
		flashLightEnabled = true;
		//this.gameObject.light.enabled = true;
	}
}


