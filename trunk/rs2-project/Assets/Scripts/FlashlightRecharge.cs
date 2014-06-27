using UnityEngine;
using System.Collections;

public class FlashlightRecharge : MonoBehaviour {
	
	public float flashLightLifeTime;
	public float flashLightReChargeTime;
	public float flashLightTimeRemaining;
	public int flashLightChargedPercent = 100;
	public bool flashLightEnabled = true;

	private float treshold;
	private float defaultTreshold = 0.5f;

	private PhotonView view;
	// Use this for initialization
	void Start () 
	{
		view = this.gameObject.GetComponent<PhotonView>();
		flashLightTimeRemaining = flashLightLifeTime;
		treshold = defaultTreshold;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (this.gameObject.light.enabled) 
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
		this.gameObject.light.enabled = false;
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


