using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Item
{
    public Transform muzzle;
    [Header("���� ���� ���")]
    public LayerMask layerMask;
    public AudioSource audioSource;
    public AudioClip[] clips;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public override void Action(Ch opponent)    //�Ҹ��� �ִϸ��̼� ���
    {
        if(opponent != null)
        {
            user = opponent;
        }
        if(user != null)
        {
            Item item = user.GetItem(ITEM_INDEX.MM9);
            if(item != null)
            {
                user.ItemRemove(item);
                Destroy(item.gameObject);   //�̰� ��� �ٸ��� ����

                Ray ray = new Ray(muzzle.transform.position, muzzle.transform.forward);
                Instantaneous_Ray(ray, layerMask);
            }
        }
    }

    protected override void Effect()
    {
        if (effecter != null)
        {
            Debug.Log($"{effecter.name} ���� {figure} ��ŭ ������");
            effecter.GetDamage(figure, this);
        }
    }
}
