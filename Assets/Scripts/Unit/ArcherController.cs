using System;
using UnityEngine;
public class ArcherController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float hp = 30f;
    public Transform currentTarget;
    private Animator animator;
    private float attackRange = 3f;
    float lastAttackTime = 0f;
    public float attackCooldown = 1.5f;
    public float damage = 5f;


    void AttackTarget()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {

            Debug.Log("Enemy menyerang target!");
            lastAttackTime = Time.time;

            animator.SetTrigger("Attack");
        }
    }

    void Update()
    {
        if (currentTarget == null || !currentTarget.gameObject.activeInHierarchy)
        {
            currentTarget = FindNearestEnemy();
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

    public void TakeDamage(float amount)
    {
        animator.SetTrigger("Attacked");

        hp -= amount;
        if (hp <= 0)
        {
            Die();
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        currentTarget = FindNearestEnemy();
    }

    void Die()
    {
        Destroy(gameObject);
    }

    Transform FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform nearest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearest = enemy.transform;
            }
        }

        return nearest;
    }
}