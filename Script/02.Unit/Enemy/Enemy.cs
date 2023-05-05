using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class Enemy : Unit
{
    public EnemyScriptble enemyScriptble;

    protected GameManager manager;
    [SerializeField]
    protected Unit target;
    [SerializeField]
    protected Vector3 direction;
    [SerializeField]
    protected Vector3 home;
    

    protected List<Unit> unitlist = new List<Unit>();
    protected RaycastHit hit = new RaycastHit();
    protected int mask = (1 << 10) | (1 << 7) | (1 << 8);  //Wall, unit, Opaque

    protected Coroutine coroutine_M = null;
    protected Coroutine coroutine_A = null;

    public override void Init()
    {
        base.Init();
        manager = FindObjectOfType<GameManager>();
        home = direction = transform.position;
    }

    private void Awake()
    {
        Init();
        StartCoroutine(Constant_Detection_Long_C());
    }

    protected IEnumerator Constant_Detection_Long_C()
    {
        while(true)
        {
            unitlist.Clear();
            if (target != null)
            {
                //타겟이 있다면 타겟이 숨어있는지 체크를 해서 타겟을 비워줘야함
                if (Physics.Raycast(transform.position, target.transform.position - transform.position, out hit, enemyScriptble.GetSensing(), mask))
                {
                    if (hit.transform != target.transform)  //타겟이 뭔가 뒤에 있어서 안보이는 상태
                    {
                        target = null;
                    }
                }
                else
                    target = null;
            }
            else
            {
                //타겟이 없으면 타겟을 찾아야함
                for (int i = 0; i < manager.GetPlayers().Count; i++)    //모든 플레이어의 캐릭터를 찾아봄
                {
                    if (Physics.Raycast(transform.position, manager.GetPlayers()[i].ch.transform.position - transform.position, out hit, enemyScriptble.GetSensing(), mask))
                    {
                        if (hit.transform.GetComponent<Ch>() != null)
                        {
                            unitlist.Add(hit.transform.GetComponent<Ch>());
                        }
                    }
                }

                //타겟 정하기
                if (unitlist.Count > 0)
                {
                    target = unitlist[0];
                }
                else
                {
                    //모든 캐릭터가 범위 밖이거나 보이지 않으니 움직여아함
                    target = null;
                    //타겟이 마지막으로 있었던 위치 까지 갔다면 다시 원래 자리로
                    if (ZeroY(transform.position) == ZeroY(direction))
                    {
                        direction = home;
                    }
                }

                for (int i = 1; i < unitlist.Count; i++)    //가장 가까운 캐릭터로 정함
                {
                    if (Vector3.Distance(transform.position, target.transform.position) > Vector3.Distance(transform.position, unitlist[i].transform.position))
                    {
                        target = unitlist[i];
                    }
                }
            }

            if(target != null)
            {
                //공격
                if (coroutine_M != null)
                {
                    StopCoroutine(coroutine_M);
                    coroutine_M = null;
                }

                if (coroutine_A == null)
                {
                    coroutine_A = StartCoroutine(AttackC());
                    GetComponent<NavMeshAgent>().isStopped = true;
                }
                direction = target.transform.position;
                transform.LookAt(direction);
            }
            else
            {
                //이동
                if (transform.position != direction && coroutine_M == null && coroutine_A == null)
                {
                    GetComponent<NavMeshAgent>().isStopped = false;
                    coroutine_M = StartCoroutine(MovingC());
                }
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    protected IEnumerator Constant_Detection_Short_C()         //근접유닛의 감지
    {
        while (true)
        {
            unitlist.Clear();
            if (target != null)  //타겟이 있으니 타겟이 사라졌는지만 확인
            {
                if (Physics.Raycast(transform.position, target.transform.position - transform.position, out hit, enemyScriptble.GetSensing(), mask))
                {
                    if (hit.transform != target.transform)  //타겟이 뭔가 뒤에 있어서 안보이는 상태
                    {
                        target = null;
                    }
                }
                else                                       //타겟이 왜인지는 모르겠으나 없어짐
                    target = null;
            }
            else              //타겟이 없으니 타겟을 찾아야함
            {
                for (int i = 0; i < manager.GetPlayers().Count; i++)    //모든 플레이어의 캐릭터를 찾아봄
                {
                    if (Physics.Raycast(transform.position, manager.GetPlayers()[i].ch.transform.position - transform.position, out hit, enemyScriptble.GetSensing(), mask))
                    {
                        if (hit.transform.GetComponent<Ch>() != null)
                        {
                            unitlist.Add(hit.transform.GetComponent<Ch>());
                        }
                    }
                }

                //타겟 정하기
                if (unitlist.Count > 0)
                {
                    target = unitlist[0];
                }
                else
                {
                    //모든 캐릭터가 범위 밖이거나 보이지 않음
                    target = null;
                    //타겟이 마지막으로 있었던 위치 까지 갔다면 다시 원래 자리로
                    if(ZeroY( transform.position )==ZeroY( direction))
                    {
                        direction = home;
                    }
                }

                for (int i = 1; i < unitlist.Count; i++)    //가장 가까운 캐릭터로 정함
                {
                    if (Vector3.Distance(transform.position, target.transform.position) > Vector3.Distance(transform.position, unitlist[i].transform.position))
                    {
                        target = unitlist[i];
                    }
                }
            }
            
            //공격
            if( target != null && Vector3.Distance(transform.position, target.transform.position) <= enemyScriptble.GetAttackRange())
            {
                if(coroutine_M != null)
                {
                    StopCoroutine(coroutine_M);
                    coroutine_M = null;
                }
                    
                if (coroutine_A == null)
                {
                    coroutine_A = StartCoroutine(AttackC());
                    GetComponent<NavMeshAgent>().isStopped = true;
                }
            }
            //이동
            else if(transform.position != direction && coroutine_M == null && coroutine_A == null)
            {
                GetComponent<NavMeshAgent>().isStopped = false;
                coroutine_M = StartCoroutine(MovingC());
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    protected IEnumerator MovingC()
    {
        while (true)
        {
            if(target != null)
            {
                direction = target.transform.position;
            }

            AutomaticMovement(direction);

            if(transform.position == direction)
            {
                coroutine_M = null;
                break;
            }

            yield return null;
        }
    }

    protected IEnumerator AttackC()
    {
        yield return new WaitForSeconds(enemyScriptble.GetAttackLate());

        if (target != null)
        {
            if(enemyScriptble.GetEffecive() == 0)   //원거리일경우 0임
            {
                target.GetDamage(enemyScriptble.GetDamage(), this);
            }
            else
            if (Vector3.Distance(transform.position, target.transform.position) <= enemyScriptble.GetEffecive())
            {
                target.GetDamage(enemyScriptble.GetDamage(), this);
            }
            if (!target.gameObject.activeSelf)
                target = null;
        }
            
        
        coroutine_A = null;
    }



    public Vector3 ZeroY(Vector3 vector)
    {
        return new Vector3(vector.x,0,vector.z);
    }
    private void OnDrawGizmos()
    {
        if(enemyScriptble != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, enemyScriptble.GetSensing());
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, enemyScriptble.GetAttackRange());
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, enemyScriptble.GetEffecive());
        }
            
    }
}
