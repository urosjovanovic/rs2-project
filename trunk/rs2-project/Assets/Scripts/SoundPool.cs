using UnityEngine;
using System.Collections.Generic;

public class SoundPool : MonoBehaviour {

    public AudioClip[] publicForwardFootsteps;
    public AudioClip[] publicSprintFootsteps;

    public AudioClip publicSideStep;
    public AudioClip publicFlashlightClick;

    public static AudioClip[] forwardFootsteps;
    private static int currentForwardFootstep = 0;

    public static AudioClip sideStep;

    public static AudioClip[] sprintFootsteps;
    private static int currentSprintFootstep = 0;

    public static AudioClip flashlightClick;


    public void Start()
    {
        forwardFootsteps = publicForwardFootsteps;
        sideStep = publicSideStep;

        sprintFootsteps = publicSprintFootsteps;
        flashlightClick = publicFlashlightClick;
    }

    public static AudioClip ForwardFootstep {
        get
        {
            currentForwardFootstep = (currentForwardFootstep + 1) % 2;
            return forwardFootsteps[currentForwardFootstep];
        }
    }

    public static AudioClip SprintFootstep
    {
        get
        {
            currentSprintFootstep = (currentSprintFootstep + 1) % 2;
            return sprintFootsteps[currentSprintFootstep];
        }
    }

    public static AudioClip SideStep
    {
        get
        {
            return sideStep;
        }
    }

    public static AudioClip FlashlightClick
    {
        get
        {
            return flashlightClick;
        }
    }


}
