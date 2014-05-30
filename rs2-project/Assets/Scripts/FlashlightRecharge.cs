using UnityEngine;
using System.Collections;

public class FlashlightRecharge : MonoBehaviour {
	
	public float flashLightLifeTime;
	public float flashLightReChargeTime;
	private float flashLightTimeRemaining;
	public int flashLightChargedPercent = 100;
	public bool flashLightEnabled = true;
	
	private PhotonView view;
	// Use this for initialization
	void Start () 
	{
		view = this.gameObject.GetComponent<PhotonView>();
		flashLightTimeRemaining = flashLightLifeTime;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (this.gameObject.light.enabled) 
		{
			flashLightTimeRemaining -= Time.deltaTime;
			flashLightChargedPercent = (int)(((float)flashLightTimeRemaining/flashLightLifeTime)*100 + 0.5f);
			if (flashLightTimeRemaining <= 0) 
			{
				view.RPC ("ReCharge", PhotonTargets.All, null);
			}

		} 
		else if(flashLightTimeRemaining<flashLightLifeTime)
		{
			flashLightTimeRemaining += (Time.deltaTime*flashLightLifeTime)/flashLightReChargeTime;
			flashLightChargedPercent = (int)(((float)flashLightTimeRemaining/flashLightLifeTime)*100 + 0.5f);
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
		flashLightEnabled = true;
		//this.gameObject.light.enabled = true;
	}
}


