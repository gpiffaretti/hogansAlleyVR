using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Singleton<Spawner>
{
    private static int NextPersonID;

    /// <summary>
    /// These are the child objects of this gameObject
    /// </summary>
    [SerializeField]
    private List<Transform> spawnPoints;

    // We keep track of taken and free spawn points. We only store their indexes
    private List<int> takenSpawnPoints;
    private List<int> freeSpawnPoints;

    private Dictionary<Person, int> personSpawnPointMap;

    [SerializeField]
    private Difficulty[] difficulties;

    [SerializeField]
    private Person[] peoplePrefabs;

    [SerializeField]
    private Transform peopleParent;

    public event Action<Person> personInstantiated;

    private Difficulty currentDifficulty;
    private int currentDifficultyIndex;
    private float difficultyStartedTimestamp;

    private Coroutine spawningCorroutine;

    AudioSource audioSource;

    [SerializeField]
    AudioClip peopleInstantiatedClip;

    private void Awake()
    {
        spawnPoints = new List<Transform>(12);
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Initialize()
    {
        Debug.Log("Spawner initialize");
        // Initialize level 0
        CheckNextDifficuly();

        // initialize lists with enough capacity to store all spawn points
        takenSpawnPoints = new List<int>(spawnPoints.Count);
        freeSpawnPoints = new List<int>(spawnPoints.Count);

        // initialize list with all free spawn points
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            freeSpawnPoints.Add(i);
        }

        personSpawnPointMap = new Dictionary<Person, int>();
    }

    public void AddSpawnPoint(SpawnPoint point)
    {
        spawnPoints.Add(point.transform);
    }

    public void StartSpawning()
    {
        Initialize();

        spawningCorroutine = StartCoroutine(SpawnPeople());
    }

    /// <summary>
    /// Stops spawning, and optionally hide all current people
    /// </summary>
    /// <param name="hideExisting">If true, hides existing people.</param>
    public void StopSpawning(bool hideExisting)
    {
        currentDifficulty = null;
        currentDifficultyIndex = 0;
        StopCoroutine(spawningCorroutine);

        if (hideExisting)
        {
            //foreach (Person person in personSpawnPointMap.Keys)
            //{
            //    person.Hide();
            //}
        }
    }

    private void CheckNextDifficuly()
    {
        if (currentDifficulty == null)
        {
            // INITIALIZE IN LEVEL 0
            currentDifficultyIndex = 0;
            currentDifficulty = difficulties[currentDifficultyIndex];
            difficultyStartedTimestamp = Time.time;

            Debug.Log("Started at level 0");

        }
        else if (currentDifficulty.DifficultyLevelDuration > 0
            && Time.time - difficultyStartedTimestamp > currentDifficulty.DifficultyLevelDuration)
        {
            // GO TO NEXT LEVEL BECAUSE ELAPSED TIME IS GREATER THAN LEVEL DURATION
            currentDifficultyIndex++;
            currentDifficulty = difficulties[currentDifficultyIndex];
            difficultyStartedTimestamp = Time.time; // save new timestamp

            Debug.Log("UP TO NEXT LEVEL!");

        }
    }

    IEnumerator SpawnPeople()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            float spawnCount = UnityEngine.Random.Range(currentDifficulty.SpawnsPerTurn.Min, currentDifficulty.SpawnsPerTurn.Max);

            for (int i = 0; i < spawnCount; i++)
            {
                // if no spawn points available, don't instantiate
                if (!IsSpawnPointAvailable()) continue;

                SpawnPerson();
            }
            audioSource.PlayOneShot(peopleInstantiatedClip);

            CheckNextDifficuly();

            // get a random time span for next spawn based on current difficulty
            float spanTime = UnityEngine.Random.Range(currentDifficulty.SpawnInterval.Min, currentDifficulty.SpawnInterval.Max);
            yield return new WaitForSeconds(spanTime);
        }
    }

    private void SpawnPerson()
    {
        Person personPrefab = GetRandomPerson();
        int spawnPointIndex = AcquireSpawnPoint();
        Transform spawnPoint = spawnPoints[spawnPointIndex];

        Person newPerson = Instantiate<Person>(personPrefab, spawnPoint.position, spawnPoint.rotation, peopleParent);

        float personDisplayTime = UnityEngine.Random.Range(currentDifficulty.DisplayTimeRange.Min, currentDifficulty.DisplayTimeRange.Max);

        newPerson.Initialize(NextPersonID++, personDisplayTime);
        newPerson.personKilled += OnPersonDeadHandler;
        newPerson.personHide += OnPersonHideHandler;

        //Debug.Log($"Add person {newPerson.Id} to spawn point map");
        personSpawnPointMap[newPerson] = spawnPointIndex; // remember who took this spawn point

        personInstantiated?.Invoke(newPerson);
    }

    private void OnPersonHideHandler(Person person)
    {
        // free the point
        int spawnPointIndexToReturn = personSpawnPointMap[person];
        FreeSpawnPoint(spawnPointIndexToReturn);

        // remove this person from the map
        //Debug.Log($"Remove person {person.Id} from spawn point map");
        personSpawnPointMap.Remove(person);
    }

    private void OnPersonDeadHandler(Person person)
    {
        
    }


    private Person GetRandomPerson()
    {
        int index = UnityEngine.Random.Range(0, peoplePrefabs.Length);
        return peoplePrefabs[index];
    }

    private bool IsSpawnPointAvailable() => freeSpawnPoints.Count > 0;

    private int AcquireSpawnPoint()
    {
        // calculate a random index within the free point list
        int random = UnityEngine.Random.Range(0, freeSpawnPoints.Count);
        int spawnPointIndex = freeSpawnPoints[random];

        // update tracking - move this point index from the 'free' list to the 'taken'
        freeSpawnPoints.RemoveAt(random);
        takenSpawnPoints.Add(spawnPointIndex);

        return spawnPointIndex;
    }

    private void FreeSpawnPoint(int spawnPointIndex)
    {
        takenSpawnPoints.Remove(spawnPointIndex);
        freeSpawnPoints.Add(spawnPointIndex);
    }
}
