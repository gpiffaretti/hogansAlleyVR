using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    Spawner spawner;

    [SerializeField]
    float sessionDuration;

    Timer timer;
    private bool isPlaying;
    private int currentScore;

    public bool IsPlaying { get { return isPlaying; } }
    public float ElapsedTime { get { return timer.ElapsedTime; } }

    public event Action gameFinished;

    // Start is called before the first frame update
    void Start()
    {
        timer = new Timer(sessionDuration);
        timer.timerFinished += OnGameFinishedHandler;
        isPlaying = false;

        spawner.personInstantiated += OnPersonInstantiated;
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
        isPlaying = true;
        timer.Start();
        spawner.StartSpawning();
    }

    public void StopGame()
    {
        isPlaying = false;
        spawner.StopSpawning(true);
    }

    private void FinishGame()
    {
        timer.Reset();
        isPlaying = false;
        spawner.StopSpawning(true);

        gameFinished?.Invoke();
        Debug.Log("Game finished!!!");
    }

    private void OnGameFinishedHandler()
    {
        FinishGame();
    }

    private void OnPersonInstantiated(Person person)
    {
        person.personDead += OnPersonDeadHandler;
        person.personShot += OnPersonShotHandler;
    }

    void OnPersonDeadHandler(Person person)
    {
        //AddScore(person.score);
    }

    void OnPersonShotHandler(Person person)
    {
        //AddScore(person.score);
    }

    private void AddScore(int score)
    {
        currentScore += score;
    }
}
