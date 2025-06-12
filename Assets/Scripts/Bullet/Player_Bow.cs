using UnityEngine;

public class Player_Bow : MonoBehaviour
{
    public Transform launchPoint;
    public GameObject arrowPrefab;
    public float shootInterval = 2f;
    private float shootTimer = 0f;
    public float attackRange = 5f;

    private Transform currentTarget;
    private Vector2 aimDirection = Vector2.right;

    void Update()
    {
        if (currentTarget == null || !currentTarget.gameObject.activeInHierarchy)
        {
            currentTarget = FindNearestEnemy();
        }

        if (currentTarget != null)
        {
            float distance = Vector2.Distance(transform.position, currentTarget.position);
            if (distance <= attackRange)
            {
                aimDirection = (currentTarget.position - transform.position).normalized;
                shootTimer += Time.deltaTime;
                if (shootTimer >= shootInterval)
                {
                    Shoot();
                    shootTimer = 0f;
                }
            }
        }
    }

    public void Shoot()
    {
        if (arrowPrefab != null && launchPoint != null)
        {
            Arrow arrow = Instantiate(arrowPrefab, launchPoint.position, Quaternion.identity).GetComponent<Arrow>();
            arrow.direction = aimDirection;
        }
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