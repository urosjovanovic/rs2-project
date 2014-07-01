﻿using UnityEngine;
using System.Collections;

public class EndGameScript : MonoBehaviour 
{
    public bool PrimWin;
	// Use this for initialization
	void Start () 
    {
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

        if(PrimWin)
        {
            if (this.gameObject.tag == "Prim")
                GameObject.Find("Win").GetComponent<MeshRenderer>().enabled = true;
            else if(this.gameObject.tag == "DarkPrim")
                GameObject.Find("Lose").GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            if (this.gameObject.tag == "DarkPrim")
                GameObject.Find("Win").GetComponent<MeshRenderer>().enabled = true;
            else if (this.gameObject.tag == "Prim")
                GameObject.Find("Lose").GetComponent<MeshRenderer>().enabled = true;
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.LoadLevel("MainMenu");
	}
}
