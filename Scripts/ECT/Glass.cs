using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : Interaction
{
    private Debris debris;
    // Start is called before the first frame update
    void Start()
    {
        debris = GetComponent<Debris>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.activeSelf)
        {
            Destruction();
        }
    }

    public override void Destruction()
    {
        if(hp <= 0)
        {
            debris.Explosion();
        }
    }
    public override void GetDamage(float damage, BasicInformation basic)
    {
        hp -= damage;
        debris.SetOffset((basic.transform.position - transform.position).normalized);
    }
}
