using UnityEngine;
using Game.Buildings;
using System.Collections;

public class BuildingTower : Building // Mewarisi dari Building
{
    // Properti Tower dari Rubrik (Level 1)
    public float attackRange = 7f;     // Diambil dari rubrik: Attack Range (float) = 7
    public float attackCooldown = 1f;  // Diambil dari rubrik: Attack Speed (float) = 1 (cooldown adalah kebalikan dari speed)
    public float damage = 3f;          // Diambil dari rubrik: Damage (float) = 3

    [Header("Tower Specific Sprites")]
    public Sprite[] constructionFrames;
    public float constructionFrameRate = 0.1f;

    // Optional: Sprites untuk animasi serangan
    // public Sprite[] attackFrames;
    // public float attackFrameRate = 0.1f;

    private float attackTimer = 0f;
    private Transform targetEnemy;
    public GameObject bulletPrefab;

    protected override void Awake()
    {
        base.Awake();
        // Tidak ada inisialisasi tambahan di Awake untuk Tower saat ini
    }

    protected override void Start()
    {
        base.Start(); // Panggil Start dari base class untuk inisialisasi HP, memulai visual konstruksi, dan Invoke FinishConstruction
        maxHP = 50f; // Diambil dari rubrik: HP (float) = 50
        currentHP = maxHP; // Pastikan HP diatur sesuai maxHP yang baru
        cost = 3; // Diambil dari rubrik: Cost To Build (int) = 3
    }

    protected override void StartConstructionVisual()
    {
        base.StartConstructionVisual(); 

        if (constructionFrames != null && constructionFrames.Length > 0)
        {
            currentVisualCoroutine = StartCoroutine(AnimateSpriteFrames(constructionFrames, constructionFrameRate, false)); 
        }
        else
        {
            Debug.LogWarning("No construction frames provided for Tower. Defaulting to ready sprite after delay.");
        }
    }

    void Update()
    {
        if (currentVisualState == BuildingVisualState.Ready)
        {
            attackTimer += Time.deltaTime;

            if (targetEnemy == null)
            {
                // TODO: Tambahkan logika untuk mencari target
                // Misalnya: FindNearestEnemy();
            }

            if (targetEnemy != null)
            {
                float distance = Vector3.Distance(transform.position, targetEnemy.position);
                if (distance <= attackRange && attackTimer >= attackCooldown)
                {
                    Attack();
                    attackTimer = 0f;
                }
            }
        }
    }

    public void SetTarget(Transform enemy)
    {
        targetEnemy = enemy;
    }

    void Attack()
    {
        if (bulletPrefab != null && targetEnemy != null)
        {
            GameObject bulletInstance = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Bullet bulletScript = bulletInstance.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetTarget(targetEnemy.gameObject, damage); // Menggunakan damage dari Tower
            }
            Debug.Log($"{gameObject.name} menyerang target!");
        }
    }

    public override void Upgrade()
    {
        if (upgradeLevel < maxUpgradeLevel)
        {
            base.Upgrade(); // Akan menaikkan upgradeLevel
            
            // Properti Level 2 dari rubrik
            maxHP = 75f; 
            damage = 4f; 
            attackRange = 7f; // Tetap sama di Level 2
            attackCooldown = 0.9f; // Attack Speed 0.9 = Cooldown 0.9
            cost = 5; // Cost To Build Level 2 adalah 5

            // Reset HP setelah upgrade
            currentHP = maxHP; 

            Debug.Log($"{gameObject.name} di-upgrade ke level {upgradeLevel}. HP: {maxHP}, Damage: {damage}, Range: {attackRange}, Cooldown: {attackCooldown}, Cost: {cost}.");
        }
        else
        {
            Debug.Log($"{gameObject.name} sudah level maksimal.");
        }
    }
}