using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicInformation : MonoBehaviour
{
    public _LARGE_FRAME large_Frame;
    public _SMALL_FRAME small_Frame;
    public float hp;
    public float maxhp;

    public virtual void Destruction()           //�ı�������
    {

    }
    public virtual void GetDamage(float damage, BasicInformation basic)
    {

    }
}
