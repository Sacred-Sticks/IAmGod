using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : Targetable
{    
    public int DamageAmount;
    [SerializeField] private float _attackRange = .5f;
    [SerializeField] private float _detectionRange = .5f;
    private float _disengageRange;
    private NavMeshAgent agent;
    private Targetable target;
    [SerializeField] private Animator anim;

    [SerializeField] private float rotationSpeed = 5f;
    private float timer = 0f;
    [SerializeField] private float _roamTime = 1.5f;
    private Vector3 previous;
    private float velocity;

    private Transform _home = null;
    private Vector3 _nextPoint = Vector3.zero;
    void Start()
    {
        layerMask = Ally ? LayerMask.GetMask("Enemy") : LayerMask.GetMask("Ally");
        target = null;
        agent = GetComponent<NavMeshAgent>();
        agent.destination = RandomNavSphere(gameObject.transform.position, 2f, 3);

        _disengageRange = _attackRange + (_attackRange * 1.2f); //adjust scale
        _attackRange *= transform.localScale.x;
        _detectionRange *= transform.localScale.x;
        _disengageRange *= transform.localScale.x;
    }
    private void Update() {
        HandleTargeting();
        UpdateAnim();
    }

    private void HandleTargeting() {
        if (target == null) { //if char has no target =
            timer += Time.deltaTime;
            if (timer > _roamTime) { //every <_roamTime> seconds there is no target
                timer = 0f;
                if (Ally && _home != null)
                    _nextPoint = RandomNavSphere(_home.position, 1f, 3);
                else
                    _nextPoint = RandomNavSphere(gameObject.transform.position, 2f, 3);                          
            }
            Targetable potentialTarget = null;
            if (Ally)
                potentialTarget = GetClosestEnemy(GameManager.Instance.EnemyList, _detectionRange);
            else
                potentialTarget = GetClosestEnemy(GameManager.Instance.AllyList, Mathf.Infinity);
            if (potentialTarget != null) {
                target = potentialTarget;
            }
        } else { //if char has target
            if (Vector3.Distance(transform.position, target.Type == TargetType.Building ? target.GetComponent<Collider>().ClosestPoint(transform.position) : target.gameObject.transform.position) > _disengageRange)
                StopAttack();
            else
                Attack(target);
        }
        if (agent.isStopped && (target == null || Vector3.Distance(transform.position, target.Type == TargetType.Building ? target.GetComponent<Collider>().ClosestPoint(transform.position) : target.gameObject.transform.position) > _attackRange)) { //if player is too far or target is null, start looking again          
            target = null;
            anim.SetBool("Attacking", false);
            agent.isStopped = false;
        }

        Vector3 targetpos = (target == null) ? _nextPoint : target.transform.position; //get target pos, move + rotate
        MoveTowards(targetpos);
        RotateTowards(targetpos);
    }

    private void UpdateAnim()
    {
        velocity = (transform.position - previous).magnitude / Time.deltaTime;
        previous = transform.position;
        anim.SetFloat("Velocity", velocity);
    }
    public void Attack(Targetable tgt)
    {
        agent.isStopped = true;
        anim.SetBool("Attacking", true);
    }
    public void StopAttack()
    {
        agent.isStopped = false;
        anim.SetBool("Attacking", false);
    }
    public override void Damage(int dmg) //take damage
    {
        Health -= dmg;
        if (Health <= 0) {
            GameManager.Instance.Death(this);
            agent.isStopped = true;
            anim.SetBool("Dead", true);
            anim.SetBool("Attacking", false);
            Destroy(gameObject, 3f);
            enabled = false;
        }            
    }
    public void DealDamage() //deal damage
    {
        if (target != null) {
            Targetable toDealTo = target.gameObject.GetComponent<Targetable>();
            if (toDealTo != null)
                toDealTo.Damage(DamageAmount);
        }

    }
    public void InitHome(Transform t)
    {
        _home = t;
    }
    public void InitTarget()
    {
        target = GetClosestEnemy(GameManager.Instance.AllyList, Mathf.Infinity);
    }
    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask) //get random point on layer mask within sphere in radius
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
    private void RotateTowards(Vector3 target) //Rotate towards target with slerp
    {
        Vector3 direction = (target - transform.position).normalized;
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
    }
    private Targetable GetClosestEnemy(List<Targetable> enemies, float distance) //Retrieves closest enemy, returns null if father than max distance input
    {
        Targetable tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Targetable t in enemies) {
            if (t == null)
                continue;
            float dist = Vector3.Distance(t.gameObject.transform.position, currentPos);
            if (dist < minDist) {
                tMin = t;
                minDist = dist;
            }
        }
        if (Ally) {
            float m = minDist;
        }
        if (minDist < distance)
            return tMin;
        else
            return null;
    }
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectionRange  * transform.localScale.x);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _attackRange * transform.localScale.x);
    }
}