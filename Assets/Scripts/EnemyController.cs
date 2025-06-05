using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float hp = 30f;
    public Transform currentTarget;
    private Animator animator;
    private float attackRange = 0.5f;
    float lastAttackTime = 0f;
    public float attackCooldown = 1.5f;
    public float damage = 5f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        currentTarget = FindNearestBuilding();
    }

    // Update is called once per frame
    void Update()
    {
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
    
    void AttackTarget()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {

            Debug.Log("Enemy menyerang target!");
            lastAttackTime = Time.time;
            

            animator.SetTrigger("Attacking");
        }
    }

    // private void OnTriggerStay2D(Collider2D other)
    // {
    //     if (other.CompareTag("Unit"))
    //     {
    //         currentTarget = other.transform;
    //     }
    // }
    //
    // void OnTriggerExit2D(Collider2D other) {
    //     if (other.CompareTag("Unit") && currentTarget == other.transform) {
    //         currentTarget = null;
    //     }
    // }

    public void TakeDamage(float amount)
    {
        hp -= amount;
        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
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
