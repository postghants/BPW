using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform player;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public Transform currentFloor;
    public float[] currentFloorStats;

    public float spawnDelay = 5;
    public int minBatchSize = 2;
    public int maxBatchSize = 4;
    public int enemy1Percent = 100;
    public int enemy2Percent = 100;
    public int enemy3Percent = 100;

    public float xSpawnDistance = 10f;
    public float ySpawnDistance = 6.5f;
    public float xSpawnDeviation = 3f;
    public float ySpawnDeviation = 4.5f;

    public float spawnTimer = 1;
    private Vector2 spawnPosition;
    private Vector2 spawnDeviation;
    private GameObject chosenEnemy;
    void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;   
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if(spawnTimer < 0)
        {
            SpawnEnemies();
            spawnTimer = spawnDelay + Random.Range(-1f, 1f);
        }
    }

    private void SpawnEnemies()
    {
        int randomEdge = Random.Range(1, 5);
        switch (randomEdge)
        {
            case 1: spawnPosition = new Vector2(xSpawnDistance, 0); break;
            case 2: spawnPosition = new Vector2(0, -ySpawnDistance); break;
            case 3: spawnPosition = new Vector2(-xSpawnDistance, 0); break;
            case 4: spawnPosition = new Vector2(0, ySpawnDistance); break;
        }
        for(int i = 0; i < Random.Range(minBatchSize, maxBatchSize); i++)
        {
            int random = Random.Range(1, 101);
            if(random <= enemy1Percent)
            {
                chosenEnemy = enemy1;
            }
            if(random > enemy1Percent && random <= enemy1Percent + enemy2Percent)
            {
                chosenEnemy = enemy2;
            }
            if(random > enemy1Percent + enemy2Percent && random <= enemy1Percent + enemy2Percent + enemy3Percent)
            {
                chosenEnemy = enemy3;
            }

            GameObject currentEnemy = Instantiate(chosenEnemy);
            if(chosenEnemy == enemy1 || chosenEnemy == enemy2)
            {
                EnemyController enemyController = currentEnemy.GetComponent<EnemyController>();
                enemyController.player = player;
                enemyController.fireTimer = Random.Range(1, enemyController.fireDelay);
                enemyController.xSpawnDistance = xSpawnDistance;
                enemyController.ySpawnDistance = ySpawnDistance;
            }
            else
            {
                EnemyMeleeController enemyMeleeController = currentEnemy.GetComponent<EnemyMeleeController>();
                enemyMeleeController.player = player;
                enemyMeleeController.xSpawnDistance = xSpawnDistance;
                enemyMeleeController.ySpawnDistance = ySpawnDistance;
            }
            
            Transform enemyTransform = currentEnemy.GetComponent<Transform>();
            enemyTransform.Translate(spawnPosition);
            if(spawnPosition.y != 0)
            {
                spawnDeviation = new Vector2(Random.Range(-xSpawnDeviation, xSpawnDeviation), 0);
            }
            if(spawnPosition.x != 0)
            {
                spawnDeviation = new Vector2(0, Random.Range(-ySpawnDeviation, ySpawnDeviation));

            }
            enemyTransform.Translate(spawnDeviation);
            enemyTransform.parent = currentFloor;
        }
    }
}
