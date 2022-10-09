using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class LightningStrike : MonoBehaviour
{
    [SerializeField] private HandsData handsData;
    [SerializeField] private SphereCastData sphereCastData;
    [System.Serializable] private struct HandsData
    {
        public GameObject leftHand;
        public GameObject rightHand;
    }
    [System.Serializable] private struct SphereCastData
    {
        public float radius;
        public Vector3 direction;
        public float maxDistance;
        public LayerMask layers;
    }

    private ParticleSystem particles;
    private Transform handTransform;
    private Vector3 initialHandPos, finalPos;

    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other != handsData.leftHand && other != handsData.rightHand) return;

        handTransform = other.transform;
        initialHandPos = handTransform.position;

        RaycastHit hit = GetSpherecast();
        Physics.Raycast(hit.collider.transform.position, Vector3.down, out hit);
        finalPos = hit.point;
    }

    private RaycastHit GetSpherecast()
    {
        Physics.SphereCast(initialHandPos, sphereCastData.radius, sphereCastData.direction, out RaycastHit hit);
        return hit;
    }
}
