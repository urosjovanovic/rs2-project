using UnityEngine;
using System.Collections;

public class LoadingScreenSettings : MonoBehaviour 
{
    private int screenWidth;
    private int screenHeight;
    private GUIText loadingText;
    private float timeDelay = 1.0f;
    private float timeRemaining;
    private int numberOfDots = 0;

	// Use this for initialization
	void Start () 
    {
        timeRemaining = timeDelay;

        screenWidth = Screen.height;
        screenHeight = Screen.width;
        
        loadingText = GetComponentInParent<GUIText>();
        if (loadingText != null)
            transform.position = new Vector2(0.5f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () 
    {
        timeRemaining -= Time.deltaTime;

        if(screenWidth != Screen.height || screenHeight != Screen.width)
        {
            if (loadingText != null)
                transform.position = new Vector2(0.5f, 0.5f);
        }
        else if(timeRemaining < 0)
        {
            if (loadingText != null)
            {
                numberOfDots = (numberOfDots + 1) % 4;

                switch(numberOfDots)
                {
                    case 0:
                        loadingText.text = "Loading";
                        break;
                    case 1:
                        loadingText.text = "Loading .";
                        break;
                    case 2:
                        loadingText.text = "Loading . .";
                        break;
                    case 3:
                        loadingText.text = "Loading . . .";
                        break;
                }
            }

            timeRemaining = timeDelay;
        }
	}
}
