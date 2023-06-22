using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMenu : MonoBehaviour
{
    private CreateUnit createUnit;
    private List<Image> images = new List<Image>();
    private UIController uIController;

    
    public Transform content;
    public Image selectedImage;

    public int index = -1;

    void Start()
    {
        uIController= FindObjectOfType<UIController>();
        createUnit = FindObjectOfType<CreateUnit>();

        for (int i = 0; i < createUnit.GetCharacters().Length; i++)
        {
            GameObject obj = new GameObject();
            obj.AddComponent<CanvasRenderer>();
            obj.AddComponent<Image>();
            obj.transform.SetParent(content, false);
            
            images.Add(obj.GetComponent<Image>());
            images[i].rectTransform.sizeDelta = new Vector2(150, 150);
            images[i].sprite = createUnit.GetCharacters()[i].paScriptble.GetSprite();
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
                selectedImage.sprite = createUnit.GetCharacters()[index].paScriptble.GetSprite();
            }
        }
        else if(Input.GetMouseButtonDown(1))
        {
            index = -1;
            selectedImage.sprite = null;
        }
    }
    
}
