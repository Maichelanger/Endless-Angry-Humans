using UnityEngine;

public class SpawnPointController : MonoBehaviour
{
    [SerializeField] private GameObject[] humanPrefabs;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int maxHumans = 5;

    private float spawnTimer = 0f;
    private int currentHumans = 0;
    private GameObject[] instantiatedHumans;

    private void Start()
    {
        instantiatedHumans = new GameObject[maxHumans];
    }

    private void Update()
    {
        if (currentHumans >= maxHumans)
        {
            CheckIfHumanDied();
            return;
        }

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnRandomHuman();
            spawnTimer = 0f;
        }
    }

    private void SpawnRandomHuman()
    {
        int randomIndex = Random.Range(0, humanPrefabs.Length);
        GameObject human = Instantiate(humanPrefabs[randomIndex], transform.position, Quaternion.identity);
        instantiatedHumans[currentHumans] = human;
        currentHumans++;
    }

    private void CheckIfHumanDied()
    {
        for (int i = 0; i < currentHumans; i++)
        {
            if (instantiatedHumans[i] == null)
            {
                currentHumans--;
                break;
            }
        }
    }
}
