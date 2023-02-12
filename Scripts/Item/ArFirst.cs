using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArFirst : EquipItem
{
    // Start is called before the first frame update
    void Start()
    {
        enemyControl = FindObjectOfType<EnemyControl>();
        audioSource = GetComponent<AudioSource>();

        aim = Instantiate(Resources.Load<GameObject>("Aim/GunAim"));
        aim.transform.SetParent(GameObject.Find("Canvas").transform, false);
    }

    // Update is called once per frame
    void Update()
    {
        Repetition();
        delay_Time += Time.deltaTime;

        Reloading();
    }

    public override void InteractOK(Player p)
    {
        base.InteractOK(p);
        p.animator.SetBool("Hand", false);
        p.animator.SetTrigger("Ar1");
    }

    public override void LeftClick(Player p)
    {
        if (Input.GetMouseButton(0))
        {
            if (!isReloading)        //재장전중이 아니라면
            {
                if (currentMagazine <= 0)   //총알이 0발이면 재장전
                {
                    if (restMagazine > 0)
                        ReloadingStart();
                    return;
                }

                if (delay_Time >= delay)    //총알 있으면 발사
                {
                    audioSource.PlayOneShot(audioClips[0]);

                    if (p.GetDecryption().interaction != null)
                    {
                        p.GetDecryption().interaction.GetDamage(damage, p);
                    }
                    else if (p.GetDecryption().enemy != null)
                    {
                        p.GetDecryption().enemy.GetDamage(damage, p);
                        p.GetAim().GetComponent<CrossHair>().HitAim();
                        enemyControl.SoundFind(soundSize, this);
                    }

                    delay_Time = 0;
                    currentMagazine--;
                    PlayAni(p);
                }
            }
        }
    }

    public override void OtherKey(Player p)
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadingStart();
        }
    }

    public void Reloading()
    {
        if (isReloading)
        {
            reloadingTime += Time.deltaTime;
        }
        if (reloadingTime >= reloading)
        {
            isReloading = false;
            if (restMagazine >= maxMagazine)
            {
                currentMagazine = maxMagazine;
                restMagazine -= maxMagazine;
            }
            else
            {
                currentMagazine = restMagazine;
                restMagazine = 0;
            }
            reloadingTime = 0;
            animator.SetBool("Reload", false);
        }
    }

    public void ReloadingStart()
    {
        isReloading = true;
        restMagazine += currentMagazine;
        currentMagazine = 0;
        reloadingTime = 0;
        animator.SetBool("Reload", true);

        audioSource.PlayOneShot(audioClips[1]);
    }

    public void PlayAni(Player p)
    {
        animator.SetTrigger("Shot");
    }
}
