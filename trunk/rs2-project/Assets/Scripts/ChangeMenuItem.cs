using UnityEngine;
using System.Collections;

public class ChangeMenuItem : MonoBehaviour
{
    public int id;

    private void OnMouseEnter()
    {
        this.transform.parent.audio.PlayOneShot(SoundPool.MenuClick);
    }

    private void OnMouseOver()
    {
        GameObject cam = GameObject.Find("Quad");
        cam.gameObject.GetComponent<MenuScript>().currentMenuItem = id;
    }

    private void OnMouseExit()
    {
        GameObject cam = GameObject.Find("Quad");
        cam.gameObject.GetComponent<MenuScript>().currentMenuItem = 0;
    }
}
