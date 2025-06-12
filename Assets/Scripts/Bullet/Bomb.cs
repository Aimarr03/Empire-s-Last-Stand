using UnityEngine;

public class Bomb : BaseBullet
{
    /*protected override void OnCollisionEnter2D(Collider2D collided)
    {
        base.OnCollisionEnter2D(collided);
        if (collided.TryGetComponent<UnitController>(out UnitController unit))
        {
            Debug.Log($"Unit {unit.gameObject.name} Take Damage");
            unit.TakeDamage(damage);
            Destroy(gameObject);
        }
    }*/
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.TryGetComponent<UnitController>(out UnitController unit))
        {
            Debug.Log($"Unit {unit.gameObject.name} Take Damage");
            unit.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
