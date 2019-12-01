using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Difficulty")]
public class Difficulty : ScriptableObject
{
    [SerializeField]
    private float difficultyLevelDuration;
    public float DifficultyLevelDuration { get { return difficultyLevelDuration; } }

    [SerializeField]
    ValueRange spawnInterval;
    public ValueRange SpawnInterval { get { return spawnInterval; } }

    [SerializeField]
    ValueRange displayTimeRange;
    public ValueRange DisplayTimeRange { get { return displayTimeRange; } }

    [SerializeField]
    ValueRange spawnsPerTurn;
    public ValueRange SpawnsPerTurn { get { return spawnsPerTurn; } }

}

[Serializable]
public class ValueRange
{
    [SerializeField]
    private float min;
    public float Min { get { return min; } }

    [SerializeField]
    private float max;
    public float Max { get { return max; } }
}
