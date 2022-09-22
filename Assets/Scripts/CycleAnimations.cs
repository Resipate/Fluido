using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleAnimations : MonoBehaviour
{
    public Animator[] enemiesAnim;
    private float timer = 0;
    private int seconds = 0;
    private bool firstFrameSinceSecond = true;
    void Update()
    {
        timer += Time.deltaTime;
        if(timer - seconds >= 1)
        {
            seconds += 1;
            firstFrameSinceSecond = true;
        }

        if (firstFrameSinceSecond)
        {
            switch (seconds)
            {
                case 0:     //up
                    updateMovementMass("up");
                    firstFrameSinceSecond = false;
                    break;
                case 3:     //right
                    updateMovementMass("right");
                    firstFrameSinceSecond = false;
                    break;
                case 6:     //down
                    updateMovementMass("down");
                    firstFrameSinceSecond = false;
                    break;
                case 9:     //left
                    updateMovementMass("left");
                    firstFrameSinceSecond = false;
                    break;
                case 12:    //panic up
                    SetState("panic", true);
                    updateMovementMass("up");
                    firstFrameSinceSecond = false;
                    break;
                case 15:    //panic right
                    updateMovementMass("right");
                    firstFrameSinceSecond = false;
                    break;
                case 18:    //panic down
                    updateMovementMass("down");
                    firstFrameSinceSecond = false;
                    break;
                case 21:    //panic left
                    updateMovementMass("left");
                    firstFrameSinceSecond = false;
                    break;
                case 24:    //dead
                    SetState("dead", true);
                    firstFrameSinceSecond = false;
                    break;
                case 27:    //Start reborn
                    SetState("dead", false);
                    SetState("panic", false);
                    SetState("reborn", true);
                    firstFrameSinceSecond = false;
                    break;
                case 30:    //End Reborn
                    SetState("reborn", false);
                    timer = 0;
                    seconds = 0;
                    break;
                default:
                    break;
            }
        }
    }

    private void updateMovementMass(string identifier)
    {
        foreach(Animator anim in enemiesAnim)
        {
            AnimUtils.SetMovement(anim, identifier);
        }
    }

    private void SetState(string indentifier, bool boolean)
    {
        foreach(Animator anim in enemiesAnim)
        {
            anim.SetBool(indentifier, boolean);
        }
    }
}
