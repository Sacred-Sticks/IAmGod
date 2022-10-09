using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrike : MonoBehaviour
{
    [SerializeField] private GameObject lightning;
    [SerializeField] private float strikeWaitTime;
    [SerializeField] private LayerMask cloudLayers;

    private Vector3 initialHandPos;
    private bool canStrike = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!(cloudLayers == (cloudLayers | 1 << other.gameObject.layer))) return;
        //Debug.Log("Trigger entered");

        initialHandPos = transform.position;

        if (canStrike)
        {
            StartCoroutine(Strike());
        }
    }

    private IEnumerator Strike()
    {
        Instantiate(lightning, initialHandPos, Quaternion.Euler(0, 0, 0));
        canStrike = false;
        yield return new WaitForSeconds(strikeWaitTime);
        canStrike = true;
    }
}
