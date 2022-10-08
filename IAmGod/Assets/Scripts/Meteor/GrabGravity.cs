using Autohand;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(Grabbable))]
[RequireComponent(typeof(Rigidbody))]
public class GrabGravity : MonoBehaviour
{
    [SerializeField] private float waitTime;

    private bool isGrabbed;
    private Grabbable grabbable;
    private Rigidbody body;

    private void Awake()
    {
        grabbable = GetComponent<Grabbable>();
        body = GetComponent<Rigidbody>();
    }

    private IEnumerator Start()
    {
        StartCoroutine(DestroyOnTimer());

        while (!isGrabbed)
        {
            yield return new WaitForEndOfFrame();
            isGrabbed = grabbable.IsHeld();
        }
        StopCoroutine(DestroyOnTimer());
        body.useGravity = true;
    }

    private IEnumerator DestroyOnTimer()
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
