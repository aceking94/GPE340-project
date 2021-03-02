using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : Pickup
{
    [SerializeField] private float damageAmount;

    protected override void OnPickUp(CharacterController cc)
    {
        // Apply damage
        cc.health.Damage(damageAmount);

        base.OnPickUp(cc);
    }
}
