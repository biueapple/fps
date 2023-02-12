using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchEnemy : MonoBehaviour
{
    private RectTransform dragRactangle;

    private Rect dragRect;
    private EnemyControl enemyControl;
    private List<GameObject> obj2 = new List<GameObject>();
    private List<GameObject> obj3 = new List<GameObject>();
    private Vector2 Center;

    private RaycastHit hit;
    public bool end;
    void Start()
    {
        Center = new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
        enemyControl = FindObjectOfType<EnemyControl>();
        end = true;
    }



    void Update()
    {
        
    }
    public List<GameObject> GetEnemyList()
    {
        return obj3;
    }

    public void SetRect(RectTransform rect)
    {
        dragRactangle = rect;
    }

    public void Find()
    {
        if (end)
        {
            end = false;
            StartCoroutine(FindEnemy());
        }
        else
        {
            end = true;
            obj2.Clear();
            obj3.Clear();
        }
    }

    IEnumerator FindEnemy()
    {
        while (true)
        {
            obj2.Clear();
            obj3.Clear();
            dragRect = new Rect();

            CalculateDargRect();
            SelectUnits();

            if(end)
            {
                yield break;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private void CalculateDargRect()
    {
        dragRect.xMin = Center.x + (dragRactangle.GetComponent<RectTransform>().anchoredPosition.x - dragRactangle.GetComponent<RectTransform>().sizeDelta.x * 0.5f);
        dragRect.xMax = Center.x + (dragRactangle.GetComponent<RectTransform>().anchoredPosition.x + dragRactangle.GetComponent<RectTransform>().sizeDelta.x * 0.5f);

        dragRect.yMin = Center.y + (dragRactangle.GetComponent<RectTransform>().anchoredPosition.y - dragRactangle.GetComponent<RectTransform>().sizeDelta.y * 0.5f);
        dragRect.yMax = Center.y + (dragRactangle.GetComponent<RectTransform>().anchoredPosition.y + dragRactangle.GetComponent<RectTransform>().sizeDelta.y * 0.5f);

    }
    private void SelectUnits()
    {
        for (int i = 0; i < enemyControl.Enemys.Count; i++)
        {
            if (dragRect.Contains(Camera.main.WorldToScreenPoint(enemyControl.Enemys[i].transform.position)))
            {
                obj2.Add(enemyControl.Enemys[i].gameObject);
            }
        }
        for (int i = 0; i < obj2.Count; i++)
        {
            Vector3 vec = (obj2[i].transform.position - Camera.main.transform.position).normalized;
            if (Physics.Raycast(Camera.main.transform.position, vec, out hit))
            {
                if(hit.transform.parent != null)
                {
                    if (hit.transform.parent.gameObject == obj2[i])
                    {
                        if(obj2[i].GetComponent<BasicInformation>().hp > 0)
                        {
                            obj3.Add(obj2[i]);
                        }
                    }
                        
                }
                else
                {
                    if (hit.transform.gameObject == obj2[i])
                    {
                        if (obj2[i].GetComponent<BasicInformation>().hp > 0)
                        {
                            obj3.Add(obj2[i]);
                        }
                    }
                }
            }
        }
    }
}
