using Autohand;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustHeight : MonoBehaviour
{
    [SerializeField] private float heightDifference;

    private void Start()
    {
        float playerHeight = AutoHandPlayer.Instance.gameObject.GetComponent<CapsuleCollider>().height;
        transform.position = new(transform.position.x, playerHeight + heightDifference, transform.position.z);
        Destroy(this);
    }
}
