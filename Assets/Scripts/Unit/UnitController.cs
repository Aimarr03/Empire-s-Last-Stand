using System;
using UnityEditor.Tilemaps;
using UnityEngine;
public class UnitController : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float hp = 40f;
    public Transform currentTarget;
    private Animator animator;
    public float attackRange = 2f;
    float lastAttackTime = 0f;
    public float damage = 3f;
    private int facingDirection = 1;
    public float attackCooldown = 1.3f;
    protected bool isDead;





    void Start()
    {
        animator = GetComponent<Animator>();
        // currentTarget = FindNearestEnemy();
    }

    void Update()
    {
        // if (currentTarget == null || !currentTarget.gameObject.activeInHierarchy)
        // {
        //     currentTarget = FindNearestEnemy();
        //     if (currentTarget == null) return;
        // }
        if (isDead)
        {
            return;
        }

        if (currentTarget != null)
        {
            float distance = Vector2.Distance(transform.position, currentTarget.position);
            if ((currentTarget.position.x > transform.position.x && facingDirection == -1) ||
                    (currentTarget.position.x < transform.position.x && facingDirection == 1))
            {
                Flip();
            }
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
            lastAttackTime = Time.time;
            animator.SetTrigger("Attack");
        }
    }

    public void TakeDamage(float amount)
    {
        animator.SetTrigger("Attacked");

        hp -= amount;
        if (hp <= 0)
        {
            isDead = true;
            animator.SetTrigger("Dead");

        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            currentTarget = other.transform;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && currentTarget == other.transform)
        {
            currentTarget = null;
            animator.SetBool("Moving", false);
        }
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
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