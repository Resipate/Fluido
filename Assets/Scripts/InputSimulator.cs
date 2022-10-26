using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSimulator : MonoBehaviour
{
    private Tweener tweener;
    private int pathingTracker;
    private int[,] playerPathing =
    {
        {10, 4},
        {10, -4},
        {-10, -4},
        {-10, 4},
    };

    private Animator anim;

    void Start()
    {
        tweener = GetComponent<Tweener>();
        pathingTracker = 0;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!tweener.tweenExists)
        {
            SetMovement(playerPathing[pathingTracker, 0], playerPathing[pathingTracker, 1]);
            pathingTracker += 1;
            if (pathingTracker >= playerPathing.GetLength(0))
            {
                pathingTracker = 0;
            }
        }
    }

    private void SetMovement(int x, int y)
    {
        if(x > transform.position.x) { AnimUtils.SetMovement(anim, "right"); }
        else if(x < transform.position.x) { AnimUtils.SetMovement(anim, "left"); }
        else if(y > transform.position.y) { AnimUtils.SetMovement(anim, "up"); }
        else if(y < transform.position.y) { AnimUtils.SetMovement(anim, "down"); }
        tweener.NewTween(this.transform, this.transform.position, new Vector3(x, y, -1), Vector3.Distance(this.transform.position, new Vector3(x, y, 0))/2);
    }
}
