using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    [SerializeField] protected int damage;
    [SerializeField] private string targetTag; // <-- Tambah target tag

    protected void OnTriggerEnter2D(Collider2D collision)
    {
            if (!collision.CompareTag(targetTag)) return;
        if (collision.CompareTag(targetTag))
        {
            Health targetHealth = collision.GetComponent<Health>();
            if (targetHealth != null)
                targetHealth.TakeDamage(damage);

            gameObject.SetActive(false); // deactivate projectile after hitting
        }
    }

    // Public method to set target tag dynamically
    public void SetTargetTag(string tag)
    {
        targetTag = tag;
    }
}
