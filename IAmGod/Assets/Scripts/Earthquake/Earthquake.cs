using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;

public class Earthquake : MonoBehaviour
{
    [SerializeField] private EarthquakeData earthquakeData;

    [System.Serializable] private struct EarthquakeData
    {
        public float maxDistance;
        public float minimumSpeed;
    }

    private Grabbable grabbable;

    private bool lastHeld, isHeld, earthquakeActive;

    private void Awake()
    {
        grabbable = GetComponent<Grabbable>();
    }

    private void FixedUpdate()
    {
        lastHeld = isHeld;
        isHeld = grabbable.IsHeld();

        if (isHeld == lastHeld) return;

        if (!isHeld && !earthquakeActive) StartCoroutine(EarthquakeCycle());
    }

    private IEnumerator EarthquakeCycle()
    {
        earthquakeActive = true;
        yield return new WaitForEndOfFrame();
        earthquakeActive = false;
    }
}
