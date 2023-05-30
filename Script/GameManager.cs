using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    //���� ó��
    private UIController uiController;
    [Header("���� ó��")]
    public GameObject ����ui;
    public Text warningText;
    public Image logo;
    public Image pressImage;

    //ĳ���� ����â
    private CreateUnit createUnit;
    private List<NewBehaviourScript> players = new List<NewBehaviourScript>();
    [Header("ĳ���� ����â")]
    public SelectMenu selectMenu;
    public GameObject inGamePanel;
    public InventoryView inventoryView;

    //�ΰ���
    //[Header("�ΰ���")]

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
    // ���� ó��
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
                ����ui.SetActive(false);
                selectMenu.gameObject.SetActive(true);
                break;
            }
            yield return null;

        }
    }

    //
    //ĳ���� ����â
    //
    public void StartButton()
    {
        if (selectMenu.index < 0 || selectMenu.index >= createUnit.GetCharacters().Length)
                return;
        //�÷��̾ ����� �ܰ�
        NewBehaviourScript p = new GameObject().AddComponent<NewBehaviourScript>();
        p.Init(70, -70, uiController, inventoryView);
        players.Add(p);
        p.name = "Player" + players.IndexOf(p).ToString();

        //ĳ���͸� ����� �ܰ�
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
    //�ΰ���
    //

    //
    //get set
    //
    public List<NewBehaviourScript> GetPlayers()
    {
        return players;
    }
}
