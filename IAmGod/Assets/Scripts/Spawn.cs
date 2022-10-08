using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject SpawnPoint { get { return spawnPoint; } private set { spawnPoint = value; } }
    [SerializeField] private GameObject spawnPoint;
}
