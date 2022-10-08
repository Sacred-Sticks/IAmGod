using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimWithVelocity : MonoBehaviour
{
    [SerializeField] private Rigidbody body;
    [SerializeField] private float particleMultiplier;
    [SerializeField] private float trailMultiplier;

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
        //particles.main.startRotationX = body.velocity.x;
        //particles.startRotation3D = body.velocity;
        //particles.rotation = body.velocity;
        lookAt = transform.position + body.velocity;

        var particleMain = particles.main;

        particleMain.startSize = body.velocity.magnitude * particleMultiplier;
        trail.startWidth = 0;
        trail.endWidth = body.velocity.magnitude * trailMultiplier;

        transform.LookAt(lookAt);
    }
}
