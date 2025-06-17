using Game.Buildings;
using System;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;
public class UnitController : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float currentHP = 40f;
    public float maxHP = 40f;
    public Transform currentTarget;
    private Animator animator;
    public float attackRange = 2f;
    float lastAttackTime = 0f;
    public float damage = 3f;
    private int facingDirection = 1;
    public float attackCooldown = 1.3f;
    public int level = 1;
    protected bool isDead;
    [SerializeField] Collider2D triggerArea;
    [SerializeField] private Vector3 StartPosition;
    [Header("UI")]
    [SerializeField] protected Canvas canvas;
    [SerializeField] protected Image hpBar;
    private Barrack currentBarrack;

    public static event Action<UnitController> OnKilled;
    
    public void SetupTroop(UnitStats stats, Vector3 startPos)
    {
        maxHP = stats.maxHP;
        currentHP = maxHP;
        damage = stats.damage;
        attackRange = stats.attackRange;
        attackCooldown = stats.attackCooldown;

        StartPosition = startPos;
    }
    public bool GetDeadStatus() => isDead;

    private void Awake()
    {
        triggerArea.enabled = GameplayManager.instance.gameState == GameState.Night;
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        GameplayManager.instance.ChangeState += Instance_ChangeState;
        canvas.gameObject.SetActive(false);
        // currentTarget = FindNearestEnemy();
        
    }
    private void OnDestroy()
    {
        GameplayManager.instance.ChangeState -= Instance_ChangeState;
    }
    private void Instance_ChangeState()
    {
        bool isNight = GameplayManager.instance.gameState == GameState.Night;
        triggerArea.enabled = isNight;
        canvas.gameObject.SetActive(false);

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
    public void DamageTarget()
    {
        if (currentTarget == null) return;
        if (currentTarget.TryGetComponent<EnemyController>(out EnemyController enemy))
        {
            Debug.Log("Unit Take Damage");
            enemy.TakeDamage(damage);
        }
    }
    public void TakeDamage(float amount)
    {
        if(isDead) return;
        currentHP -= amount;
        canvas.gameObject.SetActive(true);
        hpBar.fillAmount = currentHP/maxHP;
        if (currentHP > 0)
        {
            animator.SetTrigger("Attacked");
        }
        else
        {
            isDead = true;
            canvas.gameObject.SetActive(false);
            animator.SetTrigger("Dead");
        }
    }

    void Die()
    {
        OnKilled?.Invoke(this);
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