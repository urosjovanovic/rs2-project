using UnityEngine;
using System.Collections;
using System;

public class UIPrim : MonoBehaviour
{

    #region Components
    private GameObject flashlight;
    private PrimsControls primsControls;
    private FlashlightBehaviour fb;
    private FlashlightRecharge fr;
    #endregion

    #region Textures
    public Texture lightOn;
    public Texture lightOff;
    public Texture lightbar;
    public Texture markerIcon;
    #endregion

    // Use this for initialization
	void Start () {

        if (!(primsControls = this.transform.parent.gameObject.GetComponent<PrimsControls>())) throw new NullReferenceException("PrimsControls component in UIPrim script is null!");
        if (!(flashlight = primsControls.flashlight.gameObject)) throw new NullReferenceException("Flashlight component in UIPrim script is null!");
        if (!(fb = flashlight.GetComponent<FlashlightBehaviour>())) throw new NullReferenceException("FlashlightBehaviour component in UIPrim script is null!");
        if (!(fr = flashlight.GetComponent<FlashlightRecharge>())) throw new NullReferenceException("FlashlightRecharge component in UIPrim script is null!");

	}

    void OnGUI()
    {
        float lightbarmeter = lightOn.height/100.0f * fr.flashLightChargedPercent;
        GUI.DrawTexture(new Rect(Screen.width - lightOn.width, Screen.height - 10, lightbarmeter, 10), lightbar, ScaleMode.StretchToFill,true,1.0f);
        
        Rect position = new Rect(Screen.width-lightOff.width,Screen.height-lightOff.height,lightOff.width,lightOff.height);

        if (fb.FlashlightIsOn)
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
