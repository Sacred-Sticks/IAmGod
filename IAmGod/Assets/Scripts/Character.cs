using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    private LayerMask layerMask;
    public int Health { get; private set; }
    public bool Ally { private set { ally = value; } get { return ally; } }
    [SerializeField] private bool ally;
    [SerializeField] private int _health;
    public int DamageAmount;
    private NavMeshAgent agent;
    private Transform target;
    [SerializeField] private Animator anim;

    [SerializeField] private float rotationSpeed = 5f;
    private float timer = 0f;
    private float maxtimer = 1f;
    private Vector3 _randomPoint;
    private Vector3 previous;
    private float velocity;
    [SerializeField] private LayerMask foeMask;
    
    void Start()
    {
        layerMask = (Ally ? LayerMask.GetMask("Enemy") : LayerMask.GetMask("Ally"));
        target = null;
        agent = GetComponent<NavMeshAgent>();
        agent.destination = RandomNavSphere(gameObject.transform.position, 2f, 3);
    }
    private void Update()
    {
        if (target == null) {
            Transform potentialTarget = null;
            if (Ally)
                potentialTarget = GetClosestEnemy(GameManager.Instance.EnemyList, .5f);
            else
                potentialTarget = GetClosestEnemy(GameManager.Instance.AllyList, .5f);
            if (potentialTarget != null)
                target = potentialTarget;
        } else {
            float dist = Vector3.Distance(target.transform.position, transform.position);
            if (dist < .07f)
                Attack(target);
        }
        
        timer += Time.deltaTime;
        if (timer > maxtimer) {
            timer = 0f;
            if (target == null) {
                _randomPoint = RandomNavSphere(gameObject.transform.position, 2f, 3);
                agent.destination = _randomPoint;
            }
        }

        if (agent.isStopped && (target == null || Vector3.Distance(transform.position, target.position) > .5f)) {            
            target = null;
            anim.SetBool("Attacking", false);
            agent.isStopped = false;
        }
        
        Vector3 targetpos = (target == null) ? _randomPoint : target.transform.position;
        MoveTowards(targetpos);
        RotateTowards(targetpos);

        UpdateAnim();
    }
    private void UpdateAnim()
    {
        velocity = ((transform.position - previous).magnitude) / Time.deltaTime;
        previous = transform.position;
        anim.SetFloat("Velocity", velocity);
    }
    public void UpdateTarget(Transform tgt)
    {
        if(target == null)
        {
            target = tgt;
            agent.destination = target.transform.position;
        }
    }
    public void Attack(Transform tgt)
    {
        target = tgt;
        agent.isStopped = true;
        anim.SetBool("Attacking", true);
    }
    public void Damage(int dmg) {
        Health -= dmg;
        if (Health <= 0) {
            GameManager.Instance.Death(this);
            anim.SetBool("Dead", true);
            anim.SetBool("Attacking", false);
            Destroy(gameObject, 3f);
        }            
    }
    public void DealDamage()
    {
        if(target != null) {
            Character toDealTo = target.gameObject.GetComponent<Character>();
            if (toDealTo != null)
                toDealTo.Damage(DamageAmount);
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
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
    }
    Transform GetClosestEnemy(List<Character> enemies, float distance)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Character c in enemies) {
            float dist = Vector3.Distance(c.gameObject.transform.position, currentPos);
            if (dist < minDist) {
                tMin = c.gameObject.transform;
                minDist = dist;
            }
        }
        if (minDist > distance)
            return null;
        else
            return tMin;
    }

}
