using UnityEngine;
using System.Collections;

public class WallBehaviour : MonoBehaviour 
{
    private bool inAnimation = false;

    public void FadeOutTo(float alpha)
    {
        StartCoroutine(fadeOut(alpha));
    }

    public void FadeIn()
    {
        StartCoroutine(fadeIn());
    }

    public void Tint(Color c)
    {
        this.renderer.material.color = c;
    }

    public void SolidColor(Color c)
    {
        this.renderer.material.shader = Shader.Find("Diffuse");
        this.renderer.material.color = c;
    }

    private IEnumerator fadeOut(float alpha)
    {
        inAnimation = true;
        while(this.renderer.material.color.a > alpha)
        {
            Color c = this.renderer.material.color;
            this.renderer.material.color = new Color(c.r, c.g, c.b, c.a - 0.025f);
            yield return new WaitForSeconds(0.001f);
        }
        inAnimation = false;
    }

    private IEnumerator fadeIn()
    {
        inAnimation = true;
        while(this.renderer.material.color.a < 1.0001f)
        {
            Color c = this.renderer.material.color;
            this.renderer.material.color = new Color(c.r, c.g, c.b, c.a + 0.025f);
            yield return new WaitForSeconds(0.001f);
        }
        inAnimation = false;
    }
}
