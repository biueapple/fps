using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : BasicInformation
{
    public float nomalSensing_range;      //감지범위
    public float hit_range;       //스스로 내는 소리 범위
    public float endSensing_range;  //감지가 사라지는 범위

    public float attack_Range;      //공격범위
    public float angle_Range;       //공격각도
    public float attack_Delay;      //공격시간
    protected float attack_Delay_Time;    //공격시간재기
    public float rotationSpeed;     //회전속도

    public List<BasicInformation> targets = new List<BasicInformation>();
    public BasicInformation target;
    public EnemyControl control;

    protected NavMeshAgent agent;
    //
    public float angle;
    protected float dot;
    protected float acos;
    protected Vector3 vec;
    protected Vector3 cross;

    protected bool isRotate;
    public float angleMax;
    public float angleMin;


    protected float distance;
    public float damage;

    protected bool isAttack;

    protected AudioSource audioSource;
    public AudioClip[] audioClips;

    void Start()
    {
        Init();
    }


    void Update()
    {
        Repetition();
    }

    public virtual void Init()
    {
        agent = GetComponent<NavMeshAgent>();
        control = FindObjectOfType<EnemyControl>();
        audioSource = GetComponent<AudioSource>();
    }
    public virtual void Repetition()
    {
        if (target != null)
        {
            Angle_Calculation();
            Distance_Calculation();
            UnitRotation();
            if (distance < attack_Range && angle < angle_Range)  //거리 안에 들어오면 공격시도
            {
                isAttack = true;
                
            }

            if (isAttack)
            {
                agent.isStopped = true;

                attack_Delay_Time += Time.deltaTime;
                if (attack_Delay_Time >= attack_Delay)         //공격에 걸리는 시간
                {
                    audioSource.PlayOneShot(audioClips[0]);

                    if (distance < attack_Range && angle < angle_Range)      //시간후에도 거리와 각도안에 있다면
                    {
                        Debug.Log(damage);
                        target.GetDamage(damage, this);            //대미지
                    }

                    attack_Delay_Time = 0;
                    isAttack = false;
                    agent.isStopped = false;
                }
            }
            agent.destination = (target.transform.position);
        }
    }

    public override void GetDamage(float damage, BasicInformation basic)
    {
        hp -= damage;
        control.HitFind(basic, this);
        if (hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    public void FindTarget()
    {
        if(targets.Count <= 0)
        {
            target = null;
            return;
        }
        target = targets[0];
        for(int i = 1; i < targets.Count; i++)
        {
            if(Vector3.Distance(transform.position, target.transform.position) > Vector3.Distance(transform.position, targets[i].transform.position))
            {
                target = targets[i];
            }
        }
    }

    public void Angle_Calculation()
    {
        vec = (target.transform.position - transform.position).normalized;
        dot = Vector3.Dot(vec, transform.forward);
        cross = Vector3.Cross(vec, transform.forward);
        acos = Mathf.Acos(dot);
        angle = acos * 180 / Mathf.PI;
    }
    public void Distance_Calculation()
    {
        distance = Vector3.Distance(transform.position, target.transform.position);
    }
    public void UnitRotation()
    {
        if(angle < angleMin)
        {
            isRotate = false;
        }
        else if(angle > angleMax)
        {
            isRotate = true;
        }

        if (isRotate)
        {
            if (cross.y <= 0)
            {
                //Debug.Log("오른쪽");
                transform.eulerAngles += new Vector3(0, rotationSpeed * Time.deltaTime, 0);
            }
            else
            {
                //Debug.Log("왼쪽");
                transform.eulerAngles -= new Vector3(0, rotationSpeed * Time.deltaTime, 0);
            }
        }
    }
}
