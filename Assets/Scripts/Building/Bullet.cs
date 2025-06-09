using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    private GameObject target;
    private float damage; // Ubah ke float

    public void SetTarget(GameObject enemy, float dmg) // Ubah parameter ke float
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
            // Pastikan komponen musuh memiliki metode TakeDamage yang menerima float
            Enemy enemyScript = collision.gameObject.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damage); // Memanggil TakeDamage dengan float damage
            }
            Destroy(gameObject);
        }
    }
}