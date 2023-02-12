using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(PlayerInven))]
[RequireComponent(typeof(PlayerEquip))]
[RequireComponent(typeof(PlayerSetting))]
[RequireComponent(typeof(PlayerUI))]
[RequireComponent(typeof(PlayerMouse))]
public class Player : BasicInformation
{
    protected PlayerMove playerMove;
    protected PlayerInven playerInven;
    protected PlayerEquip playerEquip;
    protected PlayerSetting playerSetting;
    protected PlayerUI playerUI;
    protected PlayerMouse playerMouse;
    //
    protected Decryption decryption;
    //
    protected Ray ray;
    protected RaycastHit hit;
    protected Ray applyRay;
    protected Transform startPoint;         //시작점
    protected Transform endPoint;           //끝점        방향은 끝 - 시작의 nomal;
    [SerializeField]
    protected Camera m_Camera;
    protected Vector3 Screen_Center;
    public Transform CamPosi;
    private bool isNomalRay;                //false가 센터 true가 받은레이
    //
    public float interactionRange;          //상호작용 거리
    //

    public bool skill1IsOk;
    //
    protected GameObject aim;
    protected GameObject activeAim;
    //
    public Animator animator;
    //
    protected EnemyControl enemyControl;
    //
    public BasicInformation lastHitTarget;      //마지막에 날 때린 적


    public void Basic_Action()
    {
        CenterRay();

        Interacting();
        InventoryKey();
        ThrowWeapon();
        ChangeWeapon();
        AppExit();
    }

    //virtual

    //Basic_Action() 기본 행동들
    public void CenterRay()         //ray쏘기 그래픽ray도 여기서 쏨
    {
        if(isNomalRay)
        {
            applyRay = new Ray(startPoint.position, endPoint.position - startPoint.position);
        }
        else
        {
            applyRay = m_Camera.ScreenPointToRay(Screen_Center);
        }

        ray = applyRay;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            decryption.DecryptionSetting(hit);
        }
        else
        {
            decryption.Init();
        }

        if(!playerMouse.isMove)
        {
            playerUI.GraphicRay();
        }
    }
    public void Interacting()       //E키로 상호작용하기
    {
        if (decryption.interaction != null)
        {
            if (decryption.Distance <= interactionRange)
            {
                if(playerMove.enabled == true)
                {
                    playerUI.E_Activation(decryption.interaction);
                    if (Input.GetKey(KeyCode.E))
                    {
                        decryption.interaction.Interact(this);
                    }
                }
                else
                {
                    playerUI.E_Deactivation();
                }
            }
            else
            {
                playerUI.E_Deactivation();
            }
        }
        else
        {
            playerUI.E_Deactivation();
        }
    }
    public void InventoryKey()      //i로 인벤창 열기(ui와 관련된것임) 
    {
        if (playerMove.enabled == true)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                playerUI.Inventory(this);
            }
        }

    }   //I키로 인벤토리 열기
    public void ThrowWeapon()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            playerInven.ThrowingItem(playerEquip.Hand);
            SetAim(null);
        }
    }       //g키로 손에 든거 던지기
    public void ChangeWeapon()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerEquip.ChangeWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerEquip.ChangeWeapon(1);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            playerEquip.ChangeWeapon(-1);
        }
    }       //1,2로 손에 무기 바꾸기
    public void SetRay(Transform start, Transform end)      //ray를 다른걸로 바꾸기
    {
        if (start == null || end == null)
        {
            isNomalRay = false;
            return;
        }
        startPoint = start;
        endPoint = end;
        isNomalRay = true;
    }
    public void AppExit()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    //
    public override void GetDamage(float damage, BasicInformation basic)
    {
        playerUI.InitHitRotate();
        hp -= damage;
        lastHitTarget = basic;
    }   //맞았을때 대미지와 때린 상대
    //
    public BasicInformation GetLastHitTarget()
    {
        return lastHitTarget;
    }                               //마지막으로 날 때린 상대 리턴 hitRotate init()
    public void SetOriginalAim(GameObject a)
    {
        aim = a;
    }
    public void SetAim(EquipItem item)
    {
        if(activeAim == null)
        {
            activeAim = aim;
        }

        activeAim.SetActive(false);

        if (item == null)
        {
            activeAim = aim;
        }
        else
        {
            activeAim = item.GetAim();
        }

        activeAim.SetActive(true);
    }                               //playerEquip에서 allset에서 호출해줌
    public void SetAim(InstallItem item, int idnex)
    {
        if (activeAim == null)
        {
            activeAim = aim;
        }

        activeAim.SetActive(false);

        if (item == null)
        {
            activeAim = aim;
        }
        else
        {
            activeAim = item.aim;
        }

        activeAim.SetActive(true);
    }       //index는 안씀 installitme에 쓰기위한
    public void SetAim(GameObject item, int index)
    {
        if (activeAim == null)
        {
            activeAim = aim;
        }

        activeAim.SetActive(false);

        if (item == null)
        {
            activeAim = aim;
        }
        else
        {
            activeAim = item;
        }

        activeAim.SetActive(true);
    }
    public GameObject GetAim()
    {
        return activeAim;
    }
    public Camera GetCam()
    {
        return m_Camera;
    }
    public PlayerMove GetPlayerMove()
    {
        return playerMove;
    }
    public Decryption GetDecryption()
    {
        return decryption;
    }
    public PlayerSetting GetPlayerSetting()
    {
        return playerSetting;
    }
    public PlayerEquip GetPlayerEquip()
    {
        return playerEquip;
    }
    public PlayerInven GetPlayerInven()
    {
        return playerInven;
    }
    public PlayerUI GetPlayerUI()
    {
        return playerUI;
    }
    public PlayerMouse GetPlayerMouse()
    {
        return playerMouse;
    }

    public virtual void Init()
    {

    }
}
