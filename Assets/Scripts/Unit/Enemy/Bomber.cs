using System.Collections;
using UnityEngine;

public class Bomber : EnemyController
{
    public float explodingTime = 3f;
    public override void AttackTarget()
    {
        animator.SetBool("Igniting", true);
        StartCoroutine(ExplodeAfterDelay());
    }
    
    private IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(explodingTime);
        Explode(); // Call your custom method here
    }
    
    private void Explode()
    {
        animator.SetTrigger("Explode");
        Debug.Log("BOOM! Explosion triggered.");
        // Destroy(gameObject);
        // Die();
    }

    protected new void Die()
    {
        base.Die();
    }
}
