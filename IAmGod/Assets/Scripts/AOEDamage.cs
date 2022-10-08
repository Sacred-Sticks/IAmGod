using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEDamage : MonoBehaviour
{
    [SerializeField] private LayerMask damagableDealers;

    private void OnTriggerEnter(Collider other)
    {
        if (damagableDealers == (damagableDealers | (1 << other.gameObject.layer)))
        {
            other.gameObject.GetComponent<MeteorExplosion>().AddEnemy(GetComponent<Character>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (damagableDealers == (damagableDealers | (1 << other.gameObject.layer)))
        {
            other.gameObject.GetComponent<MeteorExplosion>().RemoveEnemy(GetComponent<Character>());
        }
    }
}
