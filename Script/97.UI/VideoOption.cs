using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class VideoOption : MonoBehaviour
{
    private List<Resolution> resolutions = new List<Resolution>();
    private int resolutionNum;
    public Dropdown dropdown;
    public Toggle toggle;
    private FullScreenMode fullScreenMode;

    private void Awake()
    {
        Init();
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    void Init()
    {
        for(int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRate >= 60)
                resolutions.Add(Screen.resolutions[i]);
        }

        dropdown.options.Clear();

        int optionNum = 0;
        foreach (Resolution item in resolutions)
        {
            Dropdown.OptionData optionData = new Dropdown.OptionData();
            optionData.text = item.width + " x " +item.height + " " + item.refreshRate + "hz";
            dropdown.options.Add(optionData);

            if(item.width == Screen.width && item.height == Screen.height)
            {
                dropdown.value = optionNum;
            }
            optionNum++;
        }
        dropdown.RefreshShownValue();

        toggle.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    public void DropBoxOptionChange(int x)
    {
        resolutionNum = x;
    }

    public void FullScreenButton(bool isFull)
    {
         fullScreenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void OKButton()
    {
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, fullScreenMode);
    }
}
