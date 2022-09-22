using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweener : MonoBehaviour
{
    private Tween currentTween;
    public bool tweenExists;

    private void Start()
    {
        tweenExists = false;
    }

    void Update()
    {
        if (tweenExists)
        {
            if (Vector3.Magnitude(currentTween.Target.position - currentTween.EndPos) > 0.1f)
            {
                float t = (Time.time - currentTween.StartTime) / currentTween.Duration;
                currentTween.Target.position = Vector3.Lerp(currentTween.StartPos, currentTween.EndPos, t);
            }
            else
            {
                currentTween.Target.position = currentTween.EndPos;
                tweenExists = false;
            }
        }
    }

    public void NewTween(Transform Target, Vector3 StartPos, Vector3 EndPos, float Duration)
    {
        currentTween = new Tween(Target, StartPos, EndPos, Time.time, Duration);
        tweenExists = true;
    }
}
