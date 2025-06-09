using UnityEngine;
using Game.Buildings;
using System.Collections; // Diperlukan untuk Coroutine

public class BuildingTower : Building // Mewarisi dari Building
{
    public float attackRange = 5f;
    public float attackCooldown = 1f;
    public int damage = 10;

    [Header("Tower Specific Sprites")]
    public Sprite[] constructionFrames;
    public float constructionFrameRate = 0.1f;

    // Optional: Sprites untuk animasi serangan
    // public Sprite[] attackFrames;
    // public float attackFrameRate = 0.1f;

    private float attackTimer = 0f;
    private Transform targetEnemy;
    public GameObject bulletPrefab;

    // Override metode ini untuk menampilkan visual konstruksi Tower
    protected override void StartConstructionVisual()
    {
        base.StartConstructionVisual(); // Panggil base method untuk set currentVisualState

        if (constructionFrames != null && constructionFrames.Length > 0)
        {
            currentVisualCoroutine = StartCoroutine(AnimateSpriteFrames(constructionFrames, constructionFrameRate, false)); // Non-looping
        }
        else
        {
            Debug.LogWarning("No construction frames provided for Tower. Defaulting to ready sprite after delay.");
            // Fallback jika tidak ada frame konstruksi
            // spriteRenderer.sprite = somePlaceholderSprite; // Opsional: placeholder
        }
    }

    void Update()
    {
        // Hanya lakukan logika serangan jika Tower sudah dalam keadaan Ready
        if (currentVisualState == BuildingVisualState.Ready)
        {
            attackTimer += Time.deltaTime;

            if (targetEnemy == null)
            {
                // TODO: Tambahkan logika untuk mencari target
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
            // Optional: Jika ada animasi serangan
            // if (attackFrames != null && attackFrames.Length > 0)
            // {
            //    StartCoroutine(AnimateSpriteFrames(attackFrames, attackFrameRate, false));
            // }

            GameObject bulletInstance = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Bullet bulletScript = bulletInstance.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetTarget(targetEnemy.gameObject, damage);
            }
            Debug.Log($"{gameObject.name} menyerang target!");
        }
    }

    public override void Upgrade()
    {
        base.Upgrade();
        damage += 5;
        attackRange += 1f;
        attackCooldown -= 0.1f;
        Debug.Log($"{gameObject.name} di-upgrade. Damage: {damage}, Range: {attackRange}, Cooldown: {attackCooldown}");
    }
}