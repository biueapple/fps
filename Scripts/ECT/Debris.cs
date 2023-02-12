using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    [SerializeField]
    GameObject m_goPrefap;
    [SerializeField]
    float m_force;
    [SerializeField]
    Vector3 m_offset;

    public void Explosion()
    {
        GameObject t_clone = Instantiate(m_goPrefap, transform.position, Quaternion.identity);
        Rigidbody[] t_rigids = t_clone.GetComponentsInChildren<Rigidbody>();
        for(int i = 0; i < t_rigids.Length; i++)
        {
            t_rigids[i].AddExplosionForce(m_force, transform.position + m_offset, 10.0f);
        }
        gameObject.SetActive(false);
    }
    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void SetOffset(Vector3 vector)
    {
        m_offset = vector;
    }
}
