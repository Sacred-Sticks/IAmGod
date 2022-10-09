using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strike : MonoBehaviour
{
    [SerializeField] private string tableName;
    Transform table;

    private void Awake()
    {
        table = GameObject.Find("Table").transform;
    }

    private void Update()
    {
        if (transform.position.y <= table.position.y)
        {
            GetComponent<AOEAttack>().DealDamage();
        }
    }
}
