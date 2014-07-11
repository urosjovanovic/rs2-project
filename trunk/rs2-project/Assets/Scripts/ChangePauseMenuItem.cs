using UnityEngine;
using System.Collections;

public class ChangePauseMenuItem : MonoBehaviour 
{

    public int id;
    private void OnMouseOver()
    {
        GameObject cam = GameObject.Find("PauseCamera");
        cam.gameObject.GetComponent<PauseScript>().currentMenuItem = id;
    }

    private void OnMouseExit()
    {
        GameObject cam = GameObject.Find("PauseCamera");
        cam.gameObject.GetComponent<PauseScript>().currentMenuItem = 0;
    }
}
