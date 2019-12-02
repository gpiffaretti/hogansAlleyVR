using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using System;

public class Person : MonoBehaviour
{
    private int id;
    public int Id { get { return id; } }

    [SerializeField]
    bool isEnemy;
    public bool IsEnemy { get { return isEnemy; } }

    [SerializeField]
    private int personKilledScore;
    public int PersonKilledScore { get { return personKilledScore; } }

    [SerializeField]
    private int enemyShotScore;
    public int EnemyShotScore { get { return enemyShotScore; } }

    float displayTime;

    Timer timer;
    Animator animator;

    public event Action<Person> enemyShot;
    public event Action<Person> personHide;
    public event Action<Person> personKilled;

    [SerializeField]
    AudioClip personHitClip;

    AudioSource audioSource;

    float initTime;

    bool isUp;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize(int id, float displayTime)
    {
        this.id = id;
        this.displayTime = displayTime;
        animator.SetTrigger("show");

        isUp = true;

        initTime = Time.time;
    }

    public void Hit()
    {
        if (isUp)
        {
            animator.SetTrigger("hit");
            isUp = false;
            personKilled?.Invoke(this);
            audioSource.PlayOneShot(personHitClip);
        }
    }

    private void Update()
    {
        if (Time.time - initTime >= displayTime && isUp)
        {
            if (isEnemy)
                Shoot();

            Hide();

        }
    }


    private void Shoot()
    {
        //Debug.Log($"{gameObject.name} shoots!");
        enemyShot?.Invoke(this);


    }

    public void Hide()
    {
        //Debug.Log($"{gameObject.name} hides!");
        animator.SetTrigger("hide");
        isUp = false;
    }

    void OnHideAnimationFinished()
    {
        personHide?.Invoke(this);
        Destroy(gameObject);
    }

    public override bool Equals(object other)
    {
        Person otherPerson = other as Person;
        return otherPerson != null && this.id == otherPerson.id;
    }

    public override int GetHashCode()
    {
        return id;
    }
}
