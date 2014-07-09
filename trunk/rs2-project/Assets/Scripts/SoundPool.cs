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

    public AudioClip[] publicFlashlightBuzzes;
    public static AudioClip[] FlashlightBuzzes;

    public AudioClip publicDoorSound;
    public static AudioClip doorSound;

    public AudioClip publicPickupSound;
    public static AudioClip pickupSound;

    public AudioClip publicScream;

    public AudioClip[] publicWhispers;
    public static AudioClip[] Whispers
    {
        get;
        private set;
    }

    public AudioClip publicMenuQuake;
    public AudioClip publicMenuClick;
    public AudioClip publicHeavyBreathing;

    public static AudioClip Scream
    {
        get;
        private set;
    }

    public AudioClip publicEndGameTheme;
    public static AudioClip EndGameTheme { get; set; }

    public void Start()
    {
        forwardFootsteps = publicForwardFootsteps;
        sideStep = publicSideStep;

        sprintFootsteps = publicSprintFootsteps;
        flashlightClick = publicFlashlightClick;

        FlashlightBuzzes = publicFlashlightBuzzes;

        doorSound = publicDoorSound;
        pickupSound = publicPickupSound;

        Scream = publicScream;
        Whispers = publicWhispers;
        MenuQuake = publicMenuQuake;
        MenuClick = publicMenuClick;

        EndGameTheme = publicEndGameTheme;
        HeavyBreathing = publicHeavyBreathing;
    }

    public static AudioClip FlashlightBuzz
    {
        get
        {
            var rand = new System.Random();

            return FlashlightBuzzes[rand.Next(FlashlightBuzzes.Length)];
        }
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

    public static AudioClip DoorSound
    {
        get
        {
            return doorSound;
        }
    }
    public static AudioClip PickupSound
    {
        get
        {
            return pickupSound;
        }
    }


    public static AudioClip MenuQuake { get; set; }

    public static AudioClip MenuClick { get; set; }

    public static AudioClip HeavyBreathing { get; set; }
}
