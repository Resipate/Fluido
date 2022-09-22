using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDyingStub : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetBool("dead", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
