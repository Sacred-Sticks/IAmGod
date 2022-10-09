using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnParticleFinish : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> particles;

    void Update()
    {
        for (int i = 0; i < particles.Count; i++)
        {
            if (!particles[i].isPlaying)
            {
                particles.Remove(particles[i]);
            }
        }

        if (particles.Count == 0)
        {
            Destroy(gameObject);
        }
    }
}
