using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickShooter : MonoBehaviour
{
    [SerializeField]
    LayerMask layermask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Fire!");
            float maxDistance = 10;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);

            Person hitPerson = null;
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(ray, out hit, maxDistance, layermask.value))
            {
                hitPerson = hit.collider.gameObject.GetComponent<Person>();
                hitPerson.Hit();

            }


        }
    }
}
