using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private List<GameObject> spawnPoints;

    [SerializeField] private float spawnTimer;
    [SerializeField] public float spawnFrequency;
    public bool spawningEnabled;

    [SerializeField] private ObjectPool enemyPool;

    [Header("DEBUG")]
    [SerializeField] private GameObject lastSpawnpoint;
    [SerializeField] private GameObject lastLastSpawnpoint;

    public static EnemySpawner instance;
    private void Awake()
    {
        if (instance && instance != this)
        {
            Destroy(instance);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        spawnTimer = 0;
    }

    private void Update()
    {
        if (spawningEnabled)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnFrequency)
            {
                SpawnEnemyRandom();
                spawnTimer = 0;
            }
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

        PooledObject pooledEnemy = enemyPool.GetPooledObject();

        pooledEnemy.transform.position = spawnPos.transform.position;
        pooledEnemy.transform.rotation = spawnPos.transform.rotation;

        pooledEnemy.gameObject.GetComponent<NavMeshAgent>().enabled = true;

        //Instantiate(enemy, spawnPos.transform.position, spawnPos.transform.rotation);
    }

    public void DespawnAllEnemies ()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < allEnemies.Length; i++)
            if (allEnemies[i].activeInHierarchy)
                enemyPool.DestroyPooledObject(allEnemies[i].GetComponent<PooledObject>());
    }
    
    public void DestroyEnemy(PooledObject enemy)
    {
        enemyPool.DestroyPooledObject(enemy);
    }
}
