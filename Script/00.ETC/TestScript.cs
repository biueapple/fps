using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TestScript : MonoBehaviour
{
    public GameObject cube;
    public Vector3 vec;
    public Vector3 cross;
    public float dot;

    void Start()
    {
    }


    void Update()
    {
        vec = (cube.transform.position - transform.position).normalized;
        dot = Vector3.Dot(vec, transform.forward);
        cross = Vector3.Cross(vec, transform.forward);
    }
}
