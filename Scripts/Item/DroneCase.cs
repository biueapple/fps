using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneCase : InstallItem
{

    void Start()
    {
        gameObject.SetActive(false);
    }


    void Update()
    {
        Repetition();
    }

    public override void Additional(Player p)
    {
        p.skill1IsOk = true;
    }
}
