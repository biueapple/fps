using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    private Inventory inventory;
    public Image[] equips = new Image[2];
    public Image[] consum = new Image[3];
    public Image[] items = new Image[12];
    public Image reserve;
    private Coroutine mouseFollow;
    private ITEM_KIND kind;


    public void SetInventory(Inventory inven)
    {
        inventory = inven;
    }

    public void Open()
    {
        if (inventory != null)
        {
            if (inventory.equips[0].Count > 0)
            {
                equips[0].sprite = inventory.equips[0][0].paScriptble.GetSprite();
            }
            else
            {
                equips[0].sprite = null;
            }
            if (inventory.equips[1].Count > 0)
            {
                equips[1].sprite = inventory.equips[1][0].paScriptble.GetSprite();
            }
            else
            {
                equips[1].sprite = null;
            }
            //
            if (inventory.consum[0].Count > 0)
            {
                consum[0].sprite = inventory.consum[0][0].paScriptble.GetSprite();
            }
            else
            {
                consum[0].sprite = null;
            }

            if (inventory.consum[1].Count > 0)
            {
                consum[1].sprite = inventory.consum[1][0].paScriptble.GetSprite();
            }
            else
            {
                consum[1].sprite = null;
            }

            if (inventory.consum[2].Count > 0)
            {
                consum[2].sprite = inventory.consum[2][0].paScriptble.GetSprite();
            }
            else
            {
                consum[2].sprite = null;
            }
            //
            for (int i = 0; i < items.Length; i++)
            {
                if (inventory.items.Length > i)
                {
                    if (inventory.items[i] != null && inventory.items[i].Count > 0)
                    {
                        items[i].sprite = inventory.items[i][0].paScriptble.GetSprite();
                    }
                    else
                    {
                        items[i].sprite = null;
                    }
                }
                else
                {
                    items[i].gameObject.SetActive(false);
                }
            }

            if(inventory.reserve.Count > 0)
            {
                reserve.sprite = inventory.reserve[0].paScriptble.GetSprite();
            }
        }
    }

    public void DownImage(Image image)
    {
        if(image != null)
        {
            if(items.Contains(image))
            {
                if(inventory.items[Array.IndexOf(items, image)].Count > 0)
                {
                    inventory.selected = inventory.items[Array.IndexOf(items, image)];
                    inventory.ItemChange(inventory.reserve, inventory.items[Array.IndexOf(items, image)]);
                   
                    kind = ITEM_KIND.NONE;
                    Open();
                    reserveFollow();
                }
            }
            else if(equips.Contains(image))
            {
                if (inventory.equips[Array.IndexOf(equips, image)].Count > 0)
                {
                    inventory.selected = inventory.equips[Array.IndexOf(equips, image)];
                    inventory.ItemChange(inventory.reserve, inventory.equips[Array.IndexOf(equips, image)]);

                    if(inventory.index == Array.IndexOf(equips, image))
                    {
                        inventory.HandClear();
                    }

                    kind = ITEM_KIND.EQUIP;
                    Open();
                    reserveFollow();
                }
            }
            else if(consum.Contains(image))
            {
                if (inventory.consum[Array.IndexOf(consum, image)].Count > 0)
                {
                    inventory.selected = inventory.consum[Array.IndexOf(consum, image)];
                    inventory.ItemChange(inventory.reserve, inventory.consum[Array.IndexOf(consum, image)]);

                    kind = ITEM_KIND.CONSUMPTION;
                    Open();
                    reserveFollow();
                }
            }
        }
    }

    public void UpImage(Image image)
    {
        if(inventory.selected != null)
        {
            if (image != null)
            {
                if (items.Contains(image))
                {
                    if (inventory.items[Array.IndexOf(items, image)].Count > 0)
                    {
                        if (kind != ITEM_KIND.NONE)
                        {
                            if(kind == inventory.items[Array.IndexOf(items, image)][0].kind)
                            {
                                inventory.ItemChange(inventory.items[Array.IndexOf(items, image)], inventory.selected);
                                inventory.ItemChange(inventory.items[Array.IndexOf(items, image)], inventory.reserve);
                            }
                            else
                                EmptyReserve();
                        }
                        else
                            EmptyReserve();
                    }
                    else
                    {
                        inventory.ItemChange(inventory.items[Array.IndexOf(items, image)], inventory.reserve);
                    }
                }
                else if (equips.Contains(image))
                {
                    if (inventory.equips[Array.IndexOf(equips, image)].Count > 0)
                    {
                        if (kind == inventory.equips[Array.IndexOf(equips, image)][0].kind)
                        {
                            inventory.ItemChange(inventory.equips[Array.IndexOf(equips, image)], inventory.selected);
                            inventory.ItemChange(inventory.equips[Array.IndexOf(equips, image)], inventory.reserve);
                            if(Array.IndexOf(equips, image) == inventory.index)
                            {
                                inventory.HandChange(Array.IndexOf(equips, image));
                            }
                        }
                        else
                            EmptyReserve();
                    }
                    else
                    {
                        if (inventory.reserve[0].kind == ITEM_KIND.EQUIP)
                        {
                            inventory.ItemChange(inventory.equips[Array.IndexOf(equips, image)], inventory.reserve);
                            if (Array.IndexOf(equips, image) == inventory.index)
                            {
                                inventory.HandChange(Array.IndexOf(equips, image));
                            }
                        }
                            
                        else
                            EmptyReserve();
                    }
                }
                else if (consum.Contains(image))
                {
                    if (inventory.consum[Array.IndexOf(consum, image)].Count > 0)
                    {
                        if (kind == inventory.consum[Array.IndexOf(consum, image)][0].kind)
                        {
                            inventory.ItemChange(inventory.consum[Array.IndexOf(consum, image)], inventory.selected);
                            inventory.ItemChange(inventory.consum[Array.IndexOf(consum, image)], inventory.reserve);
                        }
                        else
                            EmptyReserve();
                    }
                    else
                    {
                        if (inventory.reserve[0].kind == ITEM_KIND.CONSUMPTION)
                            inventory.ItemChange(inventory.consum[Array.IndexOf(consum, image)], inventory.reserve);
                        else
                            EmptyReserve();
                    }
                }
                else
                {
                    EmptyReserve();
                }
            }
            else
            {
                ThrowItem(); //버리는걸로
            }

            inventory.selected = null;
            StopFollow();
            Open();
        }
    }

    public void EmptyReserve()
    {
        if(inventory.selected != null)
            inventory.ItemChange(inventory.reserve, inventory.selected);
    }

    public void ThrowItem()
    {
        if(inventory.reserve.Count > 0)
        {
            int mask = (1 << 10);
            for (int i = 0; i < inventory.reserve.Count; i++)
            {
                inventory.reserve[i].gameObject.SetActive(true);
                inventory.reserve[i].transform.position = inventory.reserve[i].GetUser().transform.position + inventory.reserve[i].GetUser().transform.forward;
                inventory.reserve[i].Throw(Vector3.zero, 0, 10, mask, null, 0.1f);
            }
        }
        inventory.reserve.Clear();
    }

    public bool MoveCk(List<Item> list1, List<Item> list2, ITEM_KIND kind)  //리스트2가 리스트1에 들어갈 수 있나 조건은 kind
    {
        if (kind == ITEM_KIND.NONE)
            return true;
        if (list2.Count > 0)
        {
            if (list2[0].kind == ITEM_KIND.NONE)
            { return true; }
            if (list2[0].kind == kind)
            { return true; }
        }
        else
            return true;
        return false;   
    }

    public void reserveFollow()
    {
        if(mouseFollow == null)
        {
            if (!reserve.gameObject.activeSelf)
                reserve.gameObject.SetActive(true);
            mouseFollow = StartCoroutine(MouseFollow(reserve.gameObject));
        }
    }
    public void StopFollow()
    {
        if(mouseFollow != null)
        {
            StopCoroutine(mouseFollow);
            mouseFollow = null;
            if (reserve.gameObject.activeSelf)
                reserve.gameObject.SetActive(false);
        }
    }
    
    private IEnumerator MouseFollow(GameObject obj)
    {
        while(true)
        {
            obj.transform.position = Input.mousePosition;
            yield return null;
        }
    }
}
