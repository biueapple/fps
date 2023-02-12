using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerUI : MonoBehaviour
{
    protected Canvas canvas;
    //E 관련된 녀석들
    protected Image Inter_List;
    protected GameObject E_Image;
    protected Image E_Back;

    //INVEN 관련된 녀석들
    protected GameObject invenObject;
    protected Image[] invenImages;

    //GraphicRaycaster
    GraphicRaycaster m_gr;
    PointerEventData m_ped;
    List<RaycastResult> results;

    protected GameObject aim;
    protected Player player;

    protected MagazineText magazineText;    //총알 남은수 text로 표현하기위해
    protected BasicInformation hpUnit;
    protected HpText hpText;
    protected HitRotate hitRotate;

    protected Vector3 vec;
    protected float angle;
    protected float dot;
    protected float acos;
    protected Vector3 cross;
    private void Start()
    {
        player = GetComponent<Player>();

        invenImages = new Image[16];

        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        Inter_List = canvas.transform.GetChild(0).GetComponent<Image>();
        E_Image = canvas.transform.GetChild(1).gameObject;
        E_Back = E_Image.transform.GetChild(0).GetComponent<Image>();
        invenObject = canvas.transform.GetChild(2).gameObject;
        for(int i = 0; i < invenImages.Length; i++)
        {
            invenImages[i] = invenObject.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Image>();
        }

        m_ped = new PointerEventData(null);
        m_gr = canvas.GetComponent<GraphicRaycaster>();
        results = new List<RaycastResult>();

        aim = Instantiate(Resources.Load<GameObject>("Aim/PlayerAim"));
        aim.transform.SetParent(GameObject.Find("Canvas").transform, false);
        player.SetOriginalAim(aim);

        magazineText = GameObject.FindObjectOfType<MagazineText>();
        hpText = GameObject.FindObjectOfType<HpText>();
        hitRotate = GameObject.FindObjectOfType<HitRotate>();
        hitRotate.gameObject.SetActive(false);
    }


    //E 관련된 녀석들
    public void E_Activation(Interaction inter)
    {
        if(!E_Image.gameObject.activeSelf)
        {
            E_Image.gameObject.SetActive(true);
        }
        if(E_Image.transform.parent != Inter_List.transform)
        {
            E_Image.transform.SetParent(Inter_List.transform, false);
        }

        E_Back.fillAmount = inter.GetE_Time();
    }
    public void E_Deactivation()
    {
        if(E_Image.gameObject.activeSelf)
        {
            E_Image.gameObject.SetActive(false);
        }
        if(E_Image.transform.parent != canvas.transform)
        {
            E_Image.transform.SetParent(canvas.transform, false);
        }
    }
    public void E_Fill(float fill)
    {
        if (E_Image.activeSelf)
        {
            E_Back.fillAmount = fill;
        }
    }

    //INVEN 관련된 녀석들
    public void Inventory(Player p)                                     //playerInven에서 호출
    {
        if(invenObject.activeSelf)
        {
            invenObject.SetActive(false);
            p.GetPlayerMouse().isMove = true;
        }
        else
        {
            invenObject.SetActive(true);
            p.GetPlayerMouse().isMove = false;
        }
    }
    public void SettingInvenSprite(int index, Sprite sp)        //playerInven에서 호출
    {
        if(index > invenImages.Length || index < 0)
            return;
        invenImages[index].sprite = sp;
    }
    //

    //
    public void GraphicRay()
    {
        if(Input.GetMouseButtonDown(0))
        {
            m_ped.position = Input.mousePosition;
            
            m_gr.Raycast(m_ped, results);
            if (results.Count > 0)
            {
                Debug.Log(results[0].gameObject.transform.name);
                if (results[0].gameObject.transform.GetComponent<UIBar>() != null)
                {
                    results[0].gameObject.transform.GetComponent<UIBar>().startPoint = Input.mousePosition;
                    results[0].gameObject.transform.GetComponent<UIBar>().endPoint = results[0].gameObject.transform.position;
                    results[0].gameObject.transform.GetComponent<UIBar>().click = true;
                }
            }
            results.Clear();
        }
    }


    private void Update()
    {
        ShowMagazine();
        ShowHpUnit();
        SetHitPosition();
    }

    public void SetHitPosition()                            //마지막에 때린 사람의 위치를 표시하기 위해
    {
        if(player.GetLastHitTarget() != null)
        {
            vec = (player.GetLastHitTarget().transform.position - transform.position).normalized;
            dot = Vector3.Dot(vec, transform.forward);
            cross = Vector3.Cross(vec, transform.forward);
            acos = Mathf.Acos(dot);
            angle = acos * 180 / Mathf.PI;

            if (cross.y <= 0)
            {
                hitRotate.transform.localEulerAngles = new Vector3(0, 0, -angle);
            }
            else
            {
                hitRotate.transform.localEulerAngles = new Vector3(0, 0, angle);
            }
        }
    }
    public void InitHitRotate()
    {
        if (!hitRotate.gameObject.activeSelf)
        {
            hitRotate.gameObject.SetActive(true);
        }

        hitRotate.Init();
    }                           //HitRotate Init()
    public void SetBasicUnit(BasicInformation b)
    {
        hpUnit = b;
    }       //누구의 체력을 text에 넣을것인가
    public void ShowHpUnit()
    {
        if(hpUnit != null)
        {
            if (!hpText.maxHp.gameObject.activeSelf)
            {
                hpText.maxHp.gameObject.SetActive(true);
                hpText.Hp.gameObject.SetActive(true);
            }
            hpText.maxHp.text = hpUnit.maxhp.ToString();
            hpText.Hp.text = hpUnit.hp.ToString();
        }
        else
        {
            if(hpText.maxHp.gameObject.activeSelf)
            {
                hpText.maxHp.gameObject.SetActive(false);
                hpText.Hp.gameObject.SetActive(false);
            }
        }
    }                           //체력을 보여주기
    public void ShowMagazine()
    {
        if (player.GetPlayerEquip().Hand != null)
        {
            if (player.GetPlayerEquip().Hand.GetComponent<EquipItem>() != null)
            {
                if (player.GetPlayerEquip().Hand.GetComponent<EquipItem>().maxMagazine != 0)
                {
                    if (!magazineText.gameObject.activeSelf)
                    {
                        magazineText.gameObject.SetActive(true);
                    }
                    magazineText.restMagazine.text = player.GetPlayerEquip().Hand.GetComponent<EquipItem>().restMagazine.ToString();
                    magazineText.currentMagazine.text = player.GetPlayerEquip().Hand.GetComponent<EquipItem>().currentMagazine.ToString();
                }
                else
                {
                    if (magazineText.gameObject.activeSelf)
                    {
                        magazineText.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                if (magazineText.gameObject.activeSelf)
                {
                    magazineText.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (magazineText.gameObject.activeSelf)
            {
                magazineText.gameObject.SetActive(false);
            }
        }
    }                           //탄창을 보여주기
}
