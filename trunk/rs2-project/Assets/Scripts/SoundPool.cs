using UnityEngine;
using System.Collections.Generic;

public class SoundPool : MonoBehaviour {

	// Use this for initialization

    public List<AudioClip> soundClips;
    public static List<AudioClip> staticSoundClips;

    private enum SoundIndexes { ForwardFootstep = 0, BackwardFootstep = 1, SideStep = 2, Heartbeat = 3 };

	void Start () {
        staticSoundClips = soundClips;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static AudioClip ForwardFootstep {
        get {
            return staticSoundClips[ (int)SoundIndexes.ForwardFootstep];
        }
    }

    public static AudioClip BackwardFootstep
    {
        get
        {
            return staticSoundClips[(int)SoundIndexes.BackwardFootstep];
        }

    }

    public static AudioClip SideStep
    {
        get
        {
            return staticSoundClips[(int)SoundIndexes.SideStep];
        }
    }

    public static AudioClip Heartbeat
    {
        get
        {
            return staticSoundClips[(int)SoundIndexes.Heartbeat];
        }
    }

}
