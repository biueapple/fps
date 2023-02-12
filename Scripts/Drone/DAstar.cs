using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAstar : MonoBehaviour
{
    public float minimumDistance;
    public float speed;
    public List<_NODE> OpenList = new List<_NODE>();
    public List<_NODE> CloseList = new List<_NODE>();
    public List<_NODE> FinalList = new List<_NODE>();
    private Vector3[] vec = new Vector3[26];

    public Transform Target;


    public bool b = false;
    public bool c = false;
    public int iiii = 0;
    public int test;
    void Start()
    {
        vec[0] = new Vector3(1, 1, 1);
        vec[1] = new Vector3(1, 0, 1);
        vec[2] = new Vector3(1, -1, 1);

        vec[3] = new Vector3(0, 1, 1);
        vec[4] = new Vector3(0, 0, 1);
        vec[5] = new Vector3(0, -1, 1);

        vec[6] = new Vector3(-1, 1, 1);
        vec[7] = new Vector3(-1, 0, 1);
        vec[8] = new Vector3(-1, -1, 1);

        vec[9] = new Vector3(1, 1, 0);
        vec[10] = new Vector3(0, 1, 0);
        vec[11] = new Vector3(-1, 1, 0);

        vec[12] = new Vector3(1, 1, -1);
        vec[13] = new Vector3(0, 1, -1);
        vec[14] = new Vector3(-1, 1, -1);

        vec[15] = new Vector3(1, 0, 0);
        vec[16] = new Vector3(1, 0, -1);

        vec[17] = new Vector3(1, -1, 0);
        vec[18] = new Vector3(1, -1, -1);

        vec[19] = new Vector3(-1, 0, 0);
        vec[20] = new Vector3(-1, 0, -1);

        vec[21] = new Vector3(-1, -1, 0);
        vec[22] = new Vector3(-1, -1, -1);

        vec[23] = new Vector3(0, -1, 0);

        vec[24] = new Vector3(0, -1, -1);

        vec[25] = new Vector3(0, 0, -1);

        
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (c)
            {
                b = true;
            }
        }
        if (b)
        {
            if (iiii >= FinalList.Count)
            {
                b = false;
                iiii = 0;
            }
            if (MoveToObj(transform.gameObject, FinalList[iiii].position))
            {
                iiii++;
            }
        }
    }

    IEnumerator FindCo()
    {
        c = false;

        OpenList.Add(new _NODE(transform.position, Vector3.Distance(transform.position, Target.position)));
        OpenList[OpenList.Count - 1].previous = -1;

        while (true)
        {
            if (OpenList.Count <= 0)
            {
                Debug.Log("길이 없음");
                yield break;
            }
            int index = FindMinDis(OpenList);

            CloseList.Add(OpenList[index]);
            OpenList.RemoveAt(index);

            if (Vector3.Distance(CloseList[CloseList.Count - 1].position, Target.position) < minimumDistance)
            {
                break;
            }

            for (int i = 0; i < vec.Length; i++)
            {
                if (RayShot(CloseList[CloseList.Count - 1].position, vec[i]))
                {
                    if (PosiSame(CloseList[CloseList.Count - 1].position + vec[i]))
                    {
                        OpenList.Add(new _NODE(CloseList[CloseList.Count - 1].position + vec[i]));
                        OpenList[OpenList.Count - 1].SetDistance(Target.position);
                        OpenList[OpenList.Count - 1].previous = CloseList.Count - 1;

                    }
                    else
                    {

                    }
                }
                else
                {

                }
            }

            test++;
            if (test > 100)
            {
                break;
            }
            yield return null;
        }

        int temp = CloseList.Count - 1;

        while (true)
        {

            FinalList.Add(CloseList[temp]);
            temp = CloseList[temp].previous;

            if (FinalList[FinalList.Count - 1].previous == -1)
            {
                break;
            }
            yield return null;
        }

        FinalList.Reverse();
        c = true;
    }

    public void Find()      //코루틴이 더 좋을듯
    {
        OpenList.Add(new _NODE(transform.position, Vector3.Distance(transform.position, Target.position)));
        OpenList[OpenList.Count - 1].previous = -1;

        while (true)
        {
            if (OpenList.Count <= 0)
            {
                Debug.Log("길이 없음");
                return;
            }
            int index = FindMinDis(OpenList);

            CloseList.Add(OpenList[index]);
            OpenList.RemoveAt(index);

            if (Vector3.Distance(CloseList[CloseList.Count - 1].position, Target.position) < minimumDistance)
            {
                break;
            }

            for (int i = 0; i < vec.Length; i++)
            {
                if (RayShot(CloseList[CloseList.Count - 1].position, vec[i]))
                {
                    if (PosiSame(CloseList[CloseList.Count - 1].position + vec[i]))
                    {
                        OpenList.Add(new _NODE(CloseList[CloseList.Count - 1].position + vec[i]));
                        OpenList[OpenList.Count - 1].SetDistance(Target.position);
                        OpenList[OpenList.Count - 1].previous = CloseList.Count - 1;

                    }
                    else
                    {

                    }
                }
                else
                {

                }
            }

            test++;
            if (test > 100)
            {
                break;
            }

        }

        int temp = CloseList.Count - 1;

        while (true)
        {

            FinalList.Add(CloseList[temp]);
            temp = CloseList[temp].previous;

            if (FinalList[FinalList.Count - 1].previous == -1)
            {
                break;
            }
        }
        FinalList.Reverse();
    }

    public int FindMinDis(List<_NODE> list)            //가장 작은 값 찾기
    {
        float min = list[0].distance;
        int index = 0;

        for (int i = 1; i < list.Count; i++)
        {
            if (min > list[i].distance)
            {
                min = list[i].distance;
                index = i;

            }
        }

        return index;
    }
    public bool RayShot(Vector3 posi, Vector3 direction)    //ray쏴 충돌 찾기 Physics로 교체
    {
        Collider[] collider = Physics.OverlapBox(posi + direction, new Vector3(0.5f, 0.5f, 0.5f));

        if (collider.Length == 0)
        {
            return true;
        }
        else if (collider.Length == 1)
        {
            if (collider[0].gameObject == this.gameObject || collider[0].gameObject == Target.gameObject)
            {
                return true;
            }
        }
        else if (collider.Length == 2)
        {
            if (collider[0].gameObject == this.gameObject && collider[1].gameObject == Target.gameObject)
            {
                return true;
            }
            if (collider[1].gameObject == this.gameObject && collider[0].gameObject == Target.gameObject)
            {
                return true;
            }
        }

        return false;
    }

    public bool MoveToObj(GameObject obj, Vector3 target)
    {
        obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, Time.deltaTime * speed);

        if (obj.transform.position == target)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool PosiSame(Vector3 posi)
    {
        for (int i = 0; i < OpenList.Count; i++)
        {
            if (OpenList[i].position == posi)
            {
                return false;
            }
        }

        for (int i = 0; i < CloseList.Count; i++)
        {
            if (CloseList[i].position == posi)
            {
                return false;
            }
        }

        return true;
    }

    public int PosiSame(Vector3 posi, int j)
    {
        for (int i = 0; i < OpenList.Count; i++)
        {
            if (OpenList[i].position == posi)
            {
                return i;
            }
        }

        for (int i = 0; i < CloseList.Count; i++)
        {
            if (CloseList[i].position == posi)
            {
                return i;
            }
        }

        return -1;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < vec.Length; i++)
        {
            Gizmos.DrawWireCube(transform.position + vec[i], new Vector3(1f, 1f, 1f));
        }
    }
}

[System.Serializable]
public class _NODE     //부모노드 gfh
{
    public Vector3 position;    //위치
    public float distance;      //거리
    public int previous;
    public _NODE(Vector3 vec, float d)
    {
        position = vec;
        distance = d;
    }
    public _NODE(Vector3 vec)
    {
        position = vec;
    }
    public void SetDistance(Vector3 target)
    {
        distance = Vector3.Distance(position, target);
    }
}
