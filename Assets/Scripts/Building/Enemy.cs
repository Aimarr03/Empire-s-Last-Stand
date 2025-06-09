using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f; // Ubah ke float

    public void TakeDamage(float dmg) // Ubah parameter ke float
    {
        health -= dmg;
        Debug.Log($"{gameObject.name} took {dmg} damage. Remaining health: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} has been destroyed!");
        Destroy(gameObject);
    }
}