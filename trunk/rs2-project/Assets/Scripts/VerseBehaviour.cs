using UnityEngine;
using System.Collections;

public class VerseBehaviour : MonoBehaviour {
    
    public void FadeIn()
    {
        StartCoroutine(fadeIn());
    }

    public void Hide()
    {
        Color c = this.renderer.material.color;
        this.renderer.material.color = new Color(c.r, c.g, c.b, 0);
    }

    private IEnumerator fadeIn()
    {
        while(this.renderer.material.color.a <= 1)
        {
            Color c = this.renderer.material.color;
            this.renderer.material.color = new Color(c.r, c.g, c.b, c.a + 0.01f);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
