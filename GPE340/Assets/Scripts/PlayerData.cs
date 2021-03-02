using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerData : MonoBehaviour
{
    // Events for healing, damage, and death
    [SerializeField] private UnityEvent onHeal;
    [SerializeField] private UnityEvent onDamage;
    [SerializeField] private UnityEvent onDie;

    [SerializeField] public float maxHP = 100;
    [SerializeField] private float currentHP = 80;
   
    public float percentHP { get { return currentHP / maxHP; } } //UI percent health

    public void Damage(float damage)
    {
        //Pick the highest value between 0 and damage.
        if (damage < 0f)
        {
            damage = 0f;
        }
        else
        {
            
            // Clamp to prevent going over/under the set health
            currentHP = Mathf.Clamp(currentHP - damage, 0f, maxHP);
            SendMessage("OnDamage", SendMessageOptions.DontRequireReceiver); // call methos
        }
        // If 0, death
        if (currentHP == 0f)
        {
            SendMessage("OnDie", SendMessageOptions.DontRequireReceiver); // call method
            onDie.Invoke(); // play event
        }
    }

    public void Heal(float health)
    {
        //Pick the highest value between 0 and damage.
        if(health < 0)
        {
            health = 0f;
        }
        else
        {
            // Clamp to prevent going over/under the set health
            currentHP = Mathf.Clamp(currentHP + health, 0f, maxHP);
            SendMessage("OnHeal", SendMessageOptions.DontRequireReceiver);
        }
    }

}
