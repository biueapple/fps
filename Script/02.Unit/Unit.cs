using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Pa
{
    public int teamNum;
    protected UNITSTATE state;
    protected float moveSpeed = 1;      //움직임의 배율

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
        if (GetComponent<Rigidbody>() == null)
        {
            gameObject.AddComponent<Rigidbody>();
        }
        transform.GetComponent<Rigidbody>().MovePosition(transform.position + direction * Time.deltaTime * moveSpeed);
    }
    public void Jump(float power)
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + -transform.up * transform.localScale.y * 0.5f,
            new Vector3(transform.localScale.x, 0.05f, transform.localScale.z) * 0.5f, transform.rotation,  1 << 6);
        if (colliders.Length > 0)
        {
            Debug.Log("충돌");
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
}
