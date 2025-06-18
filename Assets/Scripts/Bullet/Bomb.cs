using Game.Buildings;
using UnityEngine;

public class Bomb : BaseBullet
{
    [SerializeField] ParticleSystem explodeEffect;
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
            OnHit();
        }
        if (collision.gameObject.CompareTag("Building"))
        {
            collision.gameObject.TryGetComponent<Building>(out Building building);
            building.TakeDamage((int)damage);
            OnHit();
        }
    }
    private void OnHit()
    {
        explodeEffect.gameObject.SetActive(true);
        explodeEffect.Play();
        explodeEffect.transform.parent = null;

        Destroy(gameObject);
    }

}
