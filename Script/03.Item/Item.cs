using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Item : Inter
{
    [Header("������ ��ȣ")]
    public ITEM_INDEX index;
    [Header("������ ����")]
    public ITEM_KIND kind;
    protected Ch user;          //���� ���°�
    protected Pa effecter;      //ȿ���� ���� �޴°�
    protected List<Unit> paList = new List<Unit>();
    [Header("�������� ��ġ")]
    public float figure;


    public virtual void Cancel()            //user ���� �ൿ�� ������ ȣ���������
    {

    }
    public override void Interaction(Pa opponent)
    {
        base.Interaction(opponent);
        if (opponent.GetComponent<Ch>() != null)        //������ ��ȣ�ۿ��� Ch�� ���� inter ��ȣ�ۿ��� Unit�� ���� (Ch > Unit > Pa)
        {
            opponent.GetComponent<Ch>().ItemAcquired(this);
        }
    }
    public void Acquired(Ch opponent)
    {
        user = opponent;

        if(GetComponent<Collider>() != null)        //�ֿ� �������� �浹�ؼ� �ȉ�
        {
            GetComponent<Collider>().enabled = false;
        }

        Passing(opponent);
    }
    public virtual void Active()        //�տ� ��ų� �����ų� ������ ȣ��
    {
        gameObject.SetActive(true);
    }
    public virtual void Action(Ch opponent) //Ȥ�� ���»���� ������ �ٲٰ� �ʹٸ� ������ �ֱ�
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
        //�ؿ��� ��� ����
    }
    protected virtual void Effect() //Parabola�� ���°�� effecter�� �ƴ϶� paList�� ������
    {
        
    }

    // action ���� ��

    public void Instantaneous_self()        //�ڱ� �ڽ��� ���
    {
        effecter = user;
        Effect();
    }
    public void Instantaneous_Ray(Ray ray, LayerMask mask)      //ray�� �´°��� ���
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

    public void Parabola(Vector3 vector, Vector3 size, float power, LayerMask layerMask, bool team)      //������ �ٴڿ� ������ ������ ��� vector �� ����, power ������ ����, layermask�� ��� �΋H���� �ߵ�����,
    {                                                                                      // team�� true�� ������ false�� �ٸ���
        Throw(vector, size, power, layerMask, true, team);        //effect�� true�� ������ �� �Լ��� action�� ȣ���ϴ� �Լ��� ���ȿ���ϱ�
        paList.Clear();
    }

    //

    public void Throw(Vector3 vector, Vector3 size, float power, LayerMask layerMask, bool effect, bool team)        //vector�� ���� power�� ����, layer�� ��� �΋H���� �ߵ�����, effect�� Effect�� ȣ������, team�� ����� ������  
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
                Debug.Log("�浹");
                
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

                Passing(user);  //�浹�ؼ� �ߵ� ������ �������� �׾��־����

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
