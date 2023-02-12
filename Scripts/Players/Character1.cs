using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character1 : Player
{
    public Item DroneCase;
    public DroneGun Drone;
    

    void Start()
    {
        Init();
    }


    void Update()
    {
        Basic_Action();
        QSkill();
    }

    public override void Init()
    {
        playerMove = GetComponent<PlayerMove>();
        playerInven = GetComponent<PlayerInven>();
        playerEquip = GetComponent<PlayerEquip>();
        playerSetting = GetComponent<PlayerSetting>();
        playerUI = GetComponent<PlayerUI>();
        playerMouse = GetComponent<PlayerMouse>();

        decryption = gameObject.AddComponent<Decryption>();
        Screen_Center = new Vector3(m_Camera.pixelWidth / 2, m_Camera.pixelHeight / 2);
        hit = new RaycastHit();
        enemyControl = FindObjectOfType<EnemyControl>();
        playerUI.SetBasicUnit(this);
        m_Camera = Camera.main;
        m_Camera.transform.parent = CamPosi;
        m_Camera.transform.localPosition = Vector3.zero;
        m_Camera.transform.localEulerAngles = Vector3.zero;
    }

    public void QSkill()        //설치한 후냐 아니냐로 나뉨
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if (!skill1IsOk)         //설치하기 전
            {
                playerEquip.NewItem(DroneCase);
            }
            else                    //설치한 후
            {
                if (GetComponent<PlayerMove>().enabled == true)      //드론 조종하지 않는 상태
                {
                    //playermove끄고 playermouse 드론으로 바꾸고 에임 드론으로 바꾸고 드론move켜주고 드론 리지드바디 켜주고 드론 collon해주고 캠위치 바꾸고
                    Drone.GetComponent<DroneGun>().enabled = true;
                    Drone.GetComponent<DroneGun>().Init();

                    GetComponent<PlayerMove>().enabled = false;
                    playerMouse.SetCamObject(Drone.gameObject);
                    playerMouse.isPlayer = false;

                    

                    m_Camera.transform.parent = Drone.Camposi;
                    m_Camera.transform.localPosition = Vector3.zero;
                    m_Camera.transform.localEulerAngles = Vector3.zero;

                    Drone.CollOn();
                    Drone.RigidOn();

                    playerEquip.NewItem(null);
                    SetAim(Drone.GetDroneAim(), 0);
                    Drone.GetDroneMove().enabled = true;
                    Drone.transform.parent = null;
                    Drone.SetPlayer(this);

                    playerUI.SetBasicUnit(Drone);
                }
                else                                                //드론 조종중인 상태
                {
                    //playermove키고 playermouse 플레이어으로 바꾸고 에임 드론으로 바꾸고 드론move켜주고 드론 리지드바디 켜주고 드론 collon해주고 캠위치 바꾸고
                    SetRay(null, null);

                    Drone.GetDroneMove().enabled = false;
                    GetComponent<PlayerMove>().enabled = true;
                    playerMouse.isPlayer = true;

                    m_Camera.transform.parent = CamPosi;
                    m_Camera.transform.localPosition = Vector3.zero;
                    m_Camera.transform.localEulerAngles = Vector3.zero;

                    Drone.CollOff();
                    Drone.RigidOff();
                    SetAim(null);
                    playerMouse.SetCamObject(null);

                    Drone.GetComponent<DroneGun>().enabled = false;

                    playerUI.SetBasicUnit(this);
                }
            }
        }
    }
}
