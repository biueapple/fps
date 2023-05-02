using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.UIElements;
using UnityEngine;

public class Ch : Unit
{
    public NewBehaviourScript player;
    protected Inventory inventory;
    public Transform hand;

    private int _9mmCount;


    public override void Init()
    {
        base.Init();
        InventoryInit();
    }
    public void InventoryInit()
    {
        inventory = new Inventory(3, this);
    }

    public override void GiveDamage(Pa victim)
    {
        player.GiveDamage(victim.gameObject);
    }

    public void ScreenShaking(float figure, float t)
    {
        if (player != null)
        {
            StartCoroutine(ScreenShakingC(figure, t)); 
        }
    }
    protected IEnumerator ScreenShakingC(float figure, float t)
    {
        float f = 0;
        while (true)
        {
            player.AddRotY(figure);

            yield return null;
            f += Time.deltaTime;
            if (f > t)
                break;
        }
    }

    public void InteractingRay(Ray ray, LayerMask mask, KeyCode code)
    {
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, float.MaxValue, mask))
        {
            if (hit.transform.GetComponent<Inter>() != null)
            {
                if (Input.GetKeyDown(code))
                {
                    hit.transform.GetComponent<Inter>().Interaction(this);
                }
            }
        }
    }
    public void ItemCreateAcquired(ITEMINDEX index, int count = 0)
    {
        inventory.CreateItem(index, count);
    }
    public void ItemAcquired(Item item)
    {
        if (inventory.AddItem(item))
        {
            item.Acquired(this);
        }
        if(item.kind == ITEMKIND.EQUIP)
        {
            inventory.EquipItem(item);
        }
    }
    public void BulletAcquired(KIND_BULLET kind, int count)
    {
        switch (kind)
        {
            case KIND_BULLET._9MM:
                _9mmCount += count;
                break;
        }
    }
    public int BulletGet(KIND_BULLET kind, int count)
    {
        int minus = 0;
        switch(kind) 
        {
            case KIND_BULLET._9MM:
                if(_9mmCount > count)
                {
                    minus = count;
                    _9mmCount -= count;
                }
                else
                {
                    minus = _9mmCount;
                    _9mmCount = 0;
                }
                break;
        }

        return minus;
    }
    public int BulletGetCount(KIND_BULLET kind)
    {
        switch (kind)
        {
            case KIND_BULLET._9MM:
                return _9mmCount;
        }
        return 0;
    }
    public void ItemUse()
    {
        if(inventory.hand != null)
        {
            inventory.hand.Action(this);    //null·Î ÇØµµ ‰Î
        }
    }
    public void ItemRemove(Item item)
    {
        inventory.RemoveItem(item);
    }
    public Item GetItem(ITEMINDEX item)
    {
        return inventory.GetItem(item);
    }
    public virtual void Skill()
    {

    }
}
