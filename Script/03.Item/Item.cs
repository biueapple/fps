using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Item : Inter
{
    [Header("아이템 번호")]
    public ITEM_INDEX index;
    [Header("아이템 종류")]
    public ITEM_KIND kind;
    protected Ch user;          //누가 쓰는가
    protected Pa effecter;      //효과를 누가 받는가
    protected List<Unit> paList = new List<Unit>();
    [Header("아이템의 수치")]
    public float figure;


    public virtual void Cancel()            //user 무언가 행동을 했을때 호출해줘야해
    {

    }
    public override void Interaction(Pa opponent)
    {
        base.Interaction(opponent);
        if (opponent.GetComponent<Ch>() != null)        //아이템 상호작용은 Ch만 가능 inter 상호작용은 Unit도 가능 (Ch > Unit > Pa)
        {
            opponent.GetComponent<Ch>().ItemAcquired(this);
        }
    }
    public void Acquired(Ch opponent)
    {
        user = opponent;

        if(GetComponent<Collider>() != null)        //주운 아이템은 충돌해선 안됌
        {
            GetComponent<Collider>().enabled = false;
        }

        Passing(opponent);
    }
    public virtual void Active()        //손에 들거나 버리거나 했을때 호츌
    {
        gameObject.SetActive(true);
    }
    public virtual void Action(Ch opponent) //혹시 쓰는사람이 누군지 바꾸고 싶다면 변수로 넣기
    {
        if(opponent != null)
        {
            user = opponent;
        }
        if(user != null)
        {
            user.ItemRemove(this);
        }
        effecter = null;
        //밑에서 골라서 쓰자
    }
    protected virtual void Effect() //Parabola를 쓰는경우 effecter가 아니라 paList가 대상들임
    {
        
    }

    // action 고르는 곳

    public void Instantaneous_self()        //자기 자신이 대상
    {
        effecter = user;
        Effect();
    }
    public void Instantaneous_Ray(Ray ray, LayerMask mask)      //ray에 맞는것이 대상
    {
        RaycastHit hit = new RaycastHit();
        effecter = null;
        if(Physics.Raycast(ray, out hit, float.MaxValue, mask))
        {
            if(hit.transform.GetComponent<Pa>() != null)
            {
                effecter = hit.transform.GetComponent<Pa>();
            }
        }
        Effect();
    }

    public void Parabola(Vector3 vector, Vector3 size, float power, LayerMask layerMask, bool team)      //던져서 바닥에 닿으면 주위가 대상 vector 는 방향, power 던지는 세기, layermask는 어디에 부딫히면 발동할지,
    {                                                                                      // team은 true면 같은팀 false면 다른팀
        Throw(vector, size, power, layerMask, true, team);        //effect가 true인 이유는 이 함수는 action이 호출하는 함수라 사용효과니까
        paList.Clear();
    }

    //

    public void Throw(Vector3 vector, Vector3 size, float power, LayerMask layerMask, bool effect, bool team)        //vector는 방향 power는 세기, layer는 어디에 부딫히면 발동할지, effect는 Effect를 호출할지, team은 대상이 팀인지  
    {
        Rigidbody rb;

        if(GetComponent<Collider>() != null)
        {
            GetComponent<Collider>().enabled = true;
        }
        else
        {
            gameObject.AddComponent<Collider>();
        }
        if(GetComponent<Rigidbody>() != null)
        {
            rb = GetComponent<Rigidbody>();
        }
        else
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (vector == Vector3.zero)
        {
            rb.AddForce((transform.up + transform.forward) * power);
        }
        else
        {
            rb.AddForce(vector * power);
        }
        StartCoroutine(collision(layerMask, size, effect, team));
    }

    protected IEnumerator collision(LayerMask layerMask, Vector3 size, bool effect, bool team)    //
    {
        while(true)
        {
            Collider[] colliders = Physics.OverlapBox(transform.position + transform.up * -transform.localScale.y * 0.5f, new Vector3(transform.localScale.x, 0.1f, transform.localScale.z) * 0.5f, Quaternion.identity, layerMask);
            if (colliders.Length > 0)
            {
                Debug.Log("충돌");
                
                if(team)
                {
                    Collider[] coll = Physics.OverlapBox(transform.position, size, transform.rotation);
                    for(int i = 0; i < coll.Length; i++)
                    {
                        if (coll[i].GetComponent<Unit>() != null)
                        {
                            if(coll[i].GetComponent<Unit>().teamNum == user.teamNum)
                                paList.Add(coll[i].GetComponent<Unit>());
                        }
                    }
                }
                else
                {
                    Collider[] coll = Physics.OverlapBox(transform.position, size, transform.rotation);
                    for (int i = 0; i < coll.Length; i++)
                    {
                        if (coll[i].GetComponent<Unit>() != null)
                        {
                            if (coll[i].GetComponent<Unit>().teamNum != user.teamNum)
                                paList.Add(coll[i].GetComponent<Unit>());
                        }
                    }
                }

                if(effect)
                {
                    Effect();
                }

                Destroy(GetComponent<Rigidbody>());
                Destroy(GetComponent<ColliderBack>());

                Passing(user);  //충돌해서 발동 했으면 아이템은 죽어있어야지

                break;
            }
            yield return null;
        }
    }
    public void ZeroSet()
    {
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
    }


    public Ch GetUser()
    {
        return user;
    }
}
