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
        if (opponent.GetComponent<Ch>() != null)        //������ ��ȣ�ۿ��� Ch�� ���� inter ��ȣ�ۿ��� Unit�� ���� (Ch > Unit > Pa)
        {
            opponent.GetComponent<Ch>().BulletAcquired(KIND_BULLET._9MM, count);
        }
        Destroy(this.gameObject);
    }
}
