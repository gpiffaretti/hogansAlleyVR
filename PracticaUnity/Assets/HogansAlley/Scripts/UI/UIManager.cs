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

        mainMenu.Show();
        current = mainMenu;
    }

    void OnButtonStartClick()
    {
        // Do something?
    }

    public void OnButtonStopClick()
    {

    }

    private void OnGameStartedHandler()
    {
        StartCoroutine(MenuTransition(inGameMenu));
    }

    private void OnGameFinishedHandler()
    {
        StartCoroutine(MenuTransition(mainMenu));
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
