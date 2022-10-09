using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowOverTime : MonoBehaviour
{
    [SerializeField] private float growTime;
    [SerializeField] private float fallTime;
    [SerializeField] private float growthMultiplier;
    [SerializeField] private float fallingMultiplier;

    private bool growing = true;
    private Rigidbody body;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(growTime);
        growing = false;
        yield return new WaitForSeconds(fallTime);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (growing)
        {
            transform.localScale += Time.deltaTime * Vector3.one * growthMultiplier;
            return;
        }

        body.velocity = -Vector3.up * fallingMultiplier;
    }
}
