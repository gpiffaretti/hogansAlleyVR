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

    [SerializeField]
    GameObject fireSprite;

    float displayTime;

    /// <summary>
    /// This is the time between the "Fire" signal and the actual shot
    /// </summary>
    float shootHintTime = 0.7f;

    Timer timer;
    Animator animator;

    public event Action<Person> enemyShot;
    public event Action<Person> personHide;
    public event Action<Person> personKilled;

    [SerializeField]
    AudioClip personHitClip;

    [SerializeField]
    AudioClip enemyShoutFireClip;

    [SerializeField]
    AudioClip enemyShotClip;

    [SerializeField]
    BoxCollider collisionBox;

    AudioSource audioSource;

    float initTime;

    bool isUp;

    Coroutine personCoroutine;

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

        personCoroutine = StartCoroutine(PersonCoroutine());
    }

    private IEnumerator PersonCoroutine()
    {
        yield return new WaitForSeconds(this.displayTime - this.shootHintTime);

        if (isEnemy)
        {
            // play fire effect
            audioSource.PlayOneShot(enemyShoutFireClip);
            fireSprite.SetActive(true);
        }

        yield return new WaitForSeconds(this.shootHintTime);

        if (isEnemy)
        {
            Shoot();
            fireSprite.SetActive(false);
        }

        Hide();
    }

    public void Hit()
    {
        if (isUp)
        {
            collisionBox.enabled = false;
            StopCoroutine(personCoroutine);
            personCoroutine = null;
            animator.SetTrigger("hit");
            isUp = false;
            fireSprite.SetActive(false);
            personKilled?.Invoke(this);
            audioSource.PlayOneShot(personHitClip);
        }
    }

    private void Shoot()
    {
        //Debug.Log($"{gameObject.name} shoots!");
        audioSource.PlayOneShot(enemyShotClip);
        enemyShot?.Invoke(this);

    }

    public void Hide()
    {
        //Debug.Log($"{gameObject.name} hides!");
        collisionBox.enabled = false;
		fireSprite.SetActive(false);
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
