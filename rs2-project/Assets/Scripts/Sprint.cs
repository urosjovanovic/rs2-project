using UnityEngine;
using System.Collections;

/// <summary>
/// Makes the character sprint when holding shift
/// </summary>
public class Sprint : MonoBehaviour
{
   
    public float sprintSpeed;
    private float walkSpeed;

    private CharacterMotor motor;
    private Transform tr;

    void Start()
    {
        motor = GetComponent<CharacterMotor>();

        //set walking speed to default character motor walking speed
        walkSpeed = motor.movement.maxForwardSpeed;

        tr = transform;
    }

    // check every frame
    void FixedUpdate()
    {
        // default speed
        float speed = walkSpeed;
        
        // if shift is pressed change the speed
        if ((Input.GetKey("left shift") || Input.GetKey("right shift")) && motor.grounded)
        {
            speed = sprintSpeed;
        }

        // set the speed
        motor.movement.maxForwardSpeed = speed; 
    }
}
