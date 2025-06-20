using UnityEngine;

public class Knight: UnitController
{
    [SerializeField] private AudioClip attackClip;
    public void PlayAttackSound() => AudioManager.instance.PlaySFXWithRandomPitch(attackClip);
}