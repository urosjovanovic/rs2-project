using UnityEngine;
using System.Collections.Generic;

public class SoundPool : MonoBehaviour {

    public AudioClip[] publicForwardFootsteps;
    public AudioClip[] publicSprintFootsteps;

    public AudioClip[] publicDarkForwardFootsteps;

    public AudioClip publicSideStep;
    public AudioClip publicFlashlightClick;

    public static AudioClip[] forwardFootsteps;
    private static int currentForwardFootstep = 0;

    public static AudioClip sideStep;

    public static AudioClip[] sprintFootsteps;
    private static int currentSprintFootstep = 0;

    public static AudioClip flashlightClick;

    public static AudioClip[] darkForwardFootsteps;
    private static int currentDarkForwardFootstep = 0;

    public AudioClip[] publicDarkSideStep;
    public static AudioClip[] darkSideStep;

    private static int currentDarkSideStep = 0;

    public void Start()
    {
        forwardFootsteps = publicForwardFootsteps;
        sideStep = publicSideStep;

        sprintFootsteps = publicSprintFootsteps;

        darkForwardFootsteps = publicDarkForwardFootsteps;
        darkSideStep = publicDarkSideStep;
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

    public static AudioClip DarkForwardFootstep
    {
        get
        {
            currentDarkForwardFootstep = (currentDarkForwardFootstep + 1) % 2;
            return darkForwardFootsteps[currentDarkForwardFootstep];
        }
    }

    public static AudioClip DarkSideStep
    {
        get
        {
            currentDarkSideStep = (currentDarkSideStep + 1) % 2;
            return darkSideStep[currentDarkSideStep];
        }
    }

}
