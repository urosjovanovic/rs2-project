using UnityEngine;
using System.Collections;

public class PauseScript : MonoBehaviour 
{

    private GameObject back;
    private GameObject menuItemPause, menuItemControls, menuItemAbout, menuItemQuit;
    private GameObject camera;
    private GameObject pauseScene, aboutScene, controlsScene;

    private int currentScene = 0;
    public int currentMenuItem;
    public bool isDarkControls = true;

    // Use this for initialization
    void Start()
    {
        Screen.showCursor = true;
        back = GameObject.Find("Shadow");
        menuItemPause = GameObject.Find("MenuItem0");
        menuItemControls = GameObject.Find("MenuItem1");
        menuItemAbout = GameObject.Find("MenuItem2");
        menuItemQuit = GameObject.Find("MenuItem3");

        pauseScene = GameObject.Find("PauseScene");
        aboutScene = GameObject.Find("AboutScene");
        controlsScene = GameObject.Find("ControlsScene");

        SetInvisible(pauseScene, true);
        SetInvisible(aboutScene, false);
        SetInvisible(controlsScene, false);

        camera = GameObject.Find("PauseCamera");
        MoveHighlightY(menuItemPause.transform.position.y);

        currentMenuItem = 1;
    }

    // Update is called once per frame
    void Update()
    {
        #region Highlight
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
        #endregion

        #region Input

        //select menu item
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            #region Menu Item Selected
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
                    PhotonNetwork.Disconnect();
                    break;
            }

            #endregion
        }
        //return to MainMenu
        else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace))
        {
            if (currentScene != 0)
            {
                SetInvisible(pauseScene, true);
                SetInvisible(aboutScene, false);
                SetInvisible(controlsScene, false);
                currentScene = 0;
            }
            else
            {
                currentScene = 0;
                ReturnToGame();
            }
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

    void OnDisconnectedFromPhoton()
    {
        Application.LoadLevel("MainMenu");
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
                                                target.transform.position.z + (isEnabled ? -10.0f : 10.0f));
    }

    void ReturnToGame()
    {
        GameObject[] cameras = GameObject.FindGameObjectsWithTag("MainCamera");
        foreach (var camera in cameras)
        {
            camera.camera.enabled = true;
            camera.GetComponent<GUILayer>().enabled = true;
        }

        GameObject endgGameCamera = GameObject.Find("EndGameCamera");
        endgGameCamera.camera.enabled = true;

        GameObject pauseCamera = GameObject.Find("PauseCamera");
        pauseCamera.GetComponent<PauseScript>().enabled = false;
        pauseCamera.GetComponent<Camera>().enabled = false;
    }

}
