using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : Interaction
{
    public Animator animator;
    private bool isOk;
    private Debris debris;
    void Start()
    {
        debris = GetComponent<Debris>();
    }

    void Update()
    {
        Repetition();
        if (gameObject.activeSelf)
        {
            Destruction();
        }
    }

    public override void InteractOK(Player p)
    {
        if(!isOk)
        {
            animator.SetTrigger("Open");
            isOk = true;
        }
        else
        {
            animator.SetTrigger("Close");
            isOk = false;
        }
    }
    public override void Destruction()
    {
        if (hp <= 0)
        {
            debris.Explosion();
        }
    }
    public override void GetDamage(float damage, BasicInformation basic)
    {
        hp -= damage;
        debris.SetOffset((basic.transform.position - transform.position).normalized);
    }
}
