using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallItem : Item
{
    public GameObject aim;
    public GameObject viewObject;
    protected RaycastHit hit;
    public float interactionRange;                  //설치 가능한 거리
    public Animator animator;
    

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public virtual void Additional(Player p)                //use에 들어갈 녀석
    {

    }

    public override void Active(Player p)           //view 보여주는
    {
        if (viewObject.transform.parent != null)
        {
            viewObject.transform.parent = null;
        }

        viewObject.transform.eulerAngles = new Vector3(0, 0, 0);

        if (p.GetDecryption().Distance != 0)
        {
            if (p.GetDecryption().HitDistance < interactionRange)                                          //설치가능 거리
            {
                if (!viewObject.activeSelf)
                {
                    viewObject.SetActive(true);
                }

                if (Physics.Raycast(p.GetDecryption().HitVec, Vector3.down, out hit))       //뭔가 부딪히면 바닥이 아니란거
                {
                    viewObject.transform.position = hit.point;
                }
                else //바닥이였다는거
                {
                    viewObject.transform.position = p.GetDecryption().HitVec;
                }
            }
            else
            {
                if (viewObject.activeSelf)
                {
                    viewObject.SetActive(false);
                }
            }
        }
    }
    public override void Use(Player p)              //설치
    {
        p.GetPlayerInven().RemoveItem(this);
        this.gameObject.SetActive(true);
        transform.parent = null;
        transform.position = viewObject.transform.position;
        transform.localEulerAngles = new Vector3(0, 0, 0);
        viewObject.SetActive(false);
        animator.SetTrigger("Open");
    }
    public override void LeftClick(Player p)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Use(p);
            Additional(p);
            p.GetPlayerEquip().NewWeapon(null);
            CollisionOn();
        }
    }
}
