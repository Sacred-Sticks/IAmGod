using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetable : MonoBehaviour
{
    protected LayerMask layerMask;
    public bool Ally { protected set { ally = value; } get { return ally; } }
    [SerializeField] protected bool ally;
    public int Health { get { return _health; } protected set { _health = value; } }
    [SerializeField] protected int _health;

    private void Start()
    {
        layerMask = (Ally ? LayerMask.GetMask("Enemy") : LayerMask.GetMask("Ally"));
    }

    public virtual void Damage(int dmg)
    { //take damage
        Health -= dmg;
        if (Health <= 0)
        {
            GameManager.Instance.Death(this);
            Destroy(gameObject);
        }
    }
}
