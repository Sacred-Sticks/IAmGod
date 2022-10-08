using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    private Damagable parent;
    private void Start() 
    {    
        parent = GetComponentInParent(typeof(Damagable)) as Damagable;
    }
    private void OnTriggerEnter(Collider other)
    {
        if((layerMask.value & (1 << other.transform.gameObject.layer)) > 0) {
            parent.UpdateTarget(other.gameObject.transform);
        }
    }
}
