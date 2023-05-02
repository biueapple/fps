using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Inter
{
    private Pa user;
    public LayerMask mask;
    public float damage;
    public GameObject flame;

    private void OnTriggerEnter(Collider other)
    {
        //Collider[] colliders = Physics.OverlapSphere(transform.position, 3f, mask);
        //for(int i = 0; i < colliders.Length; i++)
        //{
        //    colliders[i].transform.GetComponent<Unit>().GiveDamage(damage, user);
        //}
        //Destroy(this.gameObject);
        Explosion();
    }

    public void Explosion()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3f, mask);
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].transform.GetComponent<Unit>().GetDamage(damage, user);
        }
        Instantiate(flame, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public override float GetDamage(float f, Pa opponent)
    {
        Debug.Log($"{opponent}가 {transform.name}에게 {f}만큼 대미지");
        if (hp - f > 0)
        {
            hp -= f;
        }
        else
        {
            hp = 0;
            if (!immortality)
            {
                Explosion();
            }
        }
        giveD = opponent;
        return f;
    }

    public void Init(Pa pa)
    {
        user = pa;
    }
}
