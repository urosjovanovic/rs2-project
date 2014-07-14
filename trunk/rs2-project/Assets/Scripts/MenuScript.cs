using UnityEngine;
using System.Collections;
using System;

public class MenuScript : MonoBehaviour
{

    #region GameObjects

    private GameObject back;
	private GameObject menuItemPlay, menuItemControls, menuItemAbout, menuItemExit;
	private GameObject cameraObject;

    #endregion

    #region Class fields

    private int currentScene = 0;
	public int currentMenuItem;
	public bool isDarkControls = true;

    #endregion

    #region Start and update

    // Use this for initialization
	void Start () 
	{
        Screen.showCursor = true;

        if (!(back = GameObject.Find("Shadow"))) throw new NullReferenceException("Shadow object in MenuScript script cannot be found!");
		if(!(menuItemPlay = GameObject.Find ("MenuItem0"))) throw new NullReferenceException("MenuItem0 object in MenuScript script cannot be found!");
        if (!(menuItemControls = GameObject.Find("MenuItem1"))) throw new NullReferenceException("MenuItem1 object in MenuScript script cannot be found!");
        if (!(menuItemAbout = GameObject.Find("MenuItem2"))) throw new NullReferenceException("MenuItem2 object in MenuScript script cannot be found!");
        if (!(menuItemExit = GameObject.Find("MenuItem3"))) throw new NullReferenceException("MenuItem3 object in MenuScript script cannot be found!");

        if (!(cameraObject = GameObject.FindGameObjectWithTag("MainCamera"))) throw new NullReferenceException("MainCamera object in MenuScript script cannot be found!");

		MoveCameraX(0.0f);

		currentMenuItem = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        #region Highlight
        switch (currentMenuItem)
		{
			case 0:
				back.SetActive(false);
			break;	
			case 1:
				back.SetActive(true);
				MoveHighlightY(menuItemPlay.transform.position.y);
				break;
			case 2:
				back.SetActive(true);
				MoveHighlightY(menuItemControls.transform.position.y);
				break;
			case 3:
				back.SetActive(true); MoveHighlightY(menuItemAbout.transform.position.y);
				break;
			case 4:
				back.SetActive(true);
				MoveHighlightY(menuItemExit.transform.position.y);
				break;
        }
        #endregion

        #region Input
        
        //select menu item
        if(currentScene == 0)
        {
            #region Menu Item Selected

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                switch (currentMenuItem)
                {
                    case 1:
                        cameraObject.audio.Stop();
                        cameraObject.audio.PlayOneShot(SoundPool.MenuQuake);
                        cameraObject.GetComponent<CameraShake>().Shake(2f);
                        StartCoroutine(LoadSceneWithDelay("Main", 2f));
                        break;
                    case 2:
                        MoveCameraX(-20.0f);
                        currentScene = 2;
                        break;
                    case 3:
                        MoveCameraX(20.0f);
                        currentScene = 3; //about
                        foreach(var verse in GameObject.FindGameObjectsWithTag("Verse"))
                        {
                            verse.GetComponent<VerseBehaviour>().Hide();
                        }
                        StartCoroutine(LoadVerses());
                        break;
                    case 4:
                        Application.Quit();
                        break;
                }
            }

            #endregion

            else if (Input.GetKeyDown(KeyCode.DownArrow) && currentMenuItem < 4)
            {
                currentMenuItem++;
                this.transform.audio.PlayOneShot(SoundPool.MenuClick);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) && currentMenuItem > 1)
            {
                currentMenuItem--;
                this.transform.audio.PlayOneShot(SoundPool.MenuClick);
            }
        }
        //return to MainMenu
		else if(currentScene != 0 && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace)))
		{
			MoveCameraX(0.0f);
			currentScene = 0;
		}
        //switch DarkPrim's and Prim's controls
		else if(currentScene == 2 && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
        {
            #region DarkPrim's and Prim's controls

            GameObject darkPrimControls = GameObject.Find("DarkPrimControls");
			GameObject primControls = GameObject.Find("PrimControls");

            GameObject darkPrimTitle = GameObject.Find("DarkPrimTitle");
            GameObject primTitle = GameObject.Find("PrimTitle");
            GameObject controlsTitle = GameObject.Find("ControlsTitle");

			if(!isDarkControls)
			{
                darkPrimControls.GetComponent<MeshCollider>().renderer.enabled = false;
                primControls.GetComponent<MeshCollider>().renderer.enabled = true;

                darkPrimTitle.GetComponent<MeshRenderer>().enabled = false;
                primTitle.GetComponent<MeshRenderer>().enabled = true;
                controlsTitle.GetComponent<TextMesh>().color = Color.white;

			}
			else
			{
				darkPrimControls.GetComponent<MeshCollider>().renderer.enabled = true;
				primControls.GetComponent<MeshCollider>().renderer.enabled = false;

                darkPrimTitle.GetComponent<MeshRenderer>().enabled = true;
                primTitle.GetComponent<MeshRenderer>().enabled = false;
                controlsTitle.GetComponent<TextMesh>().color = Color.red;
			}

			isDarkControls = !isDarkControls;

            #endregion
        }

        #endregion
    }

    #endregion

    private IEnumerator LoadVerses()
    {
        for(int i = 1; i <= 9; i++)
        {
            var verse = GameObject.Find("Verse" + i.ToString());
            verse.GetComponent<VerseBehaviour>().FadeIn();
            yield return new WaitForSeconds(1.0f);
        }
    }

    /// <summary>
    /// Load a scene with a delay
    /// </summary>
    /// <param name="sceneName"> Scene name</param>
    /// <param name="delay"> Delay </param>
    /// <returns> IEnumerator</returns>
    private IEnumerator LoadSceneWithDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        Application.LoadLevel(sceneName);
    }

	void MoveCameraX(float value)
	{
		cameraObject.transform.position = new Vector3(value, 
		                                        cameraObject.transform.position.y, 
		                                        cameraObject.transform.position.z);
	}

	void MoveHighlightY(float value)
	{
		back.transform.position = new Vector3(back.transform.position.x, 
		                                      value, 
		                                      back.transform.position.z);
	}

}
