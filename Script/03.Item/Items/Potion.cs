using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item
{

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public override void Action(Ch opponent)
    {
        base.Action(opponent);
        Instantaneous_self();
    }

    protected override void Effect()
    {
        if(effecter != null)
        {
            effecter.GetRecovery(figure, this);
        }
        //else if(paList.Count > 0)
        //{
        //    for(int i = 0; i < paList.Count; i++)
        //    {
        //        paList[i].GiveRecovery(figure, this);
        //    }
        //}
    }
}
