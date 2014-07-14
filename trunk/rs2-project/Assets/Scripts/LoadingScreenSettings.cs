using UnityEngine;
using System.Collections;

public class LoadingScreenSettings : MonoBehaviour 
{
    private int screenWidth;
    private int screenHeight;
    private GUIText loadingText;
    private GameObject loadingArrows;
    private float timeDelay = 1.0f;
    private float timeRemaining;
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
        loadingArrows = GameObject.Find("LoadingArrows");

        if (loadingText != null)
            loadingText.transform.position = new Vector2(0.5f, 0.5f);

        if (loadingArrows != null)
            loadingArrows.transform.Translate(new Vector3(0.0f, -1.0f, 0.0f));

	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PhotonNetwork.Disconnect();
        }

        timeRemaining -= Time.deltaTime;

        if (loadingArrows != null)
            loadingArrows.transform.Rotate(new Vector3(0.0f, 0.0f, -4.0f));

        if(screenWidth != Screen.height || screenHeight != Screen.width)
        {
            if (loadingText != null)
                transform.position = new Vector2(0.5f, 0.5f);
        }
        
        if(timeRemaining < 0)
        {
            timeRemaining = timeDelay;

            if (loadingText != null)
            {
                if(waitingSecondPlayer)
                {
                    loadingText.text = waiting;
                    loadingText.fontSize = 40;
                }
                else
                {
                    loadingText.text = loading;
                    loadingText.fontSize = 50;
                }
            }
        }
	}
}
