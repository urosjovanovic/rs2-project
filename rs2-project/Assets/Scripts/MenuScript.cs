using UnityEngine;
using System.Collections;
using System;

public class MenuScript : MonoBehaviour {

	private GameObject back;
	private GameObject menuItemPlay, menuItemControls, menuItemAbout, menuItemExit;
	private GameObject camera;

	private int currentScene = 0;
	public int currentMenuItem;
	public bool isDarkControls = true;

	// Use this for initialization
	void Start () 
	{
		back = GameObject.Find ("Shadow");
		menuItemPlay = GameObject.Find ("MenuItem0");
		menuItemControls = GameObject.Find ("MenuItem1");
		menuItemAbout = GameObject.Find ("MenuItem2");
		menuItemExit = GameObject.Find ("MenuItem3");

		camera = GameObject.FindGameObjectWithTag ("MainCamera");
		MoveCameraX(0.0f);
        MoveHighlightY(menuItemPlay.transform.position.y);

		currentMenuItem = 1;
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
				back.SetActive(true);
				MoveHighlightY(menuItemAbout.transform.position.y);
				break;
			case 4:
				back.SetActive(true);
				MoveHighlightY(menuItemExit.transform.position.y);
				break;
        }
        #endregion

        #region Input
        
        //select menu item
        if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            #region Menu Item Selected
            switch (currentMenuItem)
			{
				case 1:
					Application.LoadLevel("Main");
					break;
				case 2:
					MoveCameraX(-20.0f);
					currentScene = 2;
					break;
				case 3:
					MoveCameraX(20.0f);
					currentScene = 3;
					break;
				case 4:
					Application.Quit();
					break;
            }

            #endregion
        }
        //return to MainMenu
		else if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace))
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

	void MoveCameraX(float value)
	{
		camera.transform.position = new Vector3(value, 
		                                        camera.transform.position.y, 
		                                        camera.transform.position.z);
	}

	void MoveHighlightY(float value)
	{
		back.transform.position = new Vector3(back.transform.position.x, 
		                                      value, 
		                                      back.transform.position.z);
	}


}
