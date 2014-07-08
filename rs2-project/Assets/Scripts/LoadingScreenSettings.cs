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
    private int numberOfDotsWaiting = 0;
    public bool waitingSecondPlayer = false;
    private string loading = "Loading";
    private string waiting = "Waiting for second player";

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
            timeRemaining = timeDelay;

            if (loadingText != null)
            {
                if(waitingSecondPlayer)
                {
                    numberOfDotsWaiting = (numberOfDotsWaiting + 1) % 4;
                    switch(numberOfDotsWaiting)
                    {
                        case 0:
                            loadingText.text = waiting;
                            break;
                        case 1:
                            loadingText.text = waiting + " .";
                            break;
                        case 2:
                            loadingText.text = waiting + " . .";
                            break;
                        case 3:
                            loadingText.text = waiting + " . . .";
                            break;
                    }

                    
                }
                else
                {
                    numberOfDots = (numberOfDots + 1) % 4;
                    switch(numberOfDots)
                    {
                        case 0:
                            loadingText.text = loading;
                            break;
                        case 1:
                            loadingText.text = loading + " .";
                            break;
                        case 2:
                            loadingText.text = loading + " . .";
                            break;
                        case 3:
                            loadingText.text = loading + " . . .";
                            break;
                    }

                    
                }
            }
        }
	}
}
