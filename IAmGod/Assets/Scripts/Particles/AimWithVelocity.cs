using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimWithVelocity : MonoBehaviour
{
    [SerializeField] private Rigidbody body;
    [SerializeField] private float particleMultiplier;
    [SerializeField] private float trailMultiplier;
    [SerializeField] private float lifetimeMultiplier;

    private ParticleSystem particles;
    private TrailRenderer trail;

    Vector3 lookAt;


    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
        trail = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        lookAt = transform.position + body.velocity;

        var particleMain = particles.main;

        particleMain.startSize = body.velocity.magnitude * particleMultiplier;
        particleMain.startLifetime = body.velocity.magnitude * lifetimeMultiplier;
        trail.startWidth = 0;
        trail.endWidth = body.velocity.magnitude * trailMultiplier;

        transform.LookAt(lookAt);
    }
}
