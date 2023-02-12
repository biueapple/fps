using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LongRangeEnemy : Enemy
{
    public RaycastHit hit;



    void Start()
    {
        Init();
    }

    void Update()
    {
        Repetition();
    }

    public override void Repetition()
    {
        if (target != null)
        {
            Angle_Calculation();
            Distance_Calculation();
            UnitRotation();

            if ((distance < attack_Range && angle < angle_Range) || Vector3.Distance(transform.position, target.transform.position) < 2.1f)  //거리 안에 들어오면 공격시도
            {
                
                if (Physics.Raycast(transform.position, (target.transform.position - transform.position).normalized, out hit))
                {
                    Debug.DrawLine(transform.position, hit.point, Color.red);

                    if (hit.transform.parent == null)
                    {
                        if (hit.transform == target.transform)
                        {
                            isAttack = true;
                        }
                    }
                    else
                    {
                        if (hit.transform.parent == target.transform)
                        {
                            isAttack = true;
                        }
                    }
                }
            }

            if (isAttack)
            {
                agent.isStopped = true;
                attack_Delay_Time += Time.deltaTime;
                if (attack_Delay_Time >= attack_Delay)         //공격에 걸리는 시간
                {
                    audioSource.PlayOneShot(audioClips[0]);
                    if (distance < attack_Range && angle < angle_Range || Vector3.Distance(transform.position, target.transform.position) < 2.1f)      //시간후에도 거리와 각도안에 있다면
                    {
                        if (Physics.Raycast(transform.position, (target.transform.position - transform.position).normalized, out hit))
                        {
                            if (hit.transform.parent == null)
                            {
                                if(hit.transform == target.transform)
                                {
                                    Debug.Log(damage);
                                    target.GetDamage(damage, this);            //대미지
                                }
                            }
                            else
                            {
                                if(hit.transform.parent == target.transform)
                                {
                                    Debug.Log(damage);
                                    target.GetDamage(damage, this);            //대미지
                                }
                            }
                        }
                    }
                    attack_Delay_Time = 0;
                    isAttack = false;
                    agent.isStopped = false;
                }
            }
            agent.destination = (target.transform.position);
        }
    }
}
