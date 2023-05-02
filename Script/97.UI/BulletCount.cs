using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletCount : MonoBehaviour
{
    public Text text;
    private Gun gun;

    public void Interlock(Gun gnu)
    {
        this.gun = gnu;
    }

    private void Update()
    {
        if(gun != null)
        {
            text.text = gun.magazine + "/" + gun.GetUser().BulletGetCount(gun.bullet_Kind);
        }
        else
        {
            text.text = "";
        }
    }
}
