using Autohand;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(TrailRenderer))]
public class DisableIfHeld : MonoBehaviour
{
    [SerializeField] private float threshold;

    private ParticleSystem particles;
    private TrailRenderer trail;
    private Grabbable grabbable;

    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
        trail = GetComponent<TrailRenderer>();
        grabbable = transform.parent.GetComponent<Grabbable>();
    }

    private void Update()
    {
        if (grabbable.IsHeld())
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
