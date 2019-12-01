using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using System;

public class Person : MonoBehaviour
{
    private int id;

    [SerializeField]
    bool isEnemy;
    public bool IsEnemy { get { return isEnemy; } }

    [SerializeField]
    private int personKilledScore;

    [SerializeField]
    private int enemyShotScore;

    float displayTime;

    Timer timer;
    Animator animator;

    public event Action<Person> personShot;
    public event Action<Person> personHide;
    public event Action<Person> personDead;

    float initTime;

    bool isUp;

    private void Awake()
    {
        animator = GetComponent<Animator>();
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
        personShot?.Invoke(this);


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
