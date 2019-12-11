using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    GameManager gameManager;

    [SerializeField]
    MainMenu mainMenu;

    [SerializeField]
    InGameMenu inGameMenu;

    UIMenu current;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.gameStarted += OnGameStartedHandler;
        gameManager.gameFinished += OnGameFinishedHandler;

        mainMenu.startClicked += OnButtonStartClick;
        inGameMenu.stopClicked += OnButtonStopClick;
        inGameMenu.returnClicked += OnButtonReturnClick;

        mainMenu.Show();
        current = mainMenu;
    }

    
    void OnButtonStartClick()
    {
        // Do something?
    }

    public void OnButtonStopClick()
    {
        gameManager.StopGame();
    }

    private void OnButtonReturnClick()
    {
        StartCoroutine(MenuTransition(mainMenu));
    }

    private void OnGameStartedHandler()
    {
        StartCoroutine(MenuTransition(inGameMenu));
    }

    private void OnGameFinishedHandler()
    {
        
    }

    private IEnumerator MenuTransition(UIMenu next)
    {
        yield return StartCoroutine(this.current.HideCoroutine());
        current = next;
        yield return StartCoroutine(next.ShowCoroutine());

    }


//#if UNITY_EDITOR
//        if (Input.GetKeyDown(KeyCode.S))
//        {
//            if (!gameManager.IsPlaying)
//                OnButtonStartClick();
//            else
//                OnButtonStopClick();
//        }
//#endif
    
}
