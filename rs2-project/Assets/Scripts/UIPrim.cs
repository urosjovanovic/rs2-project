using UnityEngine;
using System.Collections;

public class UIPrim : MonoBehaviour {

    private GameObject flaslight;
    private PrimsControls primsControls;

    public Texture lightOn;
    public Texture lightOff;
    public Texture lightbar;
    public Texture markerIcon;

	// Use this for initialization
	void Start () {
        primsControls = this.transform.parent.gameObject.GetComponent<PrimsControls>();
        flaslight = primsControls.flashlight.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        float lightbarmeter = lightOn.height/100.0f * flaslight.GetComponent<FlashlightRecharge>().flashLightChargedPercent;
        GUI.DrawTexture(new Rect(Screen.width - lightOn.width, Screen.height - 10, lightbarmeter, 10), lightbar, ScaleMode.StretchToFill,true,1.0f);
        
        Rect position = new Rect(Screen.width-lightOff.width,Screen.height-lightOff.height,lightOff.width,lightOff.height);
        if (flaslight.light.enabled)
        {
            GUI.DrawTexture(position, lightOn);
        }
        else
        {
            GUI.DrawTexture(position, lightOff);
        }

        GUI.DrawTexture(new Rect(Screen.width - lightOn.width - 60, Screen.height - lightOn.height + 40, 50, 50), markerIcon, ScaleMode.ScaleToFit);
        GUIStyle style = new GUIStyle();
        style.fontSize = 36;
        style.normal.textColor = Color.white;
        style.margin = new RectOffset(0, 0, 0, 0);
        style.alignment = TextAnchor.MiddleCenter;
        GUI.Label(new Rect(Screen.width - lightOn.width - 60, Screen.height - lightOn.height + 90, 50, 50), primsControls.MarkerCount.ToString(),style);
        
    }
}
