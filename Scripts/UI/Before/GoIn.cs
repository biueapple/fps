using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoIn : MonoBehaviour
{
    public int choiceIndex;
    public GameObject InGameCanvas;
    public GameObject InGame;
    public GameObject OutGameCanvas;
    public GameObject[] Characters;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void _OnButton1()
    {
        choiceIndex = 0;
    }
    public void _OnButton2()
    {
        choiceIndex = 1;
    }
    public void _OnButton3()
    {
        choiceIndex = 2;
    }
    public void _OnButton4()
    {
        choiceIndex = 3;
    }
    public void _OnButton5()
    {
        choiceIndex = 4;
    }
    public void _OnButton6()
    {
        InGameCanvas.SetActive(true);
        InGame.SetActive(true);
        GameObject obj = Instantiate(Characters[choiceIndex], Vector3.zero, Quaternion.Euler(Vector3.zero), InGame.transform);
        obj.GetComponent<Player>().Init();
        OutGameCanvas.SetActive(false);
    }
}
