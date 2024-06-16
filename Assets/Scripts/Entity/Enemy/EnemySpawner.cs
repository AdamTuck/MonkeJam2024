using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private List<GameObject> spawnPoints;

    [SerializeField] private float spawnTimer;
    [SerializeField] public float spawnFrequency;

    private void Start()
    {
        spawnTimer = 0;
    }
    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnFrequency)
        {
            SpawnEnemyRandom();
            spawnTimer = 0;
        }
    }

    private void SpawnEnemyRandom()
    {
        int randomPoint =  Random.Range(0, spawnPoints.Count-1);
        Transform spawnPos = spawnPoints[randomPoint].transform;
        Instantiate(enemy, spawnPos.position, spawnPos.rotation);
    }
}
