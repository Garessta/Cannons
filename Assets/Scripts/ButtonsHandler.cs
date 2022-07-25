using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.UIElements;

public class ButtonsHandler : MonoBehaviour
{
    private float timeLeft = 0;
    private Button shieldButton, pauseButton, screen, restartButton, resumeButton;
    private VisualElement shieldButtonInactive;
    private GroupBox menu;
    private Label timer, menuHeader;
    [SerializeField] GameObject shieldP1;
    [SerializeField] GameObject cannonP1;

    public FloatVar SP1Cooldown;

    private void OnEnable()
    {
        var UIDocument = GetComponent<UIDocument>();
        var root = UIDocument.rootVisualElement;

        shieldButton = root.Q<Button>("ShieldButton");
        shieldButtonInactive = root.Q<VisualElement>("ShieldButtonInactive");
        timer = root.Q<Label>("Timer");
        screen = root.Q<Button>("Screen");
        pauseButton = root.Q<Button>("PauseButton");
        menu = root.Q<GroupBox>("MenuScreen");

        menuHeader = menu.Q<Label>("MenuWindow");
        resumeButton = menuHeader.Q<Button>("ResumeButton");
        restartButton = menuHeader.Q<Button>("RestartButton");


        Cannon cannon = cannonP1.GetComponent<Cannon>();

        //existence of screen we are tapping ensures that we wont start shooting by clicking pause/shield buttons
        screen.RegisterCallback<PointerDownEvent>(cannon.OnScreenClick, TrickleDown.TrickleDown);

        shieldButton.RegisterCallback<ClickEvent>(ev => ActivateShield());
        pauseButton.RegisterCallback<ClickEvent>(ev => ActivatePause());
        resumeButton.RegisterCallback<ClickEvent>(ev => DisactivatePause());
        restartButton.RegisterCallback<ClickEvent>(ev => StartCoroutine(RestartGame()));

    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void DisactivatePause()
    {
        Time.timeScale = 1;
        menu.visible = false;
    }

    private void ActivatePause()
    {
        Time.timeScale = 0;
        menu.visible = true;
    }
    private void ActivateShield()
    {
        if(!shieldP1.activeSelf)
        {
            shieldP1.SetActive(true);
            shieldButton.visible = false;
            shieldButtonInactive.visible = true;
            timer.visible = true;

            timeLeft = SP1Cooldown.value;
        }
        
    }

    void Start()
    {
       

    }

    

    // Update is called once per frame
    void Update()
    {
        if (!shieldButton.visible)
        {
            timeLeft -= Time.deltaTime;
            
            timer.text = (int)(timeLeft / 60) + ":" + (int)(timeLeft % 60 / 10) + (int)(timeLeft % 60 % 10);

            if (timeLeft <= 0)
            {
                shieldButtonInactive.visible = false;
                shieldButton.visible = true;
                timer.visible = false;
                timeLeft = SP1Cooldown.value;
            }
        }
    }

    
}
