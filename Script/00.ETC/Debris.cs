using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    [SerializeField]
    GameObject[] m_goPrefap;
    [SerializeField]
    float m_force;
    [SerializeField]
    Vector3 m_offset;

    public void Explosion()
    {
        Debug.Log("123");
        for(int i = 0; i < m_goPrefap.Length; i++)
        {
            m_goPrefap[i].transform.parent = null;
            m_goPrefap[i].SetActive(true);
            m_goPrefap[i].GetComponent<Rigidbody>().AddExplosionForce(m_force, transform.position + m_offset, 10.0f);
        }
        gameObject.SetActive(false);
    }
    void Start()
    {

    }


    void Update()
    {
        
    }

    public void SetOffset(Vector3 vector, float power)
    {
        m_offset = vector;
        m_force = power;
    }
}
