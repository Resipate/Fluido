using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    private string lastInput, currentInput;
    public LevelGenerator lG;
    private Tweener tweener;
    private Animator anim;

    void Start()
    {
        tweener = GetComponent<Tweener>();
        anim = GetComponent<Animator>(); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) { lastInput = "up"; }
        else if (Input.GetKeyDown(KeyCode.S)) { lastInput = "down"; }
        else if (Input.GetKeyDown(KeyCode.A)) { lastInput = "left"; }
        else if (Input.GetKeyDown(KeyCode.D)) { lastInput = "right"; }

        if (!tweener.tweenExists)
        {
            bool canSwitchDir = false;
            Debug.Log(lastInput + " " + lG.GetSurroundingTiles((int)transform.position.x, (int)transform.position.y).GetTile(lastInput));
            switch (lG.GetSurroundingTiles((int)transform.position.x, (int)transform.position.y).GetTile(lastInput))
            {
                case 0: canSwitchDir = true; break;
                case 5: canSwitchDir = true; break;
                case 6: canSwitchDir = true; break;
                default: break;
            }
            int x = (int)this.transform.position.x;
            int y = (int)this.transform.position.y;

            if (canSwitchDir)
            {
                int[] newCoords = convertStringToDirection(lastInput, x, y);
                SetMovement(newCoords[0], newCoords[1]);
                currentInput = lastInput;
            }
            else
            {
                bool canContinuePath = false;
                switch (lG.GetSurroundingTiles((int)transform.position.x, (int)transform.position.y).GetTile(currentInput))
                {
                    case 0: canContinuePath = true; break;
                    case 5: canContinuePath = true; break;
                    case 6: canContinuePath = true; break;
                    default: break;
                }
                if (canContinuePath)
                {
                    int[] newCoords = convertStringToDirection(currentInput, x, y);
                    SetMovement(newCoords[0], newCoords[1]);
                }
            }
        }
    }

    private int[] convertStringToDirection(string direction, int x, int y)
    {
        switch (direction)
        {
            case "up": y += 1; break;
            case "down": y -= 1; break;
            case "left": x -= 1; break;
            case "right": x += 1; break;
            default: break;
        }
        return new int[] { x, y };
    }

    private void SetMovement(int x, int y)
    {
        if (x > transform.position.x) { AnimUtils.SetMovement(anim, "right"); }
        else if (x < transform.position.x) { AnimUtils.SetMovement(anim, "left"); }
        else if (y > transform.position.y) { AnimUtils.SetMovement(anim, "up"); }
        else if (y < transform.position.y) { AnimUtils.SetMovement(anim, "down"); }
        tweener.NewTween(this.transform, this.transform.position, new Vector3(x, y, -1), Vector3.Distance(this.transform.position, new Vector3(x, y, 0)) / 2);
    }
}
