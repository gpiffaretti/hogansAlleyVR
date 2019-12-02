using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : UIMenu
{
    public Button endButton;
    public TextMeshProUGUI labelTimer;
    public TextMeshProUGUI labelRound;
    public TextMeshProUGUI labelScore;
    public TextMeshProUGUI labelHighscore;

    public event Action stopClicked;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.IsPlaying)
        {
            labelTimer.text = gameManager.RemainingTime.ToString("00.00");
            labelScore.text = gameManager.CurrentScore.ToString("000000");
        }
    }

    void OnButtonEndClickedHandler()
    {
        base.gameManager.StopGame();

        stopClicked?.Invoke();
    }

    protected override void OnShow()
    {
        labelHighscore.text = gameManager.Highscore.ToString("000000");
    }

    protected override void OnHide()
    {
        
    }
}
