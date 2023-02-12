using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBar : MonoBehaviour
{
    public Vector3 startPoint;
    public bool click;
    public Vector3 endPoint;
    void Start()
    {
        
    }


    void Update()
    {
        if(click)
        {
            this.transform.position = endPoint + Input.mousePosition - startPoint;
        }

        if(Input.GetMouseButtonUp(0))
        {
            click = false;
        }
    }
}
