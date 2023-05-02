using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderBack : MonoBehaviour
{
    private List<Unit> unitList = new List<Unit>();

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.GetComponent<Unit>() != null)
        {
            unitList.Add(other.transform.GetComponent<Unit>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(unitList.Contains(other.transform.GetComponent<Unit>()))
        {
            unitList.Remove(other.transform.GetComponent<Unit>());
        }
    }

    public List<Unit> GetUnits(int teamNum)         //teamNum ������ ����
    {
        List<Unit> list = new List<Unit>();
        for(int i = 0; i < unitList.Count; i++)
        {
            if(unitList[i].teamNum == teamNum)
            {
                list.Add(unitList[i]);
            }
        }
        return list;
    }
    public List<Unit> GetEnemyUnits(int teamNum)    //teamNum �ٸ��� ����
    {
        List<Unit> list = new List<Unit>();
        for (int i = 0; i < unitList.Count; i++)
        {
            if (unitList[i].teamNum != teamNum)
            {
                list.Add(unitList[i]);
            }
        }
        return list;
    }

    public List<Unit> GetUnitList()
    {
        return unitList; 
    }
}
