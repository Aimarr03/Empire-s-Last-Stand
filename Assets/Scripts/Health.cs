using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    private Rigidbody2D body;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        if (!dead)
        {
            anim.SetTrigger("die");
            dead = true;
        }
        else
        {
            dead = false;
        }

        //}
    }

}