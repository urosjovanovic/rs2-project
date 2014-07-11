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
    private EndGamePrim endGameScript;
    #endregion

    #region Textures
    public Texture lightOn;
    public Texture lightOff;
    public Texture lightbar;
    public Texture markerIcon;
    public Texture[] deathAnimation;
    private int currentAnimationSprite = 0;
    #endregion

    private bool inAnimation = false;
    public bool InAnimation
    {
        get
        {
            return inAnimation;
        }
    }

    // Use this for initialization
	void Start () {
        try
        {
            if (!(primsControls = this.transform.parent.gameObject.GetComponent<PrimsControls>())) throw new NullReferenceException("PrimsControls component in UIPrim script is null!");
            if (!(flashlight = primsControls.flashlight.gameObject)) throw new NullReferenceException("Flashlight component in UIPrim script is null!");
            if (!(fb = flashlight.GetComponent<FlashlightBehaviour>())) throw new NullReferenceException("FlashlightBehaviour component in UIPrim script is null!");
            if (!(fr = flashlight.GetComponent<FlashlightRecharge>())) throw new NullReferenceException("FlashlightRecharge component in UIPrim script is null!");
            if (!(endGameScript = this.transform.parent.gameObject.GetComponent<EndGamePrim>())) throw new NullReferenceException("EndGame component is missing on player Prim");
        }
        catch(Exception ex)
        {
            Debug.LogError(ex.Message);
            this.enabled = false;
        }

	}

    void Update()
    {
        if(endGameScript.IsDead && !inAnimation)
        {
            this.transform.parent.gameObject.GetComponent<CharacterMotor>().canControl = false;
            StartCoroutine(Animate(0.1f));
            inAnimation = true;
        }
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

        if(endGameScript.IsDead)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), deathAnimation[0], ScaleMode.ScaleAndCrop);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), deathAnimation[currentAnimationSprite], ScaleMode.ScaleAndCrop);
        }
        
    }

    private IEnumerator Animate(float s)
    {
        while(currentAnimationSprite < 11)
        {
            if (currentAnimationSprite == 0)
            {
                yield return new WaitForSeconds(1);
                currentAnimationSprite++;
            }
            else
            {
                currentAnimationSprite++;
                yield return new WaitForSeconds(s);
            }

            if (currentAnimationSprite == 11)
            {
                inAnimation = false;
                this.transform.parent.gameObject.GetComponent<EndGamePrim>().Lose();
            }
        }

        
            
    }
}
