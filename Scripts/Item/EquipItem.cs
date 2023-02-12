using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipItem : Item
{
    protected GameObject aim;
    public float damage;
    public _WEAPONKIND weaponKind;
    public Animator animator;
    public float delay;
    protected float delay_Time;

    protected EnemyControl enemyControl;
    public float soundSize;

    public int restMagazine;        //³²ÀºÅºÃ¢
    public int currentMagazine;     //ÇöÀçÅºÃ¢
    public int maxMagazine;
    public float reloading;
    protected float reloadingTime;
    protected bool isReloading;

    protected AudioSource audioSource;
    public AudioClip[] audioClips;          //0 shot, 1 reload

    public GameObject GetAim()
    {
        return aim;
    }
    public void SetDelay(float f)
    {
        delay_Time = f;
    }
}
