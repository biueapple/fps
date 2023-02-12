using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum _ITEMKIND
{
    NONE = 0,
    CONSUMPTION,    //소비
    EQUIP,          //장착
    INSTALL,        //설치
    MISCELLANEOUS,  //기타

}

public enum _ITEMINDEX
{
    NONE = 0,
    GUN,
    DRONECASE,

}

public enum _WEAPONKIND
{
    WEAPON = 0,
    PISTOL,
    AR1,

}

public class Item : Interaction
{
    public _ITEMINDEX itemindex;
    public _ITEMKIND kind;
    //protected int number;              //몇개인지
    public float weight;            //1개당 무게가 얼마인지(총무게 = weight * number)
    public Sprite sp;
    public GameObject[] physicsObject;


    public virtual void OtherKey(Player p)
    {

    }
    public virtual void LeftClick(Player p)
    {

    }
    public override void InteractOK(Player p)
    {
        RigidbodyOff();
        CollisionOff();
        p.GetPlayerInven().GetItem(this);
    }
    public void CollisionOff()
    {
        for (int i = 0; i < physicsObject.Length; i++)
        {
            physicsObject[i].GetComponent<Collider>().enabled = false;
        }
    }
    public void CollisionOn()
    {
        for (int i = 0; i < physicsObject.Length; i++)
        {
            physicsObject[i].GetComponent<Collider>().enabled = true;
        }
    }
    public void RigidbodyOn()
    {
        gameObject.AddComponent<Rigidbody>();
    }
    public void RigidbodyOff()
    {
        Destroy(gameObject.GetComponent<Rigidbody>());
    }
}
