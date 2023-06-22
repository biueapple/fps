using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private CreateItem createItem;
    public Item hand;
    public int index;
    public List<Item>[] equips;
    public List<Item>[] consum;
    public List<Item>[] items;

    public List<Item> reserve;
    public List<Item> selected;
    private Ch owner;

    public Inventory()
    {
        items = new List<Item>[3];
        for(int i = 0; i < 3; i++)
        {
            items[i] = new List<Item>();
        }
        equips = new List<Item>[2];
        for (int i = 0; i < equips.Length; i++)
        {
            equips[i] = new List<Item>();
        }
        consum = new List<Item>[3];
        for(int i = 0; i < consum.Length; i++)
        {
            consum[i] = new List<Item>();
        }
        createItem = GameObject.FindObjectOfType<CreateItem>();
        reserve = new List<Item>();
    }
    public Inventory(int count, Ch o)
    {
        items = new List<Item>[count];
        for(int i = 0; i < count; i++)
        {
            items[i] = new List<Item>();
        }
        equips = new List<Item>[2];
        for (int i = 0; i < equips.Length; i++)
        {
            equips[i] = new List<Item>();
        }
        consum = new List<Item>[3];
        for (int i = 0; i < consum.Length; i++)
        {
            consum[i] = new List<Item>();
        }
        owner = o;
        createItem = GameObject.FindObjectOfType<CreateItem>();
        reserve = new List<Item>();
    }

    public bool ItemAcquired(Item item)
    {
        if(item.kind == ITEM_KIND.NONE)
        {
            return AddItem(item);
        }
        else if (item.kind == ITEM_KIND.EQUIP)
        {
            return AddEquip(item);
        }
        else if (item.kind == ITEM_KIND.CONSUMPTION)
        {
            return AddConsum(item);
        }
        else
        { return false; }

    }

    public bool AddItem(Item item)
    {
        for(int i = 0; i < items.Length; i++)
        {
            if (items[i].Count > 0)
            {
                if (items[i][0].index == item.index)
                {
                    items[i].Add(item);
                    item.Acquired(owner);

                    return true;
                }
            }
        }

        for(int i = 0; i < items.Length; i++)
        {
            if (items[i].Count <= 0)
            {
                items[i].Add(item);
                item.Acquired(owner);

                return true;
            }
        }

        return false;
    }

    public bool AddEquip(Item item)
    {
        for (int i = 0; i < equips.Length; i++)
        {
            if (equips[i].Count <= 0)
            {
                equips[i].Add(item);
                item.Acquired(owner);
                if (hand == null)
                {
                    HandChange(i);
                }
                return true;
            }
        }

        return AddItem(item);
    }

    public bool AddConsum(Item item)
    {
        for (int i = 0; i < consum.Length; i++)
        {
            if (consum[i].Count > 0)
            {
                if (consum[i][0].index == item.index)
                {
                    consum[i].Add(item);
                    item.Acquired(owner);

                    return true;
                }
            }
        }

        for (int i = 0; i < consum.Length; i++)
        {
            if (consum[i].Count <= 0)
            {
                consum[i].Add(item);
                item.Acquired(owner);

                return true;
            }
        }

        return AddItem(item);
    }

    public bool RemoveItem(Item item)
    {

        for(int i = 0; i < items.Length; i++)
        {
            if (items[i].Count > 0)
            {
                if (items[i].Contains(item))
                {
                    items[i].Remove(item);

                    return true;
                }
            }
        }

        for (int i = 0; i < consum.Length; i++)
        {
            if (consum[i].Count > 0)
            {
                if (consum[i].Contains(item))
                {
                    consum[i].Remove(item);

                    if (consum[i].Count == 0)
                    {
                        hand = null;
                    }

                    return true;
                }
            }
        }

        return false;
    }

    public void ItemChange(List<Item> list1, List<Item> list2)
    {
        List<Item> list = new List<Item>();
        list.AddRange(list1);
        list1.Clear();
        list1.AddRange(list2);
        list2.Clear();
        list2.AddRange(list);
    }

    public void HandCK()      //손을 다시 확인하기
    {

    }

    public void HandChange(int index)
    {
        switch (index)
        {
            case 0:
                HandClear();
                this.index = index;
                    if (equips[0].Count > 0)
                    HandSetting(equips[0][0]);
                break;
            case 1:
                HandClear();
                this.index = index;
                if (equips[1].Count > 0)
                    HandSetting(equips[1][0]);
                break;
            case 2:
                HandClear();
                this.index = index;
                if (consum[0].Count > 0)
                    HandSetting(consum[0][0]);
                break;
            case 3:
                HandClear();
                this.index = index;
                if (consum[1].Count > 0)
                    HandSetting(consum[1][0]);
                break;
            case 4:
                HandClear();
                this.index = index;
                if (consum[2].Count > 0)
                    HandSetting(consum[2][0]);
                break;

        }
    }

    private void HandSetting(Item item)
    {
        hand = item;
        hand.transform.parent = owner.hand;
        hand.ZeroSet();
        hand.Active();
    }

    public void HandClear()
    {
        if(hand != null)
        {
            hand.Cancel();
            hand.ActiveFalse();
            hand = null;
        }
    }

    //

    public Item GetItem(Item item)
    {
        for(int i = 0; i < items.Length; i++)
        {
            if (items[i].Count > 0)
            {
                if (items[i][0].index == item.index)
                {
                    return items[i][0];
                }
            }
        }

        return null;
    }
    public Item GetItem(ITEM_INDEX index)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].Count > 0)
            {
                if (items[i][0].index == index)
                {
                    return items[i][0];
                }
            }
        }

        return null;
    }

    public void CreateItem(ITEM_INDEX index, int count = 0)
    {
        if(count != 0)
        {
            Item[] itme = createItem.GetCreateItem(index, count);
            for(int i = 0; i < itme.Length; i++)
            {
                AddItem(itme[i]);
                itme[i].Acquired(owner);
            }
        }
        else
        {
            Item item = createItem.GetCreateItem(index);
            AddItem(item);
            item.Acquired(owner);
        }
    }
}
