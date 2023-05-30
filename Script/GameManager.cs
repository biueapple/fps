using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    //가장 처음
    private UIController uiController;
    [Header("가장 처음")]
    public GameObject 시작ui;
    public Text warningText;
    public Image logo;
    public Image pressImage;

    //캐릭터 선택창
    private CreateUnit createUnit;
    private List<NewBehaviourScript> players = new List<NewBehaviourScript>();
    [Header("캐릭터 선택창")]
    public SelectMenu selectMenu;
    public GameObject inGamePanel;
    public InventoryView inventoryView;

    //인게임
    //[Header("인게임")]

    void Start()
    {
        uiController = FindObjectOfType<UIController>();
        createUnit = FindObjectOfType<CreateUnit>();


        //Warning();
    }


    void Update()
    {
        
    }

    public void Exit()
    {
        Application.Quit();
    }

    //
    // 가장 처음
    //
    public void Warning()
    {
        Action action = WarningFadeOut;
        WarnindFadeIn(action);
    }
    public void WarningEnd()
    {
        Logo();
    }
    public void Logo()
    {
        Action action = LogoFadeOut;
        LogoFadeIn(action);
    }
    public void LogoEnd()
    {
        pressImage.raycastTarget = true;
        pressImage.transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(PressImageC());
    }
    

    private void WarnindFadeIn(Action action)
    {
        StartCoroutine(uiController.FadeIn(warningText, 1, 1, action));
    }
    private void WarningFadeOut()
    {
        Action action = WarningEnd;
        StartCoroutine(uiController.FadeOut(warningText, 1, 1, false, action));
    }
    private void LogoFadeIn(Action action)
    {
        StartCoroutine(uiController.FadeIn(logo, 1, 1, action));
    }
    private void LogoFadeOut()
    {
        Action action = LogoEnd;
        StartCoroutine(uiController.FadeOut(logo, 1, 1, false, action));
    }
    private IEnumerator PressImageC()
    {
        while (true)
        {
            if(Input.GetMouseButtonDown(0))
            {
                시작ui.SetActive(false);
                selectMenu.gameObject.SetActive(true);
                break;
            }
            yield return null;

        }
    }

    //
    //캐릭터 선택창
    //
    public void StartButton()
    {
        if (selectMenu.index < 0 || selectMenu.index >= createUnit.GetCharacters().Length)
                return;
        //플레이어를 만드는 단계
        NewBehaviourScript p = new GameObject().AddComponent<NewBehaviourScript>();
        p.Init(70, -70, uiController, inventoryView);
        players.Add(p);
        p.name = "Player" + players.IndexOf(p).ToString();

        //캐릭터를 만드는 단계
        p.ch = createUnit.GetCreateCharacter(selectMenu.index);
        p.ch.player = p;
        p.cam = p.ch.hand.parent.GetComponent<Camera>();
        Camera.main.gameObject.SetActive(false);

        p.GetInventoryView().SetInventory(p.ch.GetInventory());

        //
        inGamePanel.SetActive(true);
        selectMenu.gameObject.SetActive(false);
    }
    //
    //인게임
    //

    //
    //get set
    //
    public List<NewBehaviourScript> GetPlayers()
    {
        return players;
    }
}
