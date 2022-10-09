using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEDamage : MonoBehaviour
{
    [SerializeField] private LayerMask damagingLayers;

    private Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (damagingLayers == (damagingLayers | (1 << other.gameObject.layer)))
        {
            other.gameObject.GetComponent<AOEAttack>().AddEnemy(character);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (damagingLayers == (damagingLayers | (1 << other.gameObject.layer)))
        {
            other.gameObject.GetComponent<AOEAttack>().RemoveEnemy(character);
        }
    }
}
