using UnityEngine;

public abstract class AutoShooter : MonoBehaviour
{
    public Transform launchPoint;
    public GameObject projectilePrefab;
    public float shootInterval = 2f;
    public float attackRange = 5f;

    protected Transform currentTarget;
    protected Vector2 aimDirection = Vector2.right;
    protected float shootTimer = 0f;

    // Wajib ditentukan oleh class turunan
    protected abstract string TargetTag();

    protected virtual void Update()
    {
        if (currentTarget == null || !currentTarget.gameObject.activeInHierarchy)
        {
            currentTarget = FindNearestTarget();
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

    protected virtual void Shoot()
    {
        if (projectilePrefab != null && launchPoint != null)
        {
            Arrow arrow = Instantiate(projectilePrefab, launchPoint.position, Quaternion.identity).GetComponent<Arrow>();
            arrow.direction = aimDirection;
        }
    }

    protected Transform FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(TargetTag());
        Transform nearest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject target in targets)
        {
            float distance = Vector2.Distance(transform.position, target.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearest = target.transform;
            }
        }

        return nearest;
    }
}
