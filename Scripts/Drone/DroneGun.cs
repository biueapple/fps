using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneGun : BasicInformation
{
    public Transform Camposi;
    public Camera cam;

    private DroneMove droneMove;
    public GameObject[] Obj;
    private SearchEnemy searchEnemy;
    protected GameObject DroneAim;
    protected GameObject DroneSubAim;
    protected Vector3 Screen_Center;
    public float damage;
    public float delay;
    private float delaytime;
    //
    private EnemyControl enemyControl;
    public float soundSize;
    //
    protected Player player;
    public Transform muzzle;

    private AudioSource audioSource;
    public AudioClip[] audioClips;          //0 shot, 1 reload¾È¾¸, 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (droneMove.enabled == true)
        {
            if (searchEnemy.GetEnemyList().Count <= 0)
            {
                player.SetRay(null, null);
            }
            else
            {
                player.SetRay(muzzle, searchEnemy.GetEnemyList()[0].transform);
            }
        }

        delaytime += Time.deltaTime;

        if (Input.GetMouseButtonDown(1))
        {
            searchEnemy.Find();
        }
        if(searchEnemy.GetEnemyList().Count > 0)
        {
            DroneSubAim.transform.position = Camera.main.WorldToScreenPoint(searchEnemy.GetEnemyList()[0].transform.position);
            if (searchEnemy.GetEnemyList()[0].GetComponent<Enemy>().hp <= 0)
            {
                searchEnemy.GetEnemyList().RemoveAt(0);
            }
        }
        else
        {
            DroneSubAim.transform.position = Screen_Center;
        }

        if (Input.GetMouseButton(0))
        {
            if (delaytime >= delay)
            {
                audioSource.PlayOneShot(audioClips[0]);
                
                if (player.GetDecryption().interaction != null)
                {
                    player.GetDecryption().interaction.GetDamage(damage, player);
                }
                else if (player.GetDecryption().enemy != null)
                {
                    player.GetDecryption().enemy.GetDamage(damage, player);
                    player.GetAim().GetComponent<CrossHair>().hitAim.transform.position = DroneSubAim.transform.position;
                    player.GetAim().GetComponent<CrossHair>().HitAim();
                    enemyControl.SoundFind(soundSize, this);
                }

                delaytime = 0;

                enemyControl.SoundFind(soundSize, this);
            }
        }
    }

    public void Init()
    {
        droneMove = GetComponent<DroneMove>();
        searchEnemy = GetComponent<SearchEnemy>();
        Screen_Center = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
        enemyControl = FindObjectOfType<EnemyControl>();
        audioSource = GetComponent<AudioSource>();

        DroneAim = Instantiate(Resources.Load<GameObject>("Aim/DroneAim"));
        DroneAim.transform.SetParent(GameObject.Find("Canvas").transform, false);
        DroneSubAim = DroneAim.transform.GetChild(1).gameObject;
        searchEnemy.SetRect(DroneAim.transform.GetChild(0).GetComponent<RectTransform>());
    }
    public void SetPlayer(Player p)
    {
        player = p;
    }
    public DroneMove GetDroneMove()
    {
        return droneMove;
    }
    public GameObject GetDroneAim()
    {
        return DroneAim;
    }
    public void CollOn()
    {
        for (int i = 0; i < Obj.Length; i++)
        {
            Obj[i].GetComponent<Collider>().enabled = true;
        }
    }
    public void CollOff()
    {
        for (int i = 0; i < Obj.Length; i++)
        {
            Obj[i].GetComponent<Collider>().enabled = false;
        }
    }
    public void RigidOn()
    {
        gameObject.AddComponent<Rigidbody>();
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().freezeRotation = true;
    }
    public void RigidOff()
    {
        Destroy(gameObject.GetComponent<Rigidbody>());
    }
}
