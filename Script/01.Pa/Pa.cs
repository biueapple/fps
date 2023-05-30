using UnityEngine;

public class Pa : MonoBehaviour
{
    public PaScriptble paScriptble;
    protected float hp;
    protected Pa giveD;
    protected Pa giveR;
    protected Pa giveP;
    [Header("죽는가")]
    public bool immortality;



    public virtual void Init()
    {
        if(paScriptble != null)
        {
            hp = paScriptble.GetHp();
        }
        
    }
    public virtual float GetDamage(float f, Pa opponent)
    {
        Debug.Log($"{opponent}가 {transform.name}에게 {f}만큼 대미지");
        opponent.GiveDamage(this, f);
        if(hp - f > 0)
        {
            hp -= f;
        }
        else
        {
            hp = 0;
            if(!immortality)
                Passing(opponent);
        }
        giveD = opponent;
        return f;
    }
    public virtual void GiveDamage(Pa victim, float f)
    {
        Debug.Log($"{transform.name}가 {victim.name}에게 {f} 만큼 대미지");
    }
    public float GetRecovery(float f, Pa opponent)
    {
        opponent.GiveRecovery();
        if(paScriptble == null)
        {
            giveR = opponent;
            return 0;
        }

        if(hp + f <= paScriptble.GetHp())
        {
            hp += f; 
        }
        else
        {
            hp = paScriptble.GetHp();
        }
        giveR = opponent;
        return f;
    }
    public virtual void GiveRecovery()
    {

    }
    public virtual bool Deactivation()               //죽어있는가
    {
        if(gameObject.activeSelf)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    protected virtual void Passing(Pa opponent)   //죽음
    {
        giveP = opponent;
        this.gameObject.SetActive(false);
    }
}
