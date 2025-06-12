using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float hp = 30f;
    public Transform currentTarget;
    protected Animator animator;
    [SerializeField] protected float attackRange = 0.5f;
    float lastAttackTime = 0f;
    public float attackCooldown = 1.5f;
    public float damage = 5f;
    public bool dead = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        currentTarget = FindNearestBuilding();
    }
    

    // Update is called once per frame
    void Update()
    {
        if (dead) return;
        if (currentTarget == null || !currentTarget.gameObject.activeInHierarchy)
        {
            currentTarget = FindNearestBuilding();
            if (currentTarget == null) return; 
        }
        
        if (currentTarget != null)
        {
            float distance = Vector2.Distance(transform.position, currentTarget.position);

            if (distance > attackRange)
            {
                animator.SetBool("Moving", true);
                transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, moveSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool("Moving", false);
                AttackTarget();
            }
          
        }
    }
    
    public virtual void AttackTarget()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {

            Debug.Log("Enemy menyerang target!");
            lastAttackTime = Time.time;
            

            animator.SetTrigger("Attacking");
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Unit"))
        {
            currentTarget = other.transform;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Unit") && currentTarget == other.transform)
        {
            currentTarget = null;
        }
    }

    public void TakeDamage(float amount)
    {
        if(dead) return;
        hp -= amount;
        if (hp > 0)
        {
            animator.SetTrigger("Attacked");
        }
        else
        {
            dead = true;
            animator.SetBool("Dead", true);
        }
    }
    public void DamageTarget()
    {
        if (currentTarget == null) return;
        if (currentTarget.TryGetComponent<UnitController>(out UnitController unit))
        {
            Debug.Log("Unit Take Damage");
            unit.TakeDamage(damage);
        }
    }

    protected void Die()
    {
        Destroy(gameObject);
    }
    
    Transform FindNearestBuilding()
    {
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
        Transform nearest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject building in buildings)
        {
            float distance = Vector2.Distance(transform.position, building.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearest = building.transform;
            }
        }

        return nearest;
    }
}
