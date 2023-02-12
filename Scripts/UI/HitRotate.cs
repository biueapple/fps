using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitRotate : MonoBehaviour
{
    public float t;
    public float t2;
    public bool b;
    void Start()
    {
        
    }

    
    void Update()
    {
        if(b)
        {
            t2 += Time.deltaTime;
            if(t2 >= t)
            {
                b = false;
                gameObject.SetActive(false);
            }
        }
    }
    public void Init()
    {
        t2 = 0;
        b = true;
    }
}
