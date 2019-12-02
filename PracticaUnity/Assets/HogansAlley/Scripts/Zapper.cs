using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zapper : MonoBehaviour
{
    [SerializeField]
    LayerMask layermask;

    [SerializeField]
    float maxDistance = 15;

    [SerializeField]
    Transform shotOrigin;

    [SerializeField]
    AudioClip shotMissSound;

    [SerializeField]
    AudioClip shotEnemySound;

    public event Action<Person> shot;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            //Debug.Log("Fire!");
            
            Person hitPerson = null;
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(shotOrigin.position, shotOrigin.forward, out hit, maxDistance, layermask.value))
            {
                hitPerson = hit.collider.gameObject.GetComponent<Person>();
                hitPerson.Hit();

            }

            if (hitPerson != null && hitPerson.IsEnemy)
            {
                audioSource.PlayOneShot(shotEnemySound);
            }
            else
            {
                audioSource.PlayOneShot(shotMissSound);
            }

            shot?.Invoke(hitPerson);


        }
    }
}
