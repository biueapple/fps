using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Unit
{

    public EnemyScriptble enemyScriptble;

    protected GameManager manager;
    [SerializeField]
    protected Ch target;
    [SerializeField]
    protected Vector3 direction;
    [SerializeField]
    protected Vector3 home;
    
    
    protected List<Ch> unitlist = new List<Ch>();
    protected RaycastHit hit = new RaycastHit();
    protected List<Collider> colls = new List<Collider>();  //�÷��̾� ���� ����Ʈ

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

    public void SetTarget(Ch unit)
    {
        if(unit != null)
        {
            if (target == null)
            {
                target = unit;
                direction = unit.transform.position;
            }
        }
    }

    public override float GetDamage(float f, Pa opponent)
    {
        if (opponent.GetComponent<Ch>() != null)
        {
            SetTarget(opponent.GetComponent<Ch>());
        }
        else if (opponent.GetComponent<Item>() != null)
        {
            SetTarget(opponent.GetComponent<Item>().GetUser());
        }
        return base.GetDamage(f, opponent);
    }

    protected IEnumerator Constant_Detection_Long_C()
    {
        while(true)
        {
            FindTarget();

            if (target != null)
            {
                if(barricadePosition == Vector3.zero)   //(�ٸ�����Ʈ ã��)
                {
                    FindBarricade(target, enemyScriptble.GetAttackRange());
                }
                else if((barricadePosition != Vector3.zero && barricade == null) || ZeroY(barricadePosition) == ZeroY(transform.position)) //(���ǿ� �´� �ٸ����̵尡 ������ �ٷ� ����) || (�ٸ����̵�� �̵������� ����)
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
                    }
                    direction = target.transform.position;
                    transform.LookAt(direction);
                }
                else                                                 //�ٸ����̵�� �̵�
                {
                    direction = barricadePosition;

                    if (coroutine_A != null)
                    {
                        StopCoroutine(coroutine_A);
                        coroutine_A = null;
                    }

                    if (coroutine_M == null)
                    {
                        coroutine_M = StartCoroutine(MovingC());
                    }
                }
            }
            else
            {
                //�̵�
                if (transform.position != direction && coroutine_M == null && coroutine_A == null)
                {
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
            FindTarget();

            //����
            if ( target != null && Vector3.Distance(transform.position, target.transform.position) <= enemyScriptble.GetAttackRange())
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
            if (ApproximatelyVector(ZeroY(transform.position), ZeroY(direction)))
            {
                GetComponent<NavMeshAgent>().isStopped = true;
                coroutine_M = null;
                break;
            }

            GetComponent<NavMeshAgent>().isStopped = false;
            AutomaticMovement(direction);

            yield return null;
        }
    }

    protected IEnumerator AttackC()
    {
        GetComponent<NavMeshAgent>().isStopped = true;
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

    protected void FindTarget()
    {
        unitlist.Clear();
        if (target != null)
        {
            //Ÿ���� �ִٸ� Ÿ���� �����ִ��� üũ�� �ؼ� Ÿ���� ��������
            if (Physics.Raycast(transform.position, target.transform.position - transform.position, out hit, enemyScriptble.GetSensing(), (wallMask | unitMask)))
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
                if (Physics.Raycast(transform.position, manager.GetPlayers()[i].ch.transform.position - transform.position, out hit, enemyScriptble.GetSensing(), (wallMask | unitMask)))
                {
                    if (hit.transform.GetComponent<Ch>() != null)
                    {
                        Vector3 dir = (manager.GetPlayers()[i].ch.transform.position - transform.position).normalized;

                        if (Vector3.Angle(transform.forward, dir) < enemyScriptble.GetViewAngle() * 0.5f)
                        {
                            unitlist.Add(hit.transform.GetComponent<Ch>());
                        }
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
    }

    public bool ViewPlayer()
    {
        Vector3 dir = (target.transform.position - transform.position).normalized;

        if(Physics.Raycast(transform.position, dir, out hit, enemyScriptble.GetAttackRange(), wallMask | unitMask))
        {
            return true;
        }
        return false;
    }

    public bool TracePlayer()
    {
        colls.AddRange(Physics.OverlapSphere(transform.position, enemyScriptble.GetViewAngle(), unitMask));

        if(colls.Count > 0)
        {
            for(int i = 0; i < colls.Count; i++)
            {
                if (colls[i].GetComponent<Ch>() != null)
                {
                    Vector3 dir = (colls[i].transform.position - transform.position).normalized;

                    if(Vector3.Angle(transform.forward, dir) < enemyScriptble.GetViewAngle() * 0.5f)
                    {
                        target = colls[i].GetComponent<Ch>();
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public Vector3 CirclePoint(float angle)
    {
        angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad) , 0, Mathf.Cos(angle * Mathf.Deg2Rad) );    
    }

    public Vector3Int ZeroY(Vector3 vector)
    {
        return new Vector3Int((int)vector.x,0,(int)vector.z);
    }

    
}
