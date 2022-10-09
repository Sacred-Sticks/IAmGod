using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrike : MonoBehaviour
{
    [SerializeField] private GameObject lightning;
    [SerializeField] private float strikeWaitTime;
    [SerializeField] private LayerMask cloudLayers;

    private Vector3 initialHandPos;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Particle Collision");
        if (!(cloudLayers == (cloudLayers | 1 << other.gameObject.layer))) return;

        initialHandPos = transform.position;

        Instantiate(lightning, initialHandPos, Quaternion.Euler(0, 0, 0));
        StartCoroutine(Strike());
    }

    private IEnumerator Strike()
    {
        yield return new WaitForSeconds(strikeWaitTime);
        lightning.GetComponentInChildren<AOEAttack>().DealDamage();
    }
}
