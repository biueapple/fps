using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Item
{
    public Transform muzzle;
    [Header("총을 맞을 대상")]
    public LayerMask layerMask;
    public AudioSource audioSource;
    public AudioClip[] clips;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public override void Action(Ch opponent)    //소리랑 애니메이션 재생
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
                Destroy(item.gameObject);   //이거 대신 다른거 가능

                Ray ray = new Ray(muzzle.transform.position, muzzle.transform.forward);
                Instantaneous_Ray(ray, layerMask);
            }
        }
    }

    protected override void Effect()
    {
        if (effecter != null)
        {
            Debug.Log($"{effecter.name} 에게 {figure} 만큼 데미지");
            effecter.GetDamage(figure, this);
        }
    }
}
