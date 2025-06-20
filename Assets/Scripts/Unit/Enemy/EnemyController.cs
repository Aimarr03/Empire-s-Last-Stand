using Game.Buildings;
using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public string enemyType;
    public float moveSpeed = 2f;
    public float hp = 30f;
    public float maxHp = 30f;
    public Transform currentTarget;
    protected Animator animator;
    [SerializeField] protected float attackRange = 0.5f;
    private int facingDirection = 1;
    float lastAttackTime = 0f;
    public float attackCooldown = 1.5f;
    public float damage = 5f;
    public bool dead = false;

    [Header("UI")]
    [SerializeField] protected Canvas canvas;
    [SerializeField] protected Image hpBar;

    [SerializeField] protected AudioClip audio_spawn;
    [SerializeField] protected AudioClip audio_attacking;
    [SerializeField] protected AudioClip audio_die;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static event Action<string, EnemyController> EnemyDie;
    void Start()
    {
        AudioManager.instance.PlaySFXWithRandomPitch(audio_spawn);
        attackCooldown -= (float)(attackCooldown * (0.075 * GameplayManager.instance.currentNight));
        damage += (float)(damage * (0.15 * GameplayManager.instance.currentNight));
        maxHp += (float) (maxHp * (0.1 * GameplayManager.instance.currentNight));
        
        hp = maxHp;
        animator = GetComponent<Animator>();
        currentTarget = FindNearestBuilding();
        canvas.gameObject.SetActive(false);
    }
    

    // Update is called once per frame
    void Update()
    {
        if (GameplayManager.instance.gameState == GameState.Over) return;
        if (GameplayManager.instance.pause) return;
        if (dead) return;
        if (currentTarget == null || !currentTarget.gameObject.activeInHierarchy)
        {
            currentTarget = FindNearestBuilding();
            if (currentTarget == null) return; 
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
    
    public virtual void AttackTarget()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {

            Debug.Log("Enemy menyerang target!");
            lastAttackTime = Time.time;
            AudioManager.instance.PlaySFXWithRandomPitch(audio_attacking);

            animator.SetTrigger("Attacking");
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {

        if (currentTarget != null && currentTarget.CompareTag("Unit")) return;
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
        canvas.gameObject.SetActive(true);
        hpBar.fillAmount = hp / maxHp;
        if (hp > 0)
        {
            animator.SetTrigger("Attacked");
        }
        else
        {
            AudioManager.instance.PlaySFXWithRandomPitch(audio_die);
            dead = true;
            animator.SetTrigger("Dead");
            canvas.gameObject.SetActive(false);
            EnemyDie?.Invoke(enemyType, this);
        }
    }
    public void DoneAttacking()
    {
        if (currentTarget == null) return;
        bool findAnotherTarget = false;
        if (currentTarget.TryGetComponent<UnitController>(out UnitController unit))
        {
            findAnotherTarget = unit.GetDeadStatus();
        }
        if (currentTarget.TryGetComponent<Building>(out Building build))
        {
            findAnotherTarget = build.currentState != BuildingState.Constructed;
        }
        if (findAnotherTarget)
        {
            currentTarget = FindNearestBuilding();
            if (currentTarget == null)
            {
                animator.SetBool("Moving", false);
            }
        }
    }
    public void DamageTarget()
    {
        if (currentTarget == null) return;
        bool findAnotherTarget = false;
        if (currentTarget.TryGetComponent<UnitController>(out UnitController unit))
        {
            Debug.Log("Unit Take Damage");
            unit.TakeDamage(damage);
            findAnotherTarget = unit.GetDeadStatus();
        }
        else if(currentTarget.TryGetComponent<Building>(out Building build))
        {
            Debug.Log("Building Take Damage");
            build.TakeDamage((int)damage);
            findAnotherTarget = build.currentState != BuildingState.Constructed;
        }
        if (findAnotherTarget)
        {
            currentTarget = FindNearestBuilding();
            if (currentTarget == null)
            {
                animator.SetBool("Moving", false);
            }
        }
    }
    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
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
            Building buildingComponent = building.GetComponent<Building>();
            if(buildingComponent.currentState != BuildingState.Constructed) continue;
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
