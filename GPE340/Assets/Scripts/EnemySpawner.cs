using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyModel; // holds the model of the enemy
    private GameObject spawnedEnemy; // holds the spawned enemy
    public float spawnDelay; // delay between spawns
    private float nextSpawnTime; 
    private Transform tf;


    void Start()
    {
        tf = gameObject.GetComponent<Transform>();
        nextSpawnTime = Time.time + spawnDelay; //set spawn time
    }

    void Update()
    {
        // If null then spawn
        if (spawnedEnemy == null)
        {
            // Enough time passed
            if (Time.time > nextSpawnTime)
            {
                // Spawn & set next spawn time
                if (!spawnedEnemy)
                {
                    spawnedEnemy = Instantiate(enemyModel, tf.position, Quaternion.identity) as GameObject; // Spawns enemy model in location 
                    nextSpawnTime = Time.time + spawnDelay; // reset the timer
                }
               
            }
        }
        else
        {
            // Still alive so reset spawn time.
            nextSpawnTime = Time.time + spawnDelay;
        }
    }
}
