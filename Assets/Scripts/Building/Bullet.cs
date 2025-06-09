using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    private GameObject target;
    private int damage;

    public void SetTarget(GameObject enemy, int dmg)
    {
        target = enemy;
        damage = dmg;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == target)
        {
            target.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
