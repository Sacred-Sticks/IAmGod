using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    public int Health { get; private set; }
    [SerializeField] private bool Ally;
    [SerializeField] private int _health;
    [SerializeField] private int _damage;

    public void Damage(int dmg) {
        Health -= dmg;
        if (Health <= 0) {
            GameManager.Instance.Death(Ally);
            Destroy(gameObject);
        }            
    }
}
