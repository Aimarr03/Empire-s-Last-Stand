using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float hp = 30f;
    private Animator animator;
    public float attackRange = 3f;
    public float attackCooldown = 1.5f;
    public float damage = 5f;

    private float lastAttackTime = 0f;
    private Transform currentTarget;
    private int facingDirection = 1;
    private string targetTag;

    private void Start()
    {
        // Menentukan musuh berdasarkan tag sendiri
        targetTag = gameObject.tag == "Player" ? "Enemy" : "Player";
        currentTarget = FindNearestTarget();
    }

    private void Update()
    {
        if (currentTarget == null || !currentTarget.gameObject.activeInHierarchy)
        {
            currentTarget = FindNearestTarget();
            if (currentTarget == null) return;
        }

        float distance = Vector2.Distance(transform.position, currentTarget.position);

        if (distance > attackRange)
        {
            MoveToTarget();
        }
        else
        {
            AttackTarget();
        }
    }

    void MoveToTarget()
    {
        // Flip arah hadap jika perlu
        if ((currentTarget.position.x > transform.position.x && facingDirection == -1) ||
            (currentTarget.position.x < transform.position.x && facingDirection == 1))
        {
            Flip();
        }

        transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, moveSpeed * Time.deltaTime);
    }

    void AttackTarget()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Debug.Log($"{gameObject.name} menyerang target!");

            lastAttackTime = Time.time;

            // Coba berikan damage ke target jika punya komponen Fighter
            Unit targetFighter = currentTarget.GetComponent<Unit>();
            if (targetFighter != null)
            {
                targetFighter.TakeDamage(damage);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        hp -= amount;
        Debug.Log($"{gameObject.name} terkena serangan! HP tersisa: {hp}");

        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} mati!");
        Destroy(gameObject);
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    Transform FindNearestTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);
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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            currentTarget = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(targetTag) && currentTarget == other.transform)
        {
            currentTarget = null;
        }
    }
}
