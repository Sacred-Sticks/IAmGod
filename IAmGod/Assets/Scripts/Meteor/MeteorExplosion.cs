using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MeteorExplosion : MonoBehaviour
{
    public LayerMask groundLayers;

    private void OnCollisionEnter(Collision collision)
    {
        if (groundLayers == (groundLayers | (1 << collision.gameObject.layer)))
        {
            Destroy(gameObject);
        }
    }
}
