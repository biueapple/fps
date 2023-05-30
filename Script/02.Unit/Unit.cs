using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : Pa
{
    public int teamNum;
    public float barricadeFindRadius;   //바리케이드를 찾을 범위
    public Barricade barricade;      //내가 지금 몸을 숨길 바리케이드
    public Vector3 barricadePosition = Vector3.zero;
    protected UNITSTATE state;
    protected float moveSpeed = 1;      //움직임의 배율
    protected int wallMask = (1 << 10) | (1 << 8);  //Wall, Opaque
    protected int unitMask = (1 << 7);              //Unit
    protected int glassMask = (1 << 9);             //유리랑 바리케이드



    public void FindBarricade(Unit target, float range)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, barricadeFindRadius, glassMask);
        List<Transform> list = new List<Transform>();
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].GetComponent<Barricade>() != null)
            {
                colliders[i].GetComponent<Barricade>().SetPoint(new Vector3(1, 1, 1), target);
                colliders[i].GetComponent<Barricade>().SetCollider(range, target, unitMask);
                if(colliders[i].GetComponent<Barricade>().GetBarricade() != Vector3.negativeInfinity)
                {
                    list.Add(colliders[i].transform);
                }
            }
        }

        int index = NearestIndex(list.ToArray());

        if(index >= 0)
        {
            barricade = list[index].GetComponent<Barricade>();
            barricadePosition = list[index].GetComponent<Barricade>().GetBarricade() + list[index].transform.position;
        }
        else
        {
            barricade = null;
            barricadePosition = Vector3.negativeInfinity;
        }
    }
    public void Rotation(Vector3 ro)
    {
        transform.localEulerAngles = ro; 
    }
    public void Movement(Vector3 direction)       //수동이동 (키보드 방향으로 이동)
    {
        if(GetComponent<Rigidbody>() == null)
        {
            gameObject.AddComponent<Rigidbody>();
            GetComponent<Rigidbody>().freezeRotation = true;
        }
        direction = transform.TransformDirection(direction);
        transform.GetComponent<Rigidbody>().MovePosition(transform.position + direction * Time.deltaTime * moveSpeed);
    }
    public void AutomaticMovement(Vector3 direction)     //자동이동 (봇이 쓰는거)
    {
        if (GetComponent<NavMeshAgent>() == null)
        {
            gameObject.AddComponent<NavMeshAgent>();
        }
        transform.GetComponent<NavMeshAgent>().destination = direction;
    }
    public void Jump(float power)
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + -transform.up * transform.localScale.y * 0.5f,
            new Vector3(transform.localScale.x, 0.05f, transform.localScale.z) * 0.5f, transform.rotation,  1 << 6);
        if (colliders.Length > 0)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0, GetComponent<Rigidbody>().velocity.z);
            GetComponent<Rigidbody>().AddForce(transform.up * power);
        }
    }

    public virtual void RunState()
    {
        state = UNITSTATE.RUN;
        moveSpeed = 4;
    }
    public virtual void StandingState()
    {
        state = UNITSTATE.NONE;
        moveSpeed = 2;
    }
    public virtual void SittingState()
    {
        state = UNITSTATE.SITTING;
        moveSpeed = 1.5f;
    }
    public Transform Nearest(Transform[] list)
    {
        if (list == null || list.Length == 0)
            return null;
        Transform close = list[0];
        for (int i = 1; i < list.Length; i++)
        {
            if (Vector3.Distance(transform.position, list[i].position) < Vector3.Distance(transform.position, close.position))
            {
                close = list[i];
            }
        }
        return close;
    }
    public int NearestIndex(Transform[] list)
    {
        if (list == null || list.Length == 0)
            return -1;
        //if (list.Length == 1)
        //    return 0;

        Transform close = list[0];
        
        int i = 1;
        for (; i < list.Length; i++)
        {
            if (Vector3.Distance(transform.position, list[i].position) < Vector3.Distance(transform.position, close.position))
            {
                close = list[i];
            }
        }
        return --i;
    }

    public bool ApproximatelyVector(Vector3 vec1, Vector3 vec2)
    {
        if(Mathf.Approximately(vec1.x, vec2.x) && Mathf.Approximately(vec1.y, vec2.y) && Mathf.Approximately(vec1.z, vec2.z))
        {
            return true;
        }
        return false;
    }
}
