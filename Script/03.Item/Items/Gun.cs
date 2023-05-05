using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Item
{
    public Transform muzzle;
    [Header("총을 맞을 대상")]
    public LayerMask layerMask;
    public AudioSource audioSource;
    public AudioClip[] clips;
    public GunScriptble gunScriptble;

    public int magazine;
    protected List<MMNine> bullets = new List<MMNine>();
    public KIND_BULLET bullet_Kind;

    protected Coroutine reloadCoroutine;
    protected Coroutine RPMCoroutine;
    protected bool canShot;

    protected Animator animator;

    public ParticleSystem gunFire;

    public override void Init()
    {
        base.Init(); 
        animator = GetComponent<Animator>();    
    }

    public override void Cancel()       //user가 다른 행동을 하면 재장전중이던가 하던걸 취소해야함
    {
        if (reloadCoroutine != null)
        {
            StopCoroutine(reloadCoroutine);
            reloadCoroutine = null;
        }
        if (RPMCoroutine != null)
        {
            StopCoroutine(RPMCoroutine);
            RPMCoroutine = null;
        }
        audioSource.Stop();
    }
    public void Reload()        //장전
    {
        if (user.BulletGetCount(bullet_Kind) == 0)
            return;
        if (reloadCoroutine == null)
            reloadCoroutine = StartCoroutine(reloadC());
    }
    protected IEnumerator reloadC()      //재장전 기다리기
    {
        audioSource.PlayOneShot(clips[1]);
        yield return new WaitForSeconds(gunScriptble.ReloadTime());

        magazine = user.BulletGet(bullet_Kind, gunScriptble.MaxMagazine());

        bullets.Clear();

        CreateItem createItem = FindObjectOfType<CreateItem>();
        for (int i = 0; i < magazine; i++)
        {
            bullets.Add((createItem.GetCreateItem(ITEM_INDEX.MM9)).GetComponent<MMNine>());
            bullets[i].gameObject.SetActive(false);
        }

        reloadCoroutine = null;
    }
    protected IEnumerator RPMC()         //다음 발사까지 걸리는 시간 기다리기
    {
        yield return new WaitForSeconds(gunScriptble.GetRPM());
        animator.SetBool("Shot", false);
        canShot = true;
    }
    public override void Action(Ch opponent)    //소리랑 애니메이션 재생 (발사)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (magazine > 0 && canShot)
            {
                magazine--;

                audioSource.PlayOneShot(clips[0]);

                animator.SetBool("Shot", true);

                gunFire.Play();

                RPMCoroutine = StartCoroutine(RPMC());
                canShot = false;

                if (opponent != null)
                {
                    user = opponent;
                }
                if (user != null)
                {
                    bullets[magazine].gameObject.SetActive(true);
                    bullets[magazine].transform.position = muzzle.transform.position;
                    bullets[magazine].Fire(muzzle.transform.forward, user, figure);
                    user.ScreenShaking(gunScriptble.GetRecoil(), gunScriptble.GetRPM());
                }
            }
            else
            {
                Reload();
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
        else if (Input.GetMouseButtonDown(1))    //우클릭 조준
        {
            Debug.Log("줌");
            if (user.GetComponent<Animator>().GetBool("Zoom"))
            {
                user.GetComponent<Animator>().SetBool("Zoom", false);
            }
            else
            {
                user.GetComponent<Animator>().SetBool("Zoom", true);
            }
        }
    }

    protected override void Effect()
    {
        if (effecter != null)
        {
            Debug.Log($"{effecter.name} 에게 {figure} 만큼 데미지");
            effecter.GetDamage(figure, user);
        }
    }

    public override void Active()       //아이템을 들었을 때 자동 재장전
    {
        base.Active();
        Init();
        transform.localEulerAngles = new Vector3(0, 0, 0);
        if (magazine == 0)
        {
            Reload();
        }
        RPMCoroutine = StartCoroutine(RPMC());
        if(GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().enabled = true;
        }
    }
}
