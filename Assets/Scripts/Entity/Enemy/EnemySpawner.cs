using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private List<GameObject> spawnPoints;

    [SerializeField] private float spawnTimer;
    [SerializeField] public float spawnFrequency;

    [Header("DEBUG")]
    [SerializeField] private GameObject lastSpawnpoint;
    [SerializeField] private GameObject lastLastSpawnpoint;

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
        int randomPoint = Random.Range(0, spawnPoints.Count - 1);
        GameObject spawnPos = spawnPoints[randomPoint];
        //Dont let enemies spawn at the same points over and over
        while (spawnPos == lastSpawnpoint || spawnPos == lastLastSpawnpoint)
        {
            randomPoint = Random.Range(0, spawnPoints.Count - 1);
            spawnPos = spawnPoints[randomPoint];
        }

        if (lastSpawnpoint != null)
        {
            lastLastSpawnpoint = lastSpawnpoint;
            lastSpawnpoint = spawnPos;
        }else
        {
            lastSpawnpoint = spawnPos;
        }


        Instantiate(enemy, spawnPos.transform.position, spawnPos.transform.rotation);
    }
}
