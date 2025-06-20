using UnityEngine;

public class Torcher : EnemyController
{
    [SerializeField] private AudioClip audio_torchAttack;

    public void PlayAttackSFX() => AudioManager.instance.PlaySFX(audio_torchAttack, 0.3f);
}
