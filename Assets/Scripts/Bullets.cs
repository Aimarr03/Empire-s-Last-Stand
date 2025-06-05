using UnityEngine;

public class Bullets : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform FirePoint;
    [SerializeField] private GameObject[] projectile;
    [SerializeField] private string targetTag; // Bisa di-set di inspector untuk musuh/ally

    private float cooldownTimer;

    private void Attack()
    {
        cooldownTimer = 0;

        int index = FindTNT();
        projectile[index].transform.position = FirePoint.position;

        // Set target tag sebelum aktifkan
        Bullet bulletScript = projectile[index].GetComponent<Bullet>();
        bulletScript.SetTargetTag(targetTag);
        bulletScript.ActivateProjectile();
    }

    private int FindTNT()
    {
        for (int i = 0; i < projectile.Length; i++)
        {
            if (!projectile[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (cooldownTimer >= attackCooldown)
            Attack();
    }
}
