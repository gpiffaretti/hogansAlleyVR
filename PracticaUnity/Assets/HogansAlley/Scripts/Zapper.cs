using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zapper : MonoBehaviour
{
    [SerializeField]
    LayerMask layermask;

    [SerializeField]
    Transform shotOrigin;

    [SerializeField]
    AudioClip shotMissSound;

    [SerializeField]
    AudioClip shotEnemySound;

    public event Action<Person> shot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            Debug.Log("Fire!");
            float maxDistance = 10;

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
                SoundManager.Instance.Play2DSound(shotEnemySound);
            }
            else
            {
                SoundManager.Instance.Play2DSound(shotMissSound);
            }

            shot?.Invoke(hitPerson);


        }
    }
}
