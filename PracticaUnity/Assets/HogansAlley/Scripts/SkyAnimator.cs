using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyAnimator : MonoBehaviour
{

    Zapper zapper;
    Animator skyAnimator;


    private void Start()
    {
        zapper = FindObjectOfType<Zapper>();
        skyAnimator = GameObject.FindGameObjectWithTag("skyAnimator").GetComponent<Animator>();

        zapper.shot += OnShotHandler;
        Spawner spawner = FindObjectOfType<Spawner>();
        spawner.personInstantiated += OnPersonInstantiated;
    }

    private void OnPersonInstantiated(Person person)
    {
        person.enemyShot += OnEnemyShot;
    }

    private void OnEnemyShot(Person enemy)
    {
        skyAnimator.SetTrigger("innocentShot");
    }

    private void OnShotHandler(Person person)
    {
        //Debug.Log("Sky animator:");
        if (person == null)
        {
            //Debug.Log("Shot");
            skyAnimator.SetTrigger("shot");
        }
        else if (person.IsEnemy)
        {
            //Debug.Log("Shot");
            skyAnimator.SetTrigger("shot");
        }
        else
        {
            //Debug.Log("Shot innocent");
            skyAnimator.SetTrigger("innocentShot");
        }

    }
}
