using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 direction = Vector2.right;
    public float lifespawn = 10;
    public float speed;
    public float damage;
    public string objectTargetTag;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        rb.linearVelocity = direction * speed;
        RotateArrow();
        Destroy(gameObject, lifespawn);

    }
    protected void RotateArrow()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
    protected virtual void Update()
    {

    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided With Unit: " + collision.gameObject.name);
        if (!collision.gameObject.CompareTag(objectTargetTag)) return;
    }
}
