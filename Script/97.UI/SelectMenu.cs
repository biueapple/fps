using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMenu : MonoBehaviour
{
    private Ch[] characters;
    private List<Image> images = new List<Image>();
    private UIController uIController;

    public NewBehaviourScript player;
    public Transform content;
    public Image selectedImage;
    public GameObject InGamePanel;


    private int index = -1;

    void Start()
    {
        uIController= FindObjectOfType<UIController>();

        characters = Resources.LoadAll<Ch>("Character");
        for (int i = 0; i < characters.Length; i++)
        {
            GameObject obj = new GameObject();
            obj.AddComponent<CanvasRenderer>();
            obj.AddComponent<Image>();
            obj.transform.SetParent(content, false);
            
            images.Add(obj.GetComponent<Image>());
            images[i].rectTransform.sizeDelta = new Vector2(150, 150);
            images[i].sprite = characters[i].paScriptble.GetSprite();
        }
    }


    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Image image = uIController.GetGraphicRay<Image>();
            if(images.Contains(image))
            {
                index = images.IndexOf(image);
                selectedImage.sprite = characters[index].paScriptble.GetSprite();
            }
        }
        else if(Input.GetMouseButtonDown(1))
        {
            index = -1;
            selectedImage.sprite = null;
        }
    }

    public void CreateCharacter(Ch ch, Vector3 vector)
    {
        player.ch = Instantiate(ch, vector, Quaternion.identity);
        player.ch.Init();
        player.ch.player = player;
        player.cam = player.ch.hand.parent.GetComponent<Camera>();
        Camera.main.gameObject.SetActive(false);
    }

    public void StartButton()
    {
        if (index < 0 || index >= characters.Length)
            return;
        CreateCharacter(characters[index], Vector3.zero);
        InGamePanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
