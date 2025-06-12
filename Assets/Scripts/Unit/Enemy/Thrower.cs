using UnityEngine;

public class Thrower : EnemyController
{
    public Bomb bombObject;

    public void CreateBomb()
    {
        if (currentTarget == null) return;
        Vector3 enemyPos = currentTarget.position;
        SpriteRenderer enemySprite = currentTarget.GetComponentInChildren<SpriteRenderer>();
        
        float halfHeight = (enemySprite.sprite.rect.height / enemySprite.sprite.pixelsPerUnit) * 0.5f;
        halfHeight *= currentTarget.localScale.y;
        enemyPos.y += halfHeight;
        
        Vector3 aimDirection = (enemyPos - transform.position).normalized;


        Bomb bomb = Instantiate(bombObject, transform.position, Quaternion.identity);
        bomb.direction = aimDirection;
        bomb.damage = damage;
    }
}
