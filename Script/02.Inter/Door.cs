using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Inter
{
    private Animator animator;

    void Start()
    {
        Init();
    }


    void Update()
    {
        
    }

    public override void Init()
    {
        base.Init();
        animator = GetComponent<Animator>();
    }

    public override void Interaction(Pa opponent)
    {
        base.Interaction(opponent);
        animator.SetBool("Open", !animator.GetBool("Open"));
    }


}
