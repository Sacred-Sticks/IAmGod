using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Damagable : MonoBehaviour
{
    public int Health { get; private set; }
    [SerializeField] private bool Ally;
    [SerializeField] private int _health;
    [SerializeField] private int _damage;
    private NavMeshAgent agent;
    private Transform target;

    [SerializeField] private float rotationSpeed = 5f;
    private float timer = 0f;
    private float maxtimer = 1f;
    private Vector3 _randomPoint;

    void Start()
    {
        target = null;
        agent = GetComponent<NavMeshAgent>();
        agent.destination = RandomNavSphere(gameObject.transform.position, 2f, 3);
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > maxtimer) {
            timer = 0f;
            if(target == null) {
                _randomPoint = RandomNavSphere(gameObject.transform.position, 2f, 3);
                agent.destination = _randomPoint;
            }                
        }
        if (target == null) {
            MoveTowards(_randomPoint);
            RotateTowards(_randomPoint);
        } else {            
            MoveTowards(target.transform.position);
            RotateTowards(target.transform.position);
        }
        
    }
    public void UpdateTarget(Transform tgt)
    {
        if(target == null)
        {
            target = tgt;
            agent.destination = target.transform.position;
        }
    }
    public void Damage(int dmg) {
        Health -= dmg;
        if (Health <= 0) {
            GameManager.Instance.Death(Ally);
            Destroy(gameObject);
        }            
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)  //Beware, all code here and below be "borrowed"
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }
    private void MoveTowards(Vector3 target)
    {
        agent.SetDestination(target);
    }

    private void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

}
