using Autohand;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField] private SpawningData spawningData;
    [SerializeField] private MeteorData meteorData;

    [System.Serializable] private struct SpawningData
    {
        public float averageTime;
        public float timeVariance;
        public float positionY;
        public float distance;
    }
    [System.Serializable] private struct MeteorData
    {
        public GameObject meteorGameObject;
        public float speed;
    }

    float respawnTimer, spawnRotationY, spawnPosX, spawnPosZ;
    Vector3 spawnPosition;
    Quaternion spawnRotation;
    GameObject meteorInstance;
    CapsuleCollider playerCollider;

    private IEnumerator Start()
    {
        playerCollider = AutoHandPlayer.Instance.gameObject.GetComponent<CapsuleCollider>();

        while (true)
        {
            SpawnMeteor();
            SetRespawnTimer();
            yield return new WaitForSeconds(respawnTimer);
        }
    }

    private void SpawnMeteor()
    {
        SetRotation();
        SetHeight();
        meteorInstance = Instantiate(meteorData.meteorGameObject, spawnPosition, spawnRotation);
        SetPosition();
        SetVelocity();
    }

    private void SetRotation()
    {
        spawnRotationY = Random.Range(-180, 180);
        spawnRotation = Quaternion.Euler(0, spawnRotationY, 0);
    }

    private void SetHeight()
    {
        spawnPosition = new(0, spawningData.positionY, 0);
    }

    private void SetPosition()
    {
        meteorInstance.transform.position += meteorInstance.transform.forward * spawningData.distance;
        meteorInstance.transform.Rotate(0, 180, 0);
    }

    private void SetVelocity()
    {
        meteorInstance.GetComponent<Rigidbody>().velocity = meteorInstance.transform.forward * meteorData.speed;
    }

    private void SetRespawnTimer()
    {
        respawnTimer = Random.Range(-spawningData.timeVariance, spawningData.timeVariance) + spawningData.averageTime;
    }
}
