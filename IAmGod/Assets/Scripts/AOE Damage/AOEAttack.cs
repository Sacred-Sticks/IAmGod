using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEAttack : MonoBehaviour
{
    [SerializeField] private int damage;

    private List<Character> enemies = new();

    public void DealDamage()
    {
        foreach (Character enemy in enemies)
        {
            enemy.Damage(damage);
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
