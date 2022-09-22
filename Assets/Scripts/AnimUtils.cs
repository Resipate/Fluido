using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimUtils
{
    public static void SetMovement(Animator animator, string identifier)
    {
        animator.SetBool("up", false);
        animator.SetBool("down", false);
        animator.SetBool("left", false);
        animator.SetBool("right", false);
        animator.SetBool(identifier, true);
    }
}
