using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum _LARGE_FRAME
{
    NONE = 0,       //그냥 오브젝트 아무것도아닌 벽같은거
    UNIT,           //유닛(플레이어나 적)
    INTERACTION,    //상호작용 가능한 오브젝트나 아이템

}
public enum _SMALL_FRAME
{
    NONE = 0,
    PLAYER,         //UNIT
    AI,             //UNIT
    ITEM,           //INTERACTION
    ETC,            //INTERACTION

}
public enum _STRIKING_AREA  //UNIT인 경우에만
{ 
    NONE = 0,       //부위가 없음
    HEAD,
    BODY,
    ARM,
    LEG,

}

public class Decryption : MonoBehaviour
{
    public _LARGE_FRAME large_Frame;        
    public _SMALL_FRAME small_Frame;
    public _STRIKING_AREA striking_Area;
    public float Distance;            //거리
    public float HitDistance;
    //public float Figure;             //수치(대미지나 다른)
    public Interaction interaction; //상대가 상호작용 오브젝트일때만 null이 아님
    public Enemy enemy;             //상대가 적이면 null이 아님
    public Vector3 HitVec;

    public void DecryptionSetting(RaycastHit hit)
    {
        Init();

        if (hit.transform.parent == null)
        {
            if (hit.transform.GetComponent<BasicInformation>() != null)
            {
                large_Frame = hit.transform.GetComponent<BasicInformation>().large_Frame;
                small_Frame = hit.transform.GetComponent<BasicInformation>().small_Frame;

                
                //Figure = f;

                if (hit.transform.GetComponent<Interaction>() != null)
                {
                    interaction = hit.transform.GetComponent<Interaction>();
                }

                if (hit.transform.GetComponent<Enemy>() != null)
                {
                    enemy = hit.transform.GetComponent<Enemy>();
                }
            }
            Distance = Vector3.Distance(transform.position, hit.transform.position);
            HitDistance = Vector3.Distance(transform.position, hit.point);
            HitVec = hit.point;
        }
        else if (hit.transform.parent != null)
        {
            if (hit.transform.parent.GetComponent<BasicInformation>() != null)
            {
                large_Frame = hit.transform.parent.GetComponent<BasicInformation>().large_Frame;
                small_Frame = hit.transform.parent.GetComponent<BasicInformation>().small_Frame;


                //Figure = f;

                if (hit.transform.parent.GetComponent<Interaction>() != null)
                {
                    interaction = hit.transform.parent.GetComponent<Interaction>();
                }

                if (hit.transform.parent.GetComponent<Enemy>() != null)
                {
                    enemy = hit.transform.parent.GetComponent<Enemy>();
                }
            }
            Distance = Vector3.Distance(transform.position, hit.transform.position);
            HitDistance = Vector3.Distance(transform.position, hit.point);
            HitVec = hit.point;
        }
        
    }

    public void Init()
    {
        large_Frame = _LARGE_FRAME.NONE;
        small_Frame = _SMALL_FRAME.NONE;
        striking_Area = _STRIKING_AREA.NONE;
        Distance = 0;
        HitDistance = 0;
        //Figure = 0;
        interaction = null;
        enemy = null;
        HitVec = Vector3.negativeInfinity;
    }
}
