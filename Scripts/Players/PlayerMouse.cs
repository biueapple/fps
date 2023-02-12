using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouse : MonoBehaviour
{
    private Player player;
    private float m_Vertical;
    private float m_Horizontal;
    public bool isMove;
    public bool isLimit;
    public bool isPlayer;
    public float verticalMaxLimit;
    public float verticalMinLimit;
    public float DronMaxLimit;
    public float DronMinLimit;

    protected GameObject CamObj;

    void Start()
    {
        player = GetComponent<Player>();
        isMove = true;
        isPlayer = true;
    }


    void Update()
    {
        if(isMove)
        {
            MouseControl();
        }
    }

    public void MouseControl()
    {
        m_Horizontal = Input.GetAxisRaw("Mouse X") * player.GetPlayerSetting().Mouse_Horizontal_Sensitive;
        

        m_Vertical += Input.GetAxisRaw("Mouse Y") * player.GetPlayerSetting().Mouse_Vertical_Sensitive;
        if (isLimit)
        {
            if(isPlayer == true)
            {
                transform.eulerAngles += new Vector3(0, m_Horizontal, 0);

                if (m_Vertical > verticalMaxLimit)
                {
                    m_Vertical = verticalMaxLimit;
                }
                else if (m_Vertical < verticalMinLimit)
                {
                    m_Vertical = verticalMinLimit;
                }
            }
            else
            {
                CamObj.transform.eulerAngles += new Vector3(0, m_Horizontal, 0);
                if (m_Vertical > DronMaxLimit)
                {
                    m_Vertical = DronMaxLimit;
                }
                else if (m_Vertical < DronMinLimit)
                {
                    m_Vertical = DronMinLimit;
                }
            }
            
        }

        player.GetCam().transform.localEulerAngles = new Vector3(m_Vertical, 0, 0) * -1;

        if (player.GetPlayerEquip().Hand != null)
        {
            player.GetPlayerEquip().handTransform.localEulerAngles = new Vector3(player.GetCam().transform.eulerAngles.x, 0, 0);
        }
    }

    public void SetCamObject(GameObject Obj)
    {
        CamObj = Obj;
    }
}
