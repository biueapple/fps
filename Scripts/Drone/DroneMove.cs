using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMove : MonoBehaviour
{
    public float p_Speed;
    public float p_Upspeed;
    public float p_Downspeed;
    private Vector3 velocityVector;

    void Start()
    {
        
    }


    void Update()
    {
        PlayerMoveControl();
    }

    public void PlayerMoveControl()
    {
        if (Input.GetKey(KeyCode.W))
        {
            velocityVector += transform.forward * Time.deltaTime * p_Speed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            velocityVector -= transform.forward * Time.deltaTime * p_Speed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            velocityVector += (transform.right * -1) * Time.deltaTime * p_Speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            velocityVector += transform.right * Time.deltaTime * p_Speed;
        }

        if(Input.GetKey(KeyCode.Space))
        {
            velocityVector += transform.up * Time.deltaTime * p_Upspeed;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            velocityVector += (transform.up * -1) * Time.deltaTime * p_Downspeed;
        }

        transform.GetComponent<Rigidbody>().velocity = velocityVector;



        velocityVector = Vector3.zero;
    }
}
