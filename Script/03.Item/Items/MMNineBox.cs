using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMNineBox : Item
{
    public int count;
    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public override void Interaction(Pa opponent)
    {
        giveI = opponent;
        if (opponent.GetComponent<Ch>() != null)        //아이템 상호작용은 Ch만 가능 inter 상호작용은 Unit도 가능 (Ch > Unit > Pa)
        {
            opponent.GetComponent<Ch>().BulletAcquired(KIND_BULLET._9MM, count);
        }
        Destroy(this.gameObject);
    }
}
