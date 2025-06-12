using UnityEngine;
using UnityEngine.UIElements;

public class Archer: UnitController
{
    public Arrow arrowObject;

    public void CreateArrow()
    {
        if (currentTarget == null) return;
        Vector3 enemyPos = currentTarget.position;
        SpriteRenderer enemySprite = currentTarget.GetComponentInChildren<SpriteRenderer>();

        float halfHeight = (enemySprite.sprite.rect.height / enemySprite.sprite.pixelsPerUnit) * 0.5f;
        halfHeight *= currentTarget.localScale.y;

        enemyPos.y += halfHeight;

        Vector3 aimDirection = (enemyPos - transform.position).normalized;
        
        
        Arrow arrow = Instantiate(arrowObject, transform.position, Quaternion.identity);
        arrow.direction = aimDirection;
        arrow.damage = damage;
    }
}