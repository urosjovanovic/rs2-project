using UnityEngine;
using System.Collections;
using System;

public class PauseScript : MonoBehaviour
{
    #region GameObjects

    private GameObject back, pauseCamera;
    private GameObject menuItemPause, menuItemControls, menuItemAbout, menuItemQuit;
    private GameObject pauseScene, aboutScene, controlsScene;
    private GameObject prim, darkPrim;
    #endregion

    #region Class fields

    private int currentScene = 0;
    public int currentMenuItem;
    public bool isDarkControls = true;
    public bool calledByPrim;

    #endregion

    #region Start and update
    // Use this for initialization
    void Start()
    {
        Screen.showCursor = true;
        
        if (!(back = GameObject.Find("Shadow"))) throw new NullReferenceException("Shadow object in PauseScript script cannot be found!");
        if (!(pauseCamera = GameObject.FindGameObjectWithTag("PauseCamera"))) throw new NullReferenceException("PauseCamera object in PauseScript script cannot be found!");

        if (!(menuItemPause = GameObject.Find("MenuItem0"))) throw new NullReferenceException("MenuItem0 object in PauseScript script cannot be found!");
        if (!(menuItemControls = GameObject.Find("MenuItem1"))) throw new NullReferenceException("MenuItem1 object in PauseScript script cannot be found!");
        if (!(menuItemAbout = GameObject.Find("MenuItem2"))) throw new NullReferenceException("MenuItem2 object in PauseScript script cannot be found!");
        if (!(menuItemQuit = GameObject.Find("MenuItem3"))) throw new NullReferenceException("MenuItem3 object in PauseScript script cannot be found!");

        if (!(pauseScene = GameObject.Find("PauseScene"))) throw new NullReferenceException("PauseScene object in PauseScript script cannot be found!");
        if (!(aboutScene = GameObject.Find("AboutScene"))) throw new NullReferenceException("AboutScene object in PauseScript script cannot be found!");
        if (!(controlsScene = GameObject.Find("ControlsScene"))) throw new NullReferenceException("ControlsScene object in PauseScript script cannot be found!");

        prim = GameObject.FindGameObjectWithTag("Prim");
        darkPrim = GameObject.FindGameObjectWithTag("DarkPrim");

        SetInvisible(pauseScene, true);
        SetInvisible(aboutScene, false);
        SetInvisible(controlsScene, false);

        currentMenuItem = 0;
    }

    // Update is called once per frame
    void Update()
    {
        #region Highlight

        if (currentScene != 0)
            back.SetActive(false);
        else
        {
            switch (currentMenuItem)
            {
                case 0:
                    back.SetActive(false);
                    break;
                case 1:
                    back.SetActive(true);
                    MoveHighlightY(menuItemPause.transform.position.y);
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
                    MoveHighlightY(menuItemQuit.transform.position.y);
                    break;
            }
        }
        #endregion

        #region Input

        if (currentScene == 0)
        {

            #region Menu Item Selected
            //select menu item
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {

                switch (currentMenuItem)
                {
                    case 1:
                        ReturnToGame();
                        break;
                    case 2:
                        SetInvisible(pauseScene, false);
                        SetInvisible(aboutScene, false);
                        SetInvisible(controlsScene, true);
                        currentScene = 2;
                        break;
                    case 3:
                        SetInvisible(pauseScene, false);
                        SetInvisible(aboutScene, true);
                        SetInvisible(controlsScene, false);
                        currentScene = 3;
                        break;
                    case 4:
                        QuitToMainMenu();
                        break;
                }


            }
            #endregion

            else if (Input.GetKeyDown(KeyCode.DownArrow) && currentMenuItem < 4)
            {
                currentMenuItem++;
                //this.transform.audio.PlayOneShot(SoundPool.MenuClick);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) && currentMenuItem > 1)
            {
                currentMenuItem--;
                //this.transform.audio.PlayOneShot(SoundPool.MenuClick);
            }
            else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace))
            {
                ReturnToGame();
            }
        }
        //return to MainMenu
        else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace))
        {
                SetInvisible(pauseScene, true);
                SetInvisible(aboutScene, false);
                SetInvisible(controlsScene, false);
                currentScene = 0;
        }
        //switch DarkPrim's and Prim's controls
        else if (currentScene == 2 && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
        {
            #region DarkPrim's and Prim's controls

            GameObject darkPrimControls = GameObject.Find("DarkPrimControls");
            GameObject primControls = GameObject.Find("PrimControls");

            GameObject darkPrimTitle = GameObject.Find("DarkPrimTitle");
            GameObject primTitle = GameObject.Find("PrimTitle");
            GameObject controlsTitle = GameObject.Find("ControlsTitle");

            if (!isDarkControls)
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

    void MoveHighlightY(float value)
    {
        back.transform.position = new Vector3(back.transform.position.x,
                                              value,
                                              back.transform.position.z);
    }

    void SetInvisible(GameObject target, bool isEnabled)
    {
        target.renderer.enabled = isEnabled;

        Component[] a = target.GetComponentsInChildren(typeof(Renderer));
        foreach (Component b in a)
        {
            Renderer c = (Renderer)b;
            c.enabled = isEnabled;
        }

        target.transform.position = new Vector3(target.transform.position.x,
                                                target.transform.position.y,
                                                pauseCamera.transform.position.z + (isEnabled ? -7.0f : -20.0f));
    }

    void ReturnToGame()
    {
        GameObject[] cameras = GameObject.FindGameObjectsWithTag("MainCamera");
        foreach (var camera in cameras)
        {
            if(calledByPrim && camera.transform.parent.gameObject.tag == "Prim")
                camera.camera.enabled = true;
            else if (!calledByPrim && camera.transform.parent.gameObject.tag == "DarkPrim")
                camera.camera.enabled = true;

            camera.GetComponent<GUILayer>().enabled = true;
        }

        GameObject endgGameCamera = GameObject.Find("EndGameCamera");
        endgGameCamera.camera.enabled = true;

        Destroy(GameObject.FindGameObjectWithTag("PauseCamera"));

        if (calledByPrim)
        {
            prim.GetComponent<CharacterMotor>().canControl = true;
            prim.GetComponent<PrimsControls>().enabled = true;
            prim.GetComponentInChildren<UIPrim>().enabled = true;
            prim.GetComponent<EndGamePrim>().IsPause = false;
        }
        else
        {
            darkPrim.GetComponent<CharacterMotor>().canControl = true;
            darkPrim.GetComponent<DarkPrimControls>().enabled = true;
            darkPrim.GetComponentInChildren<UIDarkPrim>().enabled = true;
            darkPrim.GetComponent<EndGameDarkPrim>().IsPause = false;
        }
    }

    void QuitToMainMenu()
    {
        if (calledByPrim)
            prim.GetComponent<EndGamePrim>().CanExitGame = true;
        else
            darkPrim.GetComponent<EndGameDarkPrim>().CanExitGame = true;

        ReturnToGame();
    }

}
