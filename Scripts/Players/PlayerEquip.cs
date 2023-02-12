using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquip : MonoBehaviour
{
    public Item Hand;
    public EquipItem[] Weapons;
    //
    public Transform handTransform;
    private Player player;
    void Start()
    {
        Weapons = new EquipItem[2];
        player = GetComponent<Player>();
    }

    void Update()
    {
        if(Hand != null)
        {
            Hand.OtherKey(player);
            Hand.Active(player);
            Hand.LeftClick(player);
        }
    }

    public void NewWeapon(EquipItem item)       //새로운 무기를 주웠을 때 playerInven에서 호출
    {
        for(int i = 0; i < Weapons.Length; i++)
        {
            if (Weapons[i] == null)             //장비창에 빈곳이 있을때
            {
                Weapons[i] = item;
                Hand = item;
                AllSet();
                return;
            }
        }

        player.GetPlayerInven().ThrowingItem(Hand);
        NewWeapon(item);                        //장비창에 빈곳이 없으면 손에든걸 던지고 다시 처음부터
    }
    public void NewItem(Item item)              //손에 장비가 아닌 아이템을 들때
    {
        Hand = item;
        AllSet();
    }
    public void ChangeWeapon(int index)         //무기를 바꿀때
    {
        if(index < 0 || index > Weapons.Length - 1)
        {
            Hand = null;
            AllSet();
            return;
        }
        Hand = Weapons[index];
        AllSet();
    }
    
    //기본
    protected void AllSet()                        //Hand와 Weapon들 제자리로 Equip일경우 에임도 바꿈
    {
        for (int i = 0; i < Weapons.Length; i++)
        {
            if(Weapons[i] != null)
            {
                Weapons[i].transform.parent = player.GetPlayerInven().pocket;
                Weapons[i].transform.localPosition = Vector3.zero;
                Weapons[i].transform.localEulerAngles = Vector3.zero;
                Weapons[i].gameObject.SetActive(false);
            }
        }

        if(Hand != null)
        {
            Hand.transform.parent = handTransform;
            Hand.transform.localPosition = Vector3.zero;
            Hand.transform.localEulerAngles = Vector3.zero;
            Hand.gameObject.SetActive(true);
            if (Hand.GetComponent<EquipItem>() != null)
            {
                player.SetAim(Hand.GetComponent<EquipItem>());
                Hand.GetComponent<EquipItem>().SetDelay(0);
            }
            else
            {
                player.SetAim(null);
            }
        }
        else
        {
            player.SetAim(null);
        }
    }
}
