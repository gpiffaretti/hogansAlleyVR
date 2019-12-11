using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    Spawner spawner;

    [SerializeField]
    float sessionDuration;

    Timer timer;
    private bool isPlaying;
    public int CurrentScore { get; private set; }

    public bool IsPlaying { get { return isPlaying; } }
    public float RemainingTime { get { return timer.CountDown; } }

    private int highscore;
    public int Highscore { get { return highscore; } }

    [SerializeField]
    AudioClip gameStartsClip;

    [SerializeField]
    AudioClip gameEndsClip;

    public event Action gameStarted;
	public event Action gameFinished;

    public event Action<int> newHighscore;

    // Start is called before the first frame update
    void Start()
    {
        timer = new Timer(sessionDuration);
        timer.timerFinished += OnGameFinishedHandler;
        isPlaying = false;
        CurrentScore = 0;

        LoadHighScore();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            timer.Update();
        }
        
    }

    public void StartGame()
    {
        LoadGameElementsScene();
        StartCoroutine(StartGameCoroutine());
    }


    private bool sceneLoadedFlag = false;
    /// <summary>
    /// Loads the scene that contains buildings, spawners, people data, etc.
    /// </summary>
    private void LoadGameElementsScene()
    {
        if (!sceneLoadedFlag)
        {
            Debug.Log("Load environment scene");
            SceneManager.LoadSceneAsync("SceneObjects", LoadSceneMode.Additive);
            SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) => {
                sceneLoadedFlag = true;
                spawner = Spawner.Instance;
                spawner.personInstantiated += OnPersonInstantiated;
                Debug.Log("SceneObjects scene loaded!");
            };
        }
    }

    private IEnumerator StartGameCoroutine()
	{
        // Reset score
        CurrentScore = 0;

        // Play start game music
        SoundManager.Instance.PlayMusic(gameStartsClip);
        yield return new WaitForSeconds(gameStartsClip.length);

        // Wait until scene is loaded. Not really necessary, but let's avoid race conditions.
        yield return new WaitUntil(() => sceneLoadedFlag);

        isPlaying = true;
        timer.Reset();
        timer.Start();
        spawner.StartSpawning();
        gameStarted?.Invoke();
    }

    private void LoadHighScore()
    {
        highscore = PlayerPrefs.GetInt("highscore", 0);
    }

    private void StoreHighScore()
    {
        if (CurrentScore > highscore)
        {
            Debug.Log("New highscore!");
            highscore = CurrentScore;
            PlayerPrefs.SetInt("highscore", CurrentScore);
            newHighscore?.Invoke(highscore);
        }
        
    }

    public void StopGame()
    {
        FinishGame();
    }

    private void FinishGame()
    {
        isPlaying = false;
        spawner.StopSpawning(true);
        SoundManager.Instance.PlayMusic(gameEndsClip);

        StoreHighScore();

        gameFinished?.Invoke();
        Debug.Log("Game finished!!!");
    }

    private void OnGameFinishedHandler()
    {
        FinishGame();
    }

    private void OnPersonInstantiated(Person person)
    {
        person.personKilled += OnPersonKilledHandler;
        person.enemyShot += OnShotByEnemyHandler;
    }

    void OnPersonKilledHandler(Person person)
    {
        AddScore(person.PersonKilledScore);
    }

    void OnShotByEnemyHandler(Person person)
    {
        AddScore(person.EnemyShotScore);
    }

    private void AddScore(int score)
    {
        CurrentScore += score;
    }
}
