using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ITEMINDEX
{
    NONE = 0,
    POTION,
    PISTOL,
    MM9,
    SAORI_HANDGUN,

}
public enum ITEMKIND
{
    NONE = 0,
    EQUIP,
    CONSUMPTION,

}
public enum UNITSTATE
{
    NONE = 0,
    RUN,
    SITTING,

}
public enum KIND_BULLET
{
    _9MM,

}


public class NewBehaviourScript : MonoBehaviour
{
    public Ch ch;
    public Camera cam;
    public LayerMask layerMask;

    private float rotX;
    private float rotY;
    [Header("최대 각도외 최소 각도(Y)")]
    public float maxY;
    public float minY;

    private UIController controller;

    void Start()
    {
        //ch.Init();
        //ch.script = this;
        //cam = ch.hand.parent;
        controller = FindObjectOfType<UIController>();
    }


    void Update()
    {
        Movement();
        Jump();
        Interacting();
        HandUse();
        ChStateChange();
    }

    public void GiveDamage(GameObject enemy)
    {
        controller.OpenFadeOutUI(enemy.transform, cam ,1 , 1 , new Vector3(50,50));
    }
    public void ChStateChange()
    {
        if (ch != null)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                ch.RunState();
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                ch.StandingState();
            }
        } 
    }
    public void Movement()
    {
        if(ch!=null)
        {
            Vector3 m_Input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * 10;
            
            ch.Movement(m_Input);

            rotX += Input.GetAxis("Mouse X") * 5; //감도
            rotY -= Input.GetAxis("Mouse Y") * 5;

            rotY = Mathf.Clamp(rotY, minY, maxY);
            cam.transform.localEulerAngles = new Vector3(rotY, 0, 0);
            ch.Rotation(new Vector3(0, rotX, 0));
        }
    }
    public void Jump()
    {
        if (ch != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ch.Jump(200);
            }
        }
    }
    public void Interacting()
    {
        if(ch != null)
        {
            Ray ray = new Ray();
            ray = cam.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            ch.InteractingRay(ray, layerMask, KeyCode.E);
        }
    }
    public void HandUse()
    {
        if (ch != null)
        {
            ch.ItemUse();
        }
    }
    public void AddRotY(float f)
    {
        rotY -= f;
    }
}
