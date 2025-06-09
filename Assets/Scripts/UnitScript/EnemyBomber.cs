using UnityEngine;

public class EnemyBomber : AutoShooter
{
    protected override string TargetTag() => "Player";
}

