using Game.Buildings;
using System;
#if UNITY_EDITOR
using UnityEditor.Tilemaps;
#endif
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class UnitController : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float currentHP = 40f;
    public float maxHP = 40f;
    public Transform currentTarget;
    public SpriteRenderer spriteRenderer;
    private Animator animator;
    public float attackRange = 2f;
    float lastAttackTime = 0f;
    public float damage = 3f;
    private int facingDirection = 1;
    public float attackCooldown = 1.3f;
    public int level = 1;
    protected bool isDead;
    [SerializeField] Collider2D collideArea;
    [SerializeField] Collider2D triggerArea;
    [SerializeField] private Vector3 StartPosition;
    [Header("UI")]
    [SerializeField] protected Canvas canvas;
    [SerializeField] protected Image hpBar;
    public SpriteRenderer selection;
    private Barrack currentBarrack;

    public bool commanded = false;
    public Vector3 Target = Vector3.zero;
    public static event Action<UnitController> OnKilled;
    private SpawnManager spawnManager;
    private NavMeshAgent navMeshAgent;
    bool isDone = false;
    public void CommandTroop(Vector3 pos)
    {
        commanded = true;
        Target = pos;
        navMeshAgent.SetDestination(Target);
        //navMeshAgent.isStopped = false;
    }
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
        //spriteRenderer = GetComponent<SpriteRenderer>();
        spawnManager = FindFirstObjectByType<SpawnManager>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateUpAxis = false;
        navMeshAgent.updateRotation = false;
        navMeshAgent.speed = moveSpeed;
        transform.rotation = Quaternion.identity;
        triggerArea.enabled = GameplayManager.instance.gameState == GameState.Night;
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        GameplayManager.instance.ChangeState += Instance_ChangeState;
        canvas.gameObject.SetActive(false);
        if (GameplayManager.instance.gameState == GameState.Night)
        {
            currentTarget = FindNearestEnemy();
        }
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
        isDone = GameplayManager.instance.gameState == GameState.Morning;
    }

    void Update()
    {
        // if (currentTarget == null || !currentTarget.gameObject.activeInHierarchy)
        // {
        //     currentTarget = FindNearestEnemy();
        //     if (currentTarget == null) return;
        // }
        if (GameplayManager.instance.gameState == GameState.Over) return;
        if (GameplayManager.instance.pause) return;
        if (isDead)
        {
            return;
        }
        if (commanded)
        {
            animator.SetBool("Moving", true);
            transform.position = Vector2.MoveTowards(transform.position, Target, moveSpeed * Time.deltaTime);
            if ((Target.x > transform.position.x && facingDirection == -1) ||
                    (Target.x < transform.position.x && facingDirection == 1))
            {
                Flip();
            }
            if (Vector2.Distance(transform.position, Target) < 1)
            {
                navMeshAgent.isStopped = true;   
                animator.SetBool("Moving", false);
                commanded = false;
            }    
            return;
        }
        if (isDone) return;
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
        else
        {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, 6f, Vector2.zero);
            //Debug.Log("ENEMY HIT: " + hit.collider.gameObject);
            if(hit.collider.TryGetComponent(out EnemyController enemy))
            {
                Debug.Log("New Target Enemy = " + enemy.gameObject.name);
                currentTarget = enemy.transform;
            }
        }

    }
    
    void AttackTarget()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            animator.SetTrigger("Attack");
            if(currentTarget != null)
            {
                if (currentTarget.GetComponent<EnemyController>().dead)
                {
                    Transform enemy = FindNearestEnemy();
                    if(enemy == null)
                    {
                        Debug.Log("Done");
                        isDone = true;
                        return;
                    }
                    currentTarget = enemy;
                }
            }
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
            collideArea.enabled = false;
        }
    }

    void Die()
    {
        OnKilled?.Invoke(this);
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (currentTarget != null) return;
        if (other.CompareTag("Enemy"))
        {
            currentTarget = other.transform;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (currentTarget != null) return;
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