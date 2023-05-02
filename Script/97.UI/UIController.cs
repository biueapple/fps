using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class UIController : MonoBehaviour
{
    //GraphicRaycaster
    GraphicRaycaster m_gr;
    PointerEventData m_ped;
    List<RaycastResult> results;

    private Canvas canvas;

    private List<GameObject> uiOpens = new List<GameObject>();          //게임중에 닫고 여는 ui
    private List<GameObject> basicsUI = new List<GameObject>();         //기본적으로 항상 열러있는 ui
    public GameObject bottomUI;            //uiOpens가 0일때 esc누르면 나올 가장 아래 ui
    public Text damageUI;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();    
        m_ped = new PointerEventData(null);
        m_gr = canvas.GetComponent<GraphicRaycaster>();
        results = new List<RaycastResult>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenFadeOutUI(Transform enemy, Camera cam, float wait, float fade,  Vector2 plus)
    {
        Text text = Instantiate(damageUI ,canvas.transform);
        text.gameObject.SetActive(true);
        StartCoroutine(FadeOut(text, wait, fade));
        StartCoroutine(Follow(text, cam, enemy, wait + fade, plus));
    }
    private IEnumerator Follow(Text text, Camera cam, Transform enemy, float time, Vector3 plus)
    {
        float t = 0;
        while(true)
        {
            text.rectTransform.anchoredPosition = cam.WorldToScreenPoint(enemy.position) + plus;

            yield return null;
            t += Time.deltaTime;
            if(t >= time)
            {
                break;
            }
        }
    }
    private IEnumerator FadeOut(Text text, float wait, float fade)
    {
        yield return new WaitForSeconds(wait);
        float t = 1;
        while(true)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a * t > 1 ? 1 : text.color.a * t);
            yield return null;
            t -= Time.deltaTime / fade;
            if (t < 0)
                break;
        }
        Destroy(text.gameObject);
    }

    public void OpenBasic(GameObject obj)
    {
        basicsUI.Add(obj);
        obj.SetActive(true);
    }
    public void CloseBasic(GameObject obj)
    {
        if(basicsUI.Contains(obj))
        {
            basicsUI.Remove(obj);
        }
        obj.SetActive(false);
    }

    public void TouchUI(GameObject obj)
    {
        if (uiOpens.Contains(obj))
        {
            obj.transform.SetAsLastSibling();
        }
    }

    public bool InputOpenKey(GameObject uiObj)      //창이 열리면 true   
    {
        if (uiOpens.Contains(uiObj))
        {
            CloseUI(uiObj);
            return false;
        }
        else
        {
            OpenUI(uiObj);
            return true;
        }
    }
    public void InputCloseKey(GameObject uiObj)
    {
        CloseUI(uiObj);
    }
    public bool InputCloseKey()                 //창이 닫히면 true
    {
        if(uiOpens.Count > 0)
        {
            CloseUI();
            return true;
        }
        else
        {
            OpenUI(bottomUI);
            return false;
        }
    }


    private void OpenUI(GameObject uiObj)
    {
        uiObj.SetActive(true);
        TouchUI(uiObj);
        if(!uiOpens.Contains(uiObj))
        {
            uiOpens.Add(uiObj);
        }
            
    }
    private void CloseUI(GameObject uiObj)
    {
        uiObj.gameObject.SetActive(false);
        if(uiOpens.Contains(uiObj))
        {
            uiOpens.Remove(uiObj);
        }
    }
    private void CloseUI()
    {
        if(uiOpens.Count > 0)
        {
            uiOpens[0].SetActive(false);
            uiOpens.RemoveAt(0);
        }
    }

    //

    public Transform GetGraphicRay()
    {
        results.Clear();

        m_ped.position = Input.mousePosition;

        m_gr.Raycast(m_ped, results);

        if (results.Count > 0)
        {
            TouchUI(results[0].gameObject);
            return results[0].gameObject.transform;
        }
        return null;
    }
    public T GetGraphicRay<T>()     //가장 첫번째 ui가 T인가
    {
        results.Clear();

        m_ped.position = Input.mousePosition;

        m_gr.Raycast(m_ped, results);

        if (results.Count > 0)
        {
            TouchUI(results[0].gameObject);
            return results[0].gameObject.GetComponent<T>();
        }
        return default(T);
    }
    public T GetGraphicRay<T>(bool b)       //T를 찾아서 리턴
    {
        results.Clear();

        m_ped.position = Input.mousePosition;

        m_gr.Raycast(m_ped, results);

        if (results.Count > 0)
        {
            for(int i = 0; i < results.Count; i++)
            {
                if (results[i].gameObject.transform.GetComponent<T>() != null)
                {
                    TouchUI(results[i].gameObject);
                    return results[i].gameObject.GetComponent<T>();
                }
            }
        }
        return default(T);
    }

    public void ListSwap<T>(List<T> list, int index1, int index2)
    {
        if (index2 == -1)
            return;

        if (index1 < 0 || index2 >= list.Count)
        {
            return;
        }

        T item = list[index1];
        list[index1] = list[index2];
        list[index2] = item;

    }
    public int FindIndex<T>(List<T> list, T obj)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Equals(obj))
            {
                return i;
            }
        }
        return -1;
    }

    public Canvas GetCanvas()
    {
        return canvas;
    }
}
