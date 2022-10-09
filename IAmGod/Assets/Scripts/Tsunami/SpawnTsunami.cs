using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTsunami : MonoBehaviour
{
    [SerializeField] private LayerMask waterLayers;
    [SerializeField] private GameObject tsunamiGameObject;


    private void OnTriggerEnter(Collider other)
    {
        if (waterLayers == (waterLayers | 1 << other.gameObject.layer))
        {
            Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, waterLayers);
            Instantiate(tsunamiGameObject, hit.point, transform.rotation);
        }
    }
}
