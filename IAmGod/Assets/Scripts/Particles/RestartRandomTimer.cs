using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RestartRandomTimer : MonoBehaviour
{
    [SerializeField] private float minWait, maxWait; 

    private ParticleSystem particles;
    private Vector3 initialPos;

    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
        initialPos = transform.position;
    }

    private IEnumerator Start()
    {
        while (true)
        {
            particles.Play();
            yield return new WaitForSeconds(Random.Range(minWait, maxWait));
            transform.position = initialPos;
        }
    }
}
