using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMNine : Item
{
    public float power;

    void Start()
    {
        
    }


    void Update()
    {
        GetComponent<Rigidbody>().velocity += new Vector3(0, -0.01f, 0); 
    }

    public void Fire(Vector3 dir, Ch user, float figure)
    {
        this.user = user;
        this.figure = figure;
        transform.LookAt(transform.position + dir);
        gameObject.AddComponent<Rigidbody>();
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().mass = 0.05f;
        GetComponent<Rigidbody>().AddForce(dir * power);

        Destroy(this.gameObject, 4f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform != user.transform)
        {
            if (other.GetComponent<Pa>() != null)
            {
                other.GetComponent<Pa>().GetDamage(figure, user);
            }
            Destroy(this.gameObject);
        }

        
    }
}
