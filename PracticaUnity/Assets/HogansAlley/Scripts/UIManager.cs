using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button startButton;
    public Button endButton;

    public TextMeshProUGUI labelTimer;

    GameManager gameManager;

    

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.gameFinished += OnGameFinishedHandler;
        endButton.gameObject.SetActive(false);
        labelTimer.gameObject.SetActive(false);
    }

    public void OnButtonStartClick()
    {
        gameManager.StartGame();

        SetupInGameMenu();
    }

    public void OnButtonStopClick()
    {
        gameManager.StopGame();
        SetupMainMenu();
    }

    private void OnGameFinishedHandler()
    {
        SetupMainMenu();
    }

    private void SetupMainMenu()
    {
        startButton.gameObject.SetActive(true);
        endButton.gameObject.SetActive(false);
        labelTimer.gameObject.SetActive(false);
    }

    private void SetupInGameMenu()
    {
        startButton.gameObject.SetActive(false);
        endButton.gameObject.SetActive(true);
        labelTimer.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.IsPlaying)
        {
            labelTimer.text = $"Time: {gameManager.ElapsedTime.ToString("0.##")}";
        }
    }
}
