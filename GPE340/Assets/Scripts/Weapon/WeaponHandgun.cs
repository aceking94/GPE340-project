using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandgun :Weapon
{
    [SerializeField] private GameObject prefabBullet;
    [SerializeField] private Transform shootStart;
    [SerializeField] private float shotsPerMinute;
    [SerializeField] private int bulletDistance = 2;

    public bool shooting;
    public int singleDamage = 10;
    public int burstDamage = 5;

    private int shotCounter = 0;
    private float shootDelay;


    public override void Awake()
    {
        shootDelay = Time.time; // set delay
    }

    public override void Start()
    {
        // Call parent class
        base.Start();
    }

    public override void Update()
    {
        if (shooting)
        {
            while (Time.time > shootDelay)
            {
                // Shoot
                Shoot();
                shootDelay += 60f / shotsPerMinute;
            }
        }
        else if (Time.time > shootDelay)
        {
            //Reset timmer
            shootDelay = Time.time;
        }
    }

    public void Shoot()
    {
        if (firingMode == FiringMode.Single && shotCounter < 1) //fire one bullet with each click
        {
            // Instantiate
            GameObject bulletObject = Instantiate(prefabBullet, shootStart.position, shootStart.rotation * Quaternion.Euler(Random.onUnitSphere * bulletDistance)) as GameObject;

            // Get bullet class refrence
            Bullet bullet = bulletObject.gameObject.GetComponent<Bullet>();

            //Set damage
            bullet.damage = singleDamage;

            //Move bullet
            Rigidbody rgbd = bulletObject.GetComponent<Rigidbody>();
            rgbd.AddRelativeForce(Vector3.forward * bullet.bulletSpeed, ForceMode.VelocityChange);// move bullet

            // Increment shot counter
            shotCounter++;
        }
        else if (firingMode == FiringMode.Burst && shotCounter < 3)//fire multiple bullets with one click 
        {
            // Instantiate
            GameObject bulletObject = Instantiate(prefabBullet, shootStart.position, shootStart.rotation * Quaternion.Euler(Random.onUnitSphere * bulletDistance)) as GameObject;

            // Get bullet class refrence
            Bullet bullet = bulletObject.gameObject.GetComponent<Bullet>();

            //Set damage
            bullet.damage = burstDamage;

            //Move bullet
            Rigidbody rgbd = bulletObject.GetComponent<Rigidbody>();
            rgbd.AddRelativeForce(Vector3.forward * bullet.bulletSpeed, ForceMode.VelocityChange);

            // Increment shot counter
            shotCounter++;
        }
        else if (firingMode == FiringMode.Empty)
        {
            //relode or something else          
        }
    }

    public override void AttackStart()
    {
        shooting = true;
    }

    public override void AttackEnd()
    {
        shooting = false;
        shotCounter = 0;
    }
}
