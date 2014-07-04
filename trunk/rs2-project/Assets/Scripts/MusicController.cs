using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour {

    AudioSource music;
    GameObject DarkPrim;

    float cutoffDistance;
    float baseVolume;

	// Use this for initialization
	void Start () {
        music = this.audio;
        baseVolume = music.volume;
	}
	
	// Update is called once per frame
	void Update () {
        DarkPrim = GameObject.FindGameObjectWithTag("DarkPrim");

        if (DarkPrim)
        {
            var distance = Vector2.Distance(this.transform.position, DarkPrim.transform.position);

            music.volume = baseVolume - 1 / distance;

            if (distance < cutoffDistance)
            {
                music.volume = 0;
            }
        }
	}
}
