using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public enum FiringMode { Empty, Single, Burst};
[System.Serializable]
public enum WeaponType { None = 0, Rifle = 1, Handgun = 2 }


public abstract class Weapon : MonoBehaviour
{
    //IK points
    public Transform rightHand;
    public Transform leftHand;

    //Events
    public UnityEvent OnAttackStart;
    public UnityEvent OnAttackEnd;

    //Bools
    protected bool canShoot;
    protected bool isShooting;
   
    // Firing mode
    public FiringMode firingMode = FiringMode.Single;
    
    //Type of wepon
    public WeaponType weaponType = WeaponType.None;
    
    public virtual void Awake()
    {

    } //empty

    public virtual void Start()
    {
        
    } //empty

    public virtual void Update()
    {
       
    } //empty

    public virtual void AttackStart()
    {         
        OnAttackStart.Invoke();
    } //invoke event

    public virtual void AttackEnd()
    {         
        OnAttackEnd.Invoke();
    } //invoke event

}
