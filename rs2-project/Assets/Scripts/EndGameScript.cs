using UnityEngine;
using System.Collections;

public class EndGameScript : MonoBehaviour 
{

    GameObject text = null;
	// Use this for initialization
	void Start () {
        text = GameObject.Find("MainMenu");
        GameObject[] cameras = GameObject.FindGameObjectsWithTag("MainCamera");
        foreach (var camera in cameras)
            camera.camera.enabled = false;
        GameObject.Find("EndGameLight").light.enabled = true;
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach(var wall in walls)
        {
            wall.renderer.material.color = Color.grey;
            //wall.GetComponent<PathColor>().enabled = true;
        }
        GameObject[] floors = GameObject.FindGameObjectsWithTag("Floor");
        foreach(var floor in floors)
        {
            floor.renderer.material.color = Color.white;
            //floor.GetComponent<PathColor>().enabled = true;
            floor.GetComponent<PathColor>().Colorize();
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        //TODO: Ovo sam ti iskomentarisao jer sam obrisao onaj main menu text...

        //if(text.GetComponent<MouseOver>().isMouseOver && Input.GetMouseButtonDown(0))
        //{
        //    Application.LoadLevel("MainMenu");
        //}

        if(Input.GetKeyDown(KeyCode.Escape))
            Application.LoadLevel("MainMenu");
	}
}
