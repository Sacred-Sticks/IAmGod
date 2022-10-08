using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    public int health { get; private set; }

    public void Damage(int dmg) {
        health -= dmg;
        if (health <= 0)
            Destroy(gameObject);
    }
}
