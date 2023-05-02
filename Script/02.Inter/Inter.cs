using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inter : Pa
{
    protected Pa giveI;
    [Header("파괴돼는가")]
    public bool willBreak = false;
    [Header("파괴된다면 파편이 있는가")]
    public Debris debris;
    //
    public bool hard;   //단단한지
    

    public virtual void Interaction(Pa opponent)
    {
        giveI = opponent;
    }

    public void breaking(Pa opponent, float power = 50, bool strong = false)
    {
        if(hard)
        {
            if(strong)
            {
                if (willBreak)
                {
                    if (debris != null)
                    {
                        debris.SetOffset((opponent.transform.position - transform.position).normalized, power);
                        debris.Explosion();
                    }
                    Passing(opponent);
                }
            }
        }
        else
        {
            if (willBreak)
            {
                if (debris != null)
                {
                    debris.SetOffset((opponent.transform.position - transform.position).normalized, power);
                    debris.Explosion();
                }
                Passing(opponent);
            }
        }
    }

    public override float GetDamage(float f, Pa opponent)
    {
        if(willBreak)
            breaking(opponent);
        return base.GetDamage(f, opponent);
    }
}
