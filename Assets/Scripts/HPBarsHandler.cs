using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HPBarsHandler : MonoBehaviour
{
    public static HPBarsHandler instance { get; set; }
    public VisualElement HPBarP1, HPBarP2;
    public FloatVar HP1, HP2, HPStart;
    private Button resumeButton;
    private GroupBox menu;
    private Label menuHeader;

    public void OnEnable()
    {
        instance = this;

        HP1.value = HPStart.value;
        HP2.value = HPStart.value;
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        //since the elements are foldered, we need to use 2-step search... lame
        HPBarP1 = root.Q<VisualElement>("HPBarEmptyP1").Q<VisualElement>("HPBarFullP1");
        HPBarP2 = root.Q<VisualElement>("HPBarEmptyP2").Q<VisualElement>("HPBarFullP2");

        menu = root.Q<GroupBox>("MenuScreen");

        menuHeader = menu.Q<Label>("MenuWindow");
        resumeButton = menuHeader.Q<Button>("ResumeButton");
    }

    public void UpdateHPBars()
    {
        HPBarP1.style.width = Length.Percent(100 * HP1.value / HPStart.value);
        HPBarP2.style.width = Length.Percent(100 * HP2.value / HPStart.value);

        if (HP2.value <= 0)
        {
            ShowVictoryScreen();
        }
        if (HP1.value <= 0)
        {
            ShowLossScreen();
        }

    }

    public void ShowVictoryScreen()
    {
        menuHeader.text = "You won!";
        Time.timeScale = 0;
        menu.visible = true;
        resumeButton.visible = false;
    }

    public void ShowLossScreen()
    {
        menuHeader.text = "You lost!";
        Time.timeScale = 0;
        menu.visible = true;
        resumeButton.visible = false;
    }


    void Update()
    {


    }
}
