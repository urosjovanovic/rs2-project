using UnityEngine;
using System.Collections;

public class PrimsFootsteps : MonoBehaviour {

	AudioSource footstepsSource;
	public AudioClip forwardFootstep;
    public float footstepDelayForward;
    public float footstepDelayBackward;
    public float footstepDelaySide;

    private float currentFootstepDelay;

    private bool movingForward = false;
    private bool movingBackward = false;
    private bool movingSide = true;

	// Use this for initialization
	void Start () {

		//footstepsSource = GetComponents (typeof(AudioSource)) [0] as AudioSource;
        footstepsSource = audio;
        StartCoroutine(PlayFootsteps());
	}

    IEnumerator PlayFootsteps()
    {
        // character controller da bi smo pristupili brzini kretanja karaktera
        CharacterController controller = GetComponent<CharacterController>();
        
        while (true)
        {
            // ako se karakter krece u napred
            if (controller.isGrounded && controller.velocity.magnitude > 0.3 && movingForward)
            {
                currentFootstepDelay = footstepDelayForward;
                footstepsSource.PlayOneShot(SoundPool.ForwardFootstep); 
            }
            else if (controller.isGrounded && controller.velocity.magnitude > 0.2 && movingBackward)
            {
                currentFootstepDelay = footstepDelayBackward;
                footstepsSource.PlayOneShot(SoundPool.BackwardFootstep);
            }
            else if (controller.isGrounded && controller.velocity.magnitude > 0.2 && movingSide)
            {
                currentFootstepDelay = footstepDelaySide;
                footstepsSource.PlayOneShot(SoundPool.SideStep);
            }

            else
            {
                footstepsSource.Stop();
            }

            //mora biti zadata pauza izmedju dva koraka
            yield return new WaitForSeconds(currentFootstepDelay);
        }
    }

	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.W))
        {
            movingForward = true;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            movingForward = false;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            movingBackward = true;
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            movingBackward = false;
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            movingSide = true;
        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            movingSide = false;
        }

	}
}




