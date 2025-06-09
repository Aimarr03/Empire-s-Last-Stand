using UnityEngine;
using System.Collections.Generic;

public class TroopDummyMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveRadius = 1f;  // radius yang lebih sempit untuk sebaran yang lebih kecil
    public float minDistanceBetweenTroops = 1f;
    public float obstacleDetectionRadius = 0.3f;  // radius deteksi bangunan di depan

    private Vector3 targetPosition;
    private bool isMoving = false;
    private Animator animator;
    private SpriteRenderer spriteRenderer;  // Tambahkan SpriteRenderer

    private static List<Vector3> occupiedPositions = new List<Vector3>();

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();  // Ambil referensi SpriteRenderer

        // Set target posisi acak di sekitar spawnPoint
        targetPosition = FindNonOverlappingPosition();
        isMoving = true;

        // Mulai animasi berjalan
        if (animator != null)
            animator.SetBool("isRunning", true);

        // Atur posisi sortingOrder berdasarkan posisi awal
        UpdateSortingOrder();
    }

    void Update()
    {
        if (isMoving)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;

            // Cek apakah ada bangunan di depan dengan OverlapCircle
            Vector2 checkPos = (Vector2)(transform.position + direction * obstacleDetectionRadius);
            Collider2D hit = Physics2D.OverlapCircle(checkPos, obstacleDetectionRadius, LayerMask.GetMask("Building"));

            if (hit != null)
            {
                // Jika ada bangunan di depan, cari posisi baru
                targetPosition = FindNonOverlappingPosition();
            }

            // Gerakan troop menuju target
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            // Update sorting order sesuai posisi Y troop
            UpdateSortingOrder();

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
                animator.SetBool("isRunning", false);
                occupiedPositions.Add(targetPosition);
            }
        }
    }

    // Mencari posisi acak yang tidak tumpang tindih dengan posisi troop lain
    Vector3 FindNonOverlappingPosition()
    {
        Vector3 candidate;
        int attempts = 0;
        do
        {
            Vector2 offset = Random.insideUnitCircle * moveRadius; // Cari posisi acak di sekitar spawn
            candidate = transform.position + new Vector3(offset.x, offset.y, 0);
            attempts++;
            if (attempts > 50) break;  // Mencegah loop tanpa henti
        }
        while (IsTooCloseToOthers(candidate));  // Pastikan tidak ada tumpang tindih
        return candidate;
    }

    // Mengecek apakah posisi baru terlalu dekat dengan troop lain
    bool IsTooCloseToOthers(Vector3 pos)
    {
        foreach (var occupiedPos in occupiedPositions)
        {
            if (Vector3.Distance(pos, occupiedPos) < minDistanceBetweenTroops)
                return true;
        }
        return false;
    }

    // Mengubah sorting order berdasarkan posisi troop
    void UpdateSortingOrder()
    {
        // Atur sorting order berdasarkan posisi Y troop terhadap building
        if (transform.position.y > 0)  // Troop di depan building
        {
            spriteRenderer.sortingOrder = 1;  // Troop di depan building
        }
        else  // Troop di belakang building
        {
            spriteRenderer.sortingOrder = -1;  // Troop di belakang building
        }
    }

    void OnDestroy()
    {
        if (occupiedPositions.Contains(targetPosition))
        {
            occupiedPositions.Remove(targetPosition);
        }
    }
}
