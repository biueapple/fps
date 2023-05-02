using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sculpture : MonoBehaviour
{
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            time += Time.deltaTime;
            if(time > 3)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
