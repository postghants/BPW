using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerMovement playerMovement;
    public EnemySpawner enemySpawner;

    [SerializeField] public Transform[] floors = new Transform[11];
    [SerializeField] public ArrayList floorStats = new ArrayList(11);

    [SerializeField] public float[] floor10 = new float[12];         //0: spawnDelay
    [SerializeField] public float[] floor9 = new float[12];          //1: minBatchSize
    [SerializeField] public float[] floor8 = new float[12];          //2: maxBatchSize
    [SerializeField] public float[] floor7 = new float[12];          //3: enemy1Percent
    [SerializeField] public float[] floor6 = new float[12];          //4: enemy2Percent
    [SerializeField] public float[] floor5 = new float[12];          //5: enemy3Percent
    [SerializeField] public float[] floor4 = new float[12];          //6: xSpawnDistance
    [SerializeField] public float[] floor3 = new float[12];          //7: ySpawnDistance
    [SerializeField] public float[] floor2 = new float[12];          //8: xSpawnDeviation
    [SerializeField] public float[] floor1 = new float[12];          //9: ySpawnDeviation
    [SerializeField] public float[] floor0 = new float[12];          //10: beamsRemaining
                                                                     //11: beamMaxHealth
    public int beamsRemaining;
    public int currentFloor;
    public int topFloor;
    public float floorSpacing = 30f;
    public float floorDelay = 2f;
    public float[] currentFloorStats;

    public void Start()
    {
            floorStats.Add(floor0);
            floorStats.Add(floor1);
            floorStats.Add(floor2);
            floorStats.Add(floor3);
            floorStats.Add(floor4);
            floorStats.Add(floor5);
            floorStats.Add(floor6);
            floorStats.Add(floor7);
            floorStats.Add(floor8);
            floorStats.Add(floor9);
            floorStats.Add(floor10);

        currentFloor = topFloor;
        currentFloorStats = (float[])floorStats[topFloor];
        beamsRemaining = (int)currentFloorStats[10];
        enemySpawner.currentFloor = floors[currentFloor];
        enemySpawner.spawnDelay = currentFloorStats[0];
        enemySpawner.minBatchSize = (int)currentFloorStats[1];
        enemySpawner.maxBatchSize = (int)currentFloorStats[2];
        enemySpawner.enemy1Percent = (int)currentFloorStats[3];
        enemySpawner.enemy2Percent = (int)currentFloorStats[4];
        enemySpawner.enemy3Percent = (int)currentFloorStats[5];
        enemySpawner.xSpawnDistance = currentFloorStats[6];
        enemySpawner.ySpawnDistance = currentFloorStats[7];
        enemySpawner.xSpawnDeviation = currentFloorStats[8];
        enemySpawner.ySpawnDeviation = currentFloorStats[9];

        playerMovement.xFallDistance = currentFloorStats[6] + 0.65f;
        playerMovement.yFallDistance = currentFloorStats[7] + 0.75f;
    }
    // Update is called once per frame
    public void BeamDestroyed()
    {
        beamsRemaining--;
        if(beamsRemaining <= 0)
        {
            StartCoroutine(FinishFloor());
        }
    }

    public IEnumerator FinishFloor()
    {
        currentFloor--;
        if(currentFloor < 0)
        {
            gameManager.EndGame();
            yield return null;
        }
        else
        {
            playerMovement.disableMove = true;
            transform.Translate(new Vector2(0, floorSpacing));
            currentFloorStats = (float[])floorStats[currentFloor];
            enemySpawner.spawnTimer = floorDelay;
            enemySpawner.currentFloor = floors[currentFloor];
            enemySpawner.spawnDelay = currentFloorStats[0];
            enemySpawner.minBatchSize = (int)currentFloorStats[1];
            enemySpawner.maxBatchSize = (int)currentFloorStats[2];
            enemySpawner.enemy1Percent = (int)currentFloorStats[3];
            enemySpawner.enemy2Percent = (int)currentFloorStats[4];
            enemySpawner.enemy3Percent = (int)currentFloorStats[5];
            enemySpawner.xSpawnDistance = currentFloorStats[6];
            enemySpawner.ySpawnDistance = currentFloorStats[7];
            enemySpawner.xSpawnDeviation = currentFloorStats[8];
            enemySpawner.ySpawnDeviation = currentFloorStats[9];

            playerMovement.xFallDistance = currentFloorStats[6] + 0.65f;
            playerMovement.yFallDistance = currentFloorStats[7] + 0.75f;

            floors[currentFloor + 1].gameObject.SetActive(false);

            beamsRemaining = (int)currentFloorStats[8];
            enemySpawner.currentFloorStats = currentFloorStats;

            yield return new WaitForSeconds(floorDelay);
            playerMovement.disableMove = false;
        }
    }

    
}
