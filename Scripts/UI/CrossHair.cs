using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    public GameObject hitAim;
    private float limit;
    private float t;
    // Start is called before the first frame update
    void Start()
    {
        limit = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(hitAim.activeSelf)
        {
            t += Time.deltaTime;
            if(t >= limit)
            {
                hitAim.SetActive(false);
                t = 0;
            }
        }
    }

    public void HitAim()
    {
        hitAim.SetActive(true);
    }
}
