using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    [SerializeField]
    private float lifespan; //pick up location lifespan.

    private void Awake()
    {
        Destroy(gameObject, lifespan);
    }

    private void OnTriggerEnter(Collider collider)
    {
        CharacterController cc = collider.GetComponent<CharacterController>();
        if (cc)
        {
            OnPickUp(cc);
        }
    }

    protected virtual void OnPickUp(CharacterController cc)
    {
        Destroy(gameObject);
    }
}
