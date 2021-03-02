using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public float bulletSpeed = 200f;
    [SerializeField] private float lifespan = 30f; // Lifespan of bullet

    public void Start()
    {         
        Destroy(gameObject, lifespan);
    }

    public void OnCollisionEnter(Collision hitTarget)
    {
        if (hitTarget.gameObject.tag == "Bullet")
        {
            // Do nothing
        }         
        else
        {
            // If enemy
            GameObject enemy = GameObject.FindWithTag("Enemy");
            if (hitTarget.gameObject == enemy)
            {
                // Get enemy script
                EnemyData enemyData = enemy.GetComponent<EnemyData>();
                // Apply damage
                enemyData.health -= damage;
            }
            // Destroy bullet
            Destroy(gameObject);
        }
       
    }
}
