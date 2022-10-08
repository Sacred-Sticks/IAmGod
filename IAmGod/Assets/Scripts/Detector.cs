using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    private Character parent;
    private void Start() 
    {    
        parent = GetComponentInParent(typeof(Character)) as Character;
    }
    private void OnTriggerEnter(Collider other)
    {
        if((layerMask.value & (1 << other.transform.gameObject.layer)) > 0) {
            parent.UpdateTarget(other.gameObject.transform);
        }
    }
}