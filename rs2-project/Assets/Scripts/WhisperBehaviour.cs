using UnityEngine;
using System.Collections;
using System;

public class WhisperBehaviour : MonoBehaviour
{

    System.Random rand;
    public double whisperProbability;
    public double whisperCooldown;
    private double currentCooldown;

    // Use this for initialization
    void Start()
    {
        rand = new System.Random();
        currentCooldown = whisperCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        double next = rand.NextDouble();

        if (currentCooldown <= 0)
        {
            if (next < whisperProbability)
            {
                this.audio.PlayOneShot(SoundPool.Whispers[rand.Next(SoundPool.Whispers.Length)]);
                currentCooldown = whisperCooldown;
            }
        }
        else
        {
            currentCooldown -= Time.deltaTime;
        }

    }
}
