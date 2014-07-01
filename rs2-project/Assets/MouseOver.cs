using UnityEngine;
using System.Collections;

public class MouseOver : MonoBehaviour 
{

    public bool isMouseOver;
    private void OnMouseOver()
    {
        isMouseOver = true;
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
    }
}
