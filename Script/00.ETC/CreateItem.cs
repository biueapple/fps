using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateItem : MonoBehaviour
{
    public Item[] Items;

    public Item GetCreateItem(ITEMINDEX item)
    {
        for(int  i= 0; i < Items.Length; i++)
        {
            if (Items[i].index == item)
            {
                return Instantiate(Items[i]);
            }
        }
        return null;
    }
    public Item[] GetCreateItem(ITEMINDEX item, int count)
    {
        Item[] list = new Item[count];
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i].index == item)
            {
                for(int j = 0; j < count; j++)
                {
                    list[j] = Instantiate(Items[i]);
                }
                break;
            }
        }
        return list;
    }
}
