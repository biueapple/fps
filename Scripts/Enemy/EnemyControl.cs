using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public List<BasicInformation> Players = new List<BasicInformation>(); 
    public List<Enemy> Enemys = new List<Enemy>();
    public BasicInformation[] gameObjects;
    private void Awake()
    {
        Search();
        StartCoroutine(FindCoroutine());
    }
    public void Search()
    {
        gameObjects = GameObject.FindObjectsOfType<BasicInformation>();
        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (gameObjects[i].GetComponent<BasicInformation>() != null)
            {
                if (gameObjects[i].GetComponent<BasicInformation>().small_Frame == _SMALL_FRAME.PLAYER)
                {
                    if (!Players.Contains(gameObjects[i]))
                    {
                        Players.Add(gameObjects[i].GetComponent<BasicInformation>());
                    }
                }
                else if (gameObjects[i].GetComponent<BasicInformation>().small_Frame == _SMALL_FRAME.AI)
                {
                    if (!Enemys.Contains(gameObjects[i].GetComponent<Enemy>()))
                    {
                        Enemys.Add(gameObjects[i].GetComponent<Enemy>());
                    }
                }
            }
        }
    }
    public void Find()          //항시 발동
    {
        for (int i = 0; i < Enemys.Count; i++)
        {
            for (int j = 0; j < Players.Count; j++)
            {
                if (Players[j].gameObject.activeInHierarchy)
                {
                    if (Vector3.Distance(Enemys[i].transform.position, Players[j].transform.position) < Enemys[i].nomalSensing_range)
                    {
                        if (!Enemys[i].targets.Contains(Players[j]))
                        {
                            Enemys[i].targets.Add(Players[j]);
                        }
                    }
                }
            }
            Enemys[i].FindTarget();
        }
    }

    public void SoundFind(float sound, BasicInformation basic)
    {
        for (int i = 0; i < Enemys.Count; i++)
        {
            if(basic.gameObject.activeInHierarchy)
            {
                if(Vector3.Distance(basic.transform.position, Enemys[i].transform.position) < sound)
                {
                    if (!Enemys[i].targets.Contains(basic))
                    {
                        Enemys[i].targets.Add(basic);
                    }
                }
            }
        }
    }
    
    public void HitFind(BasicInformation player, Enemy en)      //맞을때 발동
    {
        Search();
        if (player.gameObject.activeInHierarchy)
        {
            for (int i = 0; i < Enemys.Count; i++)
            {
                if (Enemys[i].gameObject.activeInHierarchy)
                {
                    if (Vector3.Distance(Enemys[i].transform.position, en.transform.position) < en.hit_range)
                    {
                        if (!Enemys[i].targets.Contains(player))
                        {
                            Enemys[i].targets.Add(player);
                        }
                    }
                }
            }
        }
    }

    public void EndSensing()
    {
        for (int i = 0; i < Enemys.Count; i++)
        {
            for (int j = 0; j < Players.Count; j++)
            {
                if (Players[j].gameObject.activeInHierarchy)
                {
                    if (Enemys[i].targets.Count <= 0)
                        continue;
                    if (Vector3.Distance(Enemys[i].transform.position, Players[j].transform.position) > Enemys[i].endSensing_range)
                    {
                        Enemys[i].targets.Remove(Players[j]);
                    }
                }
            }
        }
    }

    public IEnumerator FindCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.0f);
            Search();
            Find();
            EndSensing();
        }
    }

    void Start()
    {

    }


    void Update()
    {
        
    }
}
