using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTsunami : MonoBehaviour
{
    [SerializeField] private LayerMask meteorLayers;
    [SerializeField] private GameObject tsunamiGameObject;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered " + other.gameObject.name + "'s trigger");
        if (meteorLayers == (meteorLayers | 1 << other.gameObject.layer))
        {
            Debug.Log("Entered Water Trigger");
            Debug.Log(other.gameObject.name);
            Physics.Raycast(other.transform.position, Vector3.down, out RaycastHit hit);
            Instantiate(tsunamiGameObject, hit.point, Quaternion.Euler(0, 0, 0));
            Destroy(other.gameObject);
        }
    }
}
