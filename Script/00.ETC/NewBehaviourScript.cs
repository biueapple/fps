using UnityEngine;
using UnityEngine.UI;

public enum ITEM_INDEX
{
    NONE = 0,
    POTION,
    PISTOL,
    MM9,
    SAORI_HANDGUN,
    GRENADE,

}
public enum ITEM_KIND
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
public enum ENEMY_KIND
{
    NONE = 0,
    TEST_ENEMY,

}


public class NewBehaviourScript : MonoBehaviour
{
    public Ch ch;
    public Camera cam;
    //public LayerMask layerMask;

    private float rotX;
    private float rotY;
    [Header("최대 각도외 최소 각도(Y)")]
    public float maxY;
    public float minY;
    private bool mouseMove = true;
    private float interRange = 5;

    private UIController controller;
    private InventoryView inventoryView;

    RaycastHit hit;
    Ray ray = new Ray();
    void Start()
    {
        //Init();
    }


    void Update()
    {
        if(mouseMove)
        {
            //캐릭터 컨트롤
            Movement();
            Jump();
            Interacting();
            HandUse();
            ChStateChange();
            HandChange();
        }
        else
        {
            //ui 컨트롤
            //아이템 옮기거나 해야함
            InputInventoryController();
        }

        //ui열고 닫기
        InputInventoryKey();
    }

    public void Init(float maxY, float minY, UIController uIController, InventoryView inventory)
    {
        controller = uIController;
        this.inventoryView = inventory;
        this.maxY = maxY;
        this.minY = minY;
    }
    public void InputInventoryController()
    {
        if (Input.GetMouseButtonDown(0))
        {
            inventoryView.DownImage(controller.GetGraphicRay<Image>());
        }
        else if (Input.GetMouseButtonUp(0))
        {
            inventoryView.UpImage(controller.GetGraphicRay<Image>());
        }
    }
    public void InputInventoryKey()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            controller.InputOpenKey(inventoryView.gameObject);
            if (controller.GetOpenList() <= 0)
            {
                mouseMove = true;
            }
            else
            {
                mouseMove = false;
                inventoryView.Open();
            }
        }
    }
    public void InputCloseKey()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            controller.InputCloseKey();
            if (controller.GetOpenList() <= 0)
            {
                mouseMove = true;
            }
            else
            {
                mouseMove = false;
            }
        }
    }
    public void GiveDamage(GameObject enemy, string str)
    {
        controller.OpenFadeOutUI(enemy.transform, cam ,1 , 1 , new Vector2(50,50), new Vector2(75,75), str);
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

            if(mouseMove)
            {
                rotX += Input.GetAxis("Mouse X") * 5; //감도
                rotY -= Input.GetAxis("Mouse Y") * 5;

                rotY = Mathf.Clamp(rotY, minY, maxY);
                cam.transform.localEulerAngles = new Vector3(rotY, 0, 0);
                ch.Rotation(new Vector3(0, rotX, 0));
            }
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
            ray = cam.ScreenPointToRay(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f));

            if (Physics.Raycast(ray, out hit, interRange/*, mask*/))
            {
                //e이미지 띄우기
                if(hit.transform.GetComponent<Inter>() != null)
                {
                    if(hit.transform.GetComponent<Inter>().uninter == false)
                    {
                        controller.OpenUI(controller.interUI.gameObject);
                    }
                    else
                        controller.CloseUI(controller.interUI.gameObject);
                }
                else
                    controller.CloseUI(controller.interUI.gameObject);
            }
            else
            {
                //e이미지 지우기
                controller.CloseUI(controller.interUI.gameObject);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (hit.transform != null && hit.transform.GetComponent<Inter>() != null)
                {
                    hit.transform.GetComponent<Inter>().Interaction(ch);
                }
            }
        }
    }
    public void HandUse()
    {
        if (ch != null)
        {
            ch.ItemUse();
        }
    }
    public void HandChange()
    {
        if (ch != null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ch.GetInventory().HandChange(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ch.GetInventory().HandChange(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ch.GetInventory().HandChange(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ch.GetInventory().HandChange(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                ch.GetInventory().HandChange(4);
            }
        }
    }
    public void AddRotY(float f)
    {
        rotY -= f;
    }

    //

    public InventoryView GetInventoryView()
    {
        return inventoryView;
    }
}
