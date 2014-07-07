using UnityEngine;
using System.Collections;

public class UIDarkPrim : MonoBehaviour {

    public Texture nvMeterOff;
    public Texture nvMeterOn;
    public Texture nvMeterFill;
    public Texture darkOverlay;
    public Texture frozenOverlay;

    private Transform parent;
    private DarkPrimControls controls;

	// Use this for initialization
	void Start () {
        parent = this.transform.parent;
        controls = parent.GetComponent<DarkPrimControls>();
	}

    void OnGUI()
    {
        if (controls.IsFrozen)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), frozenOverlay, ScaleMode.ScaleAndCrop);
        }
        else
        {
            if (controls.nightmareVision)
            {
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), darkOverlay, ScaleMode.ScaleAndCrop);
            }
            float nvMeterFillWidth = 210 / 100.0f * parent.GetComponent<LimitVision>().visionChargedPercent;
            GUI.DrawTexture(new Rect(115, Screen.height - nvMeterOff.height / 2 - 15, nvMeterFillWidth, 26), nvMeterFill, ScaleMode.StretchToFill, true, 1.0f);

            if (parent.GetComponent<DarkPrimControls>().nightmareVision)
            {
                GUI.DrawTexture(new Rect(0, Screen.height - nvMeterOn.height, nvMeterOn.width, nvMeterOn.height), nvMeterOn);
            }
            else
            {
                GUI.DrawTexture(new Rect(0, Screen.height - nvMeterOff.height, nvMeterOff.width, nvMeterOff.height), nvMeterOff);
            }
        }
        
    }
	
    
}
