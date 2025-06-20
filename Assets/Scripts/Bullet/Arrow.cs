using UnityEngine;

public class Arrow : BaseBullet
{
    [SerializeField] AudioClip arrowHit;
    /*protected override void OnCollisionEnter2D(Collider2D collided)
    {
        base.OnCollisionEnter2D(collided);
        if(collided.TryGetComponent<EnemyController>(out EnemyController enemy))
        {
            Debug.Log($"Enemy {enemy.gameObject.name} Take Damage");
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }*/
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.TryGetComponent<EnemyController>(out EnemyController enemy))
        {
            AudioManager.instance.PlaySFX(arrowHit, 0.15f);
            Debug.Log($"Unit {enemy.gameObject.name} Take Damage");
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
