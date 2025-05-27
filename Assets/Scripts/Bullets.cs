using UnityEngine;

public class Bullets : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform TNTPoint;
    [SerializeField] private GameObject[] projectile;
    private float cooldownTimer;

    private void Attack()
    {
        cooldownTimer = 0;

        projectile[FindTNT()].transform.position = TNTPoint.position;
        projectile[FindTNT()].GetComponent<Bullet>().ActivateProjectile();
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