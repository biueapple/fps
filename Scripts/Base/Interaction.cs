using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : BasicInformation
{
    protected float e_time;
    public float requiredTime;
    protected float waitingTime;
    public float requiredWaitingTime;

    public void Interact(Player p)          //보고 e키 누를때
    {
        if(waitingTime >= requiredWaitingTime)
        {
            e_time += Time.deltaTime * 2;
            if (e_time >= requiredTime)
            {
                InteractOK(p);
                e_time = 0;
                waitingTime = 0;
            }
        }
    }
    public virtual void InteractOK(Player p)    //e키를 다 눌렀을때
    {

    }
    public virtual void Active(Player p)        //들고있을때
    {

    }
    public virtual void Use(Player p)
    {

    }

    public float GetE_Time()
    {
        return e_time;
    }               //inter와 관련된
    public virtual void Repetition()
    {
        if (waitingTime <= requiredWaitingTime)
        {
            waitingTime += Time.deltaTime;
        }

        if (e_time > 0)
        {
            e_time -= Time.deltaTime;
        }
        if (e_time < 0)
        {
            e_time = 0;
        }
    }       //inter와 관련된
}
