using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(TrailRenderer))]
public class DisableIfLowVelocity : MonoBehaviour
{
    [SerializeField] private Rigidbody body;
    [SerializeField] private float threshold;
    private ParticleSystem particles;
    private TrailRenderer trail;

    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
        trail = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        if (body.velocity.magnitude < threshold)
        {
            particles.Clear();
            particles.Stop();
            trail.enabled = false;
            trail.Clear();
        }
        else
        {
            particles.Play();
            trail.enabled = true;
        }
    }
}
