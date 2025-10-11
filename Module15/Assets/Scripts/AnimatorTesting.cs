using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTesting : MonoBehaviour
{
    public Animator animator;

    public KeyCode keyToFlyTrigger = KeyCode.A;
    public KeyCode keyToFlyBool = KeyCode.S;
    public KeyCode keyToTravelBool = KeyCode.D;

    public string flyTriggerToPlay = "Fly";
    public string flyBoolToPlay = "FlyBoolean";
    public string travelBoolToPlay = "Travel";

    private bool flyBool = false;
    private bool travelBool = false;

    private void OnValidate()
    {
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(keyToFlyTrigger))
        {
            animator.SetTrigger(flyTriggerToPlay);
        }

        if (Input.GetKeyDown(keyToFlyBool))
        {
            flyBool = !flyBool;
            animator.SetBool(flyBoolToPlay, flyBool);
        }

        if (Input.GetKeyDown(keyToTravelBool))
        {
            travelBool = !travelBool;
            animator.SetBool("Travel", travelBool);
        }
    }
}
