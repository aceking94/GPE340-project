using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public int health = 8;

    void Update()
    {
        if (health <= 0)
        {
            // Die
            Destroy(this.gameObject);
        }
    }
}
