using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{

    private void Awake()
    {
        Spawner.Instance.AddSpawnPoint(this);
    }

}
