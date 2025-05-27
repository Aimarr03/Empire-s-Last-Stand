using UnityEngine;
using System.Collections.Generic;

public class TroopDummyMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveRadius = 1f; // radius lebih sempit sekarang
    public float minDistanceBetweenTroops = 1f;
    public float obstacleDetectionRadius = 0.3f; // radius deteksi bangunan di depan

    private Vector3 targetPosition;
    private bool isMoving = false;
    private Animator animator;

    private static List<Vector3> occupiedPositions = new List<Vector3>();

    void Start()
    {
        animator = GetComponent<Animator>();

        targetPosition = FindNonOverlappingPosition();
        isMoving = true;

        if (animator != null)
            animator.SetBool("isRunning", true);
    }

    void Update()
    {
        if (isMoving)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;

            // Cek obstacle bangunan di depan dengan OverlapCircle
            Vector2 checkPos = (Vector2)(transform.position + direction * obstacleDetectionRadius);
            Collider2D hit = Physics2D.OverlapCircle(checkPos, obstacleDetectionRadius, LayerMask.GetMask("Building"));

            if (hit != null)
            {
                // Jika ada bangunan di depan, hentikan gerak
                isMoving = false;
                animator.SetBool("isRunning", false);
                occupiedPositions.Add(transform.position);
                return;
            }

            // Gerak ke target
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
                animator.SetBool("isRunning", false);
                occupiedPositions.Add(targetPosition);
            }
        }
    }

    Vector3 FindNonOverlappingPosition()
    {
        Vector3 candidate;
        int attempts = 0;
        do
        {
            Vector2 offset = Random.insideUnitCircle * moveRadius;
            candidate = transform.position + new Vector3(offset.x, offset.y, 0);
            attempts++;
            if (attempts > 50) break;
        }
        while (IsTooCloseToOthers(candidate));
        return candidate;
    }

    bool IsTooCloseToOthers(Vector3 pos)
    {
        foreach (var occupiedPos in occupiedPositions)
        {
            if (Vector3.Distance(pos, occupiedPos) < minDistanceBetweenTroops)
                return true;
        }
        return false;
    }

    void OnDestroy()
    {
        if (occupiedPositions.Contains(targetPosition))
        {
            occupiedPositions.Remove(targetPosition);
        }
    }
}
