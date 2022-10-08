using Autohand;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grabbable))]
[RequireComponent(typeof(Rigidbody))]
public class GrabGravity : MonoBehaviour
{
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
        while (!isGrabbed)
        {
            yield return new WaitForEndOfFrame();
            isGrabbed = grabbable.IsHeld();
        }

        body.useGravity = true;
    }
}
