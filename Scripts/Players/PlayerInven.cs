using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInven : MonoBehaviour
{
    public ConsumptionItem[] ConItems;
    public InstallItem[] installItems;
    public MiscellaneousItem[] miscellaneousItems;
    protected Player player;

    public Transform pocket;

    void Start()
    {
        ConItems = new ConsumptionItem[16];
        installItems = new InstallItem[16];
        miscellaneousItems = new MiscellaneousItem[16];
        player = GetComponent<Player>();
    }


    void Update()
    {
        
    }

    
    public void SettingSprite(Player p)
    {
        
    }
    public void GetItem(Item item)                              //아이템 주웠을 때 item에서
    {
        if(item.GetComponent<ConsumptionItem>() != null)
        {
            AddItem(item.GetComponent<ConsumptionItem>());
        }
        else if(item.GetComponent<EquipItem>() != null)
        {
            AddItem(item.GetComponent<EquipItem>());
        }
        else if(item.GetComponent<InstallItem>() != null)
        {
            AddItem(item.GetComponent<InstallItem>());
        }
        else if(item.GetComponent<MiscellaneousItem>() != null)
        {
            AddItem(item.GetComponent<MiscellaneousItem>());
        }
        SettingSprite(player);                                  //이미지도 적용
    }
    public void RemoveItem(Item item)
    {
        if (item == null)
            return;

        item.gameObject.SetActive(false);

        if (item.GetComponent<ConsumptionItem>() != null)           //
        {
            for (int i = 0; i < ConItems.Length; i++)
            {
                if (item == ConItems[i])
                {
                    ConItems[i].transform.parent = null;
                    ConItems[i] = null;
                    break;
                }
            }
        }
        else if (item.GetComponent<EquipItem>() != null)
        {
            for (int i = 0; i < player.GetPlayerEquip().Weapons.Length; i++)
            {
                if (player.GetPlayerEquip().Weapons[i] == item)
                {
                    player.GetPlayerEquip().Weapons[i].transform.parent = null;
                    player.GetPlayerEquip().Weapons[i] = null;
                    break;
                }
            }
        }
        else if (item.GetComponent<InstallItem>() != null)
        {
            for (int i = 0; i < installItems.Length; i++)
            {
                if (item == installItems[i])
                {
                    installItems[i].transform.parent = null;
                    installItems[i] = null;
                    break;
                }
            }
        }
        else if (item.GetComponent<MiscellaneousItem>() != null)
        {
            for (int i = 0; i < miscellaneousItems.Length; i++)
            {
                if (item == miscellaneousItems[i])
                {
                    miscellaneousItems[i].transform.parent = null;
                    miscellaneousItems[i] = null;
                    break;
                }
            }
        }
        SettingSprite(player);                                  //이미지도 적용
    }                               //아이템을 사용했을때 부모 null로 item비우기
    public void ThrowingItem(Item item)                         //아이템을 버릴때 coll과 rigid켜주고 부모 null하고 날리고 item비우고
    {
        if (item == null)
            return;

        item.gameObject.SetActive(true);

        if (item.GetComponent<ConsumptionItem>() != null)           //
        {
            for (int i = 0; i < ConItems.Length; i++)
            {
                if (item == ConItems[i])
                {
                    ConItems[i].transform.parent = null;
                    ConItems[i].RigidbodyOn();
                    ConItems[i].CollisionOn();
                    ConItems[i].GetComponent<Rigidbody>().velocity = transform.forward * 10;
                    ConItems[i] = null;
                    break;
                }
            }
        }
        else if (item.GetComponent<EquipItem>() != null)
        {
            for(int i = 0; i < player.GetPlayerEquip().Weapons.Length; i++)
            {
                if (player.GetPlayerEquip().Weapons[i] == item)
                {
                    player.GetPlayerEquip().Weapons[i].transform.parent = null;
                    player.GetPlayerEquip().Weapons[i].RigidbodyOn();
                    player.GetPlayerEquip().Weapons[i].CollisionOn();
                    player.GetPlayerEquip().Weapons[i].GetComponent<Rigidbody>().velocity = transform.forward * 10;
                    player.GetPlayerEquip().Weapons[i] = null;
                    break;
                }
            }
        }
        else if (item.GetComponent<InstallItem>() != null)
        {
            for (int i = 0; i < installItems.Length; i++)
            {
                if (item == installItems[i])
                {
                    installItems[i].transform.parent = null;
                    installItems[i].RigidbodyOn();
                    installItems[i].CollisionOn();
                    installItems[i].GetComponent<Rigidbody>().velocity = transform.forward * 10;
                    installItems[i] = null;
                    break;
                }
            }
        }
        else if (item.GetComponent<MiscellaneousItem>() != null)
        {
            for (int i = 0; i < miscellaneousItems.Length; i++)
            {
                if (item == miscellaneousItems[i])
                {
                    miscellaneousItems[i].transform.parent = null;
                    miscellaneousItems[i].RigidbodyOn();
                    miscellaneousItems[i].CollisionOn();
                    miscellaneousItems[i].GetComponent<Rigidbody>().velocity = transform.forward * 10;
                    miscellaneousItems[i] = null;
                    break;
                }
            }
        }

        if(player.GetPlayerEquip().Hand == item)
        {
            player.GetPlayerEquip().Hand = null;
        }

        SettingSprite(player);                                  //이미지도 적용
    }
    //
    protected void AddItem(ConsumptionItem item)
    {
        for(int i = 0; i < ConItems.Length; i++)
        {
            if (ConItems[i] == null)
            {
                ConItems[i] = item;
                ConItems[i].gameObject.SetActive(false);
                ConItems[i].transform.parent = pocket;
                return;
            }
        }
    }              //아이템 빈곳에 넣고 부모설정하고 active false
    protected void AddItem(EquipItem item)
    {
        player.GetPlayerEquip().NewWeapon(item);
    }
    protected void AddItem(InstallItem item)
    {
        for (int i = 0; i < installItems.Length; i++)
        {
            if (installItems[i] == null)
            {
                installItems[i] = item;
                installItems[i].gameObject.SetActive(false);
                installItems[i].transform.parent = pocket;
                return;
            }
        }
    }
    protected void AddItem(MiscellaneousItem item)
    {
        for (int i = 0; i < miscellaneousItems.Length; i++)
        {
            if (miscellaneousItems[i] == null)
            {
                miscellaneousItems[i] = item;
                miscellaneousItems[i].gameObject.SetActive(false);
                miscellaneousItems[i].transform.parent = pocket;
                return;
            }
        }
    }
}
