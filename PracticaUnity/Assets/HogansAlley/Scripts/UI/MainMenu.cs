using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : UIMenu
{
    public Button startButton;

    public TextMeshProUGUI labelHighscore;

    public event Action startClicked;
    

    void Awake()
    {
        Debug.Log($"{name} Awake!");
        IsActive = false;
        canvas = GetComponent<Canvas>();
        gameManager = GameManager.Instance;
    }

    void Start()
    {
        startButton.onClick.AddListener(OnButtonStartClick);
    }

    public void OnButtonStartClick()
    {
        gameManager.StartGame();
        base.Hide();
        startClicked?.Invoke();
    }

    

    protected override void OnShow()
    {
        labelHighscore.text = gameManager.Highscore.ToString("000000");
    }

    protected override void OnHide()
    {
        
    }
}
