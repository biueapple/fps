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
                //Ÿ���� �ִٸ� Ÿ���� �����ִ��� üũ�� �ؼ� Ÿ���� ��������
                if (Physics.Raycast(transform.position, target.transform.position - transform.position, out hit, enemyScriptble.GetSensing(), mask))
                {
                    if (hit.transform != target.transform)  //Ÿ���� ���� �ڿ� �־ �Ⱥ��̴� ����
                    {
                        target = null;
                    }
                }
                else
                    target = null;
            }
            else
            {
                //Ÿ���� ������ Ÿ���� ã�ƾ���
                for (int i = 0; i < manager.GetPlayers().Count; i++)    //��� �÷��̾��� ĳ���͸� ã�ƺ�
                {
                    if (Physics.Raycast(transform.position, manager.GetPlayers()[i].ch.transform.position - transform.position, out hit, enemyScriptble.GetSensing(), mask))
                    {
                        if (hit.transform.GetComponent<Ch>() != null)
                        {
                            unitlist.Add(hit.transform.GetComponent<Ch>());
                        }
                    }
                }

                //Ÿ�� ���ϱ�
                if (unitlist.Count > 0)
                {
                    target = unitlist[0];
                }
                else
                {
                    //��� ĳ���Ͱ� ���� ���̰ų� ������ ������ ����������
                    target = null;
                    //Ÿ���� ���������� �־��� ��ġ ���� ���ٸ� �ٽ� ���� �ڸ���
                    if (ZeroY(transform.position) == ZeroY(direction))
                    {
                        direction = home;
                    }
                }

                for (int i = 1; i < unitlist.Count; i++)    //���� ����� ĳ���ͷ� ����
                {
                    if (Vector3.Distance(transform.position, target.transform.position) > Vector3.Distance(transform.position, unitlist[i].transform.position))
                    {
                        target = unitlist[i];
                    }
                }
            }

            if(target != null)
            {
                //����
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
                //�̵�
                if (transform.position != direction && coroutine_M == null && coroutine_A == null)
                {
                    GetComponent<NavMeshAgent>().isStopped = false;
                    coroutine_M = StartCoroutine(MovingC());
                }
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    protected IEnumerator Constant_Detection_Short_C()         //���������� ����
    {
        while (true)
        {
            unitlist.Clear();
            if (target != null)  //Ÿ���� ������ Ÿ���� ����������� Ȯ��
            {
                if (Physics.Raycast(transform.position, target.transform.position - transform.position, out hit, enemyScriptble.GetSensing(), mask))
                {
                    if (hit.transform != target.transform)  //Ÿ���� ���� �ڿ� �־ �Ⱥ��̴� ����
                    {
                        target = null;
                    }
                }
                else                                       //Ÿ���� �������� �𸣰����� ������
                    target = null;
            }
            else              //Ÿ���� ������ Ÿ���� ã�ƾ���
            {
                for (int i = 0; i < manager.GetPlayers().Count; i++)    //��� �÷��̾��� ĳ���͸� ã�ƺ�
                {
                    if (Physics.Raycast(transform.position, manager.GetPlayers()[i].ch.transform.position - transform.position, out hit, enemyScriptble.GetSensing(), mask))
                    {
                        if (hit.transform.GetComponent<Ch>() != null)
                        {
                            unitlist.Add(hit.transform.GetComponent<Ch>());
                        }
                    }
                }

                //Ÿ�� ���ϱ�
                if (unitlist.Count > 0)
                {
                    target = unitlist[0];
                }
                else
                {
                    //��� ĳ���Ͱ� ���� ���̰ų� ������ ����
                    target = null;
                    //Ÿ���� ���������� �־��� ��ġ ���� ���ٸ� �ٽ� ���� �ڸ���
                    if(ZeroY( transform.position )==ZeroY( direction))
                    {
                        direction = home;
                    }
                }

                for (int i = 1; i < unitlist.Count; i++)    //���� ����� ĳ���ͷ� ����
                {
                    if (Vector3.Distance(transform.position, target.transform.position) > Vector3.Distance(transform.position, unitlist[i].transform.position))
                    {
                        target = unitlist[i];
                    }
                }
            }
            
            //����
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
            //�̵�
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
            if(enemyScriptble.GetEffecive() == 0)   //���Ÿ��ϰ�� 0��
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
