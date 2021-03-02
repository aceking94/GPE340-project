using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup
{
    [SerializeField] private float healAmount;

    protected override void OnPickUp(CharacterController cc)
    {
        // Apply healing
        cc.health.Heal(healAmount);

        base.OnPickUp(cc);
    }
}
