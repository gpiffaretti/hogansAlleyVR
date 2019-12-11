using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : UIMenu
{
    public Button endButton;
    public Button returnButton;
    public TextMeshProUGUI labelTimer;
    public TextMeshProUGUI labelScore;
    public TextMeshProUGUI labelHighscore;

    public event Action stopClicked;
    public event Action returnClicked;

    void Awake()
    {
        Debug.Log($"{name} Awake!");
        IsActive = false;
        canvas = GetComponent<Canvas>();
        gameManager = GameManager.Instance;
    }

    void Start()
    {

        endButton.onClick.AddListener(OnButtonEndClickedHandler);
        returnButton.onClick.AddListener(OnButtonReturnClickedHandler);
    }


    private void OnNewHighscoreHandler(int newScore)
    {
        Debug.Log("New high score. Update label and animate");
        labelHighscore.text = newScore.ToString("000000");
        labelHighscore.GetComponent<Animator>().SetTrigger("newHighscore");
    }

    // Update is called once per frame
    void Update()
    {
        labelTimer.text = gameManager.RemainingTime.ToString("00.00");

        if (gameManager.IsPlaying)
        {
            
            labelScore.text = gameManager.CurrentScore.ToString("000000"); // TODO: improvement: update only when score changes
        }
    }

    private void OnGameFinishedHandler()
    {
        returnButton.gameObject.SetActive(true);
        endButton.gameObject.SetActive(false);
    }

    void OnButtonEndClickedHandler()
    {
        base.gameManager.StopGame();

        stopClicked?.Invoke();
    }

    private void OnButtonReturnClickedHandler()
    {
        returnClicked?.Invoke();
    }

    void OnEnable()
    {
        gameManager.gameFinished += OnGameFinishedHandler;
        gameManager.newHighscore += OnNewHighscoreHandler;
    }

    void OnDisable()
    {
        gameManager.gameFinished -= OnGameFinishedHandler;
        gameManager.newHighscore -= OnNewHighscoreHandler;
    }


    protected override void OnShow()
    {
        labelHighscore.text = gameManager.Highscore.ToString("000000");
        labelHighscore.GetComponent<Animator>().SetTrigger("normal");
        returnButton.gameObject.SetActive(false);
        endButton.gameObject.SetActive(true);

    }

    protected override void OnHide()
    {
        labelHighscore.GetComponent<Animator>().SetTrigger("normal");
    }
}
