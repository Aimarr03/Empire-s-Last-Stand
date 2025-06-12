using UnityEngine;

public class PlayerArcher : AutoShooter
{
    protected override string TargetTag() => "Enemy";
}

