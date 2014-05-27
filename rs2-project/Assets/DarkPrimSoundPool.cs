using UnityEngine;
using System.Collections;

public class DarkPrimSoundPool : MonoBehaviour
{

    public AudioClip[] publicDarkForwardFootsteps;

    public static AudioClip[] darkForwardFootsteps;
    private static int currentDarkForwardFootstep = 0;

    public AudioClip[] publicDarkSideStep;
    public static AudioClip[] darkSideStep;

    private static int currentDarkSideStep = 0;

    public void Start()
    {
        darkForwardFootsteps = publicDarkForwardFootsteps;
        darkSideStep = publicDarkSideStep;
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

