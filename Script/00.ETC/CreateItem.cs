using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateItem : MonoBehaviour
{
    private Item[] Items;

    private void Awake()
    {
        Items = Resources.LoadAll<Item>("Item");
    }

    public Item GetCreateItem(ITEM_INDEX item)
    {
        for(int  i= 0; i < Items.Length; i++)
        {
            if (Items[i].index == item)
            {
                Item t = Instantiate(Items[i]);
                t.Init();
                return t;
            }
        }
        return null;
    }
    public Item[] GetCreateItem(ITEM_INDEX item, int count)
    {
        Item[] list = new Item[count];
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i].index == item)
            {
                for(int j = 0; j < count; j++)
                {
                    list[j] = Instantiate(Items[i]);
                    list[j].Init();
                }
                break;
            }
        }
        return list;
    }
}
