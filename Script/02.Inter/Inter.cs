using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inter : Pa
{
    protected Pa giveI;
    [Header("�ı��Ŵ°�")]
    public bool willBreak = false;
    [Header("�ı��ȴٸ� ������ �ִ°�")]
    public Debris debris;
    //
    public bool hard;   //�ܴ�����
    public bool uninter;    //��ȣ�ۿ��� ���ϴ� ������Ʈ�ϰ�� true

    private void Start()
    {
        Init();
    }

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
        opponent.GiveDamage(this, f);
        if (hp - f > 0)
        {
            hp -= f;
        }
        else
        {
            hp = 0;
            if (!immortality)
            {
                if (willBreak)
                    breaking(opponent);
                else
                    Passing(opponent);
            }
        }
        giveD = opponent;
        return f;
    }

    public bool GetHard()
    {
        return hard;
    }
}
