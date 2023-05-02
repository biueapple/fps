using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private CreateItem createItem;
    public Item hand;
    public List<Item>[] items;
    private Ch owner;

    public Inventory()
    {
        items = new List<Item>[3];
        for(int i = 0; i < 3; i++)
        {
            items[i] = new List<Item>();
        }
        createItem = GameObject.FindObjectOfType<CreateItem>();
    }
    public Inventory(int count, Ch o)
    {
        items = new List<Item>[count];
        for(int i = 0; i < count; i++)
        {
            items[i] = new List<Item>();
        }
        owner = o;
        createItem = GameObject.FindObjectOfType<CreateItem>();
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

                    return true;
                }
            }
        }

        for(int i = 0; i < items.Length; i++)
        {
            if (items[i].Count <= 0)
            {
                items[i].Add(item);

                return true;
            }
        }

        return false;
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

        return false;
    }

    public void EquipItem(Item item)
    {
        hand = item;
        item.transform.parent = owner.hand;
        item.ZeroSet();
        item.Active();

        if (item.GetComponent<Gun>() != null)
        {
            GameObject.FindObjectOfType<BulletCount>().Interlock(item.GetComponent<Gun>());
        }
        else
        {
            GameObject.FindObjectOfType<BulletCount>().Interlock(null);
        }
    }

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
    public Item GetItem(ITEMINDEX index)
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

    public void CreateItem(ITEMINDEX index, int count = 0)
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
