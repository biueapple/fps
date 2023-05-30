using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBar : MonoBehaviour , IPointerDownHandler
{
    private Vector3 startPoint;
    private bool click;
    private Vector3 endPoint;
    public GameObject obj;

    public void OnPointerDown(PointerEventData eventData)
    {
        click = true;
        startPoint = Input.mousePosition;
        endPoint = obj.transform.position;
    }


    void Update()
    {
        if(click)
        {
            obj.transform.position = endPoint + Input.mousePosition - startPoint;
        }

        if(Input.GetMouseButtonUp(0))
        {
            click = false;
        }
    }
}
