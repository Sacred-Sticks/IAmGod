using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class MeteorExplosion : MonoBehaviour
{
    [SerializeField] private ExplosionData explosionData;
    [SerializeField] private AttackData attackData;

    private List<Character> enemies = new();

    [System.Serializable] private struct ExplosionData
    {
        public LayerMask groundLayers;
        public GameObject contactExplosion;
    }
    [System.Serializable] private struct AttackData
    {
        public LayerMask attackLayers;
        public int damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (attackData.attackLayers == (attackData.attackLayers | (1 << collision.gameObject.layer)))
        {
            foreach(Character enemy in enemies)
            {
                enemy.Damage(attackData.damage);
            }

            Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, explosionData.groundLayers);

            Instantiate(explosionData.contactExplosion, hit.point, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject);
        }
    }

    public void AddEnemy(Character enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(Character enemy)
    {
        enemies.Remove(enemy);
    }
}
